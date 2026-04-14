param(
    [Parameter(Mandatory = $true)]
    [string]$EventType,

    [string]$LogFile = '.github/logs/agent_log.txt'
)

$payload = [Console]::In.ReadToEnd()

if ([string]::IsNullOrWhiteSpace($payload)) {
    exit 0
}

$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))

$resolvedLogFile = if ([System.IO.Path]::IsPathRooted($LogFile)) {
    $LogFile
}
else {
    Join-Path $repoRoot $LogFile
}

$logDirectory = Split-Path -Parent $resolvedLogFile
if (-not (Test-Path -LiteralPath $logDirectory)) {
    New-Item -ItemType Directory -Path $logDirectory -Force | Out-Null
}

function Build-LogObject {
    param(
        [Parameter(Mandatory = $true)]
        [string]$RawPayload,
        [Parameter(Mandatory = $true)]
        [string]$FallbackEventType
    )

    try {
        $parsed = $RawPayload.Trim() | ConvertFrom-Json -ErrorAction Stop
    }
    catch {
        return [ordered]@{
            timestamp       = (Get-Date).ToUniversalTime().ToString('o')
            hook_event_name = $FallbackEventType
        }
    }

    $logObject = [ordered]@{}

    if ($parsed.PSObject.Properties.Name -contains 'timestamp') {
        $logObject.timestamp = $parsed.timestamp
    }
    else {
        $logObject.timestamp = (Get-Date).ToUniversalTime().ToString('o')
    }

    if ($parsed.PSObject.Properties.Name -contains 'hook_event_name') {
        $logObject.hook_event_name = $parsed.hook_event_name
    }
    else {
        $logObject.hook_event_name = $FallbackEventType
    }

    if ($parsed.PSObject.Properties.Name -contains 'tool_name') {
        $logObject.tool_name = $parsed.tool_name
    }

    if ($parsed.PSObject.Properties.Name -contains 'tool_input') {
        $logObject.tool_input = $parsed.tool_input
    }

    if ($parsed.PSObject.Properties.Name -contains 'prompt') {
        $logObject.prompt = $parsed.prompt
    }

    return $logObject
}

$logObject = Build-LogObject -RawPayload $payload -FallbackEventType $EventType
$entry = ($logObject | ConvertTo-Json -Depth 20) + "`r`n`r`n"
Add-Content -LiteralPath $resolvedLogFile -Value $entry -Encoding utf8