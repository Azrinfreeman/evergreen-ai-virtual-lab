# One tutor take in akt4 (topic MM4). Usage:
#   .\trailer\capture\Take-Tutor.ps1 -Name tutor_on_1 -Question "Apakah fungsi enzim sekatan?"
# Do not touch the mouse or keyboard while it runs (about 40 s).
param([string]$Name, [string]$Question, [int]$Seconds = 40)
. "$PSScriptRoot\Drive.ps1"
$root = Resolve-Path "$PSScriptRoot\..\.."
Start-Game
$rec = Start-Record "$root\trailer\raw\$Name.mp4" $Seconds
Start-Sleep 1
Click-At 960 788 700; Start-Sleep 3          # Masuk makmal
Click-At 370 570 800; Start-Sleep 2.5        # Aktiviti 4
Click-At 960 504 700; Start-Sleep 3.5        # Mula eksperimen
Click-At 1603 55 700; Start-Sleep 1.5        # chat icon
Click-At 960 770 600; Start-Sleep 0.4
Type-Text $Question 60; Start-Sleep 0.8
Click-At 960 882 500                         # Hantar
Start-Sleep 5                                # thinking + reply
Save-Shot "$root\Temp\qa_screens\take_$Name.png"
Start-Sleep 3
Click-At 1470 178 600; Start-Sleep 2         # Tutup
$rec.WaitForExit(($Seconds + 10) * 1000) | Out-Null
Stop-Game
"$Name done"
