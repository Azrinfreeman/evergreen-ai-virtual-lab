# Supervised capture of the route rehearsed on 2026-11-12 at physical 1920x1080.
# Activate the launched player during the bounded focus wait. Do not use other apps during a take.
# Reply text and action outcomes must be reviewed in the footage; this is not a UI assertion harness.
param(
  [ValidatePattern('^[A-Za-z0-9_-]+$')][string]$Name = 'journey-01',
  [string]$Executable = (Join-Path $PSScriptRoot '../../Temp/PlayerJourneyCapture/Evergreen.exe'),
  [ValidateRange(120,300)][int]$Seconds = 150
)
$ErrorActionPreference = 'Stop'
. "$PSScriptRoot/Drive.ps1"
$root = (Resolve-Path "$PSScriptRoot/../..").Path
$raw = Join-Path $root 'trailer/raw/player-journey'
$qa = Join-Path $root 'Temp/qa_screens/teaser'
New-Item -ItemType Directory -Path $raw,$qa -Force | Out-Null
$playerLog = Join-Path $qa "$Name-player.log"
$marksFile = Join-Path $raw "$Name-marks.json"
if ((Test-Path -LiteralPath $marksFile) -or (Test-Path -LiteralPath $playerLog)) { throw 'Take name already used.' }
$marks = [System.Collections.Generic.List[object]]::new()
$clock = [Diagnostics.Stopwatch]::new()
$rec = $null
function Mark([string]$event) {
  $marks.Add([pscustomobject]@{event=$event; seconds=[Math]::Round($clock.Elapsed.TotalSeconds,3)})
  $marks | ConvertTo-Json | Set-Content -LiteralPath $marksFile -Encoding UTF8
}
function Hold([double]$seconds) {
  $endAt = $clock.Elapsed.TotalSeconds + $seconds
  while ($clock.Elapsed.TotalSeconds -lt $endAt) {
    Assert-GameFocus
    if ($rec) { $rec.Refresh(); if ($rec.HasExited) { throw 'Recorder exited before the route completed.' } }
    Start-Sleep -Milliseconds 100
  }
}
function Require-Log([string]$pattern) {
  $found = $false
  for ($attempt=0; $attempt -lt 50; $attempt++) {
    Assert-GameFocus
    if (Test-Path -LiteralPath $playerLog) {
      $log = Get-Content -LiteralPath $playerLog -Raw
      if ($log -match 'Tutor mode: Live|Error Message:|NullReferenceException') { throw 'Unsafe tutor mode or player error.' }
      if ($log -match $pattern) { $found=$true; break }
    }
    Start-Sleep -Milliseconds 200
  }
  if (-not $found) { throw "Player log did not confirm: $pattern" }
}
try {
  Start-Game -Executable $Executable -LogFile $playerLog -Supervised
  $deadline = [DateTime]::UtcNow.AddSeconds(60)
  while ([DateTime]::UtcNow -lt $deadline) {
    try { Assert-GameFocus; break } catch { Start-Sleep -Milliseconds 200 }
  }
  Assert-GameFocus
  $script:CapturePlayer.Id | Set-Content -LiteralPath (Join-Path $qa "$Name-player.pid")
  $rec = Start-Record (Join-Path $raw "$Name.mp4") $Seconds -Backend DesktopDuplication -LogFile (Join-Path $qa "$Name-ffmpeg.log")
  $clock.Start(); Mark 'recording-start'; Hold 2
  Mark 'login'; Click-At 960 788; Hold 2
  Require-Log 'Tutor mode: Demo \(-tutor-demo\)'
  Mark 'activity'; Click-At 360 555; Hold 2
  Mark 'lesson-open'; Click-At 510 510; Hold 2
  Mark 'lesson-reading'; Click-At 1510 940; Hold 4
  Click-At 402 940; Hold 0.5; Click-At 402 940; Hold 1
  Mark 'experiment-open'; Click-At 960 510; Hold 2
  Require-Log 'Only explain about the Experiment 4'
  Click-At 750 936; Hold 1
  Mark 'procedure-diagram'; Click-At 951 926; Hold 1
  Click-At 951 926; Hold 2
  Mark 'workspace'; Hold 2
  Mark 'first-drop'; Drag-From 1718 390 788 863 1000; Hold 2
  Mark 'chat-open'; Click-At 1602 54; Hold 1
  Click-At 750 765
  Mark 'question1-typing'; Type-Text 'Apakah fungsi enzim sekatan dalam eksperimen ini?' 45; Hold 0.6
  Mark 'question1-send'; Click-At 960 882; Hold 2
  Mark 'answer1-hold'; Save-Shot (Join-Path $qa "$Name-answer1.png"); Hold 14
  Click-At 750 765
  Mark 'question2-typing'; Type-Text 'Bagaimana pula fungsi DNA ligase?' 45; Hold 0.6
  Mark 'question2-send'; Click-At 960 882; Hold 2
  Mark 'answer2-hold'; Save-Shot (Join-Path $qa "$Name-answer2.png"); Hold 11
  Mark 'chat-close'; Click-At 1470 177; Hold 1
  Mark 'resume-drop'; Drag-From 1718 689 1149 863 1000; Hold 2
  Mark 'step-complete'; Save-Shot (Join-Path $qa "$Name-step-complete.png"); Hold 2
  Mark 'next-step'; Click-At 1688 1022; Hold 2
  Mark 'dashboard-return'; Click-At 153 54; Hold 2
  Click-At 360 555; Hold 1
  Mark 'quiz-open'; Click-At 1395 510; Hold 3
  Mark 'quiz-answer'; Click-At 945 743; Hold 4
  Mark 'quiz-score'; Save-Shot (Join-Path $qa "$Name-score.png"); Hold 3
  Mark 'route-complete'
  while (-not $rec.HasExited) { Assert-GameFocus; Start-Sleep -Milliseconds 100; $rec.Refresh() }
  $rec.WaitForExit()
  if ($rec.ExitCode -ne 0) { throw 'ffmpeg failed; inspect the take log.' }
} catch {
  if ($clock.IsRunning) { Mark ('REJECTED: ' + $_.Exception.Message) }
  if ($rec -and -not $rec.HasExited) { Stop-Process -Id $rec.Id -Force -ErrorAction SilentlyContinue }
  throw
} finally {
  Stop-Game
}
# Review the flushed player log and all four checkpoint stills before accepting the take.
