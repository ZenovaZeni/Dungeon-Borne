# Prototype 0.1 Playtest Checklist

Last updated: 2026-05-01

## Purpose

This checklist locks the current Combat Sandbox baseline before adding new prototype scope.

Prototype 0.1 is not the full game. It is the first playable proof that the core combat loop can be tested in one room.

## Scene

Use:

```text
Assets/_Dungeonborn/Scenes/CombatSandbox_Prototype_0_1.unity
```

## Required Loop

Verified in Play Mode:

- Player movement works.
- Camera follows the player.
- Basic attack works and is visible.
- Dash works and has temporary afterimage feedback.
- Cleave works and is visually distinct from Basic Attack.
- Stomp works and shows a circular AOE.
- Rage/ultimate placeholder works and has visible activation feedback.
- Skeleton Grunt chases and melee attacks.
- Archer keeps distance, shoots projectiles, and respects arena walls.
- Brute moves slower, hits harder, and feels heavier than Skeleton Grunt.
- Enemies take damage.
- Enemies flash and show knockback when hit.
- Enemies die.
- Damage numbers appear and are readable enough for prototype testing.
- Brute drops Echo Axe.
- Echo Axe pickup is visible.
- Echo Axe modifies Cleave into a visible shockwave.

## Current Controls

Keyboard and mouse:

- Move: `WASD`
- Basic Attack: left mouse
- Dash: `Space`
- Cleave: `Q`
- Stomp: `E`
- Rage placeholder: `F` or ultimate binding

Mobile placeholder controls:

- On-screen movement stick
- On-screen Attack, Dash, Cleave, Stomp, and Rage buttons

## Accepted Placeholder Quality

The following are intentionally temporary:

- Primitive player/enemy shapes
- Debug attack markers
- Temporary dash afterimages
- Temporary damage number styling
- Temporary enemy telegraph shapes
- Temporary loot visuals
- Placeholder mobile HUD
- No animation
- No audio
- No final VFX

## Do Not Add Yet

Do not add these while closing Prototype 0.1:

- Companions
- PvP
- Multiplayer
- Full inventory
- Class trees
- Story systems
- Open world
- Monetization
- Procedural dungeons
- Large asset pack imports
- Blender/custom art pass

## Next Approved Step

The next smallest step after this checklist is a primitive one-room visual readability pass:

- Darker dungeon mood
- Stronger floor/wall contrast
- Better player/enemy silhouette readability
- Simple torch-like lights
- Optional light fog
- No imported assets
- No new gameplay systems
