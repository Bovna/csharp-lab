(function () {
  "use strict";

  const BOARD_CELLS = 20;
  const CELL_SIZE = 24;
  const BOARD_SIZE = BOARD_CELLS * CELL_SIZE;
  const START_SPEED = 175;
  const MIN_SPEED = 95;
  const BEST_SCORE_KEY = "kinoklik-snake-best";

  const DIRECTIONS = {
    up: { x: 0, y: -1 },
    down: { x: 0, y: 1 },
    left: { x: -1, y: 0 },
    right: { x: 1, y: 0 },
  };

  const KEY_DIRECTIONS = {
    ArrowUp: "up",
    ArrowDown: "down",
    ArrowLeft: "left",
    ArrowRight: "right",
    w: "up",
    W: "up",
    s: "down",
    S: "down",
    a: "left",
    A: "left",
    d: "right",
    D: "right",
  };

  let elements = null;
  let previousFocus = null;
  let mode = "closed";
  let snake = [];
  let food = { x: 0, y: 0 };
  let direction = DIRECTIONS.right;
  let nextDirection = DIRECTIONS.right;
  let directionLocked = false;
  let score = 0;
  let bestScore = readBestScore();
  let animationFrame = 0;
  let countdownTimer = 0;
  let lastFrameTime = 0;
  let stepAccumulator = 0;
  let pointerStart = null;

  function readBestScore() {
    try {
      const storedScore = Number.parseInt(
        window.localStorage.getItem(BEST_SCORE_KEY) || "0",
        10,
      );
      return Number.isFinite(storedScore) && storedScore > 0 ? storedScore : 0;
    } catch (_error) {
      return 0;
    }
  }

  function saveBestScore() {
    try {
      window.localStorage.setItem(BEST_SCORE_KEY, String(bestScore));
    } catch (_error) {
      // The game remains playable when storage is blocked or unavailable.
    }
  }

  function createGame() {
    if (elements || !document.body) {
      return elements;
    }

    const root = document.createElement("div");
    root.className = "snake-game";
    root.hidden = true;
    root.setAttribute("role", "dialog");
    root.setAttribute("aria-modal", "true");
    root.setAttribute("aria-hidden", "true");
    root.setAttribute("aria-labelledby", "snakeGameTitle");
    root.setAttribute("aria-describedby", "snakeGameInstructions");
    root.innerHTML = `
      <div class="snake-game__backdrop" data-snake-close></div>
      <section class="snake-game__dialog">
        <header class="snake-game__header">
          <h2 class="snake-game__eyebrow" id="snakeGameTitle">Tajna projekcija</h2>
          <div class="snake-game__header-actions">
            <button class="snake-game__restart" type="button" data-snake-restart>Nova igra</button>
            <button class="snake-game__close" type="button" data-snake-close aria-label="Napusti tajnu projekciju">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
        </header>

        <div class="snake-game__hud" aria-label="Rezultat igre">
          <p><span>Kokice</span><strong data-snake-score>0</strong></p>
          <p><span>Rekord</span><strong data-snake-best>0</strong></p>
        </div>

        <div class="snake-game__screen">
          <canvas class="snake-game__canvas" width="${BOARD_SIZE}" height="${BOARD_SIZE}"
                  tabindex="0" aria-label="Igra Snake. Filmsku vrpcu usmjeravajte strelicama ili tipkama WASD.">
            Vaš preglednik ne podržava platno potrebno za igru Snake.
          </canvas>
          <div class="snake-game__scanlines" aria-hidden="true"></div>
          <div class="snake-game__message" data-snake-message hidden aria-hidden="true"></div>
        </div>

        <p class="snake-game__instructions" id="snakeGameInstructions">
          Usmjeravajte filmsku vrpcu strelicama ili tipkama WASD i skupljajte kokice.
        </p>

        <div class="snake-game__dpad" aria-label="Kontrole smjera">
          <button type="button" class="snake-game__direction snake-game__direction--up"
                  data-snake-direction="up" aria-label="Gore">&#9650;</button>
          <button type="button" class="snake-game__direction snake-game__direction--left"
                  data-snake-direction="left" aria-label="Lijevo">&#9664;</button>
          <button type="button" class="snake-game__direction snake-game__direction--down"
                  data-snake-direction="down" aria-label="Dolje">&#9660;</button>
          <button type="button" class="snake-game__direction snake-game__direction--right"
                  data-snake-direction="right" aria-label="Desno">&#9654;</button>
        </div>

        <p class="visually-hidden" data-snake-status aria-live="assertive"></p>
      </section>`;

    document.body.appendChild(root);

    const canvas = root.querySelector(".snake-game__canvas");
    const context = canvas ? canvas.getContext("2d") : null;
    if (!canvas || !context) {
      root.remove();
      return null;
    }

    elements = {
      root,
      canvas,
      context,
      message: root.querySelector("[data-snake-message]"),
      score: root.querySelector("[data-snake-score]"),
      best: root.querySelector("[data-snake-best]"),
      status: root.querySelector("[data-snake-status]"),
      restart: root.querySelector("[data-snake-restart]"),
    };

    root.addEventListener("click", handleGameClick);
    canvas.addEventListener("pointerdown", handlePointerDown);
    canvas.addEventListener("pointerup", handlePointerUp);
    canvas.addEventListener("pointercancel", clearPointerStart);
    document.addEventListener("keydown", handleKeyDown);
    document.addEventListener("visibilitychange", handleVisibilityChange);

    return elements;
  }

  function handleGameClick(event) {
    const closeButton = event.target.closest("[data-snake-close]");
    if (closeButton) {
      closeGame();
      return;
    }

    const directionButton = event.target.closest("[data-snake-direction]");
    if (directionButton) {
      setDirection(directionButton.dataset.snakeDirection);
      elements.canvas.focus({ preventScroll: true });
      return;
    }

    if (event.target.closest("[data-snake-restart]")) {
      startGame();
      elements.canvas.focus({ preventScroll: true });
    }
  }

  function handlePointerDown(event) {
    if (mode !== "running" && mode !== "countdown") {
      return;
    }

    pointerStart = { x: event.clientX, y: event.clientY };
    if (elements.canvas.setPointerCapture) {
      elements.canvas.setPointerCapture(event.pointerId);
    }
  }

  function handlePointerUp(event) {
    if (!pointerStart || (mode !== "running" && mode !== "countdown")) {
      clearPointerStart();
      return;
    }

    const deltaX = event.clientX - pointerStart.x;
    const deltaY = event.clientY - pointerStart.y;
    clearPointerStart();

    if (Math.max(Math.abs(deltaX), Math.abs(deltaY)) < 24) {
      return;
    }

    if (Math.abs(deltaX) > Math.abs(deltaY)) {
      setDirection(deltaX > 0 ? "right" : "left");
    } else {
      setDirection(deltaY > 0 ? "down" : "up");
    }
  }

  function clearPointerStart() {
    pointerStart = null;
  }

  function handleKeyDown(event) {
    if (!elements || elements.root.hidden) {
      return;
    }

    if (event.key === "Escape") {
      event.preventDefault();
      closeGame();
      return;
    }

    if (event.key === "Tab") {
      keepFocusInDialog(event);
      return;
    }

    const requestedDirection = KEY_DIRECTIONS[event.key];
    if (requestedDirection) {
      event.preventDefault();
      setDirection(requestedDirection);
      return;
    }

    if (
      mode === "game-over" &&
      (event.code === "Space" || event.key === "Enter") &&
      !(event.target instanceof Element && event.target.closest("button"))
    ) {
      event.preventDefault();
      startGame();
    }
  }

  function keepFocusInDialog(event) {
    const focusable = Array.from(
      elements.root.querySelectorAll(
        'button:not([disabled]), [href], [tabindex]:not([tabindex="-1"])',
      ),
    ).filter((item) => !item.hidden);

    if (!focusable.length) {
      event.preventDefault();
      return;
    }

    const first = focusable[0];
    const last = focusable[focusable.length - 1];
    if (event.shiftKey && document.activeElement === first) {
      event.preventDefault();
      last.focus();
    } else if (!event.shiftKey && document.activeElement === last) {
      event.preventDefault();
      first.focus();
    }
  }

  function handleVisibilityChange() {
    if (!elements || elements.root.hidden) {
      return;
    }

    if (document.hidden && mode === "running") {
      mode = "paused";
      window.cancelAnimationFrame(animationFrame);
      animationFrame = 0;
      showMessage("PAUZA", "countdown");
      setStatus("Igra je pauzirana.");
      return;
    }

    if (!document.hidden && mode === "paused") {
      hideMessage();
      mode = "running";
      lastFrameTime = window.performance.now();
      stepAccumulator = 0;
      setStatus("Igra je nastavljena.");
      animationFrame = window.requestAnimationFrame(gameLoop);
    } else if (!document.hidden && mode === "running") {
      lastFrameTime = window.performance.now();
      stepAccumulator = 0;
    }
  }

  function setDirection(name) {
    if (
      (mode !== "running" && mode !== "countdown") ||
      directionLocked ||
      !DIRECTIONS[name]
    ) {
      return;
    }

    const requested = DIRECTIONS[name];
    const isOpposite =
      requested.x + direction.x === 0 && requested.y + direction.y === 0;
    if (isOpposite) {
      return;
    }

    nextDirection = requested;
    directionLocked = true;
  }

  function openGame() {
    if (!createGame() || !elements.root.hidden) {
      return;
    }

    previousFocus = document.activeElement;
    elements.root.hidden = false;
    elements.root.setAttribute("aria-hidden", "false");
    document.body.classList.add("snake-game-open");
    startGame();

    window.requestAnimationFrame(() => {
      if (elements.root.hidden) {
        return;
      }

      elements.root.classList.add("is-visible");
      elements.canvas.focus({ preventScroll: true });
    });
  }

  function closeGame() {
    if (!elements || elements.root.hidden) {
      return;
    }

    window.clearTimeout(countdownTimer);
    window.cancelAnimationFrame(animationFrame);
    countdownTimer = 0;
    animationFrame = 0;
    pointerStart = null;
    mode = "closed";
    elements.root.classList.remove("is-visible");
    elements.root.hidden = true;
    elements.root.setAttribute("aria-hidden", "true");
    document.body.classList.remove("snake-game-open");

    const focusTarget = previousFocus;
    previousFocus = null;
    if (focusTarget && focusTarget.isConnected && typeof focusTarget.focus === "function") {
      focusTarget.focus({ preventScroll: true });
    }
  }

  function startGame() {
    window.clearTimeout(countdownTimer);
    window.cancelAnimationFrame(animationFrame);
    animationFrame = 0;

    const middle = Math.floor(BOARD_CELLS / 2);
    snake = [
      { x: middle + 1, y: middle },
      { x: middle, y: middle },
      { x: middle - 1, y: middle },
      { x: middle - 2, y: middle },
    ];
    direction = DIRECTIONS.right;
    nextDirection = DIRECTIONS.right;
    directionLocked = false;
    score = 0;
    food = createFood();
    lastFrameTime = 0;
    stepAccumulator = 0;
    mode = "countdown";

    elements.restart.textContent = "Nova igra";
    updateScore();
    drawBoard();
    runCountdown(3);
  }

  function runCountdown(value) {
    if (mode !== "countdown") {
      return;
    }

    showMessage(String(value), "countdown");
    setStatus(`Tajna projekcija počinje za ${value}.`);
    const reduceMotion = window.matchMedia(
      "(prefers-reduced-motion: reduce)",
    ).matches;
    const delay = reduceMotion ? 450 : 850;

    countdownTimer = window.setTimeout(() => {
      if (value > 1) {
        runCountdown(value - 1);
        return;
      }

      showMessage("KRENI!", "countdown");
      setStatus("Igra je počela.");
      countdownTimer = window.setTimeout(() => {
        if (mode !== "countdown") {
          return;
        }

        hideMessage();
        mode = "running";
        lastFrameTime = window.performance.now();
        stepAccumulator = 0;
        animationFrame = window.requestAnimationFrame(gameLoop);
      }, reduceMotion ? 260 : 480);
    }, delay);
  }

  function gameLoop(timestamp) {
    if (!elements || elements.root.hidden || mode !== "running") {
      animationFrame = 0;
      return;
    }

    if (!lastFrameTime) {
      lastFrameTime = timestamp;
    }

    const elapsed = Math.min(timestamp - lastFrameTime, 180);
    lastFrameTime = timestamp;
    stepAccumulator += elapsed;

    let speed = getStepSpeed();
    while (stepAccumulator >= speed && mode === "running") {
      stepGame();
      stepAccumulator -= speed;
      speed = getStepSpeed();
    }

    if (mode === "game-over") {
      animationFrame = 0;
      return;
    }

    animationFrame = window.requestAnimationFrame(gameLoop);
  }

  function getStepSpeed() {
    return Math.max(MIN_SPEED, START_SPEED - score * 2);
  }

  function stepGame() {
    direction = nextDirection;
    directionLocked = false;

    const head = snake[0];
    const nextHead = {
      x: (head.x + direction.x + BOARD_CELLS) % BOARD_CELLS,
      y: (head.y + direction.y + BOARD_CELLS) % BOARD_CELLS,
    };
    const ateFood = nextHead.x === food.x && nextHead.y === food.y;
    const collisionBody = ateFood ? snake : snake.slice(0, -1);
    const hitFilm = collisionBody.some(
      (segment) => segment.x === nextHead.x && segment.y === nextHead.y,
    );

    if (hitFilm) {
      endGame();
      return;
    }

    snake.unshift(nextHead);
    if (ateFood) {
      score += 1;
      if (score > bestScore) {
        bestScore = score;
        saveBestScore();
      }
      food = createFood();
      updateScore();
      setStatus(`Skupljene kokice: ${score}.`);
    } else {
      snake.pop();
    }

    drawBoard();
  }

  function createFood() {
    const availableCells = [];
    for (let y = 0; y < BOARD_CELLS; y += 1) {
      for (let x = 0; x < BOARD_CELLS; x += 1) {
        if (!snake.some((segment) => segment.x === x && segment.y === y)) {
          availableCells.push({ x, y });
        }
      }
    }

    if (!availableCells.length) {
      endGame(true);
      return { x: -1, y: -1 };
    }

    return availableCells[Math.floor(Math.random() * availableCells.length)];
  }

  function endGame(completed = false) {
    mode = "game-over";
    elements.restart.textContent = "Ponovno";
    showMessage(
      completed ? "PLATNO JE OSVOJENO!" : "FILMSKA VRPCA\nJE PUKLA!",
      "game-over",
    );
    setStatus(
      `${completed ? "Platno je osvojeno" : "Filmska vrpca je pukla"}. ` +
        `Rezultat: ${score}. Pritisnite razmaknicu za novu igru.`,
    );
  }

  function updateScore() {
    elements.score.textContent = String(score);
    elements.best.textContent = String(bestScore);
  }

  function setStatus(message) {
    elements.status.textContent = "";
    window.requestAnimationFrame(() => {
      if (elements) {
        elements.status.textContent = message;
      }
    });
  }

  function showMessage(message, kind) {
    elements.message.textContent = message;
    elements.message.dataset.kind = kind;
    elements.message.hidden = false;
  }

  function hideMessage() {
    elements.message.hidden = true;
    elements.message.textContent = "";
    delete elements.message.dataset.kind;
  }

  function drawBoard() {
    const context = elements.context;
    context.clearRect(0, 0, BOARD_SIZE, BOARD_SIZE);

    const background = context.createRadialGradient(
      BOARD_SIZE / 2,
      BOARD_SIZE / 2,
      30,
      BOARD_SIZE / 2,
      BOARD_SIZE / 2,
      BOARD_SIZE * 0.7,
    );
    background.addColorStop(0, "#12251a");
    background.addColorStop(1, "#061009");
    context.fillStyle = background;
    context.fillRect(0, 0, BOARD_SIZE, BOARD_SIZE);

    context.strokeStyle = "rgba(123, 255, 180, 0.045)";
    context.lineWidth = 1;
    for (let cell = 1; cell < BOARD_CELLS; cell += 1) {
      const offset = cell * CELL_SIZE + 0.5;
      context.beginPath();
      context.moveTo(offset, 0);
      context.lineTo(offset, BOARD_SIZE);
      context.stroke();
      context.beginPath();
      context.moveTo(0, offset);
      context.lineTo(BOARD_SIZE, offset);
      context.stroke();
    }

    drawPopcorn(context);
    for (let index = snake.length - 1; index >= 0; index -= 1) {
      drawFilmSegment(context, snake[index], index === 0, index);
    }
  }

  function drawFilmSegment(context, segment, isHead, index) {
    const x = segment.x * CELL_SIZE + 2;
    const y = segment.y * CELL_SIZE + 2;
    const size = CELL_SIZE - 4;

    context.save();
    context.shadowColor = isHead
      ? "rgba(0, 230, 118, 0.75)"
      : "rgba(0, 190, 95, 0.32)";
    context.shadowBlur = isHead ? 12 : 6;
    roundedRect(context, x, y, size, size, isHead ? 7 : 5);
    context.fillStyle = isHead
      ? "#39f28f"
      : index % 2 === 0
        ? "#13bd68"
        : "#0ea659";
    context.fill();
    context.restore();

    context.fillStyle = "rgba(5, 35, 19, 0.72)";
    for (let hole = 0; hole < 3; hole += 1) {
      const holeX = x + 3 + hole * 6;
      roundedRect(context, holeX, y + 2, 3, 2, 1);
      context.fill();
      roundedRect(context, holeX, y + size - 4, 3, 2, 1);
      context.fill();
    }

    if (isHead) {
      drawEyes(context, x, y, size);
    }
  }

  function drawEyes(context, x, y, size) {
    let eyes;
    if (direction === DIRECTIONS.left) {
      eyes = [
        { x: x + 5, y: y + 6 },
        { x: x + 5, y: y + size - 6 },
      ];
    } else if (direction === DIRECTIONS.up) {
      eyes = [
        { x: x + 6, y: y + 5 },
        { x: x + size - 6, y: y + 5 },
      ];
    } else if (direction === DIRECTIONS.down) {
      eyes = [
        { x: x + 6, y: y + size - 5 },
        { x: x + size - 6, y: y + size - 5 },
      ];
    } else {
      eyes = [
        { x: x + size - 5, y: y + 6 },
        { x: x + size - 5, y: y + size - 6 },
      ];
    }

    context.fillStyle = "#04140b";
    eyes.forEach((eye) => {
      context.beginPath();
      context.arc(eye.x, eye.y, 1.8, 0, Math.PI * 2);
      context.fill();
    });
  }

  function drawPopcorn(context) {
    if (food.x < 0 || food.y < 0) {
      return;
    }

    const centerX = food.x * CELL_SIZE + CELL_SIZE / 2;
    const centerY = food.y * CELL_SIZE + CELL_SIZE / 2;
    context.save();
    context.shadowColor = "rgba(255, 214, 92, 0.8)";
    context.shadowBlur = 12;
    context.fillStyle = "#fff0ad";
    [
      [-5, -2, 5],
      [0, -5, 5.5],
      [5, -1, 5],
      [0, 2, 6],
    ].forEach(([offsetX, offsetY, radius]) => {
      context.beginPath();
      context.arc(centerX + offsetX, centerY + offsetY, radius, 0, Math.PI * 2);
      context.fill();
    });
    context.fillStyle = "#f1bf3b";
    context.beginPath();
    context.arc(centerX, centerY - 1, 3.2, 0, Math.PI * 2);
    context.fill();
    context.restore();
  }

  function roundedRect(context, x, y, width, height, radius) {
    context.beginPath();
    if (typeof context.roundRect === "function") {
      context.roundRect(x, y, width, height, radius);
      return;
    }

    const safeRadius = Math.min(radius, width / 2, height / 2);
    context.moveTo(x + safeRadius, y);
    context.lineTo(x + width - safeRadius, y);
    context.quadraticCurveTo(x + width, y, x + width, y + safeRadius);
    context.lineTo(x + width, y + height - safeRadius);
    context.quadraticCurveTo(
      x + width,
      y + height,
      x + width - safeRadius,
      y + height,
    );
    context.lineTo(x + safeRadius, y + height);
    context.quadraticCurveTo(x, y + height, x, y + height - safeRadius);
    context.lineTo(x, y + safeRadius);
    context.quadraticCurveTo(x, y, x + safeRadius, y);
    context.closePath();
  }

  window.KinoKlikSnake = Object.freeze({
    open: openGame,
    close: closeGame,
  });
})();
