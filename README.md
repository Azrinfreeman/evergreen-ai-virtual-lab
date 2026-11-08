# Evergreen STEM VirtualLab

A 2D Unity virtual science lab for secondary-school students, written in **Bahasa Malaysia** (KPS curriculum). Students work through guided lessons, run drag-and-drop lab experiments, test themselves with quizzes, and ask an **AI science tutor** questions at any point.

![Unity](https://img.shields.io/badge/Unity-6-black?logo=unity) ![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Android-blue) ![AI](https://img.shields.io/badge/AI-OpenAI%20%2B%20Claude%20Code-green)

## What it does

| Area | Details |
|---|---|
| **Dashboard** | Seven activities (Aktiviti 1–7), each with theory pages, an experiment and a quiz. |
| **Experiments** | Five interactive labs (`akt3`–`akt7`) such as DNA cloning (restriction digest, ligation, transformation), microbial fuel cells and rubber-film technology. Apparatus is dragged onto targets; each step checks the action before moving on. |
| **Quizzes** | Seven *Uji Diri* quizzes with scoring, feedback and retry. The maximum score comes from each quiz's real question count. |
| **AI tutor** | A floating "Pembantu pembelajaran" chat assistant, available on every screen. |

Flow: `LoginPage` → `Dashboard` → `Experiments/aktN` or `Quiz/Uji Diri N` → back to `Dashboard`.

## AI in this project

### 1. In-app AI tutor (OpenAI)
- Built on the [OpenAI-Unity](https://github.com/srcnalt/OpenAI-Unity) package (`Assets/Samples/OpenAI Unity/0.2.2/ChatGPT/ChatGPT.cs`).
- **Context-aware system prompts**: the assistant picks a prompt per screen (Dashboard, Akt1–Akt7, MM3–MM7) so answers stay on the topic the student is studying.
- **Scoped behaviour**: the tutor answers only science questions and stays in character as a scientist.
- Async request flow with a Malay status line ("Sedang menyediakan jawapan…"), error and empty-response handling, and input locking while a reply is pending.
- The assistant lives in a persistent `DontDestroyOnLoad` canvas, so it survives every scene change without duplicating.
- **API key handling**: the key is read from the `OPENAI_API_KEY` environment variable, falling back to `~/.openai/auth.json`. No key is stored in the project. See [`docs/OPENAI_SETUP.md`](docs/OPENAI_SETUP.md).

### 2. AI-assisted development workflow (Claude Code + Unity MCP)
The project was built and revamped with AI coding agents driving the Unity Editor directly through the [Unity MCP](https://github.com/CoplayDev/unity-mcp) bridge:
- Scenes, prefabs and UI were inspected and edited programmatically (hierarchy queries, component edits, play-mode screenshots).
- A **QA audit** of all 14 scenes was run by agents: static scans plus play-mode captures, with findings in [`docs/ai/qa-report.md`](docs/ai/qa-report.md).
- A **science-themed UI revamp** (Dashboard, akt4 and onward) is documented in [`IMPLEMENTATION.md`](IMPLEMENTATION.md), including the design spec, verification matrix and per-step evidence.
- Agent guidance and project memory live in [`CLAUDE.md`](CLAUDE.md), [`AGENTS.md`](AGENTS.md), `.claude/skills/` and `.claude/agents/`, and the docs under [`docs/ai/`](docs/ai/) (script map, scene map, decisions, project state).

## Tech stack
- Unity 6 (6000.x), uGUI + TextMeshPro, 2D physics for drag/drop triggers
- C# (`DragController`, `ItemSlot`, `NextExperimentController`, `QuestionController`, `MenuSlideController`, …)
- OpenAI-Unity (chat completions), Unity MCP (CoplayDev)

## Getting started
1. Install **Unity 6** (see `ProjectSettings/ProjectVersion.txt` for the exact version) and open this folder.
2. Open `Assets/Scenes/LoginPage.unity` and press Play.
3. Optional, for the AI tutor: set your key before launching the Editor.
   ```powershell
   setx OPENAI_API_KEY "<your key>"
   ```
   Restart Unity afterwards. Without a key the tutor reports an error and the rest of the app still works. Full guide: [`docs/OPENAI_SETUP.md`](docs/OPENAI_SETUP.md).
4. Experiments and quizzes can be opened directly from `Assets/Scenes/Experiments/` and `Assets/Scenes/Quiz/`. The persistent assistant is created by the Dashboard, so enter through it to see the tutor.

## Project layout
```
Assets/
  Scenes/            LoginPage, Dashboard, Experiments/akt3–7, Quiz/Uji Diri 1–7
  UI/Dashboard/      Dashboard artwork
  UI/Science/        Science-theme UI components (focus outline and focus scope)
  prefabs/           Apparatus, chat box and UI prefabs
  Samples/OpenAI Unity/   AI tutor
docs/ai/             Script map, scene map, QA report, decisions, project state
IMPLEMENTATION.md    Science UI revamp plan and progress log
```

## Status
Active development on the science-themed UI revamp. The Dashboard and the first akt4 steps are restyled; the remaining experiments and quizzes are still to do. See [`IMPLEMENTATION.md`](IMPLEMENTATION.md) for what is verified and what is open.

## Author
Azrin Freeman
