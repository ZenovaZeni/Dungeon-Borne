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

- Prototype 0.1 feel tuning pass:
  - Basic Attack is faster/shorter and should feel like the quick filler attack.
  - Cleave is wider, slightly stronger, and should read as the main front arc.
  - Stomp has a larger circular area and should feel heavier than Cleave.
  - Rage placeholder has a larger area, higher damage, and shorter cooldown for clearer testing.
  - Skeleton Grunt is a faster, weaker baseline melee enemy.
  - Archer has longer range, slower shots, slightly higher shot damage, and should kite backward when too close.
  - Brute is slower, tankier, harder-hitting, and attacks less often.
  - Enemy movement now routes through `CharacterController.Move` so Archer kiting should respect arena walls.
- Camera follow quality during active combat.
- Damage numbers are readable enough in motion.
- Loot drop is easy to notice.
- Brute feels meaningfully heavier than Skeleton Grunt.
- Archer keeps appropriate distance under repeated play.
- Enemy attacks are understandable enough before impact.
- First combat feel pass changes:
  - enemy knockback on player hits,
  - brighter/larger damage numbers,
  - clearer death flash,
  - enemy attack windup markers,
  - loot bob/pickup feedback,
  - Echo Axe shockwave trail.

Known feel gaps:

- Dash has temporary visual feedback but still needs real VFX/audio later.
- Enemy attacks have temporary windup markers but still need real telegraphs later.
- Enemy death is still placeholder.
- Attack markers are debug primitives, not final VFX.
- No audio feedback yet.
- Mobile UI exists only as placeholder controls, fallback labels, and cooldown text.

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

After blockers are stable, the next smallest pass should continue improving feel:

1. Tune knockback distances.
2. Tune enemy windup timing.
3. Tune damage number size/lifetime.
4. Tune loot pickup feedback.
5. Decide when the primitive feel pass is good enough to begin one-room visual pass.

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
