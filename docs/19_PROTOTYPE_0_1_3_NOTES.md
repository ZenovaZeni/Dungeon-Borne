# Prototype 0.1.3 Notes

Last updated: 2026-05-01

## Purpose

Prototype 0.1.3 is the camera and framing checkpoint after the verified Prototype 0.1.2 death and loot clarity pass.

It does not add new gameplay systems. It only improves how the existing combat sandbox is framed during play.

## Verified In Play Mode

- Movement still works.
- Dash still works.
- Combat still works.
- Enemies, loot, and attack markers remain readable.
- Camera movement feels smoother.
- The player and nearby enemies are easier to read during combat.

## Camera Changes

- Slightly wider orthographic view.
- Camera offset pulled a little higher and farther back.
- Subtle player-facing look-ahead.
- Camera focus clamped within the arena bounds.
- Scene bootstrapper updated so regenerated scenes use the same camera framing.
- Runtime visual pass also enforces the wider camera size.

## Accepted Placeholder Quality

The camera is still a prototype camera:

- No camera shake.
- No target-group framing.
- No boss-room framing.
- No mobile orientation handling beyond the current test view.
- No cinematic camera behavior.

## Next Smallest Step

Prototype 0.1.4 should focus on mobile HUD sanity only:

- Make sure on-screen buttons do not block too much action.
- Check cooldown text placement.
- Tune button size/transparency if needed.
- Keep PC controls working.
- Do not add new combat mechanics.
- Do not add inventory, classes, companions, PvP, or art imports.
