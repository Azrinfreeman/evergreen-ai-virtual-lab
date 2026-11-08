# Validation notes

## Scope

Tracked source and configuration review only. No Unity Editor, experiment, quiz, build, device, or external service was run. Scripts, scenes, assets, and package versions are preserved.

## Source observations

- ConditionalCheck parses its object's name as an integer. Validate numeric names and the referenced buttons/transforms in every instance.
- Its Number and Text cases contain no condition logic; Percent exposes the next button directly. The correctness indicator is activated after the switch regardless of whether Transform conditions pass. Do not treat these modes as verified assessment checks.
- CheckContent resets its counter inside the item loop and compares an index with that counter. Review the intended all-items condition; the source does not establish that all required items were checked.
- CheckContent indexes toAppear at positions zero through two before some null checks. Ensure the list is large enough, not merely that individual references are assigned.
- NextExperimentController accesses child i+1 while scanning active siblings. Verify last-child and multiple-active-child cases and the intended next-step target.
- DragController assumes a Canvas, RectTransform, CanvasGroup, and BoxCollider2D and divides pointer delta by canvas scale. Its object-specific handling distinguishes null from named values; check empty Inspector strings and each experiment.
- Bikar7 depends on an action-tagged trigger with DragController and a parent GetahCollection component. Check trigger setup, fill behaviour, and re-entry.
- Quiz flow uses fixed child indices and named audio objects. Both answer paths advance; after the final question the completion prompt appears, but buttons are re-enabled. Check post-completion interactions and repeated coroutine starts.
- ScoreController exposes an in-memory score without persistence in that class. Do not assume score survives scene changes or relaunch.

These are source observations rather than reproduced runtime failures.

## Suggested checks

Import with the recorded editor; validate scene references and navigation; exercise all enabled experiments and conditional modes; review scientific/quiz content with an educator; test correct/incorrect answers, completion, score, and replay; then build and test the chosen platform. Review asset rights and credentials separately before publication.

Relative documentation links and all enabled build scene paths are checked against tracked files. This review is not a full-history security audit or runtime readiness certification.
