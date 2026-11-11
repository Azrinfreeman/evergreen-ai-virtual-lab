# Evergreen STEM VirtualLab — UI and Game-Feel Revamp Proposal

Status (2026-11-12): **Phases 1 to 4 are implemented**, with the deviations recorded in [IMPLEMENTATION.md](IMPLEMENTATION.md) (for example text-only cards, no hover lift, the "Pembantu" label replaced by "Tetapan"). Phase 5 is mostly done: the complete guide is at `docs/tutorial/ui-feel.md`, 10/10 EditMode tests pass, and `Temp/ScienceUIBuild/Evergreen.exe` builds cleanly and passes a 1280×720 startup check; interactive standalone play, the full six-resolution matrix on newer pages, and the approvals A1, A9, A10 remain open. The checkboxes below were not ticked as work landed, so read the evidence log, not the boxes. Originally written 2026-11-10 after a full Play Mode scan as a proposal. Owner approval is required before any phase starts, and items marked **Needs approval** also conflict with, or extend, [AGENTS.md](AGENTS.md).

Audience: learners around 16 years old. Target feel: a real working lab that is professional enough to trust and responsive enough to be fun. "Fun" here means the interface reacts clearly to what the learner does. It does not mean decoration.

## Contents

1. [What the scan found](#1-what-the-scan-found)
2. [Design concept](#2-design-concept)
3. [Colour system with meaning](#3-colour-system-with-meaning)
4. [Type, spacing, and control states](#4-type-spacing-and-control-states)
5. [Screen-by-screen proposals](#5-screen-by-screen-proposals)
6. [Game feel](#6-game-feel)
7. [Ideas that need owner approval](#7-ideas-that-need-owner-approval)
8. [Implementation phases](#8-implementation-phases)
9. [Verification](#9-verification)
10. [Risks and hierarchy contracts](#10-risks-and-hierarchy-contracts)
11. [Decisions for the owner](#11-decisions-for-the-owner)

## 1. What the scan found

Source: Play Mode scan on 2026-11-10 at Game view 850 × 478 (captured ×2), plus read-only preview-scene queries. Screenshots are in `Temp/qa_screens/scan/` (temporary; they are deleted if `Temp` is cleared). Only the default path through each screen was captured; the six target resolutions were not checked in this scan.

| Screen | Finding | Why it matters |
|---|---|---|
| Login (`09-login`) | The Evergreen-STEM logo sits on an olive-green square that clashes with the navy palette. "Masuk makmal" uses a saturated blue (about `#1440FF`) that is not action blue `#3D5AFE`. | The first screen sets the identity and uses two unrelated colours. |
| Dashboard overview (`01-dashboard`) | Seven plain white cards with small text and no art. Every card has `ArtMask/Art`, but it is disabled and all seven share the same `bg2` sprite. The first card shows a flat cyan fill on load. | The cards are hard to tell apart. The cyan fill looks as if a choice was already made. Nothing shows what each activity contains. |
| Activity hub (`Into/Content/1..7`) | Only activity 4 has the new labels ("Belajar teori", "Mula eksperimen", "Uji diri"). Activities 1–3 and 5–7 still use all-caps labels such as "JOM BELAJAR TEORI", "MAKMAL MAYA PENKULTURAN BAKTERIA", "MAKMAL MAYA MFC" and "MAKMAL MAYA RUBBER FILM". Activities 1 and 2 have two lesson entries and no experiment. | Same role, different language and casing. The order (learn, then experiment, then quiz) is implied only by stacking. |
| Lesson page (`02`–`04`) | One row holds four buttons: Aktiviti sebelum, Kembali/Sebelum, Seterusnya, Aktiviti berikut. There is no page counter. Lesson 4's first page has clean navy/cyan apparatus drawings with no labels. Its second page opens mid-sentence ("merujuk kepada…") with the heading out of view. | Learners can't tell where they are or which button turns the page and which changes the activity. |
| akt6 Welcome (`05`) | Header buttons are clipped at the top edge. The title mixes English and Malay ("Mikrobial Fuel Cell"). The purpose card is thin, with a lot of empty slate around it. | Clipped controls look broken. The page doesn't build anticipation for the experiment. |
| akt6 Prestart (`06`) | White plastic containers disappear on the white panel. Label casing is mixed (PENUTUP, MULTIMETER, "wayar elektrik"). | Apparatus that can't be seen can't be learned. |
| akt6 procedure (`07`) | "Langkah 1" shows no total. "Cabut penutup balang" is a real Button but reads as a label. Nothing marks the object to interact with. | Learners have to guess what to do and how far along they are. |
| Quiz, Uji Diri 6 (`08`) | The most playful screen: illustrated lab, boy avatar with three faces, coin score. The first answer shows cyan on load. "Soalan 1" has no total. The coin/score block is cut off at the right edge. After answering, the correct answer is labelled "Betul:", but the learner's own pick isn't marked. | The preselected look suggests a choice was made. When the learner is wrong, they can't see what they picked next to the correct answer. |
| Quiz result (`CongratPrompt`) | "Uji diri selesai", score from `loadScore.cs` (real: `score / questions × 10`), "Laman utama" and "Cuba lagi". | The data is real but presented flat: no per-question summary, no message tied to the result. |
| Assistant | The 88 × 88 brain icon is top right on every screen, with no visible text. | Its meaning depends on the picture alone. |

Things that already work and should be kept: the navy/white/blue restyle across all 14 scenes, the slate `#26345A` bench in akt3–akt7, the primary button colour block, the quiz feedback colours with the "Betul:" prefix, the "Sedang menyemak jawapan..." lock, the quiz avatar faces and sounds (`yay`, `claps`, `salah`), and the cyan-on-navy lesson illustrations.

## 2. Design concept

**A working lab with a science notebook.** Each kind of screen keeps one material, so the surface itself tells the learner what kind of task they are doing:

| Material | Colour | Where | Meaning to the learner |
|---|---|---|---|
| Frame | Navy `#0B0B3B` | Header bars, navigation | "This is where I move around." |
| Paper | White `#FFFFFF` | Lessons, questions, dialogs, instruction bands | "This is where I read." |
| Bench | Slate `#26345A` | Experiment workspace | "This is where I use my hands." |
| Quiz room | Existing illustrated lab + coin | Uji diri scenes | "This is where I test myself." |

Three principles keep "fun" from turning into decoration:

1. **Every reaction answers an action.** A drop, a correct answer, or a finished step gets an immediate, short response. Nothing moves on its own.
2. **Show position honestly.** Counters ("Soalan 2 daripada 5", "Langkah 3 daripada 8", "Halaman 2 daripada 4") use real totals and appear only where the totals come from real data.
3. **The task is always the loudest thing.** Apparatus, the question, or the lesson text dominates. Navigation, the assistant, and the score stay smaller and in fixed places.

## 3. Colour system with meaning

### 3.1 One colour, one job

Each colour has one meaning. A colour that means something else somewhere else is a bug.

| Role | Hex | Meaning | Use | Never use for | Contrast (measured) |
|---|---|---|---|---|---|
| Frame navy | `#0B0B3B` | Structure and navigation | Header bar, text on light surfaces | Status | White on navy ≈ 18:1 |
| Action blue | `#3D5AFE` | **"Press this to move forward."** | One primary action per view: Mula eksperimen, Seterusnya, Hantar | Selection, decoration, headings | White label ≈ 5.1:1 |
| Instrument cyan | `#4FC3F7` | **"The lab is pointing at this."** | Focus ring, the interaction target in an experiment, diagram highlights | Text on white (≈ 2.0:1, fails) | On navy ≈ 9:1, on slate ≈ 6:1 |
| Bench slate | `#26345A` | Hands-on workspace | Experiment background only | Reading surfaces | White text ≈ 12:1 |
| Correct / complete | `#166534` | **Verified correct or done** | Only after the program confirms it, with ✓ and text | Hover, encouragement, decoration | On white ≈ 7:1 |
| Error | `#B91C1C` | **Wrong or destructive** | Wrong answer (with ✗ and text), destructive confirm | Back, Close, Tutup | On white ≈ 6.5:1 |
| Caution | `#92400E` | **Safety note** | Real safety guidance from the lesson, with an "Awas" label | Wrong answers | On white ≈ 7:1 |
| Score gold | `#F2B705` (proposed) | **Points earned** | Coin, score counter, result score | Buttons, warnings | Navy text on gold ≈ 10:1 |
| Secondary text | `#46556B` | Supporting information | Captions, counters | Instructions | On white ≈ 7.6:1 |

Contrast values are calculated from the hex codes with the WCAG relative-luminance formula. They still have to be confirmed in the Game view over the actual sprites.

### 3.2 Stage wayfinding colour

Activities have up to three stages. Each stage's colour previews the surface it opens, so the colour carries information instead of being a theme:

| Stage | Marker colour | Icon (to choose from existing art or draw simply) | Opens |
|---|---|---|---|
| Belajar teori | Paper: white chip, navy edge, navy icon | Open notebook | White reading pages |
| Eksperimen | Slate `#26345A` chip, white icon | Flask or beaker | Slate bench |
| Uji diri | Gold `#F2B705` chip, navy icon | Coin or question mark | Quiz room with coin score |

Stage markers always appear with their text label and in a fixed order, so the colour is never the only cue.

### 3.3 Rules

1. **Scientific colour is content, not theme.** Wire colours, substances, indicators, bacteria colonies, and apparatus art keep their colours. Never recolour them for style. (The akt6 wire tint was an exception approved by the owner; record any future one the same way.)
2. **Green and red only follow a verified result.** Never use them for hover, encouragement, or decoration.
3. **One action-blue button per view.** If two buttons both look primary, one of them isn't.
4. **On white surfaces, cyan needs a navy edge.** Use a 2-unit cyan ring inside a 2-unit navy ring, or a darker indicator.
5. **Pair every state colour with a shape and words:** ✓ + "Betul", ✗ + "Salah", "Awas:" + text. Check every status screen in grayscale and with deuteranopia and protanopia simulation.

### 3.4 Tonal scales (for states only)

These are used for hover, pressed, and disabled states. They are not new roles.

| Base | Hover | Pressed | Disabled | Tint surface |
|---|---|---|---|---|
| Action `#3D5AFE` | `#5C74FF` (in use) | `#2A44D4` (in use) | `#A0A9B9` (in use) | `#E8EDFF` |
| Navy `#0B0B3B` | `#1C1C5A` | `#05051F` | `#6B7280` | `#E8EFF8` (current `answerColor`) |
| Correct `#166534` | — | — | — | `#DDEFE1` (current `correctAnswerColor`) |
| Error `#B91C1C` | — | — | — | `#F8E7E7` (current `otherAnswerColor`) |
| Gold `#F2B705` | — | — | — | `#FFF4CC` |

Where it is edited: Button colour blocks become Unity **Preset** assets under `Assets/UI/Science/Presets/` ("Primary button", "Neutral button", "Answer button"), applied from the Inspector's preset selector. Quiz feedback colours stay as the serialized fields that already exist on `QuestionController`.

## 4. Type, spacing, and control states

Keep the rules already in [AGENTS.md](AGENTS.md): LiberationSans for body and controls, FredokaOne for short identity headings only, 8-unit spacing, and sizes at 1920 × 1080 of 48–60 for titles, 36 for sections, 28–32 for body, and 22–24 for labels.

Additions:

- **Counters** ("Soalan 2 daripada 5") use 24-unit secondary text with the current number in bold navy, so the learner's position is readable at a glance.
- **All-caps is removed** from every button and label. The activity hubs for activities 1–3 and 5–7 still need this.
- **English terms in titles**, such as "Mikrobial Fuel Cell", "MFC", and "RUBBER FILM", are flagged for owner review. They are not rewritten automatically, because some may be established names.
- **Control states:**

| State | Primary | Neutral (Kembali, Tutup) | Answer option | Interaction target (bench) |
|---|---|---|---|---|
| Normal | Blue fill, white label | White fill, navy 2-unit border, navy label | `#E8EFF8` fill, navy label | Visible outline (see 5.5) |
| Hover | `#5C74FF` | `#E8EFF8` fill | Navy 2-unit border | Outline brightens |
| Pressed | `#2A44D4`, scale 0.97 for 80 ms | `#D5DFEC` | Scale 0.98 | — |
| Focused (keyboard) | Cyan + navy double ring | Same | Same | Same |
| Selected (chosen) | — | — | Navy border, "Pilihan anda" tag | — |
| Disabled | `#A0A9B9`, label `#F2F6FA` | 50% opacity | Existing lock path (`Button.enabled = false`) | Hidden |

The **focused** and **selected** states must look different. On the quiz, the cyan fill on load is the focus colour sitting on the auto-selected button, and it reads as "chosen".

## 5. Screen-by-screen proposals

### 5.1 Login

- Put the existing logo on a white tile with an 8-unit radius and padding, so the olive square reads as part of the logo artwork and not as a coloured panel. Don't recolour the logo; it is a flattened asset.
- Change "Masuk makmal" to the primary preset (`#3D5AFE`).
- Keep the illustrated lab background with a short navy-to-transparent scrim behind the title, so the welcome feels like walking into the lab.
- Inspect the login flow first. If the button only loads the Dashboard, its label shouldn't suggest a sign-in.

### 5.2 Dashboard: the activity map

Replace the seven identical cards with **activity cards that show what is inside**:

```
┌──────────────────────────────┐
│ [lesson illustration]        │  ← enable ArtMask/Art, one real image per activity
│                              │
│ Aktiviti 6                   │  ← 22, secondary
│ Sel bahan api mikrob         │  ← 32, navy, real title (wording per owner)
│ ○ Belajar ─ ● Eksperimen ─ ◆ │  ← stage markers: only the stages that exist
└──────────────────────────────┘
```

- **Card art:** use one existing illustration from each activity's own lesson pages (lesson 4's apparatus drawings are already in the right style). Phase 0 lists which sprites exist. Where no suitable image exists, the card shows no art; it doesn't get a stock picture.
- **Stage strip:** activities 1 and 2 show Belajar → Uji diri; activities 3–7 show all three. This is content information, not progress.
- **No preselection.** Remove the visible selected fill on load. Keyboard focus still starts on the first card, but shows the focus ring, not the "chosen" look.
- **Hover:** lift the card 4 units (translate, not scale) and darken its border to navy over 120 ms. This tells the learner the card responds without moving the layout.

### 5.3 Activity hub: a three-station path

Turn the three stacked buttons into a left-to-right path that matches the stage strip on the card:

```
 ① Belajar teori  ───────  ② Mula eksperimen  ───────  ③ Uji diri
   Notebook icon             Flask icon                   Coin icon
   "Kenali konsep…"          "Tujuan: …"                  "5 soalan"
   [Buka]                    [Mula]                       [Mula]
```

- Each station uses one line of real text: the lesson heading, the experiment's purpose from its Welcome page, and the question count from `QuestionList` (5, or 6 for Uji diri 7).
- The numbers ① ② ③ are a suggested order; they don't lock stages. Nothing is locked unless the owner approves locking (section 7).
- Apply the activity 4 wording to the other six hubs: "Belajar teori", "Mula eksperimen", "Uji diri". For activities 1 and 2, use their two real lesson titles in sentence case.
- Move "Aktiviti sebelum / berikut" here (and only here), as small neutral arrows next to the activity title. That removes them from lesson pages.

### 5.4 Lessons

- **One navigation model:** bottom bar with neutral "Sebelum" on the left, "Halaman N daripada M" centred, and primary "Seterusnya" on the right ("Tamat" on the last page). "Kembali" sits top left in the header and goes to the activity hub. Phase 0 counts M from the real pages under each `Into/Content/N` branch.
- **Long pages:** fix lesson 4's second page so its heading is visible at the top of a scroll view with the scroll position reset to the top. This is a layout fix; the text stays as it is.
- **Diagram labels:** add labels to apparatus drawings only where the lesson text already names the item (for example, "kelalang kon" if the page mentions it). Owner review is required before any label is added.
- **"Maklumat" popups and numbered tabs (1–5):** keep them. Restyle the tabs as a segmented control with the current tab in navy and a "Maklumat" pill (navy edge, ⓘ icon), so they read as optional detail.
- **Reading surface:** white panel, about 60–70 characters per line, 30-unit body text, and navy headings at 36.

### 5.5 Experiments (bench HUD)

The bench treatment is done. This proposal adds clearer interaction cues.

- **Step counter:** "Langkah 3 daripada 8" in the header, using the page-to-step mapping from IMPLEMENTATION.md Stage 0.2. Pages that are phases of one step keep the same number. Where the mapping is uncertain, show only the step title.
- **Interaction target cue:** when a page opens, the object the learner must act on gets a cyan 3-unit outline with a navy edge, plus a short hint chip ("Seret elektrod ke dalam balang"), using wording that already appears in the instruction. The cue disappears once the action happens. The outline is static, with one optional 2-pulse when the page opens (300 ms total) that respects reduced motion.
- **Drop slots:** `ItemSlot` already has an `Image` that is hidden after a drop. Style that image as a dashed cyan outline with an empty fill, so the learner can see where the item goes before dragging.
- **Buttons on the bench:** actions such as "Cabut penutup balang", "Tutup bekas" and "Periksa bacaan" use the neutral preset with a small hand or arrow icon. They must look like buttons, not captions.
- **Step complete:** when `ButtonNext` (Seterusnya) appears, the instruction band shows "✓ Langkah selesai" in green text for that page, and the Seterusnya button plays a single 150 ms scale-in. Both are driven by the existing activation; no rule changes.
- **Completion:** Phase 0 inspects each experiment's final page. The target is a white card with "Eksperimen selesai", a recap of the real step titles the learner just finished, and two actions: primary "Uji diri" and neutral "Kembali ke laman utama".
- **akt6 Welcome:** fix the clipped header buttons (anchor offsets). Flag the title wording for the owner. Fill the empty slate with the purpose card plus a "Radas yang digunakan" strip that reuses the Prestart apparatus sprites.
- **Prestart apparatus on white:** put a `#DCE5EF` tray behind white or transparent items, so plastic containers keep a visible edge without being recoloured.

### 5.6 Quiz (Uji diri)

This screen has the most potential for play, and the program already provides real data for it.

- **No preselection:** clear the EventSystem's first selection or apply the "focused ≠ selected" styling. The learner's first click is their first choice.
- **"Soalan N daripada M"** in `questionNumberTopLabel`, with M from `QuestionList.instance.questionlist.Count`. This is a one-line presentation change in `QuestionController.applyQuestion()`.
- **Progress dots:** one dot per question at the top. The current dot is navy-ringed. Answered dots show ✓ (green) or ✗ (red) from this session's real results. Nothing is saved between sessions.
- **Mark the learner's pick:** after answering, the chosen option gets a navy border and a "Pilihan anda" tag, next to the existing "Betul:" label on the correct one. If they are the same option, the tag reads "Pilihan anda · Betul". This needs the clicked index passed into `showRealAnswer()`, a presentation-only change.
- **Fix the score area:** anchor `Canvas/TopRIght/Coin` to the top-right corner with a 32-unit inset, and size it from its content so it never clips.
- **Score count-up:** when `ScoreController.SetScore()` runs, the number counts up over 400 ms and the coin does one 120 ms bounce. The existing `addCoin` animator line is commented out in `QuestionController`; Phase 0 checks whether that animation exists before anything new is built.
- **Timing stays the same.** All feedback must fit inside the current waits (1 s before sound and face, 1.2 s before the reveal, 3 s before the next question). Feedback never extends or shortens them.
- **Result screen**, using real data only:
  - Title "Uji diri selesai", then "4 daripada 5 betul" (score ÷ 10) and the score in gold.
  - The progress dots repeated as a recap row.
  - One line of feedback by band (wording for the owner to confirm): all correct, "Hebat, semua betul!"; at least half, "Bagus! Ulang kaji soalan yang salah dan cuba lagi."; less than half, "Baca semula teori, kemudian cuba lagi."
  - Actions: primary "Cuba lagi", neutral "Laman utama", plus "Baca teori" if it can route to the hub (Phase 0 checks this route).

### 5.7 Learning assistant

- Show a "Pembantu" text label under the brain icon, or a tooltip on hover and focus if space is short. The prefab is shared by every scene, so this is one change.
- Keep the reserved top-right slot in every scene. Don't move it into experiment benches.

## 6. Game feel

Durations follow the existing 100–180 ms rule. The only longer value is the score count-up (400 ms), which runs while input is already locked (see section 7). Every effect respects one "Kurangkan gerakan" (reduce motion) setting; when it is on, effects show their final state without motion.

### 6.1 Tiers

| Tier | Effect | Trigger (existing event) | Values | Code needed |
|---|---|---|---|---|
| **S, state polish** | Press feedback on all buttons | Button press | Scale 0.97, 80 ms down, 120 ms back | Preset plus a small `UiPressFeedback` component |
| S | Card hover lift | Pointer enter/exit | 4-unit up, 120 ms ease-out | Same component, hover mode |
| S | Focus ring | Selection | Static double ring | `UiFocusIndicator` (already exists) |
| **M, event feedback** | Tick pop | `ItemSlot` tick child becomes active | Scale 0.6 → 1.08 → 1.0, 160 ms, ease-out-back | `UiPopOnEnable` on the tick (no `ItemSlot` change) |
| M | Drag lift | `IBeginDragHandler` / `IEndDragHandler` | Scale 1.05 and shadow on; back on release, 100 ms | `DragLiftFeedback` on the same object as `DragController` (Unity sends drag events to every handler on the object) |
| M | Seterusnya appears | Button activated by the step | Scale-in 150 ms | `UiPopOnEnable` |
| M | Correct answer | `addingmark()` timing | Option border to green, ✓ pop 160 ms, avatar smile (existing), `yay` + `claps` (existing) | Driven from `QuestionController` presentation code |
| M | Wrong answer | `incorrect()` timing | Option shake 3 × 6 units over 180 ms, ✗ pop, avatar sad (existing), `salah` (existing) | Same |
| M | Score count-up and coin bounce | `ScoreController.SetScore()` | 400 ms count, 120 ms bounce | `ScoreCounterView` called from `SetScore()` |
| **L, flow** | Result recap reveal | `CongratPrompt` activates | Dots appear one at a time, 60 ms apart | `UiPopOnEnable` with delay index |
| L | Scene fade | Scene change | 150 ms navy fade | Needs approval (touches persistent `CanvasPrime`) |

### 6.2 Sound

Reuse the existing `yay`, `claps`, and `salah` sources in the quizzes. A soft drop "click" and a step-complete chime would need new audio files from a licensed source, so they need approval (section 7). Sound must never be the only cue.

### 6.3 Shared settings

- `Assets/UI/Science/Settings/UiFeelSettings.asset` (ScriptableObject): every duration, scale, and shake distance above, editable in the Inspector.
- "Kurangkan gerakan" and "Bunyi" toggles: where they live and whether they persist is an owner decision (section 7).

## 7. Ideas that need owner approval

These either conflict with [AGENTS.md](AGENTS.md) as written, change behaviour, or add a new feature. Nothing here starts without a yes.

| # | Idea | Conflict or reason | Recommendation |
|---|---|---|---|
| A1 | **Local progress:** "Selesai" ✓ per stage on dashboard cards, saved with `PlayerPrefs` | AGENTS.md forbids completion badges not backed by real state. This would be real state, but it is a new persistence feature (stable keys, a reset option, shared computers). | Yes, as a later phase, keyed by scene name with a "Set semula kemajuan" option |
| A2 | **Stage locking:** quiz unlocks after the experiment | Changes routes, and can block a teacher who wants to start with the quiz | No. Keep the suggested order only |
| A3 | **One-shot celebration particles** on a perfect quiz score | AGENTS.md bans animated particles | No. Use the dot recap reveal (section 6.1) instead |
| A4 | **Stars (1–3) on the result screen** | Possibly a "badge"; thresholds are invented | Only if the owner sets the thresholds; otherwise use the text bands in 5.6 |
| A5 | **Return-to-origin on invalid drop** (the item glides back if dropped outside a slot) | Gameplay change: today the item stays where it lands outside akt6 | Pilot in one scene; needs `DragController` to remember the start position |
| A6 | **New SFX** (drop click, step chime) | New assets and licensing | Yes if the owner supplies or approves the source |
| A7 | **Score count-up at 400 ms** | Exceeds the 100–180 ms guidance | Yes. It runs while input is already locked, so it doesn't delay the learner |
| A8 | **Settings toggles** (reduce motion, sound) saved in `PlayerPrefs` | New persistence | Yes. AGENTS.md already requires a way to reduce motion |
| A9 | **Scene fade** through `CanvasPrime` | Touches the persistent canvas; risk of duplicates when entering scenes directly | Last, only after A8 |
| A10 | **Diagram labels on lesson illustrations** | Curriculum wording | Owner or teacher supplies terms |
| A11 | **Title wording** ("Mikrobial Fuel Cell", "MFC", "RUBBER FILM", "PENKULTURAN") | Curriculum or brand terms | Owner decides; no silent rewrites |

Owner decisions (2026-11-12): A1 approved for a later phase; A5 implemented as an akt6 pilot; A6 implemented with the existing `touchglass` and `yay` clips; A9 deferred; A10 blocked until terms are supplied; A11 uses Malay first with the English gloss at first mention. Implementation and evidence are recorded in [IMPLEMENTATION.md](IMPLEMENTATION.md).

## 8. Implementation phases

Each phase is a separate slice: inspect, change, capture, owner review, then move on. Evidence goes into [IMPLEMENTATION.md](IMPLEMENTATION.md) under a new "UI revamp" heading.

### Phase 0: inventory (read only)

- [ ] 0.1 List candidate card art for each activity from the lesson sprites, with sprite paths. Expect: a table of 7 rows, with blank rows where nothing fits.
- [ ] 0.2 Count real pages per lesson branch (`Into/Content/N`) for "Halaman N daripada M". Expect: page counts, plus notes on branches with tabs or popups.
- [ ] 0.3 Confirm or complete the page-to-step mapping for akt3–akt7 (extends IMPLEMENTATION.md Stage 0.2). Expect: a mapping per scene, or "title only" where it is uncertain.
- [ ] 0.4 Find why the first quiz answer and the first dashboard card show cyan on load (EventSystem `firstSelectedGameObject`, a script `Select()` call, or a colour block). Expect: the exact source.
- [ ] 0.5 Check whether the `addCoin` animation exists and whether "Baca teori" can route from the quiz result to the right hub page. Expect: yes or no for each.
- [ ] 0.6 Inspect each experiment's final page and the login button's real behaviour.
- [ ] 0.7 Capture baselines at the six resolutions for every screen touched in Phase 1. Expect: a before-set.

### Phase 1: quiz (highest learner payoff, smallest hierarchy risk)

- [ ] 1.1 Create Presets (primary, neutral, answer) and `UiFeelSettings`.
- [ ] 1.2 Remove the preselected look; add "Soalan N daripada M".
- [ ] 1.3 Mark the learner's pick ("Pilihan anda") through the existing `showRealAnswer()` and `resetButton()` paths.
- [ ] 1.4 Add progress dots driven by in-session results.
- [ ] 1.5 Anchor and fit the coin/score block; add the score count-up and coin bounce.
- [ ] 1.6 Add correct and wrong feedback (border, ✓/✗ pop, shake) inside the existing waits.
- [ ] 1.7 Rebuild the result screen (count, recap dots, feedback band, actions).
- [ ] 1.8 Run all 7 quizzes programmatically (right and wrong paths). Expect: identical scores to before and no Console errors. Pilot in Uji Diri 6, then the owner reviews before the other six.

### Phase 2: dashboard, hubs, lessons

- [ ] 2.1 Activity cards with real art and stage strips; remove the preselected fill; hover lift.
- [ ] 2.2 Three-station hubs; bring activities 1–3 and 5–7 in line with activity 4's wording (sentence case; terms from A11 decided by the owner).
- [ ] 2.3 One lesson navigation model with "Halaman N daripada M"; move activity switching to the hub.
- [ ] 2.4 Fix lesson 4 page 2's scroll start; restyle tabs and Maklumat pills.
- [ ] 2.5 Check `MenuSlideController` and `NextExperimentController` behaviour after every hierarchy-adjacent change.

### Phase 3: experiment interaction cues

- [ ] 3.1 Step counters from the Phase 0 mapping.
- [ ] 3.2 Visible drop slots (style the existing `ItemSlot` Image).
- [ ] 3.3 Interaction target outline and hint chip per page (akt6 pilot).
- [ ] 3.4 Tick pop, drag lift, Seterusnya scale-in, "✓ Langkah selesai".
- [ ] 3.5 Completion card per experiment.
- [ ] 3.6 Fix the akt6 Welcome clipping and the Prestart tray for white items.
- [ ] 3.7 Run the drag/drop callbacks programmatically in each scene. Expect: the same `ConditionalCheck` outcomes as before.

### Phase 4: login, assistant, settings

- [ ] 4.1 Logo tile, primary button preset, title scrim.
- [ ] 4.2 Assistant label or tooltip (prefab change, all scenes).
- [ ] 4.3 Reduce-motion and sound toggles (if A8 is approved).

### Phase 5: approved extras and integration

- [ ] 5.1 Items from section 7 that the owner approved, one at a time.
- [ ] 5.2 Full verification matrix (section 9) and a desktop player build smoke test.
- [ ] 5.3 A short guide at `docs/tutorial/ui-feel.md`: where Presets and `UiFeelSettings` live, how to change a colour or duration, and how to add the pop to a new tick.

## 9. Verification

| Check | How | Pass condition |
|---|---|---|
| Layout | Game view at 1920 × 1080, 1280 × 720, 1280 × 800, 1024 × 768, 1280 × 1024, 2560 × 1080 | No clipping, overlap, or hidden counters; the score block fully visible |
| Contrast | Measured from final colours over real sprites | 4.5:1 normal text, 3:1 large text and boundaries |
| Colour independence | Grayscale and deuteranopia/protanopia captures of quiz feedback and results | Result readable from ✓/✗ and text alone |
| Quiz behaviour | Scripted right and wrong runs of all 7 quizzes | Score, timing, and question order unchanged |
| Experiment behaviour | Scripted drag/drop and button callbacks for akt3–akt7 | Same step progression as the baseline |
| Reduce motion | Toggle on, repeat the quiz and one experiment | Final states shown, no motion, no delay |
| Focus | Scripted navigation through each changed screen | Focus ring visible and distinct from the selected state |
| Persistent UI | Direct scene entry and entry from the Dashboard | One assistant, one EventSystem |
| Console | After every C# change | No new errors; pre-existing ones listed separately |
| Build | Desktop player smoke test | Login → Dashboard → one lesson, experiment, and quiz complete |

Physical mouse and keyboard testing was skipped earlier at the owner's request. Scripted checks will be reported as scripted, not as physical play.

## 10. Risks and hierarchy contracts

- **Child-order dependencies:** `ItemSlot` uses `GetChild(0)` as the tick, `QuestionController` uses `avatarState` children 0–2 and `answerBtn[i].GetChild(0)` as the label, and `ConditionalCheck` reads page names. New visuals go on **existing objects as components**, or as **last children** with raycast off. Never as wrappers or first children.
- **Drag lift and colliders:** scaling a dragged object also scales its `BoxCollider2D`, which `ItemSlot.OnTriggerStay2D` uses in akt6 and akt7. Scale only a visual child, or test the trigger overlap before rollout.
- **Quiz colour overrides:** `showRealAnswer()` and `resetButton()` set `Image.color` directly. New styling has to go through those serialized colours, or it will be overwritten.
- **Selection vs. disabled:** input locking uses `Button.enabled = false`, which doesn't trigger the Selectable disabled tint. Lock visuals need to follow `interactable` as well (already set together in `disableAllButtons`).
- **CanvasScaler differences** (Login/Dashboard/akt3/4/5/7 = 0, akt6 = 0.515, quizzes = 1): check hit sizes per scene. Don't standardise the scalers.
- **Recovery:** copy each scene to `Temp/qa_screens/*.unity.bak` before its slice, and keep slices small enough to revert alone.

## 11. Decisions for the owner

| # | Question | Default if no answer |
|---|---|---|
| 1 | Approve the colour roles in 3.1, including the new score gold `#F2B705` | Yes |
| 2 | Stage wayfinding colours (paper / slate / gold) | Yes |
| 3 | Phase order: quiz → dashboard and lessons → experiments → login and assistant | Yes |
| 4 | Each item A1–A11 in section 7 | As recommended in the table |
| 5 | Feedback wording for the result bands in 5.6 | Wait for owner wording before shipping |
| 6 | Assistant: visible "Pembantu" label or tooltip only | Visible label |
| 7 | Should cards show an illustration where none exists, or stay text only | Text only |
