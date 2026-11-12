# A10 Draft Diagram Labels — For Owner Review

Draft labels for the lesson diagrams flagged as A10 in [IMPLEMENTATION.md](../IMPLEMENTATION.md). The labels use **only wording that already appears in the lesson text** — no new terms were invented. Review and correct before they are added to the scenes; the exact Malay wording is yours.

## How the audit worked

Every lesson page in `Dashboard.unity` (`Canvas/Container/Into/Content/N/…`) was scanned for `Image` children with real drawing sprites (not panel/background/button art). All pages share the same three bottom-strip decorations (`microscope`, `tech`, `something`), which are **decorative** and are **not** proposed for labelling. The audit found **two diagrams worth labelling**; the rest of the lesson pages have no content diagram at all (they only carry the shared bottom strip), so there is nothing to label there.

## Proposed labels

| # | Page | Object | Sprite | Proposed label | Source wording in the lesson |
|---|---|---|---|---|---|
| 1 | A2 L1 `content (3)` — "Definisi Teknologi Hijau" | `Image/Info/Image (1)` | `bumi2` | **Bumi** | the paragraph right above it talks about "sumber tenaga yang boleh diperbaharui… dari bumi" |
| 2 | A2 L1 `content (5)` — "Contoh Teknologi Hijau" | `Info/Image (1)`, `Info (1)/Image (1)`, `Info (2)/Image (1)` | `turbin angin` | **Turbin angin** | the popup text itself says "Turbin Angin" |
| 3 | A2 L1 `content (5)` — "Contoh Teknologi Hijau" | `Info (3)/Image (1)` | `pengkomposan bio` | **Pengkomposan bio** | the popup text says "Pengkomposan bio" |

These three diagrams already have their names spoken inside the lesson, so a label simply connects the picture to the wording (the A10 rule: "add labels to apparatus drawings only where the lesson text already names the item").

## Pages with no content diagram (nothing to label)

The scan confirmed the following lesson pages carry **only** the shared `microscope` / `tech` / `something` bottom strip, so A10 has no work there:

- **A1 L1** (`content`, `content (1)`, `content (3)`–`(14)`)
- **A1 L2** (`content (1)`, `content (2)`) — page 2 has `showingHand` cursor art and grey `btn_gray` text boxes, which are button styling, not diagrams
- **A2 L1** (`content`, `content (1)`, `content (4)`–`(14)`) — page 4 has `arrow` sprites which are flow-diagram arrows already labelled by the numbered step text next to them
- **A2 L2** (`content`, `content (1)`)
- **A3 L1** (`content`, `content (1)`, `content (3)`)
- **A4 L1** (`content`, `content (1)`, `content (4)`) — `arrow` sprites again already labelled
- **A5 L1** (`content`, `content (1)`, `content (3)`, `content (5)`–`(11)`)
- **A6 L1** (`content`, `content (1)`, `content (3)`, `content (4)`) — `arrow` sprites already labelled
- **A7 L1** (`content`, `content (1)`, `content (2)`–`(4)`) — `arrow` sprites already labelled

## Also relevant, not from the lesson scan

The **experiment prestart pages** (akt3–akt7 `Prestart Bahan2` / `Prestart Radas`) already label their apparatus through the existing `#DCE5EF` label bars (akt3/akt5/akt6) or plain text lists (akt7). No new labels are needed there.

## What happens if you approve

I add a small `ScienceLabel` child (the existing white chip with a navy edge used on the quiz `Maklumat` pills) under each labelled `Image`, sized to the word, positioned bottom-centre, with `raycastTarget = false`. It will follow the `Image` if the layout changes, and I will verify at 1920×1080 and 1280×720 that the label sits inside the card and does not overlap the existing text.
