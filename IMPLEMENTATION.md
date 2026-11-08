# Science UI Revamp — Implementation Plan

Status: implementation in progress. Live baseline inspection has started; the full revamp is not complete. Verified changes and remaining checks are recorded below.

Updated: 2026-11-09. This is the authoritative plan for the full science UI revamp. [AGENTS.md](AGENTS.md) defines the lasting design and development rules. Keep progress and evidence here during implementation; do not treat a written task as completed work.

## Outcome and scope

Give Evergreen STEM VirtualLab a consistent science identity and a layout that helps learners move from lesson to experiment to quiz. Retain the existing Malay content, learning sequence, scientific meaning, and working interactions while replacing inconsistent screen structure, typography, navigation, and control styling.

Cover LoginPage, Dashboard Overview, all seven Dashboard lesson pages and their subpages, all five experiments (`akt3`–`akt7`), all seven quizzes (`Uji Diri 1`–`7`), completion/retry/quit dialogs, and the existing assistant presentation. Desktop is the current target. New authentication, cloud progress, assistant backend work, new curriculum, mobile adaptation, and a framework migration are outside this plan.

The visual direction is a working science laboratory with clear notebook-like reading surfaces: existing navy/blue/cyan identity, accurate labeled apparatus, readable scientific notation, and step feedback based on actual state. Use the palette, typography, spacing, and Malay vocabulary in AGENTS.md as the shared defaults.

## Verified starting points

Inspected on 2026-11-08 through source, settings, and targeted serialized-asset searches. No current Unity hierarchy query or Game-view inspection was performed for this document.

| Area | Current editable source and constraint |
|---|---|
| Editor and packages | `ProjectSettings/ProjectVersion.txt`: 6000.6.7f1. `Packages/manifest.json`: uGUI 2.6.0, Unity test framework, UnityMCP, and OpenAI-Unity are present. |
| Build scenes | `ProjectSettings/EditorBuildSettings.asset` enables LoginPage, Dashboard, seven quizzes, and five experiments: 14 scenes. SampleScene is listed but disabled. Verify the active desktop build profile before building. |
| Entry and lessons | `Assets/Scenes/LoginPage.unity`, `Assets/Scenes/Dashboard.unity`; prior notes describe Overview and seven Into lesson pages. Confirm live hierarchy and each route before changing them. |
| Scene navigation | `Assets/SceneController.cs`: `ChangeScene(string sceneName)`, `ReloadScene()`, `QuitGame()`. Preserve scene strings and existing Inspector callbacks. |
| Lesson navigation | `Assets/MenuSlideController.cs` enumerates direct children; `Assets/NextExperimentController.cs` derives the next step from parent/sibling order and expects button text at child 0. |
| Experimental interaction | `Assets/DragController.cs` finds `Canvas` by name and divides pointer movement by its scale factor. `Assets/ItemSlot.cs` expects a tick at child 0 and reparents dragged objects. |
| Additional experiment contracts | `Assets/triggerHit.cs` and `Assets/SuhuBilikControl.cs` traverse parent/child chains and reference sliders; both occur in akt3 and akt4 serialization. `Assets/TextSlider.cs` requires child `text`; `Assets/ConditionalCheck.cs` parses its object name as an integer; `Assets/newTrigger.cs` uses an Image at child 3. Audit their actual instances before reparenting or renaming. |
| Quiz data and UI | `Assets/QuestionList.cs` owns serialized question/answer lists. `Assets/QuestionController.cs` owns question labels, answer button references, avatar children, and completion prompt. It finds named audio objects and expects answer TMP at child 0. |
| Runtime answer styling | `QuestionController.showRealAnswer()` and `resetButton()` assign hardcoded answer Image colors. Input locks use `Button.enabled`, so normal uGUI disabled styling alone does not establish the planned locked appearance. |
| Score | `Assets/ScoreController.cs` updates the score label; `Assets/loadScore.cs` computes maximum as question count × 10. Preserve the data-derived maximum. |
| Persistent assistant | `Assets/Scenes/DontDestroyCanvas.cs`, `Assets/prefabs/ChatBoxAi.prefab`, `Assets/prefabs/ChatGPT.prefab`. Historical notes identify persistent CanvasPrime; inspect actual scene wiring. |
| Reusable art/type | `Assets/UI/Dashboard/`, `Assets/UI/bg2.jpg`, `Assets/fonts/`, `Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset`. Inspect existing sprites and TMP materials before adding assets. |
| Scaling | Targeted searches show 1920 × 1080 reference canvases. Login, Dashboard, and akt4 have match 0; Uji Diri 1 has match 1. This is not a complete canvas inventory. |

Useful historical context: [dashboard polish plan](docs/ai/dashboard-ui-polish-plan.md), [project state](docs/ai/project-state.md), [script map](docs/ai/script-map.md), [scene map](docs/ai/scene-map.md), and [QA report](docs/ai/qa-report.md). These are dated records. The scene map's build-list warning is superseded by the currently inspected build settings. The dashboard plan's final status reports match 0; do not copy its earlier proposal to set every canvas to 0.5.

## Decisions and assumptions

| Decision | Basis and what would change it |
|---|---|
| Retain uGUI/TMP and existing scene routes. | Existing code and assets use them; reconsider only for an explicitly requested migration with its own compatibility plan. |
| Use the existing navy, blue, cyan, and lab photo identity. | Historical dashboard notes record this as user-selected. Science identity comes from content/layout. Replace the brand direction only when the user requests it. |
| Use LiberationSans for instructional copy; FredokaOne only for short headings. | Both are already present. Introduce another font only if a demonstrated readability/glyph requirement cannot be met with existing assets and licensing is checked. |
| Desktop first; landscape layouts based on 1920 × 1080. | Existing settings and project notes. Confirm the release target in stage 0; a mobile release adds separate touch, safe-area, orientation, and device checks. |
| Preserve learning behavior and content. | User requested a design/layout revamp. Necessary UI binding changes are allowed; curriculum, scoring, and service changes require separate scope. |
| Prove Dashboard lesson → akt4 → Dashboard first. | akt4 contains real procedural text and drag/drop with canvas-dependent movement. If baseline akt4 is broken, isolate that failure and use akt3 for the first visual slice without claiming akt4 fixed. |

The learner age range, minimum release hardware, and final smallest supported window are unconfirmed. Use clear general learner language and 1280 × 720 as the initial desktop window baseline; exercise additional aspect ratios below. Finalize release limits from actual readability and player checks rather than inventing requirements.

## Skill-guided design workflow

Use the four requested skills throughout design, implementation, and review. Read their installed instructions and relevant references before each new kind of work. Apply them within AGENTS.md and the existing Unity architecture; skill examples do not authorize framework, input-system, content, or platform migrations.

| Skill | Responsibility in this revamp | Required implementation evidence |
|---|---|---|
| `game-ui-ux` | Responsive regions, RectTransform anchors and pivots, layout ownership, per-canvas scaling, focus order, modal input ownership, and return focus. Consult `references/layout-and-flow.md`. | Per-canvas aspect policy; mouse/keyboard walkthrough; resize and overlay checks at the resolution matrix. |
| `game-ui-design` | Learning-task hierarchy, readable apparatus workspace, contextual help, consistent controls, and immediate action feedback. Consult `references/patterns.md` when designing and `references/validations.md` when reviewing. | Screenshots showing the current task, procedure, navigation, and feedback without apparatus obstruction; readable longest-content examples. |
| `color-theory` | Semantic palette roles, accessible foreground/background pairs, state colors, and color-independent meaning. Consult `references/patterns.md` for creation and `references/validations.md` for review; use `references/sharp_edges.md` when diagnosing color problems. | Calculated contrast for actual state pairs plus rendered context checks, grayscale review, and protanopia/deuteranopia/tritanopia simulation results or explicit untested notes. |
| `ui-ux-pro-max` | Product-fit research and a final pass over accessibility, interaction, layout, typography, forms, and feedback. Use focused UX searches and relevant rules from `references/quick-reference.md` and `references/pro-rules.md`. | Record the query, matching guidance, accepted recommendations, and any platform mismatch; apply the adapted checklist below. |

Research performed for this plan on 2026-11-08: UI/UX Pro Max design-system queries `science education desktop readable` and the narrower retry `education learning application` returned web showcase layouts and, on retry, Claymorphism with new fonts and colors. Neither supplied a verified overall match for this Unity laboratory. Keep the project design direction; use the skills' general principles as the fallback for visual composition. The focused query `focus not obscured --domain ux` returned relevant focus-visibility guidance, adapted here to assistant/modal placement. These searches were not persisted as a competing design-system document. Unity is not among this skill's supported stack searches; use Unity uGUI guidance for component implementation.

Keep design decisions and evidence in this file. Extend existing prefabs, TMP assets, and Inspector settings; add shared presentation assets only when repeated use justifies them. During implementation, pair the engine-neutral skills with the installed Unity uGUI skill and confirm APIs against this project's Unity version.

### Design specification to prove in the first slice

- Establish one stable header/navigation region, one main learning region, and reserved assistant/version space. Give the current learning task the strongest hierarchy; keep ordinary navigation and help secondary. Use activity cards only for selection, reading columns for lessons, a clear bench for experiments, and labeled choices for quizzes.
- Use 1920 × 1080 as the reference, with anchors, matching pivots, and Layout Groups where appropriate. Assign each region one layout owner. Preserve behavior-sensitive child order and free apparatus placement; do not put draggable objects under a layout group that repositions them. Choose CanvasScaler settings from each canvas's baseline rather than applying the skill's example match value globally.
- Define extra-width and reduced-height behavior per screen: cap lesson reading width on ultrawide displays, allow surrounding workspace to expand, and reflow lesson text/diagrams when cramped. Keep experiment instructions scrollable within their region while preserving the bench and step controls. Verify the apparatus remains large enough to use before accepting a smaller-window layout. Keep critical controls inside desktop content insets; phone/TV safe-area systems remain outside current scope.
- Use LiberationSans TMP for body/controls and FredokaOne only for short identity/activity headings. Start at title 48–60, section 36, body 28–32, supporting label 22–24 reference units; use 16/24/32 gaps, 32–48 content insets, and 8–12 panel radii. Check rendered screen-pixel readability at the smallest supported window. Reflow or scroll long Malay text instead of relying on TMP auto-size to shrink it. Verify µ, °, subscripts, and superscripts in actual lesson content.
- Show essential instructions, safety information, and current values persistently during the relevant task. Disclose optional detail only through existing help flows; do not auto-hide critical information or add tooltips as a substitute for labels. Use existing science imagery and meaningful diagrams; keep long text on opaque surfaces.

### Palette and control-state specification

Use these semantic roles consistently in scene/prefab Inspector settings. A role name describes intended use; it does not imply a theme service or asset already exists. When variants are needed, derive perceptually consistent shades with OKLCH/OKLAB, convert to sRGB values usable in Unity, and validate the resulting pairs. Do not introduce a new palette or dark theme as part of this work.

| Role | Starting value / pairing | Application |
|---|---|---|
| Primary text / header | Navy `#0B0B3B`; white text on navy headers | Titles, body text, high-contrast navigation. |
| Primary action | Blue `#3D5AFE` with white label | Main action for the current task; avoid competing primary actions. |
| Selected accent | Cyan `#4FC3F7` with navy text/edge | Current activity or step selection where actual state exists; add a visible marker or label. |
| Reading / workspace | White `#FFFFFF` / `#F2F6FA` | Opaque text panels and quiet bench surroundings; imagery keeps its scientific colors. |
| Supporting text / essential boundary | `#46556B` on a light surface | Supporting labels and control edges when needed; cyan alone is insufficient against white. |
| Correct / caution / error | `#166534` / `#92400E` / `#B91C1C` on white | Pair with accurate text and a check, warning, or cross cue. Back/Close remain neutral. |

- Specify normal, hover, pressed, selected, focused, disabled, and quiz-locked appearances for each rebuilt control. Derive hover/pressed action shades during the first slice and record their actual Inspector values and contrast before reuse. Keep labels readable in every state; do not reduce whole-control opacity without checking the composed result.
- Give keyboard focus a contrasting outline/marker distinguishable from hover and selection. Use navy on light surfaces and a contrasting light treatment on navy surfaces; verify at least 3:1 against adjacent colors. Keep the entire focused control visible above scroll edges and clear of the assistant.
- Make rebuilt hit regions at least 44 × 44 actual screen pixels at the smallest supported window, with clear separation. Reference-unit dimensions alone do not prove this after CanvasScaler scaling. Pressed feedback must preserve layout bounds and respond immediately; optional 100–180 ms transitions must not delay input or quiz timing.
- Verify normal text at ≥4.5:1 and large text/essential graphics at ≥3:1 using final foreground/background values. Include image tint, TMP material effects, alpha, CanvasGroup alpha, selected states, and modal scrims in rendered checks. Review grayscale and color-vision simulations so selection and answer outcomes remain understandable without hue.
- Apply runtime quiz feedback through the narrow QuestionController presentation changes in stage 4. Default Selectable colors alone cannot represent the existing `Button.enabled` lock. Keep locked state, revealed result, and next-question reset distinct without changing scoring or delays.

### Input, motion, and review boundaries

Retain the project's current input configuration. Provide initial selection, predictable Selectable navigation, modal focus containment, and restored focus on close for ordinary mouse/keyboard UI. Reuse existing screen controllers and callbacks; introduce only the local overlay ownership needed to prevent click-through or focus escape. The skills' generic screen-stack recommendation does not require replacing scene navigation. Check gamepad behavior if already supported; new controller support and keyboard equivalents for legacy apparatus drag interactions must be recorded as separate accessibility work rather than claimed complete.

Use existing state changes/events for new presentation updates instead of adding per-frame polling of gameplay values. Keep added cosmetic motion static when no simple reduction/disable mechanism exists. Apply UI/UX Pro Max's app checklist selectively: visible labels, focus, target size, contrast, stable layout, readable feedback, and unobscured scrolling apply here. Web ARIA/CSS, mobile units/breakpoints, Dynamic Type, new localization packages, dark mode, and screen-reader support are not inferred Unity capabilities or automatically added requirements. Record actual support and gaps.

## Target screen layouts

| Screen | Layout and behavior |
|---|---|
| LoginPage | Clear product identity and a compact, labeled entry form on a solid reading surface, with laboratory context beside or behind it. Preserve the actual entry behavior; do not introduce account services or imply working registration where none exists. |
| Dashboard Overview | One header, clear activity selection, consistently aligned activity entries, and a contained scroll region. Use existing activity art and real titles. Keep quit/assistant controls separate from activity selection. |
| Dashboard lesson pages | A topic title, readable content column, adjacent labeled diagram when applicable, and stable previous/next controls. Reflow to one column when a diagram/text split becomes cramped; scroll long content inside its reading region. |
| Experiment introduction | Title, purpose, apparatus, and safety information from existing content; a clear "Mula eksperimen" action. Do not truncate safety instructions to make the panel smaller. |
| Experiment procedure | A compact title/navigation area, dedicated apparatus workspace, a separate current-instruction region, and reachable step controls. Show observations and procedure counts only where actual state supports them. Overlay text must not intercept apparatus input. |
| Quiz | Stable question number/step region, a readable question/diagram area, generously sized labeled answer choices, and a separate score/feedback region. Answer selection, feedback timing, and progression retain existing behavior. |
| Completion and exit dialogs | Readable outcome or exit question, explicit action labels, keyboard focus, and a clear return route. Results use actual score and data-derived maximum. |
| Assistant | Consistent, reserved placement that does not cover questions, diagrams, apparatus, navigation, or dialog actions. Keep chat content editable in existing prefabs and preserve current service behavior. |

## Implementation stages

Implement sequentially. Each stage requires its own inspection, a small reviewable change, saved assets, and the listed evidence before moving on. Keep existing user edits intact; the working tree already contains package/settings/sample-script changes and untracked documentation/screenshots.

### 0. Establish a baseline

- [ ] Inspect the active Editor/version, current desktop build profile, loaded scenes, unsaved changes, and Console state. Do not save or discard unrelated scene edits.
- [ ] Capture current Game-view layouts in `Temp/qa_screens` for LoginPage, Dashboard Overview, a lesson, akt4 introduction/procedure, Uji Diri 1 question/result, and the assistant open/closed. Record current failures separately from revamp regressions.
- [ ] Inventory every CanvasScaler/render mode, EventSystem, persistent overlay, relevant prefab override, and screen callback through Unity Editor/MCP. Inspect `Assets/prefab/CongratPrompt.prefab` retry wiring; historical notes report an unconfirmed null target.
- [ ] Map the seven Dashboard activity entries to real lesson/experiment/quiz routes. Confirm akt4's Dashboard entry and baseline drag/drop. Do not assume activity numbering equals a scene route.
- [ ] Record hierarchy/name/index assumptions in the touched scenes, including MenuSlideController direct children, NextExperimentController step parents, ItemSlot tick, answer label child 0, named audio objects, and the Canvas lookup. Include triggerHit/SuhuBilikControl parent chains and slider references, TextSlider child `text`, ConditionalCheck numeric names, and newTrigger's child-3 Image. Inspect these scripts before restructuring the first akt4 slice as well as later experiments.

Evidence: baseline screenshots, route/canvas inventory, known-failure notes, and confirmed scope of the first slice. Record these under Progress and evidence below; paths may point into temporary QA output.

### 1. Ship one complete science-styled learning slice

- [ ] Apply the skill-guided design specification above to this slice. Record per-region anchors/layout ownership, canvas scaling/aspect policy, semantic color/state values, TMP sizing, modal focus behavior, and reusable asset ownership. Prove the treatments in Unity before propagating them.
- [ ] Restyle Dashboard Overview and the lesson entry/page identified for akt4. Carry the same title, navigation, reading-surface, typography, and control treatments into akt4 introduction and a representative real procedure, then verify its completion/return route.
- [x] Reuse `Assets/UI/Dashboard/` artwork and add shared presentation under `Assets/UI/Science/Prefabs/`, `Sprites/`, and `Scripts/`. These paths now exist; the shared assistant prefab and focus components are recorded below.
- [ ] Use existing TMP font assets and reusable materials/styles. Keep shared prefabs limited to recurring presentation such as action buttons and dialogs; do not replace scene controllers with a new UI framework.
- [ ] Keep visuals inside existing behavior-bearing roots when hierarchy assumptions apply. Preserve the first-child TMP/tick and ordered step containers. If a required layout cannot retain those contracts, replace only the affected implicit lookup with serialized references and test that path before continuing.
- [ ] Validate this slice at the full resolution matrix and complete at least one correct drop, an invalid drop attempt, a step transition, the final step, and return to Dashboard. Preserve baseline behavior for invalid attempts; do not invent new simulation rules.

Evidence: before/after Game-view screenshots, route and drag/drop results, any required binding change, and Console comparison. Carry these proven treatments forward only after the slice works.

### 2. Complete entry, activity selection, and lesson layouts

- [ ] Apply the proven style to LoginPage without changing entry semantics. Check current field labels, keyboard traversal, and any existing validation/error feedback.
- [ ] Finish all Dashboard activity entries, all seven Into lesson pages, and every existing subpage. Retain content order and illustrations while improving reading measure, diagram labels, scrolling, and navigation placement.
- [ ] Replace mixed English navigation labels with the agreed Malay terms where their meaning is verified. Keep scene names, callbacks, and curriculum wording intact.
- [ ] Inspect previous/next behavior at first and last pages. Avoid adding decorative children to MenuSlideController's ordered content container.
- [ ] Check Overview, each lesson page, quit prompt, and assistant together at every target aspect ratio. Record each page checked rather than claiming all seven from a sample.

Evidence: a page/route checklist covering seven lessons and their subpages, usable first/last-page controls, and screenshots of the longest content and narrowest layout.

### 3. Complete the experiments

- [ ] Finish all remaining akt4 screens, then migrate `akt3`, `akt5`, `akt6`, and `akt7` one scene at a time. Inspect each scene's own interaction scripts, not just the shared slice.
- [ ] Keep draggable apparatus, drop targets, colliders, and procedure roots intact. Apply presentation changes around them; retain world/canvas coordinate behavior and validate camera-backed canvases separately.
- [ ] Restyle existing timers, volume/temperature displays, apparatus labels, safety text, instructions, and completion feedback. Preserve units, notation, quantities, and simulation timing.
- [ ] Check every real procedure transition, correct placement, invalid action, terminal step, reset/re-entry, and return route. If changing a scale or parent affects apparatus position/collision, correct and reverify it within that scene before proceeding.

Evidence: a completion walkthrough for each of the five experiments, readable units/diagrams, no blocked apparatus interactions, and a list of any pre-existing failures still unresolved.

### 4. Complete quizzes and result/retry flows

- [x] Restyle `Uji Diri 1` using QuestionController's existing labels and answer references. Preserve answer-label child 0 and avatar-state ordering.
- [ ] Update only the runtime answer-presentation paths in `Assets/QuestionController.cs`: make normal/correct/incorrect colors Inspector-editable serialized settings used by `showRealAnswer()` and `resetButton()`, and pair feedback colors with explicit text/check/cross cues. Add an explicit presentation update alongside the existing `Button.enabled` lock/unlock path so feedback remains legible and cannot be mistaken for an available answer. Preserve answer evaluation, scoring, coroutine delays, and listener behavior. Check appearance during the locked interval, answer reveal, and subsequent reset; scene-only ColorBlock changes are insufficient.
- [ ] Verify correct and incorrect answers, feedback timing, disabled answer controls, double-click handling, question/step transitions, final score, and result prompt. Require scoring to match baseline with no duplicate input introduced by the redesign.
- [ ] Resolve any confirmed missing retry binding narrowly to the existing restart path; verify that score/question state resets and the same quiz restarts without duplicate callbacks.
- [ ] Apply the tested layout to Uji Diri 2–7. Check the longest question/answers, all diagram variants, every terminal question, and each actual question-count-based maximum. Do not hardcode a shared maximum or assume equal quiz lengths.

Evidence: question/answer/result screenshots and a per-quiz correct/incorrect/retry/return checklist, including a full Uji Diri 1 walkthrough and the terminal state of every other quiz.

### 5. Integrate overlays, dialogs, and input

- [ ] Apply consistent quit, retry, and completion dialog styling while preserving distinct action meanings. Confirm default focus, keyboard activation, cancellation where supported, and return focus.
- [ ] Update assistant placement in its actual scene/prefab owner. Preserve DontDestroyCanvas ownership and inspect sorting/raycast settings; do not instantiate a second persistent assistant.
- [ ] Check navigation from LoginPage → Dashboard → experiment → Dashboard → quiz and compare with direct scene entry. Verify one intended EventSystem per active UI context and no duplicate persistent overlay or input interception.
- [ ] Use local mock presentation data only if needed to inspect an existing chat loading/failure state; do not send paid/live assistant requests merely to verify styling. If a service state cannot be exercised, record it as untested.
- [ ] Confirm added motion does not delay input and can be reduced/disabled. If existing project settings provide no motion preference, keep the slice static rather than adding a settings subsystem for cosmetic animation.

Evidence: open/closed assistant and modal checks on lesson, experiment, and quiz screens; complete keyboard navigation of ordinary UI; direct-entry and multi-scene input results. Existing drag-only apparatus is not claimed keyboard accessible without a separately implemented equivalent interaction.

### 6. Verify the player and hand off editable sources

- [ ] Review the implemented screens using all four skills and the adapted checklist above. Record contrast/state results, grayscale/color-vision checks, unobscured focus, rendered hit sizes, long-content behavior, motion settings, and any unsupported accessibility paths. Fix new usability regressions before marking a slice complete.
- [ ] Run the visual and behavior matrix below, inspect missing scripts/references and Console errors, and compare the final diff with the intended scope. Check scene/prefab saves and imports through Unity.
- [ ] If C# changed, validate and compile it, then run relevant existing Edit/Play Mode tests. Add targeted tests only for changed logic that benefits from them; do not add tests that just repeat color or spacing assignments.
- [ ] Build the existing desktop target/profile to `Temp/ScienceUIBuild/` if compatible with its configured output process, and run a real player smoke test through LoginPage, Dashboard, one full experiment, one quiz, result/retry, and exit. Otherwise record the actual build path and prerequisite blocking this check.
- [ ] Update `docs/ai/scene-map.md`, `script-map.md`, and any affected project guide only for verified structure/behavior changes. Update historical-state pointers without treating older QA as current proof.
- [ ] Add the final customization instructions and verification limits to this file. If execution needs a separate deviation log, create `IMPLEMENTATION.notes.md` with `Deviations` and `How the run ended`; do not create it merely as an empty artifact.

Evidence: build/player results, meaningful test results or reasons unavailable, final screenshot locations, known limitations, and the verified Inspector paths for a common manual edit.

## Verification matrix and finish line

| Check | Coverage | Expected result |
|---|---|---|
| Aspect and window size | 1920 × 1080, 1280 × 720, 1280 × 800, 1024 × 768, 1280 × 1024, 2560 × 1080 | Titles, controls, diagrams, and long Malay text remain readable; no overlap, clipped safety text, unreachable scroll content, or assistant obstruction. Record any proposed release-size limit explicitly. |
| Navigation and input | All 14 enabled scenes; every Dashboard lesson/subpage; first/last steps; ordinary UI by mouse and keyboard | Existing destinations and content order preserved; focus visible; no dead controls or duplicate callbacks. |
| Scientific presentation | µL, °C, formulas, labels, diagrams, safety statements, timers | Glyphs render; units and scientific meaning match source content; values reflect actual state. |
| Experiment behavior | All five experiments, including scale-sensitive drag/drop and terminal steps | Apparatus remains usable and progression matches baseline, with no new errors or false completion. |
| Quiz behavior | Seven quizzes; correct/incorrect input, final question, retry, return | No duplicate scoring or early progression; maximum derives from real data; retry resets intended state. |
| Visual accessibility | Final text/surface pairs, essential boundaries, control states, focus, dialogs | Normal text ≥4.5:1, large text/boundaries ≥3:1; meaning available beyond color; rebuilt controls usable at smallest supported window. |
| Persistent UI | Direct scene entry and Dashboard-driven entry; assistant open/closed; dialogs | No duplicate canvas/EventSystem ownership, blocking raycast surface, lost focus, or covered learning content. |
| Player | Current desktop build target/profile and smoke path in stage 6 | Build succeeds and actual player navigation, apparatus, quiz, retry, and exit work; Editor success alone is insufficient. |
| Human customization | Change a heading, a repeated button style, and an instruction through normal Unity tooling | Change is discoverable in the owning scene/prefab/TMP asset; intended instances update; overrides and unchanged variants are explained. |

Completion requires checked stages with their evidence, the full screen inventory covered, actual player verification, and a documented human editing path. Report partial implementation or blocked checks precisely; do not label the whole revamp complete while required screens or behavior remain unverified.

## Risks, stop conditions, and recovery

- Hierarchy changes can break procedural and quiz scripts. Check the named contracts in stage 0, preserve them where possible, and isolate necessary binding corrections. Do not perform a broad gameplay refactor to simplify restyling.
- Scaling changes can break apparatus coordinates and lesson titles. Choose settings per canvas from rendered evidence; keep known-working values until each replacement is verified.
- Shared prefab/TMP edits can affect untouched scenes. Prefer explicit prefab variants or scoped materials where treatments differ; inspect overrides and all dependent screens before accepting shared changes.
- Historical retry/step bugs may block a baseline walkthrough. Reproduce and record them first; fix a narrow UI integration defect when necessary, but report simulation/data repairs that require separate scope.
- If the live project differs from this inventory, update verified paths/contracts before dependent work. If an intended edit would overwrite unrelated unsaved work, leave that scene untouched and report the conflict. Continue independent documentation or asset inspection.
- Before each scene migration, establish a recoverable version-control checkpoint limited to the intended assets, or retain a targeted external backup if committing is not part of the task. Preserve asset and `.meta` pairs. Revert only the failed slice; never reset the working tree to remove unrelated edits. Reopen affected scenes and check serialized references after recovery.

## Human editing path after implementation

Layouts remain in the existing `.unity` scenes and their prefab instances. Quiz questions/answers remain in QuestionList's serialized Inspector data. Experiment instructions remain in their owning scene/TMP components unless an inspected existing source already owns them. Runtime state remains in the current controllers.

Shared new presentation assets, if required, live under `Assets/UI/Science/`; existing dashboard assets remain under `Assets/UI/Dashboard/`. Store a new theme data asset only when real cross-screen settings require propagation, and record its actual component/Inspector path here when implemented. A palette written in Markdown alone does not update Unity objects.

For the final handoff, record the exact scene/prefab/component for one heading edit, one shared action-button color edit, and one instruction edit. Verify each example in the Game view, explaining prefab overrides and which instances inherit the change. Do not advertise proposed assets as existing editable controls.

## Progress and evidence

### First slice execution — 2026-11-08

The first slice edits `Assets/Scenes/Dashboard.unity` (Overview and activity 4), `Assets/Scenes/Experiments/akt4.unity`, and the existing assistant presentation only where needed for these screens. Two small presentation components under `Assets/UI/Science/Scripts/` provide keyboard focus outlines and page/dialog focus ownership. Scene layouts remain serialized Inspector content; no runtime screen construction or package changes are planned. Propagation to the remaining screens follows the stage 1 verification gate.

Baseline: Unity 6000.6.7f1 is connected; LoginPage was clean and the Editor was outside Play Mode. Active target is StandaloneWindows64. Existing package/settings/sample changes are preserved. Targeted source/`.meta` backups are in `%LOCALAPPDATA%/Evergreen/UIRevampBackup/2026-11-08/`.

At 1920 × 1080, the existing Login button enters Dashboard without field validation; Register has no callback. Dashboard activity 4 selects Teknologi Pengklonan and its experiment button loads `akt4`. Existing callback-driven introduction and procedure entry were exercised. A drag outside the drop targets remained draggable; accepted drops reparented labels, enabled ticks, and disabled dragging. The step-2 callback was exercised; automatic next-button availability needs a separate frame-based recheck. These are programmatic event checks, not a physical mouse walkthrough.

Baseline screenshots and inventories are under `Temp/qa_screens/`: `baseline-login-play.png`, `baseline-dashboard.png`, `baseline-activity4.png`, `baseline-lesson4.png`, `baseline-akt4.png`, `baseline-akt4-procedure.png`, `scene-inventory.txt`, and `dashboard-routes.txt`. The initial edit-mode camera-only login capture does not show overlay UI and is not visual evidence for the login layout.

All 14 enabled scenes have one scene EventSystem. Dashboard owns CanvasPrime; quizzes and experiments rely on persistence when entered from Dashboard. CanvasScaler match values: Login/Dashboard/akt3/akt4/akt5/akt7 = 0, quizzes = 1, akt6 = 0.515. akt7 is Screen Space Camera; other scene canvases are Overlay. No scaling values have been changed.

Pre-existing issues: `Assets/prefab/CongratPrompt.prefab` has null ChangeScene/ReloadScene targets; disabled activity-2 info navigation has two null SetActive targets; several experiment/assistant buttons have empty serialized callbacks. Activity 4's extra-info panels contain electron/MFC/rubber-film text inconsistent with their cloning heading. Preserve that text and flag it for curriculum review. Initial Console inspection found an MCP transport warning; no compile error was observed.

Design research: `keyboard focus visible --domain ux` matched visible focus and unobscured focus guidance. Apply that guidance to uGUI selection/assistant placement; web CSS examples are not Unity APIs.

- [x] Inspect project version, package manifest, build scene list, relevant scripts, targeted UI serialization, and previous project notes.
- [x] Create local AGENTS.md and this implementation plan with science design rules, real integration constraints, rollout stages, and verification criteria.
- [x] Complete a fresh-reader handoff review; verify and add the extra experiment hierarchy contracts and runtime quiz styling requirements it identified.
- [x] Update this plan using game-ui-ux, game-ui-design, color-theory, and ui-ux-pro-max instructions/references, with project-specific layout, palette, state, input, and review requirements. This is documentation work; the design specification still requires Unity validation.
- [ ] Complete stage 0 and record current Editor baseline, routes, canvases, and known failures.
- [ ] Complete stages 1–6 and record per-stage evidence.

Document checks on 2026-11-08: relative Markdown links resolve; both new files pass trailing-whitespace checks. Calculated contrast for navy/white, secondary text/white, white/action blue, navy/cyan, and the three semantic text colors/white ranges from 5.13:1 to 18.66:1. These solid-color calculations do not verify rendered images, overlays, or control states. No runtime, visual, compile, or player-build success is asserted by these initial checkmarks.

- [x] akt4 safety text (`Canvas/Prestart Bahan2/Content/2`) is vertically scrollable in the Editor (viewport 1509×362, content 1509×478); static check only, not yet exercised by mouse/touch in Play Mode. "onDrop" Console entries are the existing `Debug.Log`, not errors.
- [x] 2560×1080 Play Mode route Dashboard → Aktiviti 4 → Mula eksperimen → akt4 intro → DNA-steps diagram → safety scroll rendered (`Temp/qa_screens/akt4-intro.png`, `akt4-diagram-fixed3.png`, `akt4-safety2.png`). Routes were invoked programmatically, not by mouse.
- Fixed: akt4 `Prestart Bahan2/Content/1/Radas` diagram labels hung below the card and under "Seterusnya". `Radas/Image` scaled to 0.85 and `Radas` moved to y=51 (a HorizontalLayoutGroup overrides the Image's own position, so edit the parent). Safety text scrolls with a visible scrollbar.
- Note: edit-mode screenshots are blank; capture only in Play Mode. Stop/Play returns to Dashboard as the active scene, so akt4 is opened additively for edits.
- [x] akt4 step 1 (Play Mode, 2560×1080): dropped Plasmid/Sisipan GFP labels on their slots via `IDropHandler` events (not a physical mouse drag): labels parent to the slot, ticks show, dragging disables, "Seterusnya" appears, and it advances to step 2. Invalid-drop and return-to-Dashboard not yet re-run on the revamp. Fixed: step-1 instruction strip overlapped the tube caps (`ScienceInstructions` shortened, text moved up).
- Open: akt4 steps 2–5 are still the old style (outlined Fredoka text, `LANGKAH N` headings, `Mula semula` unstyled spacing); only step 1 is restyled. This is the next piece of stage 1/3 work.
- [x] akt4 steps 2–5 headers restyled in `akt4.unity` by script: shared `ScienceHeaderShared`/`ScienceFooterShared` bars under `Experiments` (siblings 1–2), `Langkah N`/title/footer/`Seterusnya` copied from step 1 styling, Fredoka outline text swapped to LiberationSans SDF, yellow rich text replaced by blue bold (#1D4ED8), step-5 result labels set to navy. Checked in Play Mode at 2560×1080: steps 2, 2(1), 4, 5(4). **Not visually checked:** steps 3, 3(1), 4(1), 5, 5(1)–5(3) bodies. Apparatus-local white labels (e.g. "Plasmid DNA: 20 µL", "MASUKKAN", "Suhu BIlik", "Ligasi", timer) were font-swapped only; check each against its own background.
- Env note: with Dashboard+akt4 both open in the Editor, Play Mode reports "2 event systems" (artifact). With only akt4 open the count is 1. Dashboard was saved once by mistake (it already carried the revamp diff).

### akt4 apparatus and remaining page review — 2026-11-08

This follow-up supersedes the earlier notes that the remaining page bodies were unchecked. It changes only presentation in `Assets/Scenes/Experiments/akt4.unity`; no gameplay scripts, callbacks, apparatus transforms, step names/order, or CanvasScaler settings were changed.

- [x] Change reagent `source` labels, pale-tube captions, percentage labels, and the room-temperature caption to navy. Keep white text on dark Ligasi labels, instrument readouts, and blue action buttons. Use sentence case for `Masukkan` and `Suhu bilik`.
- [x] Move the two pipette hints into a left reading area above the footer, clear of the tube and pipette. Widen and restyle `Hasil sebenar` to match the primary action treatment.
- [x] Capture and inspect the visible page states `3`, `3 (1)`, `4 (1)`, `5`, and `5 (1)`–`5 (4)` at 1920×1080, 1280×720, 1280×800, 1024×768, 1280×1024, and 2560×1080. No clipping or footer collisions were observed in these states. Pages were selected programmatically; these captures do not prove completion of their procedures or cover every hidden apparatus state. Existing bottom-anchored apparatus leaves substantial empty space at taller aspect ratios.
- [x] At 2560×1080, exercise step-1 drag handlers with an outside-target release: no ItemSlot at the release position, unchanged parent, dragging still enabled, alpha/raycast/collider restored, both ticks off, and Next hidden. Then invoke the two valid drop handlers: labels attach, ticks appear, dragging disables, Next appears, and its callback advances to `2`. These are programmatic pointer/drop checks, not physical mouse tests or a wrong-label-on-wrong-slot validation test.
- [x] Activate the final page and invoke its existing finish button. Dashboard loads with one active EventSystem and the `Canvas`/`CanvasPrime` canvases. This verifies the return callback, not a full experiment walkthrough or Dashboard-driven re-entry.
- [x] Save only akt4 after leaving Play Mode. Its Canvas hierarchy reports zero missing scripts, and the Editor reports the saved scene clean. Comparing serialized object IDs with the initial scene copy found no added or removed objects. An Editor scene copy preserving the initial unsaved state is at `Temp/qa_screens/akt4-before-followup.unity`.

Evidence: 48 full-size captures use `Temp/qa_screens/akt4-check-<width>x<height>-<page>.png`; parentheses in page names become underscores. Six `akt4-sheet-<width>x<height>.jpg` contact sheets were inspected alongside full-size representative captures. The first fast capture attempt dropped pending frames; the complete set was recaptured with time between layout changes and captures. Use direct `ScreenCapture.CaptureScreenshot` in Play Mode when the camera tool returns a plain background.

Console limits: the Console already contained `ConditionalCheck.cs:37` name-parsing `FormatException` entries and recursive PlayerLoop errors referencing `EditorApplication.Step` before this follow-up's edits. They remain unresolved; this is not a clean-Console sign-off. No C# files changed and no build or automated test run is claimed. The `Pemulihan sel (In progress)` heading remains an existing indication of unfinished behavior; later procedure logic still needs a normal walkthrough.

Inspector editing: open akt4 and expand `Canvas/Experiments/Content`. Reagent captions are the TMP components named `source` below each step's apparatus hierarchy. In `3 (1)`, edit `Text (TMP) (6)` to change the pipette hint or its RectTransform (bottom-left anchor, position 48/144, size 440/112). The photo action is `5 (4)/Content (1)/table/Button`: edit its Image, Button color states, RectTransform, and child TMP label. These are scene-owned settings; changing this button does not propagate to other buttons. Keep existing parents and sibling order intact.

Still required: physical mouse and keyboard interaction, full pipetting/timer/heat-shock/recovery/spreading paths, hidden apparatus states and all source-label variants, actual-photo dialog open/close/focus, rendered hit-area checks, Dashboard-driven re-entry with the persistent overlay, remaining revamp stages, and the desktop player build/smoke test.

### akt4 photo dialog and later interactions — 2026-11-08

This follow-up implements the photo-dialog checks left open above and exercises representative later interactions. Gameplay code, scientific quantities, timing rules, and existing button callbacks remain unchanged.

- [x] Restyle `Canvas/Experiments/Content/5 (4)/Result` with an opaque workspace surface, navy heading, aspect-preserving original `resultv4.png` photo, and a labeled neutral `Tutup` action. The selected close button uses the existing cyan selection color and navy focus outline. Its 240×88 reference-unit area renders at about 128×47 pixels at 1024×768.
- [x] Add a nested Canvas with **Override Sorting enabled, order 100**, a GraphicRaycaster, and a CanvasGroup with **Ignore Parent Groups enabled**. Reuse `UiFocusScope`, with the close button as Initial Selection and Block Other Canvases enabled. This blocks the underlying scene and persistent assistant while keeping the modal interactive. The underlying root Canvas now has an explicit CanvasGroup.
- [x] Keep focus on the modal when its background is clicked: `Result/bg` has a Pointer Down EventTrigger calling the scene EventSystem's `SetSelectedGameObject` with `Result/Button`. Verify initial close focus, background-click refocus, submit-to-close, and focus return to the photo opener. Add the existing focus indicator to the opener too.
- [x] Inspect the photo dialog at all six required resolutions. Captures: `Temp/qa_screens/photo-dialog-<width>x<height>.png` and `photo-dialog-sheet.jpg`. The photo and heading fit without overlap. Preserve the photo asset; only its Image layout changes.
- [x] Verify native Windows Enter closes the selected `Tutup` control after the Game view receives input focus, with focus returned to `Hasil sebenar`. The first Enter attempt before focusing the Game view did nothing. This is a focused native-input check, not full keyboard coverage; Escape-to-close has not been added.
- [x] Return to Dashboard, invoke its existing Aktiviti 4 and Mula eksperimen callbacks, then activate the final page for the modal check. One EventSystem remains. During the modal, CanvasPrime (sorting order 3) and the lesson Canvas are non-interactable, while the Result Canvas (order 100) remains interactive. Submit-to-close restores both underlying groups and the opener selection. Capture: `photo-dialog-dashboard-entry.png`. Entry routing was programmatic, not a physical mouse walkthrough.
- [x] In `3 (1)` at 2560×1080, move the five active `plasmid2` pipettes into the tube collider through the existing drag handlers. Unity's actual 2D trigger callbacks advance `TriggerController.turnCount` to 5, show Correct and Next, and allow progression to `4`. No collision callback or completion state was directly forced. Capture: `ligasi-pipetting-completed.png`.
- [x] Exercise the existing tube actions and progression through incubation, cell thawing, DNA addition, heat shock, and recovery. Observe the active displays reach 16°C / 1:00 for ligation incubation, 0:30 after DNA addition, 0:45 in the heat-shock water bath, and 37°C / 1:00 in recovery. Captures: `ligation-incubation-after-action.png`, `dna-addition-timer.png`, `heat-shock-water-bath.png`, and `recovery-active-timers.png`. These checks verify displayed states and callbacks, not scientifically correct timing.
- [x] In `5 (3)`, use repeated drag-handler movements into and out of the plate trigger. The existing trigger increments the display to 100%, reveals the confirmation action, and its callback reveals Next; Next advances to the final result page. Capture: `spreading-100-percent.png`.

Existing gameplay defects reproduced, left outside the presentation change:

- `ConditionalCheck.Start()` uses `Int32.Parse(transform.name)` at line 37 and throws on names such as `3 (1)` and `5 (4)`. A fresh runtime log listener reproduced this; it is not only an old Console entry. Do not rename these steps to suppress the error: other code depends on names/order.
- Several actions reveal Next before their timers finish. The heat-shock water-bath Next callback reveals the final step-advance button without passing through the existing return-to-ice branches described by the instruction. Those callbacks and timings were preserved.
- The incubation instruction permits overnight at 16°C or one hour at room temperature; the exercised display shows 16°C together with 1:00. The timer units/meaning and this combination need science/gameplay review.
- Recovery still says `(In progress)` and unused/hidden callback branches include null targets. This pass does not establish those branches as functional.

Human editing: the dialog stays under `5 (4)/Result`; edit its `text` Image/TMP, `Image`, and `Button/Label` in the Inspector. Keep the Canvas sorting and CanvasGroup settings above together: resetting Override Sorting exposes the page navigation above the dialog. Keep the bg EventTrigger's EventSystem and close-button references valid. `Result/Button` retains its existing `Result.SetActive(false)` callback; the opener retains `Result.SetActive(true)`. New components reuse the existing scene-owned UI scripts; no new C# file or package was introduced. A pre-edit scene copy is at `Temp/qa_screens/akt4-before-modal.unity`.

Final source checks: all 117 existing Button callback blocks match the pre-edit scene copy; no existing serialized object IDs were removed. The scene reports zero missing scripts. The final post-Play-Mode scene copy matches the saved scene, and akt4 is left saved outside Play Mode. Document whitespace checks passed. No build or automated test run was performed for this scene-only change.

Remaining verification: full mouse drag workflows, all alternate/hidden branches and early-step pipetting, broader keyboard navigation, review/fix of the gameplay defects above, the other experiment/quiz scenes, and desktop build/player smoke testing. The whole revamp remains incomplete.


### Gameplay repairs and remaining UI rollout — 2026-11-08

The user deferred akt4's remaining physical walkthrough and authorized gameplay repairs, the other screens, and shared UI. The earlier unchecked walkthrough/player tasks remain open.

Implemented source changes:

- `ConditionalCheck.Start` reads the numeric part of Unity-suffixed page names (`3 (1)`, `5 (4)`) and preserves the Inspector value for nonnumeric names.
- `CheckContent` requires every configured item, ignores optional null output entries, and reveals outputs once rather than reopening controls every frame. Its existing `toDisappear` field retains the active-state meaning used in these scenes.
- `TimerText` clamps completion after long frames, supports an optional simulated duration, reveals configured Completion Targets, and invokes On Completed once. Existing public timing fields remain serialized. Temperature displays can approach targets from either direction.
- `NextExperimentController` resolves the next sibling from its own page and honors explicit page references.
- akt4's timed stages reveal Next on timer completion. The heat-shock water-bath action waits for 45 simulated seconds, follows the existing transfer/return-to-ice apparatus, and waits for 120 simulated seconds on the ice clock before advancing. Recovery's existing 250 µL action starts the 37°C/timer presentation; Next waits for its endpoint. The duplicate recovery action is hidden, and null callbacks on that recovery page are removed.
- `QuestionController` exposes answer/feedback colors and an optional feedback label. Correct answers receive a text prefix as well as color. Existing answer data, scoring, locking, and coroutine delays are retained.
- The existing assistant request now shows loading/empty/error status and restores input in `finally`. Blank submissions are ignored. Model/account configuration and tutoring scope are unchanged; no live request is used for QA.
- `UiFocusScope` keeps modal keyboard selection inside the dialog and supports an Inspector-assigned Cancel Button for Escape when the legacy input manager is enabled.

Scene/layout rollout:

- LoginPage is an entry screen: its existing Login callback routes directly to Dashboard, and Register has no callback. The unused email/password/register controls are retained but inactive. The visible action is `Masuk makmal`.
- Dashboard's remaining activity menus and lessons use LiberationSans, navy reading text, neutral reading surfaces, and Malay navigation labels. Presentation-only lesson reveal Animators (70, with no animation events) are sampled at their final poses and disabled; their clips remain editable. Experiment Animators are not disabled by this change.
- Experiments akt3/akt5/akt6 have shared instruction/header/footer treatments without moving apparatus children. akt7 is handled separately because its procedure pages are direct children of Experiments. Existing content and callback routes remain the source of truth.
- Uji Diri 1–7 have an opaque question/answer reading panel, consistent answer controls and focus, Malay navigation, and modal quit/result dialogs with labeled actions.
- `Assets/UI/Science/Prefabs/LearningAssistant.prefab` owns the shared assistant presentation. Dashboard uses this connected prefab, and experiments/quizzes instantiate it for direct scene entry. The existing DontDestroyCanvas behavior handles scene navigation duplicates. It adds no EventSystem.

Verified so far:

- Unity compilation completed after the C# edits with zero Console errors at the checks performed.
- Play Mode callback checks: heat-shock Next remains hidden during the water-bath and ice waits; the ice display reaches `2:00` before Next appears. Recovery starts with Next hidden and reaches `1:00` / 37°C before it appears. These are callback checks, not a physical drag walkthrough or a curriculum timing sign-off.
- Ten targeted Editor regression assertions passed: four procedure-name cases, all-items-required evaluation, optional null outputs, one-shot outputs, timer overshoot, cooling to a lower temperature, and next-page resolution with another sibling active. Evidence: `Temp/qa_screens/gameplay-regression-checks.txt`. These are direct Editor assertions, not a Unity Test Runner suite.
- Quiz callback check: answer input locks, one correct response awards 10 points, then the next question appears with controls enabled. One EventSystem and one persistent overlay remain; the result dialog blocks the underlying lesson and assistant.
- Saved-scene visual QA began with 191 states across 1920×1080, 1280×720, 1280×800, 1024×768, 1280×1024, and 2560×1080. Captures and overflow/hit-area findings are under `Temp/qa_screens/review-*` and `revamp-layout.tsv`. An initial pass exposed delayed lesson reveals and unrecorded prefab presentation overrides; those were corrected before restarting the pass. A capture alone is not a visual pass or gameplay verification. The follow-up below supersedes this initial capture status.

Human editing: page layout/text remains in each original scene. Edit timer duration, multiplier, Completion Targets, and On Completed on the relevant TimerText component; the new heat-shock ice timer is at `Canvas/Experiments/Content/5 (1)/Content (1)/table/petri dish (7)/clock`. Edit shared assistant layout/status text in LearningAssistant.prefab. Quiz answer colors and feedback reference are on QuestionController; question/answer content remains on QuestionList. UiFocusScope owns Initial Selection and Cancel Button for each modal. Scene-specific prefab overrides are intentional where the older dialog/menu prefabs are retained.

Pending: finish the saved-layout review and corrections, inspect all actual interaction/hidden states, full quiz scoring/retry routes, assistant network failure/retry behavior, broad keyboard/mouse walkthroughs, and a desktop build/player smoke test. No whole-revamp completion is claimed.

### Resumed QA and final corrections — 2026-11-09

The requested screen rollout and gameplay repairs are implemented. The whole revamp remains open until the manual interaction and player checks below pass. akt4's physical walkthrough remains deferred at the user's request.

- [x] Recover the completed 220-state capture run: `revamp-final-layout.tsv` contains 1,320 checks at the six required resolutions. Inspect its 11 contact sheets and follow up on the subsequent saved corrections. These initial captures include faults corrected later; do not use them as the final visual baseline.
- [x] Increase the height of the technology explanation in Dashboard lesson paths `1/1/content (7)` and `2/1/content (7)`. A focused pass of these lessons, akt3 readouts, and the akt6 soil action reports zero TMP overflow in 90 checks (`resume-layout.tsv`). TMP meshes were refreshed before reading overflow flags. Initial captures in that pass accidentally left Overview visible beneath the two lessons; their corrected isolated captures are in the next pass.
- [x] Recheck the 29 existing Dashboard lesson dialogs, the two technology lessons, the rubber lesson text, and seven akt7 states at six resolutions: 234 checks, zero TMP overflow (`resume-final-layout.tsv`). Inspect the three `resume-sheet-*.jpg` sheets and representative full-size captures. The first akt7 setup hid shared decoration; `akt7-verified-*` supersedes those seven states and retains the shared background, header, and navigation (42 checks).
- [x] Separate akt7's final additional instruction from its result action. The instruction uses a left reading column; only the final static apparatus group is scaled to fit the bench. No drag objects or collider transforms are changed in this correction. Correct copied entry headings to identify experiment 7, without replacing the existing reagent lists or placeholder illustrations.
- [x] Give akt7's existing result photo an opaque reading surface, aspect-preserving Image, labeled `Tutup`, initial/cancel focus, and underlying input blocking. Keep this world-space dialog within its own 1920×810 canvas bounds. Its close action sits at the upper left, clear of the persistent assistant. The assistant remains visible but blocked while the dialog is open. Callback checks verify focus enters the dialog, close restores the opener, and underlying interaction is restored at all six sizes (`akt7-polish-checks.tsv`). `akt7-final-page-*` and `akt7-final-modal-*` are the final captures; their 12 checks report no TMP overflow.
- [x] Verify the saved seven-quiz callback runs: correct runs score 50 for quizzes 1–6 and 60 for quiz 7; incorrect runs score zero. Each run reports one EventSystem and one assistant, with zero recorded runtime errors (`quiz-route-checks.txt`). These are programmatic checks, not mouse walkthroughs.
- [x] Make quiz waiting states explicit. `QuestionController` retains its existing `Button.enabled` lock and also updates `interactable` for disabled tint/navigation, with `Sedang menyemak jawapan...` in the feedback label. A correct answer followed by a duplicate Submit event awards 10 once; the next question restores enabled/interactable controls and clears the status (`quiz-lock-regression.txt`). Scoring rules and delays are unchanged.
- [x] Force two assistant failures locally by temporarily removing its API client in Play Mode. Both attempts display the Malay error and restore Send/input; blank input is ignored (`assistant-offline-checks.txt`). The original client is restored. This exercises the exception path without network traffic; loading, real HTTP failures, empty responses, and successful online replies are not verified by this check.

Screenshots and temporary evidence remain under `Temp/qa_screens/`. Pages are activated programmatically; overflow checks do not establish usable hit areas, scientific accuracy, or correct apparatus interactions. Some akt7 prestart illustrations still carry an existing placeholder notice and copied apparatus imagery. Those assets and scientific terminology need content review; no replacement scientific content is invented here.

Human editing: open Dashboard and expand `Canvas/Container/Into/Content` for lesson text and dialogs. In akt7, final instructions are `Canvas/Experiments/4/Text (TMP) (2)` and `(3)`; the new reading background is `ScienceProcedureNote`, the static apparatus scale is on `Content/table`, and the photo is `Result/Image`. Keep `Result`'s Canvas, GraphicRaycaster, CanvasGroup, and UiFocusScope together. Edit shared assistant controls in `Assets/UI/Science/Prefabs/LearningAssistant.prefab`; its button states are inherited unless an instance has an explicit override. Quiz colors and feedback reference remain on QuestionController, and question content remains on QuestionList. Layout edits require checking the actual scene because shared code does not propagate scene-owned sizing.

Remaining checks: full mouse/keyboard workflows and hidden apparatus branches, physical quiz retry/return interaction, online assistant behavior, grayscale/color-vision review, and the complete desktop player walkthrough. The native Windows automation connection returned `Computer Use native pipe is unavailable: failed to connect native pipe: The system cannot find the file specified. (os error 2)` on repeated attempts. No standalone mouse/keyboard pass is claimed.

Build and startup verification: the Windows64 Mono development build succeeded at `Temp/ScienceUIBuild/Evergreen.exe` with all 14 enabled scenes, zero errors, and five warnings (90.35 seconds; 415.66 MB). Warnings concern two deprecated object-search APIs in UiFocusScope, the assistant's existing nonserialized field types, and a TMP shader debug pragma. No package or shader source was changed to suppress them. The player launched with 1280×720 window arguments, remained running, and initialized Unity 6000.6.7f1 and its graphics device. Its startup log contained no exception/error/assertion matches. The test process was then stopped. Evidence: `windows-player-startup-check.txt` and `windows-player-startup.log` under `Temp/qa_screens/`. This is a startup check only; it does not satisfy the full player navigation/experiment/quiz/exit requirement.

Final checks: akt7 is saved outside Play Mode and reports zero missing scripts. Modified gameplay C# passes `git diff --check`; this document has no trailing whitespace or missing Markdown link targets. No Unity Test Runner suite or complete accessibility sign-off is claimed.

### Quiz interaction verification - 2026-11-09

The user requested skipping mouse/keyboard testing and proceeding with quiz interaction. The broad physical input walkthrough is now skipped by request, rather than waiting on desktop automation. The checks below use Unity UI EventSystem Submit events in Editor Play Mode; they do not establish physical input or standalone player behavior.

- [x] Complete all seven quizzes with correct answers and again with incorrect answers. Correct totals are 50 for quizzes 1-6 and 60 for quiz 7; incorrect totals are zero. Every answer advances exactly one question and restores input for the next question.
- [x] Send a duplicate Submit immediately after every answer. Disabled answer controls reject it, with no extra score or question advance.
- [x] Verify result dialogs open, own selection, and block underlying answers. Check displayed score totals against each quiz's actual question count.
- [x] Activate `Cuba lagi` in every quiz through Submit. The same quiz reloads at question 1, step 0, with zero score and its result dialog hidden. Complete the retried quiz to verify its callbacks and score reset.
- [x] Exercise `Kembali` then `Teruskan` in every quiz; the dialog closes and the current quiz continues. Exercise both the result's `Laman utama` action and the quit confirmation's `Laman utama` action in every quiz; both reach Dashboard with one EventSystem and one persistent assistant.
- [x] Inspect the quiz 1 result screenshot at the current 2560 x 1080 Game view. The displayed zero score, result actions, and focused retry control are readable. Unity Console inspection after the runs returned zero errors or warnings.

Evidence: `Temp/qa_screens/quiz-interaction-checks.txt` records both completed passes and the supplementary score/quit checks with no failed assertions. `quiz-interaction-result.png` captures quiz 1's result. Scaled waits were accelerated with a temporary Play Mode time scale of 10; the original value of 1 was restored. These are temporary in-memory checks, not a Unity Test Runner suite; no gameplay code, quiz data, scene, or prefab was edited for this verification.

This supersedes the earlier pending quiz retry/return callback checks. Physical mouse/keyboard testing remains skipped by user request. Remaining revamp work is online assistant verification, grayscale/color-vision and hit-area review, akt7 scientific content review, and the rest of the standalone player walkthrough within the revised testing scope. Earlier deferred apparatus branches remain unverified.

### Assistant response verification - 2026-11-09

Changed the owned `Assets/Samples/OpenAI Unity/0.2.2/ChatGPT/ChatGPT.cs` component. Its existing Send event now delegates to a task-returning request method, allowing local responses to exercise the actual UI handling without changing the configured model, prompts, API account, or SDK package. API error payloads now show `Jawapan tidak dapat dimuatkan. Cuba lagi.` rather than being reported as an empty reply.

- [x] Verify delayed loading, successful replies, null choices, empty choices, blank reply text, a simulated API error payload, a simulated transport exception, successful retry, and a long reply. Loading disables Send/input; duplicate requests are ignored; completion restores both controls. Successful replies are trimmed and displayed; empty and failed replies use their distinct Malay status messages.
- [x] Check the actual Send listener in fresh Play Mode with blank input and a temporarily unavailable client, restoring the original client afterward. Close the assistant during a delayed local request, navigate Dashboard to quiz 1, complete the request while hidden, and reopen it. The reply is retained and one persistent assistant remains.
- [x] Inspect loading, error, and long-reply captures at the current 2560 x 1080 Game view. Long conversation content exceeds the viewport and can be moved to the bottom through ScrollRect. This is programmatic scrolling, not physical input testing or a new six-resolution layout pass.
- [x] Validate the changed script with zero diagnostics, wait for successful Unity compilation, inspect Console with zero errors/warnings, and run the focused C# whitespace check.

The final evidence has 76 passing assertions and no failures: `Temp/qa_screens/assistant-response-checks.txt` (69) and `assistant-entry-checks.txt` (7). Loading/error/long-reply PNGs are in the same directory. An initial exploratory run found no Send listener in that session; a fresh Play Mode session had one listener, passed the real Send checks, and retained one listener after the response/navigation checks. The initial evidence is retained as `assistant-response-checks-initial.txt`; no cause or production fix for that transient observation is claimed.

Human editing: shared controls, message templates, and the status label remain in `Assets/UI/Science/Prefabs/LearningAssistant.prefab`; request/status handling remains in the existing ChatGPT component above. Local test responses and request delegates existed only in memory and were not saved into assets. No new package or test assembly was added.

Limits: these checks make no live API requests and do not verify HTTP handling, model/account access, online reply quality, curriculum accuracy, or request timeout behavior. The SDK endpoint is hardcoded and its transport has no timeout in the inspected request path. Live service verification therefore remains open. The pre-existing source also contains an embedded API credential; it was not used or copied into this evidence. Credential rotation and moving production requests behind a server remain separate service/security work. The previous standalone build predates this small assistant correction and was not rebuilt in this pass.

### Activity 7 content cleanup - 2026-11-09

The user has no approved lab protocol and requested typo and other clear corrections. This pass edits only `Assets/Scenes/Experiments/akt7.unity`; it does not establish the scientific validity of the recipe or waste-capture method.

- [x] Correct the swapped apparatus/materials headings, `getah ahli` to `getah asli`, `Slaid kacak` to `Slaid kaca`, unit spacing/capitalization, and the calcium carbonate subscript. Complete the entry title, remove an unclosed cyan markup tag from the Flory-Rehner instruction, and replace the copied voltage heading with the existing waste-capture task. Label the existing photo as an observation reference rather than a measured simulation result.
- [x] Hide unrelated burner, swab, glove, rod, and pipette illustrations on the preparation pages. Reuse the existing oven, beaker, and microscope sprites for matching apparatus. An incomplete white stirrer silhouette was also hidden after visual review. Notices state where illustrations are unavailable. Original object names, child order, sprite assets, and gameplay objects remain intact.
- [x] Fit the 13 apparatus entries and 10 materials into readable two-column lists using their existing RectTransforms. Disable the old horizontal layout and apparatus ScrollRect because all entries now fit on their page. The first capture exposed clipping that TMP overflow checks missed; the corrected pages were recaptured and inspected.
- [x] Check seven states at 1920 x 1080, 1280 x 720, 1280 x 800, 1024 x 768, 1280 x 1024, and 2560 x 1080: 42 checks, no TMP overflow. Inspect six contact sheets and representative full-size preparation captures. Refresh all six apparatus captures after hiding the stirrer silhouette.
- [x] Verify the existing Start callback opens the procedure directly. The preparation pages remain reference content bypassed by that route; their existing Next callbacks still connect apparatus to materials to procedure. Seven navigation/content assertions pass. No new route was introduced.

Evidence: `Temp/qa_screens/akt7-content-before.json`, `akt7-science-content-changes.txt`, `akt7-science-layout.tsv`, `akt7-science-navigation.txt`, and `akt7-science-*.png`. Contact sheets are `akt7-science-sheet-*.jpg`. Pages were activated programmatically; no physical input, apparatus workflow, or new player build was tested.

Human editing: open akt7 and expand `Canvas/Prestart Bahan2` for apparatus and `Canvas/Prestart Radas` for materials. Their legacy object names are retained. Edit the existing TMP children under `Content/Image/Radas`; each cell uses ordinary anchors, with the old layout component disabled. The three retained icons remain ordinary Image sprite assignments. Procedure text stays under `Canvas/Experiments`; the result illustration is the existing flattened `Assets/Scenes/Experiments/result7.png`, whose embedded captions were not rewritten.

Remaining content review: confirm the stated reagent forms and concentrations (including ZDEC, ZnO, and sulfur), recipe quantities and mixing/curing conditions, sample dimensions, swelling equilibrium and inputs needed for Flory-Rehner calculations, and the evidence/conditions for waste capture. Existing lesson and quiz claims were not rewritten without a reference. Some procedure labels remain small, and the pre-existing step 3 upper instruction is partly obscured at ultrawide; zero TMP overflow is not a full layout/accessibility sign-off. This task completes the requested clear corrections, not curriculum approval or a replacement scientific protocol.

### Proposed experiment visual consistency pass - 2026-11-09

Status: plan only, requested after the user supplied an experiment screenshot showing a large white instruction header, a heavily darkened illustrated lab, bright apparatus cues, and a disconnected bottom strip. No scene, prefab, or gameplay change is implemented by this plan. This section is the authoritative next visual pass within the existing revamp plan.

Read-only preview-scene inspection confirms three background assets across akt3-akt7: `Assets/UI/bgLab.jpg`, `Assets/pictures/labg2.jpg`, and `Assets/UI/bglab3.jpg`. Their tints differ even within a scene: akt3's procedure background uses #262626, akt5 uses #5E5E5E, akt6's procedure uses #332F2F, and akt7 uses #575555. akt4 has a different workspace treatment. The prior header/text rollout did not establish a consistent apparatus workspace. Preserve the current Canvas render modes and scaler settings, including akt6's 0.515 match and akt7's camera-backed Canvas.

Proposed direction: a bright working laboratory with a quiet apparatus bench. Use #F2F6FA for the common workspace, white for opaque reading surfaces, #0B0B3B for text and the compact navigation header, #3D5AFE for the primary action, and #4FC3F7 with a navy edge for selection/focus accents. Keep the existing lab artwork as secondary context on introductions; remove the detailed full-screen illustration from directly behind working apparatus by covering it with the shared opaque bench surface. Do not darken the whole experiment to make apparatus stand out. Dark scrims belong to open dialogs only.

- [ ] Audit visible backgrounds, tints, CanvasGroups, masks, material colors, selection cues, and runtime overrides in all five experiments, including preparation, active, waiting, completion, and photo states. Record which bright colors represent substances or electrical conventions and which are purely interface cues.
- [ ] Build a reviewable akt6 slice: its introduction, the titanium-wire/anode step shown by the user, one later step, and completion. Use a compact navigation row, a distinct instruction region, a quiet bench, and an action footer only where actions exist. Start around 96-112 reference units for navigation; let instruction height follow content. Check actual apparatus clearance before setting final dimensions. Keep the assistant in reserved navigation space. Make the detached bottom strip serve real controls or remove its decorative background.
- [ ] Standardize apparatus labels, instruction text, neutral Back/Close controls, primary actions, disabled states, and completion cues. Use LiberationSans and the existing project type/spacing guidance. Keep labels on opaque light surfaces. Change neon cues only where they are presentation, preserving scientific colors and simulation meaning. Pair success/selection colors with text or shape. Review the akt6 Game-view slice before propagating it.
- [ ] Reuse a small Inspector-editable background/header/button treatment under `Assets/UI/Science/` where appropriate. Keep gameplay-bearing objects in their current parents and order. Add decorative surfaces behind them without raycast interception; do not wrap drag roots in layout groups or change their scale to fit a template. Keep scene-specific apparatus layouts and existing sprite sources.
- [ ] Apply the proven treatment to akt3, akt5, akt7, then akt4, checking each scene independently. Cover every procedure page and preparation/result dialog. Fix akt7's known ultrawide instruction overlap in this pass. Preserve all scene routes, apparatus callbacks, timers, scoring, and curriculum content. Retain per-scene differences needed by the experiment rather than forcing identical apparatus positions.
- [ ] Verify every affected state at 1920 x 1080, 1280 x 720, 1280 x 800, 1024 x 768, 1280 x 1024, and 2560 x 1080. Inspect screenshots for clipping, overlap, apparatus/background separation, readable labels, transparent apparatus edges, focus, and assistant collisions; TMP overflow alone is insufficient. Check normal text at 4.5:1, large text/essential boundaries at 3:1, and selection/result meaning in grayscale and color-vision simulations.
- [ ] Run available programmatic checks for representative drag/drop callbacks, invalid actions, transitions, timing gates, completion, and return routes after each scene. Physical mouse/keyboard testing remains skipped by the user's earlier request; do not report callback checks as a physical walkthrough. Inspect Console, missing scripts/references, persistent assistant ownership, and build/startup after the integrated pass. Record actual verification limits here.

Acceptance: all five experiments share the same background policy, navigation/instruction treatment, label styling, action colors, and dialog scrim rules. Detailed scenery no longer competes with working apparatus. No essential content is obscured at the six target resolutions, and programmatic behavior checks show no new regressions. Human editors can change the owned scene/prefab Inspector settings without runtime screen construction. Full scientific validation and assistant service changes remain outside this visual pass.

Recovery: retain the original art assets, .meta files, GUIDs, callbacks, and hierarchy contracts. Make scene changes in small batches so a failed visual slice can be reverted without rolling back unrelated gameplay fixes. Do not regenerate all scenes or replace their controllers to apply the style.
