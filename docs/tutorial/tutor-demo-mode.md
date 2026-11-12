# Tutor without an OpenAI key (scripted replies)

The tutor chat normally needs an OpenAI key (see [OPENAI_SETUP.md](../OPENAI_SETUP.md)). Without one, it answers from a fixed script so the whole chat flow can still be run and tested.

These replies are written text, not model output. The tutor knows only the topic of the card you clicked (`CheckBehaviour.BehaviourName`), not what is on screen.

## Files

| Path (under `Assets/Samples/OpenAI Unity/0.2.2/ChatGPT/`) | Role |
|---|---|
| `ChatGPT.cs` | `Mode`, `Demo Script` and `Hide Demo Notice` fields; picks live or scripted at `Start` |
| `DemoTutorProvider.cs` | Key check (never reads the key) and the scripted response |
| `Demo/DemoTutorScript.cs` + `DemoTutorScript.asset` | The editable answers |
| `Demo/DemoTutorMatcher.cs` | Keyword matching, no network |
| `Demo/Tests/DemoTutorMatcherTests.cs` | 10 Edit Mode tests |

The script asset is assigned on `Assets/prefabs/ChatGPT.prefab`.

## How a question is answered

1. Lowercase the question, drop punctuation.
2. In the active topic, the entry with the most keyword hits wins (a tie goes to the earlier entry).
3. No hit: off-topic list (reply declines), then the topic fallback, then the generic fallback.

## Edit an answer or add a topic

Select `DemoTutorScript.asset`. Under `Topics`, add an element, set `Behaviour Key` (same as the card, e.g. `MM4`), then add entries with `Keywords` and `Answer`. Use the `Source` field to note where the facts came from. `Notice` is the status line shown after each scripted reply.

## Run it

1. Set `Mode` to `Demo` on the prefab (needed on a machine that has a key, since `Auto` would pick live).
2. Play `Dashboard`, click `MM4`, open the chat, ask "Apakah fungsi enzim sekatan?".
3. Expect: status "Sedang menyediakan jawapan..." for about 0.8 s, then the answer bubble.

Tests: Window > General > Test Runner > EditMode > `Evergreen.TutorDemo.Tests`.

## Recording footage

`Hide Demo Notice` removes the notice after scripted replies. Use it only for recording. Leave it off in the app people run, so scripted text is not presented as model output. If the footage shows scripted replies, say so in the video description or end card.

## Verified (2026-11-09, Editor)

- Edit Mode tests: 10 of 10 pass.
- Play Mode, `Mode = Demo`, topic MM4: input locked and "Sedang menyediakan jawapan..." shown, then the enzyme answer appeared and the notice showed. No new errors.
- `Auto` on this machine chose `Live` (a key exists here), as designed.

## Wording review (2026-11-09)

Clear language issues were corrected in `DemoTutorScript.asset`: plural "Langkah-langkahnya", a comma in the colony-screening answer, a less redundant MM3 objective, "sel bahan bakar mikrob (microbial fuel cell, MFC)" for MM6, "berkaitan dengan" for MM7, and "sedang anda buka" in the generic fallback. The scientific claims and the enzyme answer used in the trailer were not changed. The 10 Edit Mode tests still pass.

The owner approved this Malay wording against the curriculum on 2026-11-09. XbaI is written "Xbal" in the scene; `Ligasi` and `Kejutan Haba` answers are kept general because the scenes do not state temperatures or timings for them.

## Not verified

- `Auto` with no key; the network-off run; Android or built players.
- Off-topic and fallback replies in Play Mode (covered by unit tests only).

## Force demo mode with `-tutor-demo`

Start any player build with the `-tutor-demo` argument to force the scripted replies, whatever the `Mode` field says and whether or not a key exists on the machine:

```powershell
.\Evergreen.exe -tutor-demo
```

The player log then shows `Tutor mode: Demo (-tutor-demo)` and no request is sent to OpenAI. The trailer capture helper (`trailer/capture/Drive.ps1`) always uses this flag. Release builds already behave this way unless `Allow Live In Release` is on; the flag also covers Development builds.
