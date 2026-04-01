param(
    [string]$EventType = "Unknown"
)

$logsDir = ".github/logs"
if (-not (Test-Path $logsDir)) {
    New-Item -ItemType Directory -Path $logsDir -Force | Out-Null
}

$logFile = Join-Path $logsDir "agent_log.txt"

$inputData = $input | ConvertFrom-Json -ErrorAction SilentlyContinue

$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"

$logData = @{
    timestamp = $timestamp
}

if ($EventType -eq "PreToolUse") {
    $logData.tool_name = $inputData.tool_name
    $logData.tool_input = $inputData.tool_input
}

$logEntry = $logData | ConvertTo-Json
Add-Content -Path $logFile -Value $logEntry -ErrorAction SilentlyContinue

exit 0
