# Trailer publishing

The owner chose repository delivery on 2026-11-09. `trailer/out/trailer.mp4`, `readme-loop.mp4` and `trailer-poster.png` are unignored and linked from the root `README.md`. Raw footage, previews and QA output remain ignored. YouTube or a GitHub Release is optional; the copy below is ready if an external version is wanted later.

## YouTube copy

**Title:** Evergreen STEM Virtual Lab | Unity Science Lab + OpenAI Tutor

**Description:**

```text
A Malay-language 2D science lab built in Unity: seven activities, guided lessons, drag-and-drop procedures, quizzes and an optional topic-aware science tutor.

The tutor sends the selected activity's topic prompt and the student's question to OpenAI chat completions from Unity C#. It does not see the student's screen or experiment progress.

Project: https://github.com/Azrinfreeman/evergreen-ai-virtual-lab

Chapters
0:00 Activities
0:05 Guided lessons
0:10 Lab procedure
0:20 Quiz
0:27 OpenAI tutor
0:40 How the tutor gets its context
0:45 Project link

Trailer disclosure: the tutor exchanges shown are scripted for this trailer; the app uses OpenAI with your own key. The configured model ID is gpt-6-luna, confirmed by the project owner.

OpenAI wordmark is a trademark of OpenAI.

#Unity #STEM #OpenAI #ScienceEducation #BahasaMalaysia
```

## GitHub Release alternative

With GitHub CLI installed and authenticated:

```powershell
gh release create trailer-v1 `
  trailer/out/trailer.mp4 `
  trailer/out/readme-loop.mp4 `
  --repo Azrinfreeman/evergreen-ai-virtual-lab `
  --title "Evergreen STEM Virtual Lab trailer" `
  --notes "50-second 1080p60 trailer and a 10-second silent README loop. Tutor exchanges are scripted for the trailer; the app uses OpenAI with your own key."
```

## README delivery

The root `README.md` contains a clickable poster linked to `trailer/out/trailer.mp4` and a separate link to `trailer/out/readme-loop.mp4`. The scripted-exchange disclosure appears below the links. These paths are relative, so they work both locally and on GitHub once the files are committed and pushed.

## Checks before publishing

The owner approved the final review, Malay wording, curriculum imagery and `gpt-6-luna` model ID on 2026-11-09. No independent API test was run. For future edits, repeat these checks:

- Watch the final video with sound and with the sound off. Automated checks here used stills, contact sheets, media metadata and loudness measurements, not playback.
- Confirm that the OpenAI account serves the configured `gpt-6-luna` model ID.
- Confirm any Malay wording edits listed in `docs/tutorial/tutor-demo-mode.md` against the curriculum.
- Confirm that the lesson imagery and curriculum text may appear in a public video.
- Keep the scripted-exchange disclosure in the video description even though it also appears on the end card and loop.

## Logo source

The end-card wordmark is the first frame of the official `OpenAI_Wordmark_Gif.gif` linked from <https://openai.com/brand>. It is scaled but not redrawn, recoloured or cropped. The source files are in `trailer/edit/public/`; regenerate that ignored folder from `trailer/raw/` and the source URL if it is deleted.
