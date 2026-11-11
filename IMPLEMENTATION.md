# Science UI Revamp — Implementation Plan

Status: implementation in progress. Live baseline inspection has started; the full revamp is not complete. Verified changes and remaining checks are recorded below.

Updated: 2026-11-09. This is the authoritative plan for the full science UI revamp. [AGENTS.md](AGENTS.md) defines the lasting design and development rules. Keep progress and evidence here during implementation; do not treat a written task as completed work.

Next proposal (2026-11-10, not approved or started): [UI_REVAMP_PLAN.md](UI_REVAMP_PLAN.md) covers wayfinding, a colour system with meaning, and game feel across all screens. Record its evidence here once phases begin.

UI_REVAMP_PLAN progress (2026-11-10): Phase 0.4 done. The cyan answer on load came from the `Question` `UiFocusScope.initialSelection` (answer A) combined with the answer buttons' cyan `selectedColor`. Phase 1.2 done in the Uji Diri 6 pilot only: `initialSelection` cleared in the scene, and `QuestionController.applyQuestion()` now writes "Soalan N daripada M" (applies to all 7 quizzes). Play Mode check: no selection on load, label "Soalan 1 daripada 5", no Console errors; capture `Temp/qa_screens/scan/10-quiz6-after.png`. Not done: the same `initialSelection` fix in Uji Diri 1-5 and 7, the rest of Phase 1, physical mouse/keyboard check, other resolutions.

UI_REVAMP_PLAN Phase 1 pilot, Uji Diri 6 (2026-11-10, saved). New scripts under `Assets/UI/Science/Scripts/`: `UiMotion` (reduce-motion key `ui.reduceMotion` in PlayerPrefs; no settings toggle exists yet), `UiFeedbackMotion` (pop and wobble using scale and rotation only), `QuizProgressPills`. Changed: `QuestionController` (records the clicked option; the picked answer is tagged "Pilihan anda:" or "Pilihan anda · Betul:"; records each result in the pills), `ScoreController` (400 ms count-up plus a coin pop, skipped when reduced motion is on), `loadScore` (result line "N daripada M betul (S / X mata)" plus a message from the plan's wording bands; wording is a proposal and awaits owner confirmation). Scene: new `Canvas/QuizProgress` (6 pills, hidden beyond the question count, placed below the question panel and before `QuitPrompt` in sibling order); `UiFeedbackMotion` on the four answer buttons and the coin icon; the coin block re-anchored to its container's top-right; the old "Pilih tindakan seterusnya." line now holds the result message. Play Mode run at 8x speed with a scripted pattern (wrong, right, wrong, right, right): pills read 1 Salah, 2 Betul, 3 Salah, 4 Betul, 5 Betul; the score ended at 30 and the result showed "3 daripada 5 betul (30 / 50 mata)". No Console errors. Captures: `Temp/qa_screens/scan/11-quiz6-wrong.png`, `12-quiz6-result.png`. Known: the coin's dark score bar still looks cut off at the right edge in the capture (its sprite appears wider than its box); the old question panel shows through the result dialog's edges (existing layout); "Cuba lagi" starts with the cyan focus fill. Not tested: other quizzes (the three C# changes also run there, but their scenes have no pills, motion components or message label wired), physical input, six resolutions, reduce-motion on.

Known-issue fixes, Uji Diri 6 (2026-11-10, saved, Play Mode captures `Temp/qa_screens/scan/13`-`17-*.png`, no Console errors): coin block no longer uses a layout group, is scaled 0.7 and anchored top-right, score text 39 bold white; `CongratPrompt` and `QuitPrompt` panels anchored to (0.15,0.12)-(0.85,0.86) so the question panel no longer shows through, title band stretched full width; dialog buttons use `selectedColor` = `normalColor`, so focus shows only the navy `UiFocusIndicator` outline. Not captured: QuitPrompt visuals (same changes), physical input, the six resolutions. "Laman utama" is still the only primary button on the result dialog.

Phase 1 rollout (2026-11-10, saved): the Uji Diri 6 setup was applied by script to Uji Diri 1-5 and 7: `initialSelection` cleared, `QuizProgress` pills, `UiFeedbackMotion` on answers and coin, `coinMotion` and `messageLabel` wired, coin re-anchored (layout group removed, scale 0.7, score text 39), dialog panels and title bands re-anchored, dialog `selectedColor` = `normalColor`. Play Mode: quiz 7 scripted run (wrong, right, wrong, right, wrong, right) gave 6 pills "Salah/Betul", result "3 daripada 6 betul (30 / 60 mata)", capture `Temp/qa_screens/scan/18-quiz7-start.png`. Quizzes 1-5 start-state check: no selection, "Soalan 1 daripada 5", 5 pills. No Console errors. Not done: full playthrough and result capture for quizzes 1-5, physical input, six resolutions, reduce-motion on.

Quiz verification pass (2026-11-10), Play Mode, scripted input (not physical mouse/keyboard):
- Full scripted playthroughs (wrong, right, wrong, right, ...) on quizzes 1-5 and 7 (quiz 6 earlier): pills read Salah/Betul, score and result line "N daripada M betul (S / X mata)" with the band message were correct. Captures `Temp/qa_screens/scan/21`-`23-*result.png` and `19`. A poll that read the dialog on its first frame saw the old text because `loadScore.Start` had not yet run; a later read was correct (not a product bug).
- Found and fixed: the rollout had not persisted the dialog buttons' `selectedColor` (still cyan), so "Cuba lagi" opened cyan. Re-applied through `SerializedObject` in all seven scenes and re-read from disk: all `FFFFFF`. Capture shows white fill with navy ring.
- Found and fixed: at 4:3 and 5:4 the coin icon overlapped the question panel's top-right corner (the right margin is a fixed share of screen width). Coin scale 0.7 -> 0.55 and score font 39 -> 43 in all seven scenes. Re-checked at 1024x768, 1280x1024 and 2560x1080: clear of the panel.
- Game view sizes checked on quiz 7 (question, answered, result dialog where noted): 1920x1080, 1280x720, 1280x800, 1024x768, 1280x1024, 2560x1080 (`30`-`32-q7-*.png`). Layout, 6 pills and long Malay answers fit at every size; the answer with three lines at 1280x1024 stays inside its button. The dialog was checked at 1024x768 and 2560x1080 only.
- No Console errors.
- Physical mouse and keyboard (clicking through a quiz, tabbing answers, Escape on the quit dialog): checked by the project owner, who reported everything working (owner report, no capture).
- Still not tested: reduce-motion on, quizzes other than 7 at the non-default sizes, the QuitPrompt visuals.

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

Status: **plan only, revised after a read-only akt6 inspection.** Requested after the user supplied an experiment screenshot (akt6, first procedure page). No scene, prefab, or gameplay change has been made for this pass. This section is the authoritative next visual pass within the existing revamp plan. It supersedes the earlier draft of this section, whose bright-bench direction conflicts with the apparatus art (see "Bench decision").

#### What the akt6 screenshot shows

Source: the user's screenshot (Editor Game view, about 2000 × 1160) and a read-only Unity hierarchy query of `akt6` on 2026-11-09. The Game view was not captured by this pass, and `Temp/qa_screens` no longer exists, so earlier evidence images are gone. Percentages are estimated from the screenshot.

| # | Observation | Why it hurts |
|---|---|---|
| 1 | The white top band takes about 26% of the window height and holds four controls plus the instruction. Kembali, "Langkah 1", the assistant button and Uji diri sit at three different heights and sizes, and the assistant button ends just above the instruction line and spans the end of its text. | Hard to scan; the bench loses space; the assistant crowds the instruction. |
| 2 | Numbering disagrees: the header says "Langkah 1", the instruction starts with "2.", and the footer says "1. Sediakan balang". | Learners cannot tell which step they are on. Which number is real is unverified. |
| 3 | The full-bleed lab illustration is tinted dark (akt6's procedure tint is recorded as `#332F2F`) with shelves, flasks and a monitor drawn behind the apparatus, and visible through the jar. | Scenery competes with the apparatus; the palette is a muddy brown-grey, not the navy identity. |
| 4 | The jar is white line art with an orange lid, drawn for a dark backdrop. | It rules out a light bench unless the art changes (see "Bench decision"). |
| 5 | The floating "CABUT PENUTUP BALANG" label is all caps, in a white box away from the lid. | Unclear whether it is a hint, a drop target or a button. Unverified. |
| 6 | The pale footer strip is detached, carries one line and no controls, and the jar's base overlaps it. | Decoration that costs space and collides with apparatus. |
| 7 | Thin dark gutters show at the left and right of the white bands. | The bands appear inset from the canvas edges. Anchors unverified. |
| 8 | Kembali, Uji diri and the assistant button are plain white rectangles with thin grey borders and no visible states. | They do not match the Dashboard or akt4 treatment. |

#### Live structure of akt6 (read-only)

- Scene roots: `Main Camera`, `Canvas`, `SideMenu` (inactive), `EventSystem`, `LearningAssistant` (an in-scene instance of the persistent assistant with `DontDestroyCanvas`).
- `Canvas` children: `Welcome` (active), `Prestart Bahan2`, `Experiments`, two scene-change buttons, one disabled text.
- `Canvas/Experiments` children, in order: `bg`, `ScienceFooterShared`, `ScienceHeaderShared`, `Content`. The white band, the footer strip and the dark background are **three shared Image objects serving all 11 step pages**, left by the earlier header/footer rollout. Restyling them once changes every akt6 page.
- `Content` has 11 pages named `0`, `0 (1)` to `0 (11)`, each with `ConditionalCheck` and `UiFocusScope` and 5 to 7 children. The instruction text lives inside each page, not in the shared header, so moving it means editing each page's text rectangle. Page names must not change because `ConditionalCheck` reads them.
- Gameplay scripts in the scene (component counts): `DragController` 96, `ItemSlot` 32, `CheckOnEnabled` 31, `ConditionalCheck` 11, `NextExperimentController` 11, `SceneController` 2, `DisplayItem` 1. These are the contracts to protect; the `AGENTS.md` hierarchy rules apply to each.
- Not read: the colors and sprites on `bg`, the header and the footer, because the tools used here cannot return component properties. Task 0.4 reads them.

Read-only preview-scene inspection confirms three background assets across akt3-akt7: `Assets/UI/bgLab.jpg`, `Assets/pictures/labg2.jpg`, and `Assets/UI/bglab3.jpg`. Their tints differ even within a scene: akt3's procedure background uses #262626, akt5 uses #5E5E5E, akt6's procedure uses #332F2F, and akt7 uses #575555. akt4 has a different workspace treatment. The prior header/text rollout did not establish a consistent apparatus workspace. Preserve the current Canvas render modes and scaler settings, including akt6's 0.515 match and akt7's camera-backed Canvas.

Direction: a working lab bench with notebook-style reading surfaces. Every experiment page has the same three zones: a navy navigation bar, a white instruction band, and one flat bench with nothing decorative behind the apparatus. Use #0B0B3B for the bar, #FFFFFF for the instruction band, #3D5AFE for the primary action, and #4FC3F7 with a navy edge for selection and focus. Keep the existing lab artwork as context on the introduction pages only. Dark scrims belong to open dialogs only.

#### Bench decision (gate before any rollout)

The earlier draft proposed a bright `#F2F6FA` bench. The akt6 jar is white line art, and apparatus sprites are flattened images with no editable source, so white outlines would disappear on a light bench and tinting would also recolor the orange lid. Before choosing, render akt6's first procedure page three ways as screenshots only and let the owner pick:

| Variant | Bench | Expected trade-off |
|---|---|---|
| A (recommended on current evidence) | Flat solid navy, no illustration | White apparatus stays legible; matches the navy identity; loses the lab scenery |
| B | Light `#F2F6FA` bench with a navy tray panel behind each apparatus group | Bright overall; adds a panel per group and may clip the jar's base |
| C | Current illustration under a flat navy scrim at high opacity | Keeps some scenery; the scrim darkens the whole scene, which this plan otherwise avoids |

The recommendation comes from the one apparatus visible in the screenshot. The other four experiments' sprites have not been checked; if one is drawn for a light background, the decision is made per scene.

#### Target layout (1920 × 1080 reference units)

| Zone | Size and color | Contents | Notes |
|---|---|---|---|
| Navigation bar | Full width × 104, `#0B0B3B`, anchored to the canvas edges with no inset | Left: neutral "Kembali" (its label must name its real destination). Next: "Langkah N daripada M" only when both values come from the real procedure. Right: "Uji diri", then a reserved slot for the assistant button | Buttons 84 units high. Calculated hit size: 84 × 0.533 = 44.8 px at 1024 px wide with scaler match 0; akt6 uses match 0.515, scale about 0.62, so about 52 px. Confirm in the Game view |
| Instruction band | Full width, `#FFFFFF`, height follows content (about 120 to 140 for two lines) | LiberationSans 30, navy text, key term in bold `#1D4ED8` (already used in akt4), left-aligned column capped near 1400 units | Total top chrome about 240 units, 22% of the height, against about 36% in the screenshot (26% header plus about 10% footer) |
| Bench | Remaining height (about 740), flat color from the bench decision | Apparatus, untouched | No scenery behind working apparatus |
| Action | "Seterusnya" and similar actions sit at the right end of the instruction band if the existing button can move there without breaking script references; otherwise a 72-unit bar that exists only when an action does | | The detached footer strip is removed |
| Apparatus labels | White chip, 2-unit navy border, 8-unit radius, sentence case, 24 to 26 units | Existing label text | Removes the all-caps floating box; leader line only if the label names a specific object |
| Control states | Normal, hover, pressed, selected/focused (cyan plus navy edge), disabled | Neutral controls: white outline on navy. Primary: `#3D5AFE`, white label | Back and Close are never red |

#### Decisions needed from the owner

| Decision | Default if no answer |
|---|---|
| Bench variant A, B or C | A, after the three screenshots are reviewed |
| Instruction position: top band (this plan), or a bottom tray | Top band; a bottom tray would cover apparatus that currently overlaps the footer |
| Assistant button: reserve its slot in each scene, or move it into the bar by editing the shared prefab (affects every scene) | Reserve the slot now; revisit the prefab after the akt6 pilot |
| Pilot order | akt6, then akt3, akt5, akt7, then akt4 last so its existing treatment is brought in line |

Owner answers (2026-11-09): (1) Yes, the owner chooses among bench variants A, B and C after the screenshots are rendered. (2) The assistant button stays floating; each experiment scene reserves a slot for it and `LearningAssistant.prefab` is not edited. (3) Pilot order accepted as the recommendation: akt6, akt3, akt5, akt7, akt4 last.

Stage 0: baseline (read and capture only; nothing changes)

- [ ] 0.1 Save a scene copy of `akt6` for rollback (an Editor copy under `Temp/qa_screens/`) and capture baseline Game-view screenshots of every akt6 page and dialog at 1920 × 1080, 1280 × 720, 1280 × 800, 1024 × 768, 1280 × 1024 and 2560 × 1080. Expect: a before-set to compare against.
- [ ] 0.2 Resolve the numbering conflict (header "Langkah 1", instruction "2.", footer "1. Sediakan balang"). Read the real procedure order from the 11 pages and define one rule that shows a number only when it is real. Expect: a written mapping of page name to step number and total, or a decision to drop the count.
- [ ] 0.3 Identify what the "CABUT PENUTUP BALANG" label and the footer line are (hint, drop target, button), what `Kembali` and `Uji diri` do, and the assistant button's anchor in `LearningAssistant.prefab`. Expect: every restyled label names its real action.
- [ ] 0.4 Read the colors, sprites, anchors and raycast flags on `bg`, `ScienceHeaderShared` and `ScienceFooterShared`. Expect: the cause of the side gutters and which objects block clicks.
- [ ] 0.5 Contract audit: list the objects and index or name assumptions for `DragController`, `ItemSlot`, `NextExperimentController`, `ConditionalCheck`, `CheckOnEnabled` and `DisplayItem` in akt6, and confirm nothing depends on `Content` being sibling index 3 of `Experiments`. Expect: a list of what must not move.
- [ ] 0.6 Audit visible backgrounds, tints, CanvasGroups, masks, material colors, selection cues, and runtime overrides in all five experiments, including preparation, active, waiting, completion, and photo states. Record which bright colors represent substances or electrical conventions and which are purely interface cues.

Stage 0 findings (2026-11-09, Play Mode on akt6, scene file unchanged and hash-identical to the rollback copy):

- Evidence: `Temp/qa_screens/akt6.unity.bak` (rollback), `akt6_welcome_2000x1125.png`, and `akt6_pg_*.png` for all 11 procedure pages, all at the Game view's fixed size of about 2000 × 1125. The six other resolutions could not be captured with the available tools and need a manual Game-view check. Pages were shown by activating them from a script (the Mula button's click event was invoked once); no physical mouse walkthrough was done.
- "CABUT PENUTUP BALANG" is a real `Button` with 5 click listeners, not a hint. The same all-caps style is used on about 15 buttons across the pages (also "TUTUP BEKAS", "PERIKSA BACAAN", "ANOD", "KATOD"). Restyle them as buttons with states.
- Stale or stacked text exists in the current content, independent of styling:
  - Pages `0`, `0 (1)`, `0 (4)` and `0 (5)` still show the step 1 instruction ("2. Cabut kepala balang ...") above the real instruction, and the step 1 footer line ("1. Sediakan balang") under later steps. On `0 (5)` (fill with soil) that text describes a different step.
  - Pages `0 (10)` and `0 (11)` show several footer lines drawn on top of each other and unreadable.
  - Headers read "Langkah 1" on the anode and cathode pages, "Langkah 2 (Pasang Anod)" on the soil page and "Langkah 3 (Pasang Anod)" for the second electrode. Naming and numbering are inconsistent, and the "(Pasang Anod)" suffix on step 3 may be a copy error. Needs the owner's confirmation before any wording change.
  - Typos: "kontaner", "eletrod", "mengunakkan", "Hasilkan akan dapat dilihat". Flag for owner review rather than correct silently.
- Layout: the white band and footer stop about 10 units short of the right edge on every page, showing a dark strip. The "ANOD" label on `0 (2)` is cut off by the footer, and the jar base overlaps the footer on every page. Page `0 (2)` uses plain green and grey rectangles as electrodes.
- Page order in the hierarchy is `0` to `0 (8)`, then `0 (11)`, then `0 (10)`. `0 (9)` does not exist. The real on-screen order still needs to come from `NextExperimentController`.
- The Welcome page has the same header treatment and a mostly empty white panel.

Stage 1: bench gate

- Stage 1.1 evidence (2026-11-09, temporary Play Mode edits only, scene hash unchanged): `Temp/qa_screens/akt6_bench{A,B,C}_{p1,p10}.png` for page `0` (jar and lid) and page `0 (10)` (jar and multimeter). Findings: A and C look almost the same because C's high-opacity scrim hides the illustration. B's navy tray looks boxy, and the "Sambung ke resistor" label overflows its edge. On pure navy `#0B0B3B` the dark maroon electrode wires and the black soil cap have low contrast. Next step is to test a lighter slate bench (about `#26345A`) in place of pure navy.
- Stage 1.1 slate follow-up (2026-11-09, temporary Play Mode edits, scene hash unchanged): `Temp/qa_screens/akt6_slate_*.png` (pages `0`, `0 (2)`, `0 (5)`, `0 (10)`) with a flat `#26345A` bench. The jar outline, soil cap and orange parts read better than on `#0B0B3B`. The maroon wire sprite is still low contrast against `#26345A`, so that apparatus art is the limit, not the bench colour. The per-page footer text is dark navy and unreadable on the slate bench; it needs a light colour when moved onto the bench. The "ANOD" label on `0 (2)` is still cut off at the bottom of its white box.
- Stage 1.1 wire finding (2026-11-09): the faint in-jar wires are not a separate sprite. They are 45 `Image` objects using `Assets/pictures/png2/Png/Parts/Pink_Line_Horizontal.png` (referenced only by `akt6.unity`) with `Image.color` set to `#1E1E1E`, which darkens the pink art to maroon. No sprite edit is needed. Tinting those instances in Play Mode fixed the contrast on the slate bench: white gives magenta-pink, `#F4D8A8` gives crimson (`Temp/qa_screens/akt6_wire_*.png`). The colour is a single value shared by all 45 instances, so the change is one colour to apply and to revert. The wire colour is not tied to a scientific convention in the scene (all 45 share one tint); the owner should confirm that before the change is saved. The multimeter probes use a different sprite (`Assets/wire.png`) and are unaffected.
- Stage 2 first slice (2026-11-09, saved to `akt6.unity`, rollback copy `Temp/qa_screens/akt6.unity.bak`): `Experiments/bg` set to no sprite and `#26345A`; `ScienceFooterShared` hidden; 45 `Pink_Line_Horizontal` Images tinted `#F4D8A8`; stale `Text (TMP) (3)` hidden on pages `0`, `0 (1)`, `0 (4)`, `0 (5)` and stale `Text (TMP) (2)` on `0 (4)`, `0 (5)`; the two stacked footer lines on `0 (10)` and `0 (11)` separated (y 96 and 56); per-page footer text set to `#F2F6FA`. No wording changed. Evidence: `Temp/qa_screens/akt6_s2_*.png` (about 2000 × 1125, Play Mode, pages shown by script). Not done yet: header controls (Kembali, Uji diri, assistant slot) still unstyled and the white band still stops short of the right edge; caps button labels, "ANOD" clipping, Seterusnya visibility and the other resolutions are unchecked.
- Stage 2 second slice (2026-11-09, saved, owner-approved wording fixes): typos corrected (kontena, elektrod, menggunakan, "Hasilnya akan", "Katod ="); headers now "Langkah 2: Pasang Anod", "Langkah 3: Pasang Katod", "Langkah 4: Tutup dan Sambung", "Langkah 5: Perhati dan Rekod". `Experiments` rect offset changed from -9/-9 to 0/0, which removes the dark right strip. Kembali and Uji diri moved to (+-160, -56) so they sit inside the band; step titles set to 36 at y -56. All 11 `Seterusnya` buttons are the primary style (`#3D5AFE` normal, `#5C74FF` hover, `#2A44D4` pressed, white label); caps button labels changed to sentence case ("Cabut penutup balang", "Tutup bekas", "Periksa bacaan"). ANOD label text rect on `0 (2)` and `0 (3)` reset to stretch so it fits its box. Console showed no errors after the changes. Evidence: `Temp/qa_screens/akt6_s3_*.png`, `akt6_s4_anod.png` (2000 x 1125, Play Mode). Not done: Welcome page, assistant slot, other resolutions, mouse walkthrough, keyboard focus check.
- Stage 2 third slice (2026-11-09, owner-requested): the assistant button in `Assets/UI/Science/Prefabs/LearningAssistant.prefab` (shared by every scene) is now an 80 x 80 icon button at the top right, left of Uji diri, using the existing `brain.png` child that was previously hidden. The "Pembantu pembelajaran" text object is kept but inactive; the open/close listener and chat box are untouched. Rollback copy: `Temp/qa_screens/LearningAssistant.prefab.bak`. Other scenes need a check, since the old button was 280 x 96 at a different position. The icon has no visible text, so its meaning rests on the picture alone. Welcome page in akt6: bench `#26345A` instead of the lab photo, reading panel resized to 1200 x 320, `Mula eksperimen` as the primary button, "TUJUAN EKSPERIMEN:" changed to "Tujuan eksperimen:" (case only). Evidence: `Temp/qa_screens/akt6_s5_*.png` (the panel height was reduced afterwards and has not been re-captured).
- Stage 2 fourth slice (2026-11-09, saved): akt6 `Prestart Bahan2`. Bench `#26345A`, white reading panel, navy "Bahan-bahan" heading (was `BAHAN-BAHAN`; it was previously hidden behind the scroll viewport, which now starts 100 units lower), `Seterusnya` primary style, labels navy text on `#DCE5EF` bars (white text had overflowed its bar onto white). `Radas` HorizontalLayoutGroup spacing 149.75 -> 80 so the first item is no longer clipped; jars item position is layout-driven. Kembali and Uji diri height 80 -> 88. Evidence: `Temp/qa_screens/akt6_s8_prestart.png`. Remaining on this page: label bars differ in width and font size (existing per-item sizes), and the MULTIMETER bar is cut at the viewport's right edge; the apparatus art is not size-matched. The assistant icon is still 80 x 80. Mouse/keyboard focus and the six resolutions are unchecked.
- Stage 2 fifth slice (2026-11-09, saved): Prestart label bars unified (200 x 104 units, 22-unit navy text on `#DCE5EF`, centred under each item, no overlap); assistant icon raised to 88 x 88 at (-272, -12) in `LearningAssistant.prefab` (owner chose to keep the brain image). Note: label bars and some items carry local scales (0.3025), so sizes were set through the ratio to the Canvas scale. Evidence: `Temp/qa_screens/akt6_s11_prestart.png`, `akt6_s12_focus.png`. Keyboard check (scripted, not real key presses): from `seterusnya`, up -> assistant, left -> Kembali, right -> Uji diri; `UiFocusIndicator` is present, but a focus ring was NOT visible in the capture after selecting by script, so it is unconfirmed. Console: no errors. Not verified: real mouse and Tab presses, the six window sizes (Game view dropdown, manual).
- akt3 pass (2026-11-09, saved, scripted treatment copied from akt6): bench `#26345A` on Welcome, both Prestart pages and Experiments; `ScienceFooterShared` hidden; Kembali/Uji diri at (+-160, -56) x 88 tall; step titles 36 at y -56 (17 pages); all Seterusnya/Mula buttons primary style; `SETERUSNYA` -> "Seterusnya" (pages 14, 15), `TUTUP` -> "Tutup" (Petri buttons); prestart panels white with navy headings ("Bahan-bahan", "Radas:"); prestart label bars navy text on `#DCE5EF` (Bahan2 170 x 104 at 22; Radas 148 x 104 at 20, below the 22 guideline because six items share the row); Welcome panel 1200 x 320; "TUJUAN EKSPERIMEN:" -> "Tujuan eksperimen:". Evidence: `Temp/qa_screens/c3_*.png`, `b3_*.png`. The capture tool lags one frame after a page switch, so file names can show the previous page. Wording flagged, NOT changed: Prestart Radas header says "Makmal Maya 1" while the other pages say "Makmal Maya 3"; pages 6-9 all read "Langkah 6" (and 10 reads "Langkah 7"). Not done: Play-mode run of the drag/drop steps, keyboard focus, six window sizes.
- akt3 wording fix (2026-11-09, owner-approved, saved): Prestart Radas header "Makmal Maya 1" -> "Makmal Maya 3"; step titles "Langkah 9..15" renumbered to "Langkah 8..14" (pages 6-9 still read "Langkah 6").
- akt5 pass (2026-11-09, saved): bench `#26345A` on Welcome, both Prestart pages and Experiments (Prestart bench moved to first sibling so the white header band shows above it); footer hidden; header buttons at (+-160, -56); 19 buttons primary style; `SETERUSNYA` -> "Seterusnya", `TUTUP BEKAS` -> "Tutup bekas", `HIDUPKAN` -> "Hidupkan", `MULAKAN` -> "Mulakan", `TAMAT` -> "Tamat" (grep found no script that reads these labels; only `NextExperimentController` sets "Tamat"); 18 step titles 36 at y -56; Prestart panels white with navy headings ("Radas", "Bahan", case only); label bars 200 x 104 at 22, navy on `#DCE5EF`; 9 body texts 24 -> 32; body text navy on the white header band, white on the slate bench; `<color=#1D4ED8>` tags on slate changed to `#4FC3F7` (markup only). Evidence: `Temp/qa_screens/d5_*.png`, `g5_*.png`. Method note: a first bar pass hit container objects (scroll `content`, item roots, heading panels); it was reverted with the Editor undo history and redone with a stricter filter. Not verified: drag/drop steps, real Tab/mouse, six window sizes; page 13 ("Mulakan") and the Hidupkan buttons keep the original white style.
- akt5 wording fix (2026-11-09, owner-approved, saved, 42 texts): Prestart subtitle "(Cara A: Agar-agar nutrien)" removed; "vessel/Vessel" -> "bekas/Bekas"; "volumetric" -> "volumetrik"; "70 °C darjah celsius" -> "70 °C"; "sample" -> "sampel"; "deionized" -> "ternyahion"; "Sambung Wire" -> "Sambung wayar"; "Langkah 3 " trailing space removed; "HASIL" -> "Hasil"; "HASIL DARI GAMBAR SEBENAR" -> "Hasil daripada gambar sebenar". No change needed: pages 10/10 (1) are phases of one step and 13 (1) is step 14 (sequence is 1-14); the Prestart headings match their content (only the object names are swapped). Terms "bekas" and "ternyahion" should be confirmed against the curriculum. Evidence: `Temp/qa_screens/h5_*.png`.
- akt7 pass (2026-11-09, saved): bench `#26345A` on Welcome, both Prestart pages and Experiments; header buttons at (+-160, -56) x 88; 11 Seterusnya/Mula/Tamat buttons primary style; Welcome panel white with navy text, 1200 x 320, text lifted clear of the Mula button; "Langkah N" titles 28 -> 36 at y -56 (4 pages), trailing space removed from "Langkah 3 "; Prestart placeholder notes white on the bench. akt7 already had white Prestart panels, shared header, procedure note and sentence-case labels from earlier slices, so no label bars were needed (items are text lists). Evidence: `Temp/qa_screens/a7_*.png`, `b7_*.png`. Flagged, NOT changed: Welcome/Prestart say "Makmal maya" in places and "Makmal Maya" in others; page 4's heading "Penangkapan sisa menggunakan filem getah" is white on the white header band and invisible. Not verified: drag/drop and the Hasil panel, real Tab/mouse, six window sizes.
- akt7 follow-up (2026-11-09, saved, owner approved both): "Makmal maya" -> "Makmal Maya" in 2 texts (Welcome, Image); page 4 heading `Experiments/4/Text (TMP) (4)` moved onto the slate at y -336, height 56, size 36, white, so it no longer sits invisible on the white band. Both earlier "flagged, NOT changed" items are resolved. Evidence: `Temp/qa_screens/c7_p4.png` (heading clear of the note panel and step text at the captured size).
- akt4 pass (2026-11-09, saved): akt4 previously had a navy header band and a light `#F2F6FA` workspace; brought in line with the other scenes. Bench `#26345A` (sprite removed, raycast off) on Welcome, Prestart Bahan2 and Experiments; every `ScienceHeader`/`ScienceHeaderShared` band white with navy title text; header buttons (Laman utama, Mula semula) at (+-160, -56), 192 x 88; 13 primary buttons (Mula eksperimen, Seterusnya, Hasil sebenar) given the standard colour block (`#3D5AFE` / `#5C74FF` / `#2A44D4` / `#A0A9B9`) with white labels; "SETERUSNYA" -> "Seterusnya" (2 texts). Text colour follows surface: instruction lines under the band and floating apparatus labels (`source` quantities, TempSlider "Suhu bilik", jar/tube/dish captions, result captions, "Tarik pipet" notes) white on the slate, `#1D4ED8` emphasis -> `#4FC3F7`. Page 1 header, footer and instruction text now share page 2's font style and sit centred on the slate (its white instruction strip is disabled); leading space trimmed from page 1's heading; Prestart title 30 -> 36; the two "Tarik pipet" notes narrowed to 320 so they clear the tube label. Footer bands (white) kept because they carry "Bahagian N" text. The Hasil dialog keeps its own navy title bar. Evidence: `Temp/qa_screens/b4_*`, `c4_*`, `d4_result.png`, `sheet4_*`, `sheet4c.png`. Not verified: drag/drop and pipette interactions, timers, real Tab/mouse, six window sizes. In captures Laman utama shows the cyan selected state on step pages (button is auto-selected on entry).
- [ ] 1.1 Render the three bench variants for akt6's first procedure page as screenshots, with a Play Mode capture and one view of the jar and lid at 1024 × 768. Expect: three images the owner compares; no variant saved into the scene until chosen.
- [ ] 1.2 Owner picks a variant. Check the other four experiments' apparatus against that choice before propagating.

Stage 2: akt6 pilot (restyle in place first, extract to a prefab only after review)

- [ ] 2.1 Restyle the three shared siblings in place: `ScienceHeaderShared` becomes the navigation bar, `bg` becomes the flat bench, `ScienceFooterShared` is removed or reduced to a real action bar. Keep their sibling order, the page names `0` to `0 (11)`, and every parent. Fix edge anchors so no gutters show.
- [ ] 2.2 Move each page's instruction text into the new band by changing its rectangle only, and apply the shared text style. Add decorative surfaces behind gameplay objects with raycast off. Do not wrap drag roots in layout groups or change apparatus scale.
- [ ] 2.3 Restyle the apparatus labels, Kembali, Uji diri and the next action with the control states in the layout table. Reserve the assistant slot.
- [ ] 2.4 Review the introduction, the titanium-wire/anode step from the screenshot, one later step and the completion state at the six sizes. Expect: no clipping or overlap, jar and lid clear of every band, labels readable, focus visible. Owner reviews before any other scene is touched.
- [ ] Standardize apparatus labels, instruction text, neutral Back/Close controls, primary actions, disabled states, and completion cues. Use LiberationSans and the existing project type/spacing guidance. Keep labels on opaque light surfaces. Change neon cues only where they are presentation, preserving scientific colors and simulation meaning. Pair success/selection colors with text or shape. Review the akt6 Game-view slice before propagating it.
- [ ] Reuse a small Inspector-editable background/header/button treatment under `Assets/UI/Science/` where appropriate. Keep gameplay-bearing objects in their current parents and order. Add decorative surfaces behind them without raycast interception; do not wrap drag roots in layout groups or change their scale to fit a template. Keep scene-specific apparatus layouts and existing sprite sources.
- [ ] Apply the proven treatment to akt3, akt5, akt7, then akt4, checking each scene independently. Cover every procedure page and preparation/result dialog. Fix akt7's known ultrawide instruction overlap in this pass. Preserve all scene routes, apparatus callbacks, timers, scoring, and curriculum content. Retain per-scene differences needed by the experiment rather than forcing identical apparatus positions.
- [ ] Verify every affected state at 1920 x 1080, 1280 x 720, 1280 x 800, 1024 x 768, 1280 x 1024, and 2560 x 1080. Inspect screenshots for clipping, overlap, apparatus/background separation, readable labels, transparent apparatus edges, focus, and assistant collisions; TMP overflow alone is insufficient. Check normal text at 4.5:1, large text/essential boundaries at 3:1, and selection/result meaning in grayscale and color-vision simulations.
- [ ] Run available programmatic checks for representative drag/drop callbacks, invalid actions, transitions, timing gates, completion, and return routes after each scene. Physical mouse/keyboard testing remains skipped by the user's earlier request; do not report callback checks as a physical walkthrough. Inspect Console, missing scripts/references, persistent assistant ownership, and build/startup after the integrated pass. Record actual verification limits here.

Acceptance: all five experiments share the same background policy, navigation/instruction treatment, label styling, action colors, and dialog scrim rules. Detailed scenery no longer competes with working apparatus. No essential content is obscured at the six target resolutions, and programmatic behavior checks show no new regressions. Human editors can change the owned scene/prefab Inspector settings without runtime screen construction. Full scientific validation and assistant service changes remain outside this visual pass.

Recovery: retain the original art assets, .meta files, GUIDs, callbacks, and hierarchy contracts. Make scene changes in small batches so a failed visual slice can be reverted without rolling back unrelated gameplay fixes. Do not regenerate all scenes or replace their controllers to apply the style.

### Revamp Phase 2.1 - dashboard activity cards - 2026-11-10

Changed in Assets/Scenes/Dashboard.unity (cards under `Overview/Content/Body/Content`):
- Removed the preselected cyan card: `Overview` `UiFocusScope.initialSelection` cleared and each card Button's `selectedColor` set equal to `normalColor` (same approach as the quizzes).
- Card text restyled: "Aktiviti N" 22 in `#46556B`, title 32 in navy, 28/20/28/56 text margins. Wording unchanged.
- Added a text stage strip per card (`Stages` child, raycast off): "Belajar · Uji diri" for activities 1-2, "Belajar · Eksperimen · Uji diri" for 3-7, taken from the existing scenes (experiments akt3-akt7, quizzes Uji Diri 1-7).
- New `Assets/UI/Science/Scripts/UiCardHover.cs`: border fades `#46556B` to navy in 0.12 s on pointer or keyboard focus; instant when `UiMotion.Reduced` is on. No lift, because the cards sit in a Layout Group.
- Cards stay text only: only Aktiviti 2 and 7 have fitting lesson images, so art would make the grid uneven. The plan's "lift 4 units" and "real art" were not followed; owner decision pending.

Verified in Play Mode (scripted): 1920x1080, 1024x768 and 1280x720 captures (`Temp/qa_screens/scan/42-dash-after-*.png`) show no preselection, full text, no clipping; all seven card `onClick` calls activate the matching `Into/Content` child 1-7; no new Console errors (the Console log line seen is the existing ChatGPT prompt `Debug.Log`).
Not checked: 1280x800, 1280x1024, 2560x1080 after the change (before the change, 2560x1080 already hid the Aktiviti 7 row behind the scroll bar), hover visuals, reduce-motion on.
Possible content mix-up to flag: Aktiviti 3 and 5 lessons both use `vertical farming` and `loi rawatan` pictures.

### Revamp Phase 2.2 (wording only) - hub labels - 2026-11-10

Owner confirmed: text-only dashboard cards, border-darkening hover (no lift), and the shared `vertical farming` / `loi rawatan` pictures in Aktiviti 3 and 5 are intended.

Changed in `Dashboard.unity` (hubs `Into/Content/N/0` for 1-2, `.../N/bg` for 3-7); text only, no button, route or hierarchy changes:
- Hub titles from ALL CAPS to sentence case ("Aktiviti 6: Teknologi sel bahan bakar mikrob (Microbial Fuel Cell)"). English terms left as they were (plan item A11 is the owner's decision).
- Hubs 3, 5, 6, 7 button labels now match activity 4: "Belajar teori", "Mula eksperimen", "Uji diri". The old labels that named the experiment ("Makmal maya MFC", "Makmal maya Circular Waste", ...) are gone from the hub; the experiment's own page still names it.
- Hubs 1-2 keep their two lesson titles in sentence case ("Jom belajar konsep STEM", "Kenapa STEM?", "Kenapa teknologi hijau?" - a question mark added to the latter) plus "Uji diri".

Verified: Play Mode captures of hubs 1, 4, 6 at 1920x1080 and 1024x768 (`Temp/qa_screens/scan/43-hub*`); no new Console errors. Not done: the three-station layout, activity arrows moved to the hub, and the button routes were not re-clicked after the text change (text-only edit).

### Revamp Phase 2.2 (layout) - three-station hubs - 2026-11-10

Changed in `Dashboard.unity` on all seven hubs (`Into/Content/N/0` for 1-2, `.../N/bg` for 3-7). Existing wrappers, Buttons, `onClick` routes, names and order are kept:
- `Content/Body`: VerticalLayoutGroup replaced by a HorizontalLayoutGroup (spacing 32); each wrapper's `LayoutElement` is now flexible width, 400 high.
- Each Button: label 36 top-left, a white `Number` badge (1, 2, 3 = suggested order, nothing is locked) and, where real text exists, a `Detail` line at 24.
- Detail lines: experiment purpose copied from the Welcome page of akt3, akt5 and akt6 (the Welcome page text is the source; if it changes, edit the `Detail` child here too); "5 soalan" on Uji diri (6 on Uji diri 7), counts read from each quiz's `QuestionList`. Hubs 4 and 7 show no experiment line: akt4's purpose is a long paragraph and akt7 has none (owner to supply a short line if wanted). Hubs 1-2 have no experiment, so only the quiz line.
- Not done: moving "Aktiviti sebelum / berikut" next to the title (planned with 2.3, because lesson pages also carry arrows).

Verified in Play Mode: captures of hubs 1, 4, 6, 7 at 1920x1080 and 1024x768 (`Temp/qa_screens/scan/44-hub*`); the theory button on all seven hubs and the second lesson button on hubs 1-2 activate the expected `Into/Content/N` child; experiment and quiz buttons still carry their `SceneController.ChangeScene` listeners (not invoked, as they load scenes); no new Console errors.
Not checked: 1280x720, 1280x800, 1280x1024, 2560x1080, keyboard focus on the stations, hover.

### Revamp Phase 2.2 (arrows, hub 4, focus fix) - 2026-11-11

Changed in `Dashboard.unity`:
- "Aktiviti sebelum / berikut" moved into the hub header (top-right, 208x52, font 22, stacked). New `Assets/UI/Science/Scripts/UiActivityArrows.cs` on `Into/LeftSide` shows them only while a hub is visible and disables "sebelum" on activity 1 and "berikut" on activity 7. `MenuSlideController` and its button wiring are untouched. Lesson pages no longer show the arrows; "Laman utama" is the way out.
- Hub 4 station buttons recoloured to action blue (#3D5AFE, white label) to match the other hubs; its `UiFocusScope.initialSelection` cleared (no preselection). Hub 4 background is still light while hubs 1-3 and 5-7 keep the dark photo background (inconsistent; owner to decide).
- `selectedColor` set equal to `normalColor` on all 332 Dashboard buttons (all have `UiFocusIndicator`), so the focus ring is the selection cue and the cyan selected fill is gone.

Verified in Play Mode: arrows visible on hubs and hidden on a lesson page; disabled states on activities 1 and 7; clicking "berikut" on hub 1 opens activity 2; hub 4 captures at 1280x720 and 1280x800 (viewed), 1920x1080 with focus ring, 1280x1024 and 2560x1080 (`Temp/qa_screens/scan/46-hub4-*`, 45-*); no new Console errors.
Not checked: Tab order through the stations, hover, reduce-motion on, hubs other than 4 at 1280x720/1280x800/1280x1024/2560x1080, real mouse clicks on the arrows.

### Revamp Phase 2.2 (hub 4 background, hub 4 and 7 experiment lines) - 2026-11-11

Owner decisions: unify hub 4 on the dark photo background; write the missing experiment lines.
- `Into/Content/4/Image` now uses the `bg3` sprite at colour #989898, same as hubs 2, 3, 5, 6, 7.
- New `Detail` lines on the "Mula eksperimen" station of hub 4 ("Memasukkan gen GFP ke dalam plasmid pUC19 dan mengesahkannya dengan PCR.") and hub 7 ("Menghasilkan filem getah daripada lateks dan melihat cara filem itu menangkap sisa mikroplastik."). Both are my short summaries written from the akt4 Welcome purpose and the akt7 procedure pages; curriculum owner should review wording.
Verified: Play Mode captures of hubs 4 and 7 at 1920x1080 (`Temp/qa_screens/scan/47-*`); text fits in the cards. Not checked: other sizes for these two lines.

### Revamp Phase 2.3 (pilot, activity 4 only) - lesson page indicator - 2026-11-11

Activity 4's lesson (`Into/Content/4/1`) already had Kembali/Sebelum bottom-left and Seterusnya bottom-right on its three pages (`content`, `content (1)`, `content (4)`). Added a `PageIndicator` text child to each page: "Halaman 1/2/3 daripada 3", 26 pt, #46556B, centred on the button row. It is static text, so edit it by hand if pages are added or removed. No buttons, listeners or hierarchy order changed.
Verified in Play Mode at 1920x1080 (`Temp/qa_screens/scan/49-l4-p1..p3.png`): all three pages show the indicator level with the buttons; Seterusnya on pages 1-2 moves forward; no focus-fill regression on Seterusnya.
Observed, not changed: page 3 has no Seterusnya/Tamat visible until the five clone-type popups are opened (existing gating, `Next` starts inactive); page 2 body starts with lowercase "merujuk" and has no heading (plan 2.4). Other activities 1-3, 5-7 not yet converted; awaiting owner review of this pilot.

### Revamp Phase 2.3 (rollout, activities 1-3 and 5-7) - lesson page indicator - 2026-11-11

Owner approved the pilot. Added a `PageIndicator` to every page on each lesson's forward chain (found by following the "Seterusnya" `onClick` targets). Chains: A1 lesson 1 = 14 pages, A1 lesson 2 = 2, A2 lesson 1 = 5, A2 lesson 2 = 2, A3 = 4, A5 = 13, A6 = 4, A7 = 5 (49 indicators). Pages that nothing links to get none.
Placement changed from the pilot: the bottom button row collides with the lab-apparatus strip on most lessons and #46556B is unreadable on the dark pages. The indicator is now a white pill with a navy edge and navy 26 pt text, top-centre of the page (`PageIndicator` with an Image/Outline and a `Label` TMP child). It is static text; edit by hand if pages change. Buttons, listeners and child order are untouched.
Verified in Play Mode at 1920x1080: walked every lesson through its real "Seterusnya" buttons; each showed 1/N to N/N and ended on the expected last page. Captures `Temp/qa_screens/scan/51-*` (A1 L1, A1 L2, A6, A7) show the pill clear of the apparatus strip and readable on dark and light pages. Not checked: smaller window sizes, keyboard focus, reduce-motion.
Observed, not changed (owner decisions): "Extra Info Button" placeholder text is visible on A2 L1 page 5, A5 and A6 last pages; "KENAPA STEM ITU PENTING?" heading is nearly invisible on A1 L2 page 2; its "Maklumat" buttons wrap ("Makl umat") and "Tamat" has no "Sebelum"; A2 L1 `content (6)`-`(14)`, A3 `content (5)`-`(11)` and similar pages are not linked from any chain (possible authoring loss).

### Revamp Phase 2.3 follow-up (owner decisions on lesson issues) - 2026-11-11

- Removed the ten "Extra Info Button" placeholder `Hint` objects (A2 L1 pages 5 and 3, A2 L2, A3, A5, A6, A7 - no script referenced "Hint", no button toggled them).
- A1 lesson 2 page 2: title now white (was navy on the dark photo); "Maklumat" buttons widened to 160 with 24 pt labels so they no longer wrap; added a "Sebelum" button (clone of Tamat, mirrored left) that hides this page and shows page 1 (two persistent listeners).
- A4 lesson page 2: body now starts "Merujuk..." (capital); the "Teknologi pengklonan" heading was positioned above the card (off-screen) and now sits inside the card top, aligned with the body text.
- Not linked, per owner: the unreachable pages in A2 L1 and A3 stay as they are. A4 page 3 gating (Seterusnya/Tamat after the five popups) kept as is.
Verified in Play Mode at 1920x1080 (`Temp/qa_screens/scan/52-*`, `53-A4p2.png`): A1 L2 page 2 and A2 L1 last page look right, "Sebelum" returned to page 1. After the final 32-unit heading nudge the A4 page was not re-captured.

### Revamp Phase 2.5 - hierarchy contract re-check - 2026-11-11

`MenuSlideController` (on `Canvas/Container/Into/Content`) still has seven children in the order 1-7; Phase 2 only added children inside lesson pages and hubs, never at this level. `NextExperimentController` is not used in the Dashboard scene (experiment scenes only, untouched so far).
Verified in Play Mode: activating hub 1 and calling `DownArrow` seven times shows 2,3,4,5,6,7 then stays on 7; `UpArrow` goes back 6..1 and stays on 1. Note for maintainers: `Contents` is filled in `Start`, and `Into` starts inactive, so the arrows only work once the object has been active for a frame (pre-existing behaviour).
Not checked here: the experiment scenes' `NextExperimentController` (Phase 3 will re-check it).

### Revamp Phase 3.1 (akt6 pilot) - step counters - 2026-11-11

The akt6 procedure headers already carried real step numbers (pages `0`-`0 (3)` = Langkah 1, `0 (4)`-`0 (5)` = 2, `0 (6)` = 3, `0 (7)`-`0 (8)` = 4, `0 (11)` and `0 (10)` = 5), so the counter comes from existing content, not an invented mapping. Each header (`Content/<page>/Text (TMP) (1)`, 36 pt) now reads "Langkah N daripada 5", followed by the existing title where there is one (for example "Langkah 3 daripada 5: Pasang Katod"). Pages that are phases of one step share the number. Static text: edit by hand if a step is added.
Widest header is 855 of 979 units ("Langkah 5 daripada 5: Pemerhatian dan catatan data"). Scene saved. Verified in Play Mode at 1920x1080 for pages `0 (6)` and `0 (11)` (`Temp/qa_screens/scan/60-akt6-*.png`; pages were shown by script, not by clicking through). Not checked: other pages by eye, other sizes, akt3/akt5/akt7 (not touched; plan says the owner reviews the akt6 pilot first).
Observed, not changed: page `0 (6)` draws the second electrode as a plain neon-green rectangle; page `0 (10)`/`0 (11)` order in the hierarchy is `(11)` then `(10)` (NextExperimentController follows sibling order).

### Revamp Phase 3.1 follow-up and 3.2 (akt6 pilot) - step 1 title and drop-slot cue - 2026-11-11

- Step 1 headers (pages `0` to `0 (3)`) now read "Langkah 1 daripada 5: Sediakan Balang". The title is my wording, taken from the existing footer line "1. Sediakan balang"; the owner approved own wording.
- Drop slots: the akt6 `ItemSlot` targets (`collider2d` Images) were solid neon-green quads. They now use the new 9-sliced sprite `Assets/UI/Science/Sprites/SlotCue.png` (4 px #4FC3F7 border, faint cyan fill, Image type Sliced, colour white). 27 slots restyled. The border is solid, not dashed: a dashed edge was not feasible on these sprite-less quads. The one transparent slot on page `0 (8)` (lid collision area) is left invisible.
- New `Assets/UI/Science/Scripts/UiHideSlotCueOnTick.cs` on each slot's `tick` child (28). It disables the slot Image while the tick is active and re-enables it when the tick turns off. `ItemSlot.cs` is unchanged and `tick` stays child 0.
- Verified in Play Mode at 1920x1080 (`Temp/qa_screens/scan/62-p2.png`, `62-p6.png`): the cue renders as a thin cyan frame. Toggling the tick by script on active slots (pages `0 (2)`, `0 (4)`) gave enabled=true -> false -> true as intended.
- Not checked: real mouse drag-and-drop, other window sizes, pages `0 (5)` and `0 (7)`-`0 (10)` by eye.

### Revamp Phase 3.3 (decision) and 3.4 tick pop (akt6 pilot) - 2026-11-11

- 3.3 not built as a separate chip/outline. Each akt6 page's blue instruction already is the hint, and the 3.2 slot cue marks the drop target. The draggable sources are nested chains (for example `anodElektrod` > `wayar (2)` > `wayar (4)`) with several disabled `DragController`s, so the "active source" per page cannot be identified without guessing. Owner can ask for it page by page if wanted.
- New `Assets/UI/Science/Scripts/UiPopOnEnable.cs` on all 28 slot `tick` children: scale 0.6 -> 1.08 -> 1.0 over 160 ms (Inspector fields), scale only, skipped when `UiMotion.Reduced`. Compiles with no errors; scene saved.
- Verified in Play Mode: after the pop the tick returns to its rest scale (1.31), and with reduce motion on it shows the rest scale at once. Not observed: the mid-pop frames by eye, and a real drag-and-drop.
- Still to do in 3.4: drag lift, Seterusnya scale-in, "Langkah selesai" text (LiberationSans has no check glyph, so text only).

### Revamp Phase 3.4 (partial) - drag lift and scale-in scripts written, NOT attached - 2026-11-11

- New `Assets/UI/Science/Scripts/UiDragLift.cs` (1.08x while dragging, only when the sibling `DragController` is enabled, skipped on reduced motion) and `UiScaleInOnEnable.cs` (0.85 -> 1 over 200 ms for the Seterusnya button).
- Not done: the Unity MCP tools were not available in this session, so the scripts were not compiled, attached to akt6 objects, or tested. The "Langkah selesai" text is also not added (needs scene edits).
- Next session with MCP: compile check, add `UiDragLift` to the akt6 draggables with an enabled `DragController`, `UiScaleInOnEnable` to the Seterusnya buttons, then add the text.

### Revamp Phase 3.4 (rest) - drag lift, Seterusnya scale-in, "Langkah selesai" (akt6 pilot) - 2026-11-11

- UnityMCP was reachable over HTTP (127.0.0.1:8080) although this session had no MCP tools; driven by JSON-RPC. The two scripts from the earlier entry compile with no errors.
- `UiDragLift` added to all 96 objects with a `DragController` in akt6 (it lifts only while that controller is enabled). `UiScaleInOnEnable` added to the 11 `NextExperimentController` Seterusnya buttons under `Experiments/Content/0*/Button` (the Prestart "seterusnya" button is always active, left alone).
- New child `StepDoneLabel` (white chip, bold green `#166534` "Langkah selesai", 260x48, above the button) on those 11 buttons; it appears when the button does. Scene saved.
- Not verified: Play Mode behaviour (lift, scale-in, label position at other resolutions), real drag-and-drop.

### Revamp Phase 3.6 (check only, no change) - akt6 Welcome and Prestart at 1920x1080 - 2026-11-11

- Captured `Temp/qa_screens/scan/36-welcome.png` and `36-prestart.png` in Play Mode and inspected them. Welcome: title, Tujuan card and Mula eksperimen are all fully visible. Prestart: the MULTIMETER bar is no longer cut off; all five label bars are inside the white panel.
- Cosmetic leftovers, not changed: the white Prestart panel is centred about 8 px right of the canvas centre and the "Bahan-bahan" heading sits tight under the header; label bars differ in height (wrapped labels); the title is left-aligned rather than centred.
- Not checked: other window sizes. `.claude/settings.local.json` no longer denies `Read(./Temp/**)` (owner approved) so screenshots can be inspected.

### Revamp Phase 3.6 follow-up - cosmetic leftovers fixed (akt6) - 2026-11-11

- Welcome and Prestart titles: centred between the header buttons (anchor max x 0.66 -> 0.80, Center alignment), size 40 -> 36 to avoid a one-word last line.
- Prestart: `Content` x -7.76 -> 0 (panel and Seterusnya now centred); "Bahan-bahan" heading y 0 -> -20 under the header.
- Label bar heights: measured equal (104 px) in the capture; the earlier note was a 1 px offset, not changed.
- Checked at 1920x1080 in Play Mode (`Temp/qa_screens/scan/36-*.png`) before the font change; the 36 pt title was checked by line count only. Scene saved.

### Revamp Phase 3.5 (akt6 pilot) - completion card on the last step - 2026-11-11

- The 3.4 `StepDoneLabel` on the last page (`Experiments/Content/0 (11)/Button`) is now `CompletionCard`: white card 380x160 above the Seterusnya button, "Eksperimen selesai!" (bold green `#166534`, 32) + "Anda telah melengkapkan 5 langkah. Cuba Uji diri untuk menguji kefahaman anda." (navy, 22). It shows when the button does and scales in with it. The wording is my own; owner can edit it. No new script; scene saved.
- Checked in Play Mode at 1920x1080 with the page forced active (`Temp/qa_screens/scan/35-card.png`): card clear of the multimeter and the screen edge. Not checked: other sizes, the card appearing through real play.
- Observed, not changed: in that capture the last button still reads "Seterusnya". `NextExperimentController` only relabels it "Tamat" (and loads Dashboard) when the page has no next sibling under `Content`; page `0 (11)` evidently has one, or Start had not run. Worth checking what the last button does.

### Correction to 3.5 - card was on the wrong page - 2026-11-11

- Checked `NextExperimentController` against the real sibling order under `Experiments/Content`: `0 (8)` -> `0 (11)` -> `0 (10)`; there is no `0 (9)`. The script picks the next page by sibling index, so the true last page is `0 (10)` (its button becomes "Tamat" and loads Dashboard), not `0 (11)`. The "Seterusnya" label seen on `0 (11)` is correct, not a bug.
- Moved `CompletionCard` to `0 (10)/Button`; `0 (11)/Button` got its plain "Langkah selesai" chip back. Scene saved. Not re-captured.
- Open question for the owner: the order puts "Periksa bacaan voltan" (`0 (11)`, multimeter) before "Sambungkan wayar ... kepada resistor" (`0 (10)`), and both are headed "Langkah 5 daripada 5". Unchanged; confirm whether the order is intended.
- Owner confirmed (2026-11-11): the page order `0 (8)` -> `0 (11)` -> `0 (10)` is intended; no change. The two pages share the heading "Langkah 5 daripada 5" by design.

### Revamp Phase 3.7 (akt6 pilot) - drag/drop check, scripted - 2026-11-11

- Play Mode, page `0 (2)`, draggable `anodElektrod2` -> slot `anod2`. Begin-drag and end-drag events were sent through `ExecuteEvents` (not a real mouse); the drop used the real 2D trigger (`ItemSlot.OnTriggerStay2D`).
- Results: while dragging the scale went 1.99 -> 2.14 (1.08x) and came back to 1.99 on end-drag; after the drop the tick is active at its rest scale 1.31, the slot cue Image is disabled, the object is parented to the slot and its `DragController` is disabled. `UiScaleInOnEnable` is present on the page's Seterusnya button.
- Not observed: the visible animation frames, the Seterusnya button appearing and scaling in, the other pages, or a real mouse/touch drag. Play Mode stopped; scene untouched by the test.
- akt6 slices 3.1-3.7 are now done; stopping here for the owner's review before any other experiment.

### Revamp rollout - akt3 - 2026-11-11 (owner chose akt3 as the next experiment)

- akt3 differs from akt6: 17 pages, 14 steps, only 2 `ItemSlot`s (`label` under the petri dishes, page `3`... drop targets there are plain white quads), 17 `DragController`s, 19 `NextExperimentController` buttons.
- Applied: `SlotCue` sprite (Sliced, white) + `UiHideSlotCueOnTick` + `UiPopOnEnable` on the 2 slots/ticks; `UiDragLift` on 17 draggables; `UiScaleInOnEnable` + "Langkah selesai" chip on the 18 non-final Seterusnya buttons; `CompletionCard` ("Eksperimen selesai! Cuba Uji diri ...", 380x124) on the final page's "Tamat" button (page `17`, always visible there); headers "Langkah N" -> "Langkah N daripada 14" (17 texts).
- Not done: named step titles (akt6 got "Langkah 1 daripada 5: Sediakan Balang"); akt3 needs 14 wordings from the owner or approval to write them. Page `6`-`9` all read "Langkah 6 daripada 14" (sub-pages, unchanged).
- Checked in Play Mode at 1920x1080 by forcing pages `3` and `17` active (`Temp/qa_screens/scan/a3-p3.png`, `a3-p17.png`); card height reduced after the first capture so it clears the table, not re-captured. Not checked: the slot cue rendering on the dish page, a drop, other sizes, other pages.

### akt3 step titles - 2026-11-11

- Headers now "Langkah N daripada 14: <title>" on all 17 pages. Titles are my own wording from each page's instruction (owner previously approved own wording for akt6): 1 Sterilkan Kawasan Kerja, 2 Sediakan Piring Petri, 3 Sterilkan Dawai Gelung, 4 Sejukkan Dawai Gelung, 5 Celup Kultur Bakteria, 6 Calit Piring Petri A (4 pages), 7 Sterilkan Rod Kaca, 8 Sejukkan Rod Kaca, 9 and 10 Titis Kultur ke Piring B, 11 Ratakan Kultur Bakteria, 12 Tutup dan Terbalikkan Piring, 13 Simpan Piring Petri, 14 Perhati Koloni Bakteria. Scene saved.
- Checked in Play Mode: page 15 (longest title) fits on one line at 1920x1080 (`Temp/qa_screens/scan/a3-p15.png`). Other pages by measured height only.

### Revamp rollout - akt4 - 2026-11-11

- akt4 is built differently: 13 pages in sections (headers "Langkah 1..4" restart per "Bahagian", already titled by the earlier restyle), 58 `DragController`s, 9 `ItemSlot`s, 13 Seterusnya controllers; footer is a white band with the button inside it. So no header changes.
- Applied: `UiDragLift` on 58 draggables; `UiHideSlotCueOnTick` + `UiPopOnEnable` on all 9 slot ticks; `SlotCue` sprite on the 6 plain white slot quads only (the 3 already tinted/translucent green or 0.345 alpha slots on pages `2`... left as designed); `UiScaleInOnEnable` + "Langkah selesai" chip on 12 Seterusnya buttons; on the final page `5 (4)` (button reads "Tamat") a one-line "Eksperimen selesai!" chip. The two-line card used in akt6/akt3 overlapped the "TRANSFORMASI BERJAYA" label here, so it was reduced to one line.
- Checked in Play Mode at 1920x1080 on pages `2` and `5 (4)` (`Temp/qa_screens/scan/a4-2.png`, `a4-54.png`); the one-line card was not re-captured. Not checked: the other 11 pages, drops, other sizes.

### akt4 follow-up checks - 2026-11-11

- Captured all 13 pages (`Temp/qa_screens/scan/a4all-0..12.png`, 1920x1080, button forced visible) and inspected pages index 1 (step 2), 5 (step 3) and 12 (Hasil): chips sit on the slate above the footer button and clear the content; the one-line "Eksperimen selesai!" chip on `5 (4)` clears the "TRANSFORMASI BERJAYA" label. The other 10 captures were not opened.
- Drop test on page `2` (draggable `A` -> slot of `petri dish (1)`, begin-drag/drop/end-drag sent by script): scale 1.00 -> 1.08 -> 1.00; after the drop the tick is active at rest scale 1.31, the cue is hidden and `A`'s `DragController` is disabled. First attempt hit a harness artefact (slot `Start()` had not run yet) and was repeated after a 2 s wait. The only Console entry was the existing "onDrop" `Debug.Log`.
- Still not checked: real mouse/touch drag, other window sizes, the other 10 pages by eye.

### Revamp rollout - akt5 - 2026-11-11

- akt5: 18 pages (14 steps + result pages `15`, `15 (1)`, `15 (2)`), 32 `DragController`s, 1 `ItemSlot` (the large `scale` image, which already has a sprite, so no `SlotCue`), 18 Next controllers (button child is named `Button (1)`).
- Applied: `UiDragLift` on 32 draggables; `UiPopOnEnable` on the one tick; headers "Langkah N" -> "Langkah N daripada 14" (15 texts); `UiScaleInOnEnable` + "Langkah selesai" chip on 12 initially hidden "Seterusnya" buttons (13 chips including the final one); one-line "Eksperimen selesai!" chip above the final "Tamat" button on `15 (2)`. Buttons that are always visible on their page (`9`, `13`, `13 (1)` "Mulakan", `15`, `15 (1)`) were left alone because a chip there would appear before the step is done. Scene saved.
- No step titles: page text is partly stale/duplicated (hidden objects carry the old "Selepas beberapa hari..." lines), so titles from it would be guesses. Needs owner wording.
- Checked in Play Mode at 1920x1080: result page `15 (2)` (chip clear of the graphs and button) and page `6` header ("Langkah 6 daripada 14"). Not checked: other pages, drops, other sizes.

### Revamp rollout - akt7 - 2026-11-11

- akt7: 4 step pages (`2 (1)`, `2`, `3`, `4`, direct children of `Experiments`), 2 `DragController`s (`rubberfilm`), 2 `ItemSlot`s, Next buttons named `Button (1)`.
- Applied: `UiDragLift` on the 2 draggables; `UiPopOnEnable` on the 2 slot ticks; headers "Langkah N" -> "Langkah N daripada 4" (4 texts); `UiScaleInOnEnable` + "Langkah selesai" chip on the 3 initially hidden "Seterusnya" buttons. Scene saved.
- Left alone: the two slots (a white mould shape and a near-transparent 516x100 collision strip, so no `SlotCue`); page `4` buttons ("Seterusnya", "Hasil", which are always visible, so no chip); no step titles (existing page text is not a clean title; needs owner wording). akt7 science content still needs curriculum review.
- Checked in Play Mode at 1920x1080 (`Temp/qa_screens/scan/70-akt7-p2.png`, `70-akt7-p4.png`, pages shown by script): header, chip above the button, and final "Tamat" look right. Not checked: real drops, page 3, other sizes.
- Console after the run held an "onDrop" log and "The variable tick of ItemSlot has not been assigned" (ItemSlot). I did not trace whether these are old entries or from this run; the Play Mode run did not perform a drop.

### Revamp - akt6 3.4/3.5/3.6 and 3.7 drop checks (akt7, akt5) - 2026-11-11

- akt6 3.4/3.5: found already in the scene: `UiDragLift` on 92 of 92 draggables, `UiScaleInOnEnable` on 11 buttons, "Langkah selesai" chips on the 10 hidden "Seterusnya" pages, "Eksperimen selesai!" chip (`CompletionCard`) on the `0 (10)` button. No change made this run.
- akt6 3.6: Welcome is not clipped at 1920x1080 (`Temp/qa_screens/scan/80-welcome.png`). On Prestart Bahan2 the jars were white on a white panel, so the second `Content/Image` is now #DCE5EF and the 6 label images are white (`81-prestart.png`: jars visible). Not fixed: a small notch at the panel's top right where two `Content/Image` rects (1509x632 and 1509x527 at y -48) overlap.
- 3.7 akt7 (Play Mode, drag moved onto slot by script): page 3 gives tick on, Next shown, `DragController` disabled, film parented to the slot. Page 2's slot sits in `table/1 (1)`, which a later button activates; with that applied the result is the same.
- 3.7 akt5 page 1 (`ItemSlot.OnDrop` called with a synthetic `PointerEventData`): tick on, drag disabled, parent `scale`, slot image hidden, local position (0,100,0), `UiDragLift` present.
- Not tested: akt6 physics drops (`OnTriggerStay2D` / `ConditionalCheck`), akt3, akt4, real mouse drags, other window sizes.

### Status check - akt3 and akt4 - 2026-11-11

- akt3 (read-only scan): already carries the pattern: `UiDragLift` 17/17 draggables, tick pop 2/2, 18 `UiScaleInOnEnable` buttons with "Langkah selesai" chips, headers "Langkah N daripada 14: <title>". Open flags: pages 6-9 (`6`, `7`, `8`, `9`) all read "Langkah 6 daripada 14", and Welcome says "Makmal Maya 1" vs "3" (owner to confirm).
- akt4 (read-only scan): `UiDragLift` 58/58, tick pop 9/9, 12 scale-in buttons with chips. Headers are still plain "Langkah N" and the numbering restarts across sub-sections (1-4, 1-3, 1-4, 1-2...), so a "daripada N" total would be a guess. Needs owner confirmation of the real step sequence.
- 3.7 not run for akt3, akt4 or akt6 physics drops. Nothing was changed in this entry.

### Owner replies applied: akt3/akt4/akt5/akt7 headers - 2026-11-11

- akt3: pages 6-9 all reading "Langkah 6 daripada 14" confirmed intended (sub-steps before the next step); left as is. The "Makmal Maya 1" vs "3" flag: all three akt3 texts (Welcome, Prestart Bahan2, Prestart Radas) already read "Makmal Maya 3", and no scene under `Assets` contains "Makmal Maya 1", so there was nothing to change.
- akt4: headers now count the linked pages only (Next goes to the next sibling, so 12 step pages, then the "Hasil" page): "Langkah 1 daripada 12" ... "Langkah 12 daripada 12" (12 texts). The old numbering restarted per "Bahagian"; the "Bahagian N" line is unchanged. Scene saved.
- akt5 and akt7: proposed step titles written by me, for owner review. Edit them in the header text of each page.
  - akt5 (14): Timbang sampel; Tambah bahan pencerna; Tutup bekas sampel; Masukkan ke aksesori; Ketatkan bekas; Masukkan ke dalam digester; Cernakan sampel; Keluarkan bekas sampel; Sediakan alat penurasan; Bilas dan pindahkan sampel (pages `10` and `10 (1)`); Pindahkan ke tiub plastik; Hidupkan alat FIMS; Susun tiub sampel; Mulakan mesin FIMS.
  - akt7 (4): Campurkan bahan lateks; Awet lateks getah; Ukur ketumpatan paut silang; Tangkap sisa dengan filem.
  - Titles were shortened where the 979-unit header would run past one line. Checked in Play Mode at 1920x1080 on akt5 page 6 (`Temp/qa_screens/scan/90-akt5-title.png`): one line, no overlap. Other titles not viewed.
- akt7 console check: after clearing the console, a scripted drop on page 3 produced no errors or warnings, so the earlier "onDrop" and "tick unassigned" lines were stale entries, not from the current scene state. `ItemSlot.tick` is set in `Start()`, which only runs once the slot is active.

### Revamp Phase 4 (login, assistant label, settings) - 2026-11-11

- 4.1 Login (`Assets/Scenes/LoginPage.unity`): `LoginButton` now uses the primary preset (white image, normal/selected #3D5AFE, hover light blue, pressed darker, disabled grey) instead of a white tint over a blue image with a cyan selected colour. `ScienceWelcome` title 36 -> 48. The logo tile and title scrim from the proposal were not applied: the login already places the logo and title on a white reading panel, so a white tile would be invisible and there is no background behind the title for a scrim. The button only calls `SceneController.ChangeScene("Dashboard")`, so "Masuk makmal" stays (it does not claim a sign-in).
- 4.2 Assistant label (shared `Assets/UI/Science/Prefabs/LearningAssistant.prefab`, used by 13 scenes): new `PembantuLabel` child, 112 x 30 under the 88 x 88 brain button at (-272, -104), white sliced `SciencePanel`, `#46556B` edge, "Pembantu" at 20 bold navy, raycasts off. It sits before `ChatBoxAi` in sibling order, so the chat scrim covers it. No scene had to be edited.
- 4.3 Settings: new `Assets/UI/Science/Scripts/UiSound.cs` (PlayerPrefs `ui.sound`, default on, sets `AudioListener.volume`) and `UiSettingsPrompt.cs`. Dashboard gained a neutral "Tetapan" button at the bottom right (168 x 48 at -256, 20) and a 760 x 480 dialog with "Kurangkan gerakan" and "Bunyi" switches ("Hidup" green / "Mati" grey, text carries the state) and a primary "Tutup" button. The dialog reuses the existing `UiFocusScope` (initial focus on the first switch, Escape closes, blocks the Dashboard cards and the "Tetapan" button). Reduce motion uses the existing `ui.reduceMotion` key.
- Checked in Play Mode at 1920x1080: login (`95-login.png`), Dashboard with the label and "Tetapan" (`97-dash-settings-btn.png`), dialog (`97-settings.png`), toggled dialog (`97-settings-on.png`), quiz 1 label (`98-quiz-labelled2.png`), akt6 label (`99-akt6-labelled.png`). Scripted toggles changed the labels to Hidup/Mati, wrote both PlayerPrefs and set `AudioListener.volume` 0/1; "Tutup" closed the dialog; the values were restored afterwards. With reduce motion on, a test `UiPopOnEnable` stayed at scale 1. The assistant still opens, and the label does not block raycasts. Console: no errors.
- Not checked: other window sizes, real mouse/keyboard presses, persistence across an app restart, and the dialog is only on the Dashboard (other scenes have no settings entry). Rollback copies: `Temp/qa_screens/Dashboard.unity.bak`, `LoginPage.unity.bak`, `LearningAssistant-phase4.prefab.bak`.

### Phase 4 follow-up (owner: settings in every scene, sound scope) - 2026-11-11

- Settings moved into the shared `Assets/UI/Science/Prefabs/LearningAssistant.prefab`, so all 13 scenes get them at the same header corner: a "Tetapan" chip (112 x 30) under the brain button at (-272, -104). The Dashboard-only `TetapanButton` and `TetapanPrompt` were removed from `Dashboard.unity`; the dialog, `UiSettingsPrompt`, `UiFocusScope` and wiring now live in the prefab (the dialog is the last child, so it renders and takes clicks above the assistant and the chat).
- Layout trade-off: the "Pembantu" label could not stay. In the quizzes the white question card starts 151 px from the top, so a second stacked chip overlapped it (seen in `101-quiz-chips.png`). The single slot under the brain now holds "Tetapan". The assistant is still named in the chat panel header ("Pembantu pembelajaran"). A hover/focus tooltip for the brain is possible later if the owner wants the name visible before opening.
- Sound scope refined: `UiSound` no longer sets `AudioListener.volume`. It mutes only `AudioSource`s under the scene root named `Sfx` (the quiz effects `salah`, `touch`, `yay`, `claps`), and reapplies the saved setting on every `SceneManager.sceneLoaded`. Other audio, including a future assistant voice, is untouched.
- Checked in Play Mode at 1920x1080: Dashboard (`102-dash-chip.png`), quiz 1 (`102-quiz-chip.png`), akt6 (`102-akt6-chip.png`, `102-akt6-settings.png`). Chip opens the dialog in the quiz and akt6; toggles and "Tutup" work. With sound off the four `Sfx` sources had `mute=true` and `AudioListener.volume` stayed 1; after reloading quiz 1 the new scene's sources were muted again from the saved value; turning sound back on unmuted them. Console: no errors.
- Not checked: the other 10 scenes by eye (same prefab), other window sizes, real mouse/keyboard. The Dashboard scene-specific settings objects were deleted, so its previous bottom-right entry no longer exists.

### Phase 4 follow-up correction (settings chip size) - 2026-11-11

- Final "Tetapan" chip: 88 x 44 at (-272, -102) with 22-unit text, so it matches the brain button's width, meets the 44 px hit target, and clears the quiz card (top edge at 151) and the akt6 instruction band. Text is secondary grey and regular weight so it reads as a link, not a second primary button. Checked in Play Mode at 1920x1080: `105-quiz-chip.png`, `106-akt6-chip.png`, `106-dash-chip.png`.
- The shared prefab carries the dialog, so no per-scene settings objects were added. Only the Dashboard had scene-level objects, and they were removed.

### Phase 4 review fixes - 2026-11-11

- `UiSound` now also mutes the background music (`MusicLoopingController` sources, on the Login scene and kept across scenes), not only the `Sfx` quiz effects. Checked in Play Mode on `LoginPage`: both music sources `mute=True` with sound off, `False` after turning it back on. Compiles with no errors.
- Removed the stray, unassigned `UiSettingsPrompt` from the `TetapanPrompt` dialog object in `LearningAssistant.prefab`; the wired one on the prefab root remains. Dashboard now shows one prompt.
- Not checked: music continuing to stay muted across the Login -> Dashboard load, other scenes.

### akt7 re-check - 2026-11-11

- Confirmed in the scene (no changes made): `UiDragLift` on 2/2 draggables, `UiPopOnEnable` on both slot ticks, `UiScaleInOnEnable` on 3 hidden "Seterusnya" buttons with "Langkah selesai" chips, "Langkah N daripada 4" headers. This matches the akt7 rollout entry above; the rollout was already finished.

### ItemSlot tick race - 2026-11-11

- `ItemSlot.cs`: `tick` is now resolved in `Awake()` instead of `Start()`. The "variable tick of ItemSlot has not been assigned" message came from `OnTriggerStay2D` firing (akt6/akt7) before `Start` on a freshly enabled slot. Behaviour is otherwise unchanged. Not re-run in Play Mode.

### Music mute across scene load - 2026-11-11

- Play Mode: sound switched off on `LoginPage`, then `Dashboard` loaded by script. After the load both music sources (`1`, `1 (1)`) were `mute=True` and `UiSound.Enabled` stayed false. Sound was switched back on afterwards. Only Login -> Dashboard was tested, not the experiment or quiz scenes.
- Same check on `akt7`, `akt3` and `Uji Diri 1` (loaded from Play Mode by script): every audio source, music and quiz effects, was muted with sound off. Sound re-enabled afterwards.

### akt5 stale body text removed - 2026-11-11

- Deactivated (not deleted) 30 TMP objects, `Text (TMP) (2)` and `(3)` on pages 4-13 and 15, whose text was the copied "Selepas beberapa hari (2-5 hari)..." / "Aliran elektron..." paragraph. Each page keeps its own step instruction. Re-enable them if the owner supplies real wording.
- 7 full-width instruction texts (`Text (TMP) (4)`) now stop 400 px short of the right edge (`offsetMax.x = -400`) so long lines wrap instead of running under the "Tetapan" chip. Checked on page 7 at 1920x1080 (`Temp/qa_screens/scan/35-card.png`); other pages not captured.
- Draft supporting text for akt5 pages 4-14 written to `docs/akt5-draft-wording.md` for owner review; not applied to the scene.

### akt5 approved notes applied - 2026-11-11

- Owner approved `docs/akt5-draft-wording.md`. The 12 notes (pages 4-9, 10, 10 (1), 11, 12, 13, 13 (1)) now use the re-enabled `Text (TMP) (2)` on each page: italic navy 28 pt, top-anchored in the white band under the header (y -200 under two-line instructions, -140 under bottom-anchored ones), right edge 400 px short of the canvas edge so it clears the Tetapan chip. Captured pages 5, 7, 9 at 1920x1080 (`Temp/qa_screens/scan/note{5,7,9}.png`); the other nine not captured.
- Pre-existing, not changed: on bottom-instruction pages (e.g. page 5) the instruction line overlaps the "Aksesori digester" label.

### akt5 bottom-instruction overlap fixed - 2026-11-11

- On the 7 pages whose step instruction was bottom-anchored (4, 5, 6, 8, 11, 12, `13 (1)`), `Text (TMP) (4)` now sits in the white band under the header (top-left aligned, navy, 32 pt, right edge 400 px short of the canvas edge) with the approved note below it at y -156. This removes the overlap with the "Aksesori digester" label. Captured pages 5 and 12 (`Temp/qa_screens/scan/fix5.png`, `fix12.png`): clear. Page 15 (result) has no overlap and was left as is. Pages 4, 6, 8, 11 and `13 (1)` not captured; `13 (1)` has the longest instruction and could wrap onto the note.
