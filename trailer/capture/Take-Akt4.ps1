# Take 2.2: akt4 drag-and-drop (Langkah 1, label the two tubes). Usage:
#   .\trailer\capture\Take-Akt4.ps1 -Name akt4_drag_1
# Do not touch the mouse or keyboard while it runs (about 45 s).
param([string]$Name = 'akt4_drag_1', [int]$Seconds = 45)
. "$PSScriptRoot\Drive.ps1"
$root = Resolve-Path "$PSScriptRoot\..\.."
Start-Game
$rec = Start-Record "$root\trailer\raw\$Name.mp4" $Seconds
Start-Sleep 1
Click-At 960 788 700; Start-Sleep 3          # Masuk makmal
Click-At 370 570 800; Start-Sleep 2.5        # Aktiviti 4
Click-At 960 504 700; Start-Sleep 3          # Mula eksperimen (menu)
Click-At 760 936 700; Start-Sleep 2.5        # Mula eksperimen (intro page)
Click-At 952 928 600; Start-Sleep 2.5        # Seterusnya: safety rules
Click-At 952 928 600; Start-Sleep 2          # Seterusnya
Click-At 952 928 600; Start-Sleep 2.5        # Seterusnya: Langkah 1
Save-Shot "$root\Temp\qa_screens\take_${Name}_a.png"
Drag-From 1718 391 789 862 1000; Start-Sleep 1.5      # Plasmid -> left tube
Drag-From 1718 689 1149 862 1000; Start-Sleep 2       # Sisipan GFP -> right tube
Save-Shot "$root\Temp\qa_screens\take_${Name}_b.png"
Start-Sleep 3
$rec.WaitForExit(($Seconds + 10) * 1000) | Out-Null
Stop-Game
"$Name done"
