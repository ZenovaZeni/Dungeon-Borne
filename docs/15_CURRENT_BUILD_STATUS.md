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
- Enemies flash when hit.
- Enemies can take damage.
- Enemies can die.
- All three initial enemies can be killed.

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

## Known Issues / Not Yet Fully Verified

Still needs playtest verification:

- Camera follow quality during active combat.
- Skeleton Grunt consistently chases and melee attacks.
- Archer keeps appropriate distance and shoots visible projectiles.
- Brute moves slowly and hits harder than the grunt.
- Enemies damage the player.
- Damage numbers are readable enough in motion.
- Loot drop is easy to notice.
- Echo Axe pickup works reliably.
- Echo Axe modifies Cleave into a visible shockwave after pickup.

Known feel gaps:

- Dash feels empty and needs visual/audio feedback.
- Enemy attacks need telegraphs.
- Enemy death is still placeholder.
- Attack markers are debug primitives, not final VFX.
- No audio feedback yet.
- Mobile UI exists only as placeholder controls and cooldown text.

## Next Priority

Continue first playtest verification.

Do not add new features.

Fix only blockers in this order:

1. Console red errors.
2. Input actions not firing.
3. Abilities not applying damage.
4. Enemies not moving or attacking.
5. Loot not dropping or picking up.
6. Echo Axe shockwave not appearing.
7. Damage numbers or attack feedback not readable.

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

