# Prototype 0.1.4 Notes

Last updated: 2026-05-01

## Purpose

Prototype 0.1.4 is the mobile HUD sanity checkpoint after the verified Prototype 0.1.3 camera and framing pass.

It does not add new gameplay systems. It only improves how much the placeholder mobile controls block the combat view.

## Verified In Play Mode

- Keyboard and mouse controls still work.
- On-screen movement stick still works.
- On-screen action buttons remain present.
- HUD blocks less of the arena.
- Cooldown labels remain readable.
- Combat loop remains playable.

## HUD Changes

- Movement stick is smaller.
- Movement stick is less opaque.
- Action buttons are smaller.
- Action buttons are less opaque.
- Button spacing is cleaner on the right side.
- Cooldown labels are smaller.
- Cooldown labels sit closer to their related buttons.
- Runtime HUD tuner applies changes to the current scene without requiring scene regeneration.
- Scene bootstrapper now creates regenerated HUDs with the same sizing and opacity.

## Accepted Placeholder Quality

The HUD is still temporary:

- Button art is primitive.
- Icons are not final.
- Mobile layout has not been tested on real devices.
- No haptics.
- No accessibility options.
- No final control customization.

## Next Smallest Step

Prototype 0.1.5 should focus on audio placeholders only:

- Basic Attack swing/hit sound placeholder.
- Dash sound placeholder.
- Enemy hit/death sound placeholder.
- Loot drop/pickup sound placeholder.
- No imported audio packs yet.
- No new gameplay systems.
- No inventory, classes, companions, PvP, or art imports.
