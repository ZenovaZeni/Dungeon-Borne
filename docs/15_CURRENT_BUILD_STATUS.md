# Current Build Status

Last updated: 2026-05-01

## Project State

The Unity project opens in Unity 6.4 and contains a generated Prototype 0.1 scene:

```text
Assets/_Dungeonborn/Scenes/CombatSandbox_Prototype_0_1.unity
```

The project has been initialized as a git repository and pushed to:

```text
https://github.com/ZenovaZeni/Dungeon-Borne
```

## Verified In Play Mode

The following have been manually verified:

- Player movement works.
- Dash works.
- Basic attack input works.
- Cleave input works.
- Stomp input works.
- Rage/ultimate placeholder input works.
- Attack markers are visible.
- Dash afterimage feedback is visible.
- Prototype control overlay is visible.
- Runtime fallback labels make skill buttons readable.
- Enemies flash when hit.
- Enemies can take damage.
- Enemies can die.
- All three initial enemies can be killed.
- Skeleton/Brute melee attack markers are visible.
- Archer ranged attack/projectile works.
- Damage numbers appear.
- Brute drops Echo Axe.
- Echo Axe can be picked up.
- Echo Axe modifies Cleave into a visible shockwave.
- Prototype 0.1 feel tuning pass works in Play Mode.
- Archer kiting respects arena walls.
- Prototype 0.1.1 readability pass works in Play Mode.
- Enemy labels and health bars are visible.
- Compact cooldown HUD is readable.
- Echo Axe pickup label is readable.
- Basic Attack slash and hit spark are visible.

## Current Prototype Content

Scene:

- One test arena room.
- Angled top-down camera.
- Placeholder primitive geometry.

Player:

- One playable Fighter placeholder.
- Movement.
- Dash.
- Basic attack.
- Cleave.
- Stomp.
- Rage placeholder.

Enemies:

- Skeleton Grunt.
- Archer.
- Brute.

Systems:

- Unity Input System action asset.
- Health/damage/death model.
- Cooldown model.
- Damage number prefab and spawner.
- Loot item definition.
- Echo Axe legendary modifier.
- Editor bootstrap tool for regenerating the combat sandbox.

## Prototype 0.1 Baseline

Prototype 0.1 is now considered verified as the Combat Sandbox baseline.

See:

```text
docs/16_PROTOTYPE_0_1_PLAYTEST_CHECKLIST.md
```

Known feel gaps:

- Dash has temporary visual feedback but still needs real VFX/audio later.
- Enemy attacks have temporary windup markers but still need real telegraphs later.
- Enemy death is still placeholder.
- Attack markers are debug primitives, not final VFX.
- No audio feedback yet.
- Mobile UI exists only as placeholder controls, fallback labels, and cooldown text.

## Prototype 0.1.1 Readability Pass

Prototype 0.1.1 is now considered verified as the first readability checkpoint after the Combat Sandbox baseline.

See:

```text
docs/17_PROTOTYPE_0_1_1_NOTES.md
```

Do not add new gameplay systems.

Verified readability improvements:

1. Darker dungeon mood.
2. Stronger floor/wall contrast.
3. Clearer player/enemy silhouette colors.
4. Simple torch-like placeholder lights.
5. Light fog and darker camera backdrop.
6. Bootstrapper defaults matching the visual pass.
7. Enemy name labels and simple health bars.
8. Smaller top-left prototype controls overlay.
9. Compact skill cooldown text near mobile buttons.
10. Clearer Echo Axe pickup label.
11. Basic Attack slash streak and hit spark placeholders.

Retested in Play Mode:

1. Combat remains readable.
2. Player/enemies stand out from the arena.
3. Skill markers and enemy telegraphs are still visible.
4. Fog and darkness do not hide loot, damage numbers, or projectiles.
5. Rage remains obvious but no longer deletes a full-health Archer in one hit.
6. Enemy labels/health bars help identify targets without hiding combat.
7. HUD cooldown text no longer clumps over the combat area.
8. Echo Axe pickup text is easy to see and understand.
9. Left-click Basic Attack visibly shows both swing and hit impact.

## Prototype 0.1.2 Death And Loot Clarity

Verified in Play Mode:

1. Enemies show a short death flash, shrink, and puff before removal.
2. Enemy death remains clear without delaying combat too much.
3. Echo Axe drop shows a temporary gold beam/ring/pulse when it appears.
4. Echo Axe pickup still works.
5. No new loot, inventory, enemy, or combat systems were added.

Player preference for a later polish pass:

- Enemy death should eventually read more like enemies falling over instead of fading/shrinking away.

## Prototype 0.1.3 Camera And Framing

Verified in Play Mode:

1. Camera view is slightly wider for better combat context.
2. Camera has subtle facing-direction look-ahead.
3. Camera focus is clamped inside the arena so framing does not drift too far outside the room.
4. Player, enemies, loot, and attack markers remain readable.
5. No combat, enemy, loot, or input systems were changed.
6. Movement and combat feel smoother with the updated camera framing.

## Prototype 0.1.4 Mobile HUD Sanity

Verified in Play Mode:

1. On-screen movement stick is smaller and less opaque.
2. On-screen action buttons are smaller and less opaque.
3. Button spacing leaves more of the arena visible.
4. Cooldown text is smaller and tucked closer to the relevant buttons.
5. Keyboard and mouse controls still work.
6. No gameplay mechanics were changed.

## Prototype 0.1.5 Audio Placeholders

Verified in Play Mode:

1. Basic Attack has a placeholder swing sound.
2. Basic Attack hit has a placeholder impact sound.
3. Dash has a placeholder sound.
4. Cleave, Stomp, and Rage have placeholder skill sounds.
5. Enemy death has a placeholder sound.
6. Echo Axe drop and pickup have placeholder sounds.
7. Sounds are generated at runtime; no imported audio assets were added.
8. No gameplay mechanics were changed.

Follow-up tuning:

- First audio pass sounded too much like small "tink tink" tones.
- Runtime placeholder audio was retuned toward lower whoosh/thud sounds for combat and softer chimes for loot.
- Follow-up retune uses one non-spatial prototype audio source so the lower placeholder sounds remain audible in Play Mode.

## Explicit Non-Goals Right Now

Do not build:

- Companions
- PvP
- Multiplayer
- Full inventory
- Class trees
- Story systems
- Open world
- Monetization
- Large art pass
- Large asset pack integration
