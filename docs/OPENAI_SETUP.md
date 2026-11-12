# OpenAI API key setup (AI tutor)

The AI tutor (`Assets/Samples/OpenAI Unity/0.2.2/ChatGPT/ChatGPT.cs`) never stores a key in the project. It looks for one in this order:

1. The `OPENAI_API_KEY` environment variable.
2. `auth.json` in your user profile (the OpenAI-Unity package default).

Without a key the tutor falls back to fixed sample answers (see "Run without a key" below).

## Run without a key

The `Mode` field on the `ChatGPT` component (prefab `Assets/prefabs/ChatGPT.prefab`) has three values:

| Mode | Behaviour |
|---|---|
| `Auto` (default) | Live tutor if `OPENAI_API_KEY` is set or `auth.json` exists, otherwise fixed sample answers |
| `Live` | Always call OpenAI |
| `Demo` | Always use the fixed sample answers; no network call |

Sample answers are fixed text from the `DemoTutorScript` asset, matched to the question by keywords. They do not come from an AI model. After each one the status line shows the notice set on that asset. The Console logs one line at startup: `Tutor mode: Live` or `Tutor mode: Demo (...)`. Full guide: [tutorial/tutor-demo-mode.md](tutorial/tutor-demo-mode.md).

## Option A: environment variable (recommended)

Windows (PowerShell):
```powershell
setx OPENAI_API_KEY "sk-..."
```
macOS / Linux:
```bash
echo 'export OPENAI_API_KEY="sk-..."' >> ~/.zshrc
```
**Fully quit and restart Unity (and Unity Hub)** so it picks up the variable. Builds read the variable of the machine they run on.

## Option B: auth.json

Create `%USERPROFILE%\.openai\auth.json` (Windows) or `~/.openai/auth.json`:
```json
{
  "api_key": "sk-...",
  "organization": "org-..."
}
```
`organization` is optional.

## Rules
- Never commit a key. `.openai/`, `auth.json` and `.env*` are gitignored.
- If a key is ever committed or pasted somewhere public, revoke it at <https://platform.openai.com/api-keys> and create a new one.
- For a shipped build, do not embed a key in the client. Route requests through your own backend that holds the key.
- Release player builds (not Development, not the Editor) never call OpenAI directly: the `ChatGPT` component's `Allow Live In Release` option is off by default, so the tutor always uses the scripted replies even if `OPENAI_API_KEY` is set on the learner's machine. Turn it on only for a build that talks to your own backend. If no `DemoTutorScript` is assigned, a release build disables the tutor instead of going live. Development builds and the Editor behave as described above.
- GitHub secret scanning and push protection are enabled on the repository (2026-11-12), so a pushed key-shaped string is flagged and blocked.

## Model
The model ID is the `model` field on the `ChatGPT` component in the Inspector (default `gpt-6-luna`). The API account needs access to the model you choose.
