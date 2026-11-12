# Tutor takes (recorded 2026-11-09)

Source of every reply: scripted (`DemoTutorScript.asset`), capture build with `Mode = Demo`, `Hide Demo Notice = true`. Topic: MM4 (Aktiviti 4, "Mula eksperimen"). Not live model output. Each take starts a fresh game and runs `trailer/capture/Take-Tutor.ps1`.

| Take | File | Question typed | Reply kind | Reply shown |
|---|---|---|---|---|
| on_1 | `trailer/raw/tutor_on_1.mp4` | Apakah fungsi enzim sekatan? | answer | Enzim sekatan (Xbal dan HindIII dalam makmal ini) memotong DNA pada tapak tertentu. Plasmid dan sisipan GFP dicerna dahulu sebelum disambungkan. Setiap enzim digunakan sebanyak 1 µL bersama penampan sekatan 10X. |
| on_2 | `tutor_on_2.mp4` | Apakah itu GFP? | answer | Dalam eksperimen ini, gen Protein Pendarfluor Hijau (GFP) dimasukkan ke dalam vektor plasmid pUC19. Langkahnya ialah pencernaan enzim sekatan, pengikatan, transformasi dan pengesahan oleh PCR. |
| on_3 | `tutor_on_3.mp4` | Kenapa plat agar perlu ampicillin? | answer (weak) | Campuran disebarkan ke atas plat agar LB dengan ampicillin, kemudian diinkubasi semalaman pada suhu 37 °C. Selepas itu koloni disaring dalam langkah Saringan Koloni. |
| off_1 | `tutor_off_1.mp4` | Siapa menang piala dunia? | decline | Maaf, saya hanya boleh menjawab soalan berkaitan sains. Cuba tanya tentang aktiviti atau eksperimen yang sedang dipilih. |
| off_2 | `tutor_off_2.mp4` | Apakah lagu artis paling popular? | decline | (same decline) |
| off_3 | `tutor_off_3.mp4` | Berapa harga resipi nasi lemak? | decline | (same decline) |

Screenshots of each reply: `Temp/qa_screens/take_<name>.png`.

## Notes for the edit
- on_3 does not answer "why"; the scenes do not state why ampicillin is used. Prefer on_1 and on_2 for the film, or ask a question the script answers ("Apakah yang berlaku pada plat agar?").
- The reply text says "Xbal" (as the scene does), not "XbaI". Owner decision pending.
- The status line under the reply reads "Semak jawapan bersama bahan pembelajaran." (the notice is hidden).
- Captions must not call these exchanges live, real-time or unedited. End card carries the disclosure line.

# Other takes (recorded 2026-11-09, same capture build)

| Take | File (`trailer/raw/`) | What it shows | Script |
|---|---|---|---|
| 2.1 | `dash_1.mp4` | Login, dashboard, cursor across the Aktiviti cards (hover state on Aktiviti 4) | inline in session; positions in `Drive.ps1` notes |
| 2.2 | `akt4_drag_1.mp4`, `akt4_drag_2.mp4` | akt4 Langkah 1: Plasmid onto the left tube slot, Sisipan GFP onto the right; ticks, large check, "Seterusnya" appears | `Take-Akt4.ps1` |
| 2.3 | `quiz_1.mp4`, `quiz_2.mp4` | Uji Diri 4, Soalan 1, correct answer (third option) clicked | inline in session |

Observations for the edit:
- Aktiviti 1 card stays cyan on the dashboard (it holds focus); it can read as a selection. Hover on other cards is a pale blue.
- akt4 slots do not check which label goes on which tube; the take pairs them correctly by hand.
- Quiz, correct answer: mascot happy, option turns green with "Betul:", but the other options tint pink and the line "Jawapan betul ditandakan di bawah." still shows, and the coin counter reads 0 at 1.8 s. These come from existing quiz code (`showRealAnswer`); not changed. Consider cropping or trimming that moment, or tell the owner.
- The quiz scene is the old style (not part of the akt3-akt7 restyle).
