# UI Feel, Colours, and Settings Guide

This guide explains where the science UI feel components, colour settings, and learner switches live in the Unity project, how to edit them in the Inspector, and what hierarchy rules to preserve.

The original revamp proposal ([UI_REVAMP_PLAN.md](../../UI_REVAMP_PLAN.md)) suggested a shared `UiFeelSettings.asset` ScriptableObject and Unity `.preset` assets. The implementation uses **serialized Inspector fields on each component** instead, so there is no `UiFeelSettings` or Preset file to sync—select the GameObject in the scene or prefab and edit its component directly.

## 1. Files and Assets

| Path | Role |
|---|---|
| `Assets/UI/Science/Scripts/UiMotion.cs` | Static `UiMotion.Reduced` switch (`PlayerPrefs` key `ui.reduceMotion`) |
| `Assets/UI/Science/Scripts/UiSound.cs` | Static `UiSound.Enabled` switch (`PlayerPrefs` key `ui.sound`, default `1`); mutes `Sfx`, `MusicLoopingController`, and `UiPlaySoundOnEnable` sources on scene load |
| `Assets/UI/Science/Scripts/UiSettingsPrompt.cs` | Controller for the "Tetapan" dialog (`Kurangkan gerakan` and `Bunyi` toggles) |
| `Assets/UI/Science/Scripts/UiPopOnEnable.cs` | One-shot scale pop when an object becomes active (slot `tick` children) |
| `Assets/UI/Science/Scripts/UiScaleInOnEnable.cs` | Ease-out cubic scale-in when a hidden action button (`Seterusnya`) appears |
| `Assets/UI/Science/Scripts/UiDragLift.cs` | Scales a draggable while `DragController` is active and being dragged |
| `Assets/UI/Science/Scripts/UiHideSlotCueOnTick.cs` | Hides the parent `ItemSlot`'s `Image` while the `tick` child is active |
| `Assets/UI/Science/Scripts/UiFeedbackMotion.cs` | `Pop()` and `Wobble()` on quiz answer buttons and the coin icon |
| `Assets/UI/Science/Scripts/UiCardHover.cs` | Fades a Dashboard activity card's border colour on pointer hover or keyboard selection |
| `Assets/UI/Science/Scripts/UiFocusIndicator.cs` | Drives a `UnityEngine.UI.Outline` for keyboard focus vs. resting border |
| `Assets/UI/Science/Scripts/UiFocusScope.cs` | Sets initial selection, traps modal focus, blocks underlying `CanvasGroup`s, and binds `Escape` |
| `Assets/UI/Science/Scripts/UiPlaySoundOnEnable.cs` | Plays an `AudioClip` when enabled (after the first `0.5 s` of scene load) |
| `Assets/UI/Science/Scripts/QuizProgressPills.cs` | Updates the 5–6 question status pills (`Betul` / `Salah`) in `Uji Diri 1`–`7` |
| `Assets/UI/Science/Scripts/UiActivityArrows.cs` | Shows `Aktiviti sebelum / berikut` only on Dashboard activity hubs and disables them at the ends |
| `Assets/UI/Science/Scripts/UiProgress.cs` | Static local progress store (`PlayerPrefs` keys `ui.progress.<id>`) and `ResetAll()` |
| `Assets/UI/Science/Scripts/UiProgressMarker.cs` | Sets one progress id complete when its finish object activates |
| `Assets/UI/Science/Scripts/UiProgressBadge.cs` | Shows the "Selesai" chip on a Dashboard hub station when its id is complete |
| `Assets/UI/Science/Scripts/UiProgressBadgeGroup.cs` | Refreshes every badge under the Dashboard `Into` object when it becomes active or a scene loads |
| `Assets/UI/Science/Prefabs/LearningAssistant.prefab` | Shared persistent overlay (`CanvasPrime` / `LearningAssistant`) containing the brain button, `Tetapan` chip, chat box, and `TetapanPrompt` dialog |
| `Assets/UI/Science/Sprites/SlotCue.png` | 9-sliced cyan frame (`#4FC3F7` border, faint cyan fill) used on `ItemSlot` images |
| `Assets/DragController.cs` | Existing drag script extended with `returnToOrigin` and `returnDuration` |
| `Assets/ScoreController.cs` | Existing quiz score script extended with `coinMotion` and `countDuration` |
| `Assets/QuestionController.cs` | Existing quiz controller with serialized answer/feedback colours and `progressPills` reference |

## 2. Component Reference

| Component | Attached to | Inspector fields and defaults |
|---|---|---|
| `UiPopOnEnable` | `ItemSlot` `tick` child (`GetChild(0)`) | `startScale` `0.6`, `overshoot` `1.08`, `duration` `0.16` s |
| `UiScaleInOnEnable` | Hidden step `Button` (`Seterusnya`) | `startScale` `0.85`, `duration` `0.2` s |
| `UiDragLift` | Same GameObject as `DragController` | `liftScale` `1.08` |
| `UiHideSlotCueOnTick` | `ItemSlot` `tick` child | None (reads `transform.parent.GetComponent<Image>()`) |
| `UiFeedbackMotion` | Quiz answer buttons (`A`–`D`) and `Coin` | `popScale` `1.04`, `popDuration` `0.16` s, `wobbleDegrees` `1.5`, `wobbleDuration` `0.18` s |
| `UiCardHover` | Dashboard card border `Graphic` | `restColor` `#46556B`, `activeColor` `#0B0B3B`, `duration` `0.12` s |
| `UiFocusIndicator` | Focusable `Button`s (requires `Outline`) | `focusColor` `#0B0B3B`, `outlineWidth` `4`, `borderColor` `#46556B`, `borderWidth` `1.5` |
| `UiPlaySoundOnEnable` | Slot `tick`s, `Seterusnya` buttons, `CompletionCard` | `clip` (`touchglass` or `yay`), `volume` `0.55` (`0.28` on steps, `0.6` on completion), `pitch` `1` (`1.15` on steps) |
| `DragController` | Experiment draggables | `returnToOrigin` (enabled in `akt6`, off elsewhere), `returnDuration` `0.18` s |
| `ScoreController` | Quiz score manager | `coinMotion` (`UiFeedbackMotion`), `countDuration` `0.4` s |

## 3. Common Manual Edits

### Change a duration or scale

1. Open the target scene (for example `Assets/Scenes/Experiments/akt6.unity` or `Assets/Scenes/Quiz/Uji Diri 6.unity`).
2. Select the object that owns the effect:
   - **Tick pop:** `Canvas/Experiments/Content/<page>/.../collider2d/tick` → `UiPopOnEnable`.
   - **Next button scale-in:** `Canvas/Experiments/Content/<page>/Button` → `UiScaleInOnEnable`.
   - **Drag lift or return speed:** draggable object → `UiDragLift` (`liftScale`) or `DragController` (`returnDuration`).
   - **Quiz score count-up:** `ScoreController` object → `ScoreController` (`countDuration`).
3. Adjust the slider in the Inspector and save the scene (`Ctrl+S`).
4. **Expected result:** In Play Mode (with `Kurangkan gerakan` set to `Mati`), the animation runs with the new timing or scale. Because values are per-instance on scene objects, edit the other scenes if you want the change everywhere.

### Change a button or quiz colour

Button states and quiz feedback colours are stored on standard uGUI / custom components rather than Preset files:

- **Primary action buttons (`Mula eksperimen`, `Seterusnya`, `Masuk makmal`):** select the `Button` component and edit its `ColorBlock`:
  - `Normal Color` and `Selected Color`: `#3D5AFE`
  - `Highlighted Color`: `#5C74FF`
  - `Pressed Color`: `#2A44D4`
  - `Disabled Color`: `#A0A9B9`
  - Keep `Selected Color` equal to `Normal Color` so keyboard selection shows the `UiFocusIndicator` outline instead of a permanent fill.
- **Neutral buttons (`Kembali`, `Tutup`, `Laman utama`, `Cuba lagi`):** white `Image` (`#FFFFFF`), `Selected Color` = `Normal Color` (`#FFFFFF`), with `UiFocusIndicator` providing the `#46556B` resting border and `#0B0B3B` focus ring.
- **Quiz answer and reveal colours:** open `Assets/Scenes/Quiz/Uji Diri 1.unity`–`7.unity`, select the object with `QuestionController`, and edit:
  - `answerColor`: `#E8EFF8` (resting answer fill)
  - `correctAnswerColor`: `#DDEFE1` (revealed correct option)
  - `otherAnswerColor`: `#F8E7E7` (other options after reveal)
- **Quiz progress pills:** on `Canvas/QuizProgress` (`QuizProgressPills`), edit `pendingColor` (`#DCE5EF`), `currentColor` (`#FFFFFF`), `correctColor` (`#166534`), and `wrongColor` (`#B91C1C`).
- **Tetapan switch labels:** open `Assets/UI/Science/Prefabs/LearningAssistant.prefab` and edit `onColor` (`#166534`) and `offColor` (`#46556B`) on the root `UiSettingsPrompt`.

### Add the pop, cue hide, and sound to a new `ItemSlot` tick

1. Ensure the `tick` GameObject is **child index 0** of the `ItemSlot` (`ItemSlot.Awake()` calls `transform.GetChild(0)`).
2. On the `ItemSlot` GameObject itself, assign `Assets/UI/Science/Sprites/SlotCue.png` to its `Image` component (`Image Type = Sliced`, `Color = #FFFFFF`).
3. Select the `tick` child (which should start inactive) and add:
   - `UiPopOnEnable` (`startScale = 0.6`, `overshoot = 1.08`, `duration = 0.16`)
   - `UiHideSlotCueOnTick`
   - `AudioSource` (`Play On Awake = false`, `Loop = false`, `Spatial Blend = 0`)
   - `UiPlaySoundOnEnable` (`clip = Assets/sfx/touchglass.wav`, `volume = 0.55`, `pitch = 1`)
4. **Expected result:** When an item is dropped into the slot and `tick` activates, the cyan slot border hides, the tick pops to `1.08×` and settles at its original scale, and `touchglass` plays once.

## 4. Learner Settings (`Tetapan`)

The learner can open **Tetapan** from the chip directly under the top-right brain button in every scene (owned by `Assets/UI/Science/Prefabs/LearningAssistant.prefab`):

- **Kurangkan gerakan (`UiMotion.Reduced`, key `ui.reduceMotion`):** when set to `Hidup`, `UiPopOnEnable`, `UiScaleInOnEnable`, `UiDragLift`, `UiFeedbackMotion`, `UiCardHover`, `ScoreController`, and `DragController` skip their animations and apply the final state immediately.
- **Bunyi (`UiSound.Enabled`, key `ui.sound`):** when set to `Mati`, `UiSound.Apply()` mutes all `AudioSource`s under the `Sfx` root, `MusicLoopingController` background music, and all `UiPlaySoundOnEnable` components across scene loads.

> **Testing tip:** If `UiDragLift` or `UiPopOnEnable` appears to do nothing in the Editor, open `Tetapan` in Play Mode (or check `PlayerPrefs.GetInt("ui.reduceMotion")`) and verify that `Kurangkan gerakan` is `Mati`.

### Local progress ("Selesai" chips)

Progress is saved per stage with an explicit id, `a{1-7}.belajar`, `a{1-7}.eksperimen` or `a{1-7}.ujidiri`. The id is set in the Inspector, not derived from scene or object names.

- **Finish marker:** `UiProgressMarker` goes on the object that completes a stage (the last lesson button, the experiment's final button, or the quiz's result object). Set its `progressId`. It writes the id when it activates.
- **Hub badge:** `UiProgressBadge` goes on each station's `ProgressBadge` under `Into/Content/N`. Its `progressId` must match the marker that completes that stage.
- **Reset:** the "Set semula kemajuan" button in `Tetapan` calls `UiProgress.ResetAll()`, which clears all 21 ids and the keys from the first version.
- **Hubs 1 and 2:** both lessons share one Belajar id (`a1.belajar`, `a2.belajar`), so finishing either lesson ticks that hub's Belajar station.

## 5. Non-Obvious Constraints and Contracts

- **Never reorder or wrap gameplay children:**
  - `ItemSlot` requires `tick` at `transform.GetChild(0)`.
  - `QuestionController` requires the answer label `TextMeshProUGUI` at `answerBtn[i].GetChild(0)` and the three avatar faces at `avatarState` children `0`–`2`.
  - `MenuSlideController` (`Canvas/Container/Into/Content` in `Dashboard`) and `NextExperimentController` iterate direct children by sibling index.
  - `ConditionalCheck` parses the numeric prefix of its GameObject name (`0 (2)`, `3 (1)`).
  - Attach new feel scripts directly to existing objects, or add purely visual objects (`StepDoneLabel`, `CompletionCard`, `PageIndicator`) as **last children** with `raycastTarget = false`.
- **Collider scaling during drag:** `UiDragLift` scales the dragged `Transform`, which also scales its `BoxCollider2D`. In `akt6` and `akt7` (`ItemSlot.OnTriggerStay2D`), re-test drops after increasing `liftScale`.
- **Trigger-drop delay on `returnToOrigin`:** `DragController.ReturnToStart()` waits **two `FixedUpdate` steps** after `OnEndDrag` before checking whether the item was reparented or disabled by `ItemSlot.OnTriggerStay2D`. Do not remove those waits, or `akt6` physics drops will snap back before the trigger fires.
- **Scene-load sound guard:** `UiPlaySoundOnEnable.OnEnable()` ignores activations while `Time.timeSinceLevelLoad < 0.5f` so objects that start active in a scene do not play a sound cue on load.

## 6. Verification Status and Limitations

- **Verified in Editor Play Mode (scripted):**
  - `Uji Diri 1`–`7` progress pills, answer feedback motion, score count-up, and dialog focus.
  - `akt3`–`akt7` tick pops, slot-cue hiding, drag lift (`1.00 → 1.08 → 1.00`), `Seterusnya` scale-in, and sound cues.
  - `akt6` `returnToOrigin` on valid drops, outside drops, and wrong-electrode drops with `Kurangkan gerakan` both on and off.
  - `Tetapan` switches persisting in `PlayerPrefs` and muting `Sfx`, music, and `UiPlaySoundOnEnable` across scene loads.
- **Not yet verified:**
  - Physical mouse/touch drag walkthroughs across all five experiments (quiz mouse/keyboard was verified by the owner).
  - Listening to audio cues in a standalone desktop player build (`Temp/ScienceUIBuild` needs to be rebuilt after the latest scene edits).
  - Progress chips through real finish routes. Only `a6.eksperimen` and `a4.belajar` were checked, by activating the marker in Play Mode. The other ids were set by script.
