# Helpers for trailer takes: launch the capture build, move the cursor, click, type, record with ffmpeg.
# Dot-source this file: . .\trailer\capture\Drive.ps1
# The capture build is borderless 1920x1080 at desktop 0,0. Do not touch the mouse or keyboard during a take.

Add-Type -AssemblyName System.Windows.Forms
if (-not ('Win' -as [type])) { Add-Type @"
using System; using System.Runtime.InteropServices;
public class Win {
  [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
  [DllImport("user32.dll")] public static extern bool SetCursorPos(int x, int y);
  [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint dx, uint dy, uint d, UIntPtr e);
  [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
  [DllImport("user32.dll")] public static extern bool SetWindowPos(IntPtr h, IntPtr a, int x, int y, int cx, int cy, uint f);
  [DllImport("user32.dll")] public static extern bool GetCursorPos(out POINT p);
  [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
  [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr h, out RECT r);
  public struct POINT { public int X, Y; }
  public struct RECT { public int Left, Top, Right, Bottom; }
}
"@
}
[Win]::SetProcessDPIAware() | Out-Null

$script:GameExe = Join-Path $PSScriptRoot '..\..\Builds\TrailerCapture\Evergreen.exe'

function Start-Game([string]$Executable = $script:GameExe, [string]$LogFile, [switch]$Supervised) {
  if ($script:CapturePlayer -and -not $script:CapturePlayer.HasExited) { throw 'A capture player is already owned by this session.' }
  if (-not (Test-Path -LiteralPath $Executable -PathType Leaf)) { throw "Player not found: $Executable" }
  # -tutor-demo forces the scripted tutor replies, so a take never calls OpenAI even if a key exists.
  $launchArgs = @('-popupwindow','-screen-width','1920','-screen-height','1080','-screen-fullscreen','0','-tutor-demo')
  # The child process also never sees a key from this shell.
  Remove-Item Env:\OPENAI_API_KEY -ErrorAction SilentlyContinue
  if ($LogFile) { $launchArgs += @('-logFile', ('"' + $LogFile + '"')) }
  # The player is the visible interactive subject of the recording, not a background helper.
  $script:CapturePlayer = Start-Process -FilePath $Executable -ArgumentList $launchArgs -PassThru -WindowStyle Normal
  $p = $script:CapturePlayer
  for ($i = 0; $i -lt 60; $i++) {
    $p.Refresh()
    if ($p.HasExited) { throw 'Capture player exited before its window became available.' }
    if ($p.MainWindowHandle -ne [IntPtr]::Zero) { break }
    Start-Sleep -Milliseconds 500
  }
  if ($p.MainWindowHandle -eq [IntPtr]::Zero) { Stop-Game; throw 'Capture player window timed out after 30 seconds.' }
  # Allow Unity's splash window to give way to the real player, then refresh its handle.
  Start-Sleep -Seconds 4
  for ($attempt = 0; $attempt -lt 10; $attempt++) {
    $p.Refresh()
    if ($p.HasExited) { throw 'Capture player exited during startup.' }
    [Win]::SetWindowPos($p.MainWindowHandle, [IntPtr](-1), 0, 0, 1920, 1080, 0x0040) | Out-Null
    [Win]::SetForegroundWindow($p.MainWindowHandle) | Out-Null
    Start-Sleep -Milliseconds 500
    if ([Win]::GetForegroundWindow() -eq $p.MainWindowHandle) { break }
  }
  # Supervised rehearsal may activate the window using the desktop tool; input guards remain active.
  if (-not $Supervised) { try { Assert-GameFocus } catch { Stop-Game; throw } }
}

function Stop-Game {
  if ($script:CapturePlayer -and -not $script:CapturePlayer.HasExited) {
    Stop-Process -Id $script:CapturePlayer.Id -Force -ErrorAction SilentlyContinue
    if (-not $script:CapturePlayer.WaitForExit(5000)) { throw 'Owned capture player did not stop within five seconds.' }
  }
  $script:CapturePlayer = $null
}

function Assert-GameFocus {
  if (-not $script:CapturePlayer) { throw 'No capture player is owned by this session.' }
  $script:CapturePlayer.Refresh()
  if ($script:CapturePlayer.HasExited) { throw 'Capture player exited.' }
  if ([Win]::GetForegroundWindow() -ne $script:CapturePlayer.MainWindowHandle) { throw 'Capture player lost focus; aborting input.' }
  $rect = New-Object Win+RECT
  if (-not [Win]::GetWindowRect($script:CapturePlayer.MainWindowHandle, [ref]$rect) -or
      $rect.Left -ne 0 -or $rect.Top -ne 0 -or $rect.Right -ne 1920 -or $rect.Bottom -ne 1080) {
    throw 'Capture requires the player window at desktop 0,0 with size 1920x1080.'
  }
}

# Smooth cursor move in `ms` milliseconds with ease-in-out.
function Move-Mouse([int]$x, [int]$y, [int]$ms = 600) {
  Assert-GameFocus
  $s = New-Object Win+POINT; [Win]::GetCursorPos([ref]$s) | Out-Null
  $steps = [Math]::Max(8, [int]($ms / 16))
  for ($i = 1; $i -le $steps; $i++) {
    Assert-GameFocus
    $t = $i / $steps; $e = $t * $t * (3 - 2 * $t)
    [Win]::SetCursorPos([int]($s.X + ($x - $s.X) * $e), [int]($s.Y + ($y - $s.Y) * $e)) | Out-Null
    Start-Sleep -Milliseconds 16
  }
}

function Click-At([int]$x, [int]$y, [int]$ms = 600) {
  Move-Mouse $x $y $ms
  Start-Sleep -Milliseconds 120
  Assert-GameFocus
  [Win]::mouse_event(0x0002, 0, 0, 0, [UIntPtr]::Zero)
  try { Start-Sleep -Milliseconds 70 } finally {
    [Win]::mouse_event(0x0004, 0, 0, 0, [UIntPtr]::Zero)
  }
}

# Types text one character at a time. Avoid + ^ % ~ ( ) { } [ ] in text (SendKeys specials).
function Type-Text([string]$text, [int]$delayMs = 55) {
  foreach ($c in $text.ToCharArray()) {
    Assert-GameFocus
    $key = if ('+^%~(){}[]'.Contains([string]$c)) { '{' + $c + '}' } else { [string]$c }
    [System.Windows.Forms.SendKeys]::SendWait($key)
    Start-Sleep -Milliseconds $delayMs
  }
}

# Records the 1920x1080 region at desktop 0,0 for $seconds at 60 fps. Returns the ffmpeg process.
function Start-Record([string]$outFile, [int]$seconds) {
  Assert-GameFocus
  if (Test-Path -LiteralPath $outFile) { throw "Recording already exists: $outFile" }
  $parent = Split-Path -Parent $outFile
  if ($parent) { New-Item -ItemType Directory -Path $parent -Force | Out-Null }
  $args = @('-hide_banner','-loglevel','error','-n','-f','gdigrab','-framerate','60','-offset_x','0','-offset_y','0','-video_size','1920x1080','-draw_mouse','1','-i','desktop','-t',"$seconds",'-c:v','libx264','-preset','veryfast','-crf','14','-pix_fmt','yuv420p',('"' + $outFile + '"'))
  Start-Process -FilePath ffmpeg -ArgumentList $args -PassThru -WindowStyle Hidden
}

function Save-Shot([string]$outFile) {
  Assert-GameFocus
  ffmpeg -hide_banner -loglevel error -y -f gdigrab -framerate 5 -offset_x 0 -offset_y 0 -video_size 1920x1080 -i desktop -frames:v 1 $outFile
}

# Press at (x1,y1), glide to (x2,y2), release. Unity needs intermediate moves while the button is down.
function Drag-From([int]$x1, [int]$y1, [int]$x2, [int]$y2, [int]$ms = 900) {
  Move-Mouse $x1 $y1 500
  Start-Sleep -Milliseconds 150
  [Win]::mouse_event(0x0002, 0, 0, 0, [UIntPtr]::Zero); Start-Sleep -Milliseconds 120
  try {
    Move-Mouse $x2 $y2 $ms
    Start-Sleep -Milliseconds 150
  } finally {
    [Win]::mouse_event(0x0004, 0, 0, 0, [UIntPtr]::Zero)
  }
}
