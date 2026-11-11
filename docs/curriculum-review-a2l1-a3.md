# Curriculum review — A2 L1 pages 6–14 and A3 (bacteria lesson)

The user asked for an audit of the A2 L1 STEM continuation and the A3 bacteria lesson. Both were checked against their own text in `Dashboard.unity`. This review **does not rewrite any science**; it lists what I would flag to a teacher.

## How it was done

I read the actual `TextMeshProUGUI` text on every page of the two chains, in Play Mode via the live hierarchy (not from the saved scene YAML, so the wording is exactly what the learner sees).

## A2 L1 — the newly linked STEM continuation (pages 6–14)

The chain is coherent: page 5 ends "Contoh Teknologi Hijau", page 6 opens "Sains untuk Masa Depan", then S-T-E-M in order (T on 7–10, E on 11–13, M on 14). The flow works.

| Page | Finding | Severity | Why it matters |
|---|---|---|---|
| `content (6)` — "Sains untuk Masa Depan" | Content is three generic cards (Pembelajaran / Inovasi / Penemuan). It does not mention "Sains" explicitly by name in any bullet. | Low | The heading says "Sains untuk Masa Depan" but the three cards describe STEM habits, not science specifically. A teacher may want one bullet that names "Sains" so the page teaches what its heading says. |
| `content (7)` — "Apakah T-teknologi" | Line 1 uses the English word **"gadget"** (not translated). Line 4 ends with no full stop ("Mari kita terokai konsep teknologi yang lebih luas dan mendalam"). | Low | One untranslated English word, and a missing sentence terminator. |
| `content (8)` — "Teknologi dalam kehidupan seharian" | Card 1 title is "Botol Air" but the text says "menyelesaikan masalah **dehidrasi**" — a borrowed English medical term with no Malay equivalent given. | Low | "Dehidrasi" is common Malay-usage, but a teacher may prefer "kehilangan air dalam badan". |
| `content (10)` — "Masa Depan Teknologi" | Table row 3 title is **"Bioteknologi"** but the row's "Impak pada Masyarakat" cell says **"Pengingkatan kualiti dan jangka hayat"** — "Pengingkatan" is not a standard Malay noun (should be "Peningkatan"). | **Medium** | A real typo: the word does not exist in standard Malay; it should be "Peningkatan kualiti dan jangka hayat". |
| `content (11)` — "Apakah E-Engineering (Kejuruteraan)" | Sub-title line reads "Seni Sains untuk kehidupan manusia" — unclear Malay (probably meant "Sains untuk kehidupan manusia"). The body also says "meningkatkan **kuality** hidup manusia" — a typo for "kualiti". | **Medium** | Two real language issues: a malformed sub-title and a typo ("kuality" → "kualiti"). |
| `content (12)` — "Perpaduan Pengetahuan, Matematik, dan Pengalaman" | Heading ends with a trailing space and a pipe character in the source (`Pengalaman \\|`). | Low | A trailing pipe character will render as a stray vertical bar. The heading itself is fine. |
| `content (13)` — "Proses Reka Cipta dan Kejuruteraan" | Card 2 title "Reka Bentuk" has a trailing pipe character in the source. Card 4 body reads "Menjalankan ujian menyeluruh untuk memastikan **kualiti.**" (correct). No other issue. | Low | Same stray-pipe rendering issue. |
| `content (14)` — "Apakah M - Matematik" | Heading in the source is "Apakah  M - Matematik" with **two spaces** after "Apakah". | Low | Extra whitespace; cosmetic only. |

**Summary:** two real language defects the owner should fix (**"Pengingkatan" → "Peningkatan"** on page 10, and **"kuality" → "kualiti"** on page 11). The rest are English borrowings or cosmetic whitespace that a teacher may or may not want changed. No scientific claims in this chain are wrong; they are all general STEM statements.

## A3 — the four-page bacteria lesson

| Page | Finding | Severity | Why it matters |
|---|---|---|---|
| `content (1)` — "Bakteria" | Body text starts "Bakteria adalah mikroorganisma yang terdapat hampir di mana-mana di Bumi, termasuk di dalam dan pada tubuh manusia. Walaupun sesetengah bakteria boleh menyebabkan kemudaratan, yang lain adalah penting untuk kesihatan, ekosistem, dan pelbaga…" | Low | Text is cut in the hierarchy view I read, so I could not confirm the last word is spelled correctly; the visible portion is correct. |
| `Image (1)` — "Bakteria Baik" | Title is "Bakteria Baik" but the body calls the same thing "bakteria **berfaedah**" in the very next sentence. | Low | Inconsistent naming in the same paragraph. A teacher may want one term throughout ("bakteria baik" is the simpler, learner-facing wording). |
| `content (3)` — "Contoh dan Peranan" | Five numbered items: Kesihatan Manusia, Proses Alam Sekitar, Pengurai (cth., spesies Bacillus), Aplikasi Perindustrian, Bakteria Asid Laktik. | — | No language issue found. The list is factually consistent with the bacteria lesson. |

**Summary:** the A3 lesson is clean. The only real flag is the "Bakteria Baik / bakteria berfaedah" inconsistency on page 3, which is a wording choice, not a factual error.

## What I did not check

- I did not verify the A2 L1 page 6–14 and A3 text against the Malaysian national curriculum syllabus (KSSM), because no curriculum document is available in this session.
- I did not re-check akt7's science content in this pass; the akt7 review remains the same "confirm the reagent forms/quantities and waste-capture method" list recorded in the akt7 content-cleanup entry in [IMPLEMENTATION.md](../IMPLEMENTATION.md).

## Status - 2026-11-12

Already fixed before this pass: "Pengingkatan" → "Peningkatan" (page 10) and "kuality" → "kualiti" (page 11). The stray pipe characters on pages 12–13 are no longer in the scene.

Applied in `Dashboard.unity` (both activity 1 and activity 2 copies where they exist):
- Page 14: "Apakah  M" → "Apakah M" (double space).
- Page 13: "dan  mempertimbangkan" → "dan mempertimbangkan" (double space).
- Page 7: full stop added to "Mari kita terokai konsep teknologi yang lebih luas dan mendalam".

Open, for the teacher:
- Page 7: "gadget" (keep, or translate, for example to "alat").
- Page 8: "dehidrasi" (keep, or "kehilangan air dalam badan").
- Page 11: sub-title "Seni Sains untuk kehidupan manusia" (keep, or "Sains untuk kehidupan manusia").
- Page 6: the three generic cards do not name "Sains" (add a bullet, or keep).
- A3 page 3: the review flagged "Bakteria Baik" against "bakteria berfaedah". The body text says "Bakteria baik, juga dikenali sebagai bakteria berfaedah", which gives the two terms as aliases, so this may be intended. Confirm before changing.
- akt7 science content (reagent forms and quantities, curing conditions, Flory-Rehner inputs, waste-capture method): see the akt7 entry in IMPLEMENTATION.md. This needs a reference or teacher sign-off.

## Teacher decisions - 2026-11-12 (owner answers)

- Page 7 "gadget": kept.
- Page 8 "dehidrasi": kept.
- Page 11 sub-title: changed to "Sains untuk kehidupan manusia" (both activity copies).
- A3 page 3 "Bakteria Baik" / "bakteria berfaedah": kept both; the body states they are the same thing.
- Still open: page 6 (add a bullet naming "Sains", or keep), and the akt7 science review.
