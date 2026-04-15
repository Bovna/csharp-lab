param(
    [string]$payload,
    [string]$EventType = "Unknown"
)

function Get-FirstPropertyValue {
    param(
        [Parameter(Mandatory = $true)]$Object,
        [Parameter(Mandatory = $true)][string[]]$Names
    )

    foreach ($name in $Names) {
        if ($null -ne $Object -and $Object.PSObject.Properties.Name -contains $name) {
            $value = $Object.$name
            if ($null -ne $value -and -not [string]::IsNullOrWhiteSpace([string]$value)) {
                return [string]$value
            }
        }
    }

    return $null
}

$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot "..\.."))
$logPath = Join-Path $repoRoot ".github\logs\agent_log.txt"

$logDirectory = Split-Path -Parent $logPath
if (-not (Test-Path -LiteralPath $logDirectory)) {
    New-Item -ItemType Directory -Path $logDirectory -Force | Out-Null
}

$rawPayload = ""
if (-not [string]::IsNullOrWhiteSpace($payload)) {
    $rawPayload = $payload
}
else {
    $stdinPayload = [Console]::In.ReadToEnd()
    if (-not [string]::IsNullOrWhiteSpace($stdinPayload)) {
        $rawPayload = $stdinPayload
    }
}

if ([string]::IsNullOrWhiteSpace($rawPayload)) {
    exit 0
}

try {
    $parsed = $rawPayload.Trim() | ConvertFrom-Json -ErrorAction Stop

    $sessionId = Get-FirstPropertyValue -Object $parsed -Names @("session_id", "sessionId", "SessionId")
    if ([string]::IsNullOrWhiteSpace($sessionId)) {
        $sessionId = $env:COPILOT_SESSION_ID
    }

    $transcriptPath = Get-FirstPropertyValue -Object $parsed -Names @("transcript_path", "transcriptPath", "TranscriptPath")
    if ([string]::IsNullOrWhiteSpace($transcriptPath)) {
        $transcriptPath = $env:VSCODE_TARGET_SESSION_LOG
    }

    $toolUseId = Get-FirstPropertyValue -Object $parsed -Names @("tool_use_id", "toolUseId", "tool_call_id", "toolCallId", "id")

    $cwd = Get-FirstPropertyValue -Object $parsed -Names @("cwd", "working_directory", "workingDirectory")
    if ([string]::IsNullOrWhiteSpace($cwd)) {
        $cwd = (Get-Location).Path
    }

    $entry = [ordered]@{
        timestamp       = if ($parsed.timestamp) { $parsed.timestamp } else { (Get-Date).ToUniversalTime().ToString("o") }
        hook_event_name = if ($parsed.hook_event_name) { $parsed.hook_event_name } else { $EventType }
        session_id      = $sessionId
        transcript_path = $transcriptPath
        tool_use_id     = $toolUseId
        cwd             = $cwd
    }

    if ($parsed.PSObject.Properties.Name -contains "tool_name") {
        $entry.tool_name = $parsed.tool_name
    }

    if ($parsed.PSObject.Properties.Name -contains "tool_input") {
        $entry.tool_input = $parsed.tool_input
    }

    if ($parsed.PSObject.Properties.Name -contains "prompt") {
        $entry.prompt = $parsed.prompt
    }

    Add-Content -LiteralPath $logPath -Value (($entry | ConvertTo-Json -Depth 20) + "`r`n") -Encoding utf8
}
catch {
    Add-Content -LiteralPath $logPath -Value ("[{0}] {1}: {2}`r`n" -f (Get-Date).ToUniversalTime().ToString("o"), $EventType, $rawPayload) -Encoding utf8
}
