# Player-journey teaser — take record

Recorded 2026-11-12 from local commit `5ec7015455aaa2c83ebe96c883bb8acfa5d88e7f`, with the existing unrelated workspace changes preserved. This is the app's **scripted demo tutor**, not a live OpenAI exchange. No model was used for either reply.

## Build and route

- Player: `Temp/PlayerJourneyCapture/Evergreen.exe`, Development, Unity 6000.6.7f1, launched with `-tutor-demo`.
- Assembly-CSharp.dll SHA-256: `53193FA97ABCDFF55281357664DEB469C8EB552735F70BCFD93D65119EA47893`.
- Topic: `MM4`, selected through Aktiviti 4 → Mula eksperimen. Player log contains the matching cloning prompt and `Tutor mode: Demo (-tutor-demo)`, with no `Tutor mode: Live`, API error or exception lines found.
- Actual route: LoginPage → Dashboard → Aktiviti 4 → lesson pages 1–2 → back to hub → experiment introduction, diagram and safety page → step 1 → first label → both chat questions → close chat → second label → Langkah selesai → step 2 → Dashboard → Aktiviti 4 → Uji Diri 4 → question 1 answer → score 10 and question 2.
- Both pending states greyed out input/send; both completed replies restored them. The demo notice remained visible. Both replies fit the visible chat viewport without scrolling.

## Exact exchange

Question 1: **Apakah fungsi enzim sekatan dalam eksperimen ini?**

> Enzim sekatan (XbaI dan HindIII dalam makmal ini) memotong DNA pada tapak tertentu. Plasmid dan sisipan GFP dicerna dahulu sebelum disambungkan. Setiap enzim digunakan sebanyak 1 µL bersama penampan sekatan 10X.

Question 2: **Bagaimana pula fungsi DNA ligase?**

> Pengikatan (ligasi) menyambung sisipan GFP ke dalam plasmid yang telah dicerna. Makmal ini menggunakan T4 DNA Ligase 1 µL dan penampan ligase 10X 2 µL.

Visible status after each reply: **Mod demo: jawapan contoh, bukan daripada model AI.**

Authoritative reply source: `Assets/Samples/OpenAI Unity/0.2.2/ChatGPT/Demo/DemoTutorScript.asset`, topic `MM4`. The quantities and procedure references are preserved from that asset; footage does not rewrite or composite the answers. Editorial holds are 13 seconds for answer 1 and 10 seconds for answer 2; at 31 and 25 whitespace-separated words respectively, both stay below three words per second.

## Source and selected marks

Original: `trailer/raw/player-journey/journey-01.mp4`. Staged copy: `trailer/edit/public/player-journey/journey-01.mp4`. Original event marks: `trailer/raw/player-journey/journey-01-marks.json`. Timing below is in seconds from the capture script's stopwatch; encoder startup can shift the first video frame slightly. The composition's reviewed source in/out points are authoritative for the edit.

| Event | Mark |
|---|---:|
| Login click | 2.095 |
| Aktiviti 4 | 5.567 |
| Lesson reading page | 12.480 |
| Experiment route | 22.498 |
| Step 1 workspace | 34.446 |
| First label drag | 36.527 |
| Open chat | 41.902 |
| First question typing / send | 45.750 / 50.338 |
| First answer checkpoint | 53.793 |
| Follow-up typing / send | 69.806 / 73.181 |
| Second answer checkpoint | 76.610 |
| Close chat / resume label drag | 88.187 / 90.599 |
| Step complete / advance | 96.100 / 98.664 |
| Open quiz / answer | 108.087 / 112.519 |
| Score checkpoint / route complete | 118.091 / 121.620 |

The selected source slices, captions and framing live in `TEASER_SHOTS` in `trailer/edit/src/PlayerJourneyTeaser.tsx`. All important actions play at normal speed. Navigation gaps are visible cuts; neither pending response interval is accelerated. The opening apparatus shot is a preview of the later interaction.

## Capture QA and limits

Source metadata: H.264/yuv420p, 1920 × 1080, 150.000 seconds, 9,000 output frames at 60 fps. Desktop Duplication replaced the slower GDI capture for this take. ffmpeg reports **96 duplicated and 2 dropped frames** during timestamp resampling; this is a 60 fps export, not a native 60 fps simulation claim. Four pixels are cropped from each desktop edge and the remaining image scaled to 1080p to exclude the Windows capture outline. No app panel, reply or notice is replaced.

The take script initially appended a rejection marker because Windows PowerShell returned a null process exit code without a retained handle. The ffmpeg log ends normally at 9,000 frames, ffprobe confirms the complete file and a full decode to null completed without errors. All four checkpoint stills and a 5-second contact sheet were inspected. The take is recovered for editing based on that evidence; the original rejection marker remains in the marks file. `Drive.ps1` now retains the process handle and the take script waits for process completion; a separate retained-handle check returned exit code 0. A complete second take with that exit-code fix has not been run.

Logs/checkpoints are local under `Temp/qa_screens/teaser/`: `journey-01-player.log`, `journey-01-ffmpeg.log`, `journey-01-answer1.png`, `journey-01-answer2.png`, `journey-01-step-complete.png`, `journey-01-score.png`, `journey-01-sheet.png`. The raw checkpoint stills include the OS outline; the encoded video excludes it. The player closed gracefully; no unrelated player or Editor was stopped.

Earlier experiments: the GDI smoke produced 382 frames/10 seconds; Desktop Duplication with software encoding produced 582 without CFR; direct BGRA AMF encoding failed with `SubmitInput() failed with error 18`; conversion to NV12 worked but produced 583 frames/10 seconds. The selected take uses the simpler software encoder and explicit CFR resampling.

## Rebuild and edit

Run `trailer/capture/Take-PlayerJourney.ps1` with a **new** `-Name`; it refuses existing take/log paths. Its coordinates are physical 1920 × 1080 desktop pixels. The script needs supervised focus activation and review of actual UI output; its checkpoints are not automated assertions of reply text or score. It stops recording on focus loss during monitored waits/input and preserves only its own player process ownership.

Copy the accepted MP4 to `trailer/edit/public/player-journey/journey-01.mp4`. From `trailer/edit/`:

```powershell
node scripts/gen-score.mjs public/player-journey/score.wav 30
npm run studio
npm run teaser:preview
npm run teaser:preview:master
npm run teaser
npm run teaser:master
```

The render commands produce `*-source.mp4`; the following `*:master` command performs two-pass loudness normalization and packages limited-range H.264/yuv420p with AAC audio. The mastering script refuses existing outputs; use a new output name when retaining another cut. Measurements are saved beside each mastered MP4 as `.loudness.json`. Its duration follows the measured video stream, avoiding extra loudness-filter tail audio.

The music and composition are editable sources; MP4 files and poster images are exports. The new media remains ignored. Existing `Trailer`, `ReadmeLoop`, their outputs, README links and release files are preserved. Full uninterrupted sound-on/muted playback review must be recorded in the implementation plan before calling the teaser finished.
