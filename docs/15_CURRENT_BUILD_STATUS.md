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
