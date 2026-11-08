# Evergreen STEM VirtualLab — Project Instructions

## Purpose and scope

Apply these instructions throughout this Unity project alongside the user's global development standards. Keep the same priorities: correctness, human ownership, usability, and focused changes. This file adds the project's science theme and UI language; it does not duplicate the global instructions.

The product is a Malay-language science learning environment with lessons, interactive experiments, and quizzes. The design should help learners understand a concept, perform a procedure, and interpret feedback. Every screen should feel part of the same virtual laboratory.

The authoritative revamp plan is [IMPLEMENTATION.md](IMPLEMENTATION.md). Its unchecked tasks describe future work, not implemented features. Historical dashboard notes under `docs/ai/` describe earlier work; inspect current assets before relying on them.

## Project facts and boundaries

- Unity version: read `ProjectSettings/ProjectVersion.txt` (6000.6.7f1 when these instructions were written). Older notes name an earlier version.
- Runtime UI uses uGUI Canvas, RectTransform, and TextMeshPro. Extend this system; a visual revamp does not require a UI Toolkit or input-system migration.
- Entry scenes: `Assets/Scenes/LoginPage.unity` and `Assets/Scenes/Dashboard.unity`.
- Experiments: `Assets/Scenes/Experiments/akt3.unity` through `akt7.unity`. Quizzes: `Assets/Scenes/Quiz/Uji Diri 1.unity` through `Uji Diri 7.unity`.
- Existing scope is desktop first, based on project notes. Preserve compatibility with mouse and keyboard. Add mobile-specific work when requested; do not change orientation or build targets as part of styling.
- Preserve lesson content, experiment rules, quiz answers, scoring, scene routes, and serialized events. Handle layout-related code changes narrowly and verify their behavior.
- Inspect the current login flow before changing its presentation. Its scene name does not establish that authentication or registration services exist.

## Science design direction

Use a clear, welcoming working laboratory with the reading discipline of a science notebook. Scientific identity comes from real apparatus, specimen illustrations, labeled diagrams, measured values, procedural steps, and meaningful observations.

- Make the current learning task the strongest visual element. In experiments, protect the apparatus workspace from navigation, instructions, and assistant overlays.
- Retain recognizable Evergreen assets and the existing navy, blue, and cyan identity. Keep the laboratory photo as contextual imagery; place long text on opaque reading surfaces.
- Use diagrams tied to the actual lesson. Keep arrows, labels, scales, units, and relationships scientifically accurate. Separate simulated observations from explanatory illustrations.
- Use step numbers only for real sequences and progress only when backed by actual state. Do not add invented learning statistics, completion badges, or scientific measurements.
- Avoid decorative atom icons, unrelated molecule patterns, animated particles, neon control rooms, and ornamental graph grids. A themed background cannot replace a useful layout.
- Use cards for selectable activities when that helps scanning. Use reading columns for lessons, labeled answer controls for quizzes, and a clear bench area for experiments; do not force every screen into the same card grid.

## Color and materials

These are revamp defaults, not a claim that every existing scene already uses them. Reuse the existing dashboard sprites before creating alternatives. Validate final combinations in Unity, including image overlays and control states.

| Role | Default | Use |
|---|---|---|
| Lab navy | `#0B0B3B` | Header, primary text on light surfaces |
| Action blue | `#3D5AFE` | Primary actions; white label |
| Instrument cyan | `#4FC3F7` | Selected indicators and diagrams; navy label |
| Reading surface | `#FFFFFF` | Lessons, questions, dialogs |
| Workspace surface | `#F2F6FA` | Quiet background around content |
| Secondary text | `#46556B` | Supporting text on light surfaces |
| Correct / completed | `#166534` | Paired with a check and descriptive text |
| Caution | `#92400E` | Safety or attention; paired with a label |
| Error / destructive | `#B91C1C` | Error or destructive action; paired with text |

- Navy, blue, and cyan come from the palette recorded in `docs/ai/project-state.md`; the surfaces and semantic colors above are proposed additions.
- Reserve error styling for errors and destructive actions. Ordinary Back and Close controls should be neutral and clearly labeled.
- Use at least 4.5:1 contrast for normal text and 3:1 for large text and essential control boundaries. Color must never be the only indication of a result or selection.
- Cyan on white is not sufficient for an essential boundary or focus indicator by itself; pair it with a contrasting navy edge or use a darker indicator.
- Prefer solid panels, restrained borders, and small corner radii. Add depth only when it explains layering, such as a dialog above a lesson. Keep diagrams readable without shadows or glass effects.

## Typography, layout, and interaction

- Use existing LiberationSans TMP assets for body text and controls. Restrict FredokaOne to short identity or activity headings; keep Wasted-Vindey and Child Hood out of new instructional body text. Verify scientific glyphs such as µ, °, subscripts, and superscripts.
- At the existing 1920 × 1080 reference size, start with title 48–60, section heading 36, body 28–32, and supporting label 22–24 reference units. Check actual readability at smaller windows; these are starting values, not permission to shrink long text until it fits.
- Use sentence case, left-aligned instructional text, about 45–75 characters per line where practical, and comfortable line spacing. Use scrolling or content reflow for long lessons.
- Use an 8-unit spacing rhythm with 16, 24, and 32-unit gaps. Start with 32–48-unit content insets and 8–12-unit panel radii at the reference size; adapt through layout components.
- Use anchors, Layout Groups, and LayoutElement deliberately. Avoid competing layout ownership or a ContentSizeFitter setup that causes rebuild loops. Do not change every CanvasScaler to one match value: existing scenes differ, and earlier notes report a dashboard regression at 0.5.
- Keep navigation, the screen title, and the current task in consistent locations. Reserve space for the assistant and version label rather than letting them overlap content.
- Provide visible normal, hover, pressed, selected, focused, and disabled states where relevant. Check mouse and keyboard navigation, dialog focus, and return focus. Newly added or rebuilt controls should have at least a 44 × 44 screen-pixel hit area at the smallest supported window.
- Use short feedback transitions, generally 100–180 ms, without delaying input or experiment state changes. Avoid looping decorative motion. Added nonessential motion must have a simple way to be reduced or disabled.

## Malay UI language and scientific content

Keep learner-facing interface copy in Malay, including replacement text for mixed English controls. Preserve proper nouns and scientific names. Keep code identifiers and existing serialized scene names unchanged.

| Meaning | Preferred wording |
|---|---|
| Application home | Laman utama |
| Select an activity | Pilih aktiviti |
| Start experiment | Mula eksperimen |
| Instructions / apparatus | Arahan / Radas |
| Purpose / procedure | Tujuan / Prosedur |
| Observation / conclusion | Pemerhatian / Kesimpulan |
| Self-assessment | Uji diri |
| Previous / next | Sebelum / Seterusnya |
| Retry / finish | Cuba lagi / Tamat |
| Back / close / exit app | Kembali / Tutup / Keluar aplikasi |
| Learning assistant | Pembantu pembelajaran |

- Use short, specific verbs and consistent terms. A label must describe the action it actually performs; use "Kembali ke laman utama" when that is the destination.
- Show procedure position as "Langkah 2 daripada 5" only when both values come from the real procedure. Do not add observation or conclusion inputs unless supported by an implemented learning workflow.
- Preserve quantities, formulas, safety guidance, scientific units, and the meaning of questions. Flag uncertain curriculum terminology for review rather than silently rewriting it.
- Give corrective feedback that explains the next action when that information exists. Do not invent explanations or reveal quiz answers through new hints.
- Treat the assistant as optional help, visually secondary to verified lesson material. Keep its open, closed, loading, empty, and failure states readable when those states exist; do not expand its service behavior during restyling.

## Unity editing and human ownership

- Inspect scenes, prefab instances, overrides, and callbacks through Unity Editor/MCP when available. Use targeted text searches as a fallback; avoid full reads or manual structural edits of scene and prefab YAML.
- Preserve `.meta` files and GUIDs. Move or rename Unity assets through supported Editor operations. Do not merge the existing `Assets/prefab/` and `Assets/prefabs/` folders during this revamp.
- Extend existing assets in `Assets/UI/Dashboard/`, `Assets/fonts/`, and the current prefabs. Create new shared UI under `Assets/UI/Science/` only when existing assets cannot serve the role.
- Keep scene layouts and content editable in the Inspector. Use prefabs/variants and TMP styles or materials for recurring treatments. Add a small theme data asset only when shared settings need to propagate; do not construct whole screens with runtime C#.
- New owned C# files use PascalCase and focused responsibilities, with serialized private references for designer-editable values. Preserve legacy script names and serialized fields unless a verified migration is part of the task.
- Audit name, hierarchy, and index dependencies before moving objects. `MenuSlideController`, `NextExperimentController`, `DragController`, `ItemSlot`, and `QuestionController` depend on child order, parents, names, or first-child components. Decorative wrappers can break these assumptions.
- Also inspect `triggerHit` and `SuhuBilikControl` parent/child chains and slider references, `TextSlider`'s child named `text`, `ConditionalCheck`'s numeric object name, and `newTrigger`'s Image at child 3. Preserve these contracts or replace only the affected lookup with a verified serialized reference.
- Inspect runtime presentation as well as prefab styling: `QuestionController.showRealAnswer()` and `resetButton()` override answer colors, while input locking uses `Button.enabled`. New feedback colors and locked states must work through those existing paths without changing quiz rules or timing.
- Check `Assets/Scenes/DontDestroyCanvas.cs` and the persistent `CanvasPrime` overlay before changing assistant placement. Validate both direct scene entry and navigation from Dashboard for duplicate or missing UI/EventSystems.
- Keep generated screenshots in `Temp/qa_screens` unless the user requests deliverable images. Avoid generated folders, solution files, and package internals during ordinary searches.

## Verification and handoff

For each implemented slice, inspect the actual Game view and record the resolution and interaction checked. Start at 1920 × 1080, then check 1280 × 720, 1280 × 800, 1024 × 768, 1280 × 1024, and 2560 × 1080. Test long Malay content, diagrams, scroll limits, focus, prompts, and overlay collisions. Recheck experimental drag/drop and question/scoring behavior after hierarchy changes.

After C# changes, validate scripts, wait for compilation, and inspect Console errors. Record pre-existing failures separately. Complete a desktop player smoke test before calling the whole revamp complete. Documentation-only changes require document checks, not a claimed runtime test.

Keep implementation status and evidence in [IMPLEMENTATION.md](IMPLEMENTATION.md). Update nearby guides only when their described behavior changes. Report what changed, where a human edits common settings, what was verified, and any remaining limitations.
