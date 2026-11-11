# UI feel: where it lives and how to change it

Each effect is a small component under `Assets/UI/Science/Scripts/`. Values are Inspector fields on the component, so edit them on the object in the scene or prefab. There is no shared `UiFeelSettings` asset and no Preset assets; the original plan proposed them, but the revamp used per-component fields instead.

## Components

| Component | Put it on | Fields (default) |
|---|---|---|
| `UiPopOnEnable` | A slot's `tick` child | `startScale` 0.6, `overshoot` 1.08, `duration` 0.16 s |
| `UiScaleInOnEnable` | A Seterusnya button that starts hidden | `startScale` 0.85, `duration` 0.2 s |
| `UiDragLift` | An object with a `DragController` | `liftScale` 1.08 |
| `UiHideSlotCueOnTick` | A slot's `tick` child | none; hides the slot's cue image while the tick is active |
| `UiFeedbackMotion` | Quiz answer buttons and the coin | `popScale` 1.04, `popDuration` 0.16, `wobbleDegrees` 1.5, `wobbleDuration` 0.18 |
| `UiCardHover` | Dashboard activity cards | `restColor` #46556B, `activeColor` #0B0B3B, `duration` 0.12 s |
| `UiPlaySoundOnEnable` | An object that should play a cue when it appears | `clip`, `volume` 0.55, `pitch` 1; silent for 0.5 s after a scene loads |
| `DragController` | A draggable | `returnToOrigin` (off by default; on for akt6), `returnDuration` 0.18 s |

## Add the pop to a new tick

1. Select the slot's `tick` child (it must stay child 0 of the slot; `ItemSlot` reads `GetChild(0)`).
2. Add `UiPopOnEnable` and `UiHideSlotCueOnTick`.
3. Keep the tick's rest scale as it is; the pop returns to it.

## Settings the learner controls

Both are saved in `PlayerPrefs` and set from the "Tetapan" dialog in the shared `LearningAssistant` prefab.

- `ui.reduceMotion` (`UiMotion.Reduced`): every effect above skips its motion and shows the final state.
- `ui.sound` (`UiSound.Enabled`, default on): mutes sources under the scene root `Sfx`, the background music, and any source with `UiPlaySoundOnEnable`.

## Rules to keep

- Scale and rotation only; never move layout-owned objects.
- New visuals go on existing objects as components, or as last children with raycast off. Do not wrap or reorder gameplay objects (see the hierarchy contracts in `UI_REVAMP_PLAN.md`).
- Scaling a dragged object also scales its `BoxCollider2D`. akt6 and akt7 slots use triggers, so test the drop after changing `liftScale`.
- Colours carry meaning (green and red only after a verified result); see section 3 of `UI_REVAMP_PLAN.md`.
