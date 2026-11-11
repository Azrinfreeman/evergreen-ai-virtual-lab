# Helpers for trailer takes: launch the capture build, move the cursor, click, type, record with ffmpeg.
# Dot-source this file: . .\trailer\capture\Drive.ps1
# The capture build is borderless 1920x1080 at desktop 0,0. Do not touch the mouse or keyboard during a take.

Add-Type -AssemblyName System.Windows.Forms
Add-Type @"
using System; using System.Runtime.InteropServices;
public class Win {
  [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
  [DllImport("user32.dll")] public static extern bool SetCursorPos(int x, int y);
  [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint dx, uint dy, uint d, UIntPtr e);
  [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
  [DllImport("user32.dll")] public static extern bool SetWindowPos(IntPtr h, IntPtr a, int x, int y, int cx, int cy, uint f);
  [DllImport("user32.dll")] public static extern bool GetCursorPos(out POINT p);
  public struct POINT { public int X, Y; }
}
"@
[Win]::SetProcessDPIAware() | Out-Null

$script:GameExe = Join-Path $PSScriptRoot '..\..\Builds\TrailerCapture\Evergreen.exe'

function Start-Game {
  Stop-Process -Name Evergreen -Force -ErrorAction SilentlyContinue
  Start-Process -FilePath $script:GameExe -ArgumentList '-popupwindow','-screen-width','1920','-screen-height','1080','-screen-fullscreen','0'
  $p = $null
  for ($i = 0; $i -lt 40 -and -not $p; $i++) { Start-Sleep -Milliseconds 500; $p = Get-Process Evergreen -ErrorAction SilentlyContinue | ? { $_.MainWindowHandle -ne 0 } }
  Start-Sleep -Seconds 4
  [Win]::SetWindowPos($p.MainWindowHandle, [IntPtr](-1), 0, 0, 1920, 1080, 0x0040) | Out-Null
  [Win]::SetForegroundWindow($p.MainWindowHandle) | Out-Null
  Start-Sleep -Seconds 1
}

function Stop-Game { Stop-Process -Name Evergreen -Force -ErrorAction SilentlyContinue }

# Smooth cursor move in `ms` milliseconds with ease-in-out.
function Move-Mouse([int]$x, [int]$y, [int]$ms = 600) {
  $s = New-Object Win+POINT; [Win]::GetCursorPos([ref]$s) | Out-Null
  $steps = [Math]::Max(8, [int]($ms / 16))
  for ($i = 1; $i -le $steps; $i++) {
    $t = $i / $steps; $e = $t * $t * (3 - 2 * $t)
    [Win]::SetCursorPos([int]($s.X + ($x - $s.X) * $e), [int]($s.Y + ($y - $s.Y) * $e)) | Out-Null
    Start-Sleep -Milliseconds 16
  }
}

function Click-At([int]$x, [int]$y, [int]$ms = 600) {
  Move-Mouse $x $y $ms
  Start-Sleep -Milliseconds 120
  [Win]::mouse_event(0x0002, 0, 0, 0, [UIntPtr]::Zero); Start-Sleep -Milliseconds 70
  [Win]::mouse_event(0x0004, 0, 0, 0, [UIntPtr]::Zero)
}

# Types text one character at a time. Avoid + ^ % ~ ( ) { } [ ] in text (SendKeys specials).
function Type-Text([string]$text, [int]$delayMs = 55) {
  foreach ($c in $text.ToCharArray()) { [System.Windows.Forms.SendKeys]::SendWait([string]$c); Start-Sleep -Milliseconds $delayMs }
}

# Records the 1920x1080 region at desktop 0,0 for $seconds at 60 fps. Returns the ffmpeg process.
function Start-Record([string]$outFile, [int]$seconds) {
  $args = @('-hide_banner','-loglevel','error','-y','-f','gdigrab','-framerate','60','-offset_x','0','-offset_y','0','-video_size','1920x1080','-draw_mouse','1','-i','desktop','-t',"$seconds",'-c:v','libx264','-preset','veryfast','-crf','14','-pix_fmt','yuv420p',$outFile)
  Start-Process -FilePath ffmpeg -ArgumentList $args -PassThru -WindowStyle Hidden
}

function Save-Shot([string]$outFile) {
  ffmpeg -hide_banner -loglevel error -y -f gdigrab -framerate 5 -offset_x 0 -offset_y 0 -video_size 1920x1080 -i desktop -frames:v 1 $outFile
}

# Press at (x1,y1), glide to (x2,y2), release. Unity needs intermediate moves while the button is down.
function Drag-From([int]$x1, [int]$y1, [int]$x2, [int]$y2, [int]$ms = 900) {
  Move-Mouse $x1 $y1 500
  Start-Sleep -Milliseconds 150
  [Win]::mouse_event(0x0002, 0, 0, 0, [UIntPtr]::Zero); Start-Sleep -Milliseconds 120
  Move-Mouse $x2 $y2 $ms
  Start-Sleep -Milliseconds 150
  [Win]::mouse_event(0x0004, 0, 0, 0, [UIntPtr]::Zero)
}
