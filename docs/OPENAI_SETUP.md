# OpenAI API key setup (AI tutor)

The AI tutor (`Assets/Samples/OpenAI Unity/0.2.2/ChatGPT/ChatGPT.cs`) never stores a key in the project. It looks for one in this order:

1. The `OPENAI_API_KEY` environment variable.
2. `auth.json` in your user profile (the OpenAI-Unity package default).

Without a key the app still runs; the tutor shows "Jawapan tidak dapat dimuatkan. Cuba lagi."

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

## Model
The model ID is the `model` field on the `ChatGPT` component in the Inspector (default `gpt-6-luna`). The API account needs access to the model you choose.
