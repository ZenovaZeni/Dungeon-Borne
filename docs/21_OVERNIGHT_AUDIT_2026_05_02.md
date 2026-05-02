# Overnight Prototype Audit

Created: 2026-05-02

## Purpose

This note captures the useful long-run audit work after Prototype 0.1.8 and the in-progress Prototype 0.1.9 player-hit readability pass.

The goal is not to expand the game. The goal is to keep the one-room combat sandbox easier to test, easier to understand, and safer to keep iterating.

## Current Verified Baseline

The current playable baseline includes:

- One combat sandbox scene.
- One Fighter placeholder.
- Movement, dash, Basic Attack, Cleave, Stomp, and Rage placeholder.
- Skeleton Grunt, Archer, and Brute.
- Enemy melee and ranged attacks.
- Damage numbers.
- Enemy hit flash and knockback.
- Enemy death fall-over placeholder.
- Echo Axe loot drop and pickup.
- Echo Axe Cleave shockwave.
- Placeholder audio.
- `R` sandbox reset shortcut through the Unity Input System.

## In-Progress Work

Prototype 0.1.9 adds player-hit readability:

- Red ring at the player's feet when damaged.
- Red body flash when damaged.
- Runtime attachment for the current scene.
- Bootstrapper attachment for regenerated scenes.

This needs Play Mode verification.

## Important Audit Notes

Some older subagent notes reported missing scene references for projectiles, damage numbers, loot, and shockwave prefabs. The live scene has those references wired now:

- `PlayerCombatController.shockwavePrefab` is assigned.
- `DamageNumberSpawner.damageNumberPrefab` is assigned.
- Enemy `LootDropper.pickupPrefab` references the Echo Axe pickup prefab.
- Enemy `EnemyBrain.projectilePrefab` references the Archer projectile prefab.

Do not re-fix those unless a fresh Play Mode test shows a real regression.

## Smallest High-Value Next Steps

### 1. Verify Player-Hit Feedback

Test:

- Let Skeleton or Brute hit the player.
- Let Archer projectile hit the player.
- Confirm the red hit ring and flash are visible.
- Confirm damage numbers still appear.
- Confirm movement/combat continue normally after the hit.

If weak:

- Increase ring scale slightly.
- Increase flash lifetime slightly.
- Do not add player death, potions, healing, or health UI yet.

### 2. Add A Tiny Player Health Readout Later

Only after player-hit feedback is verified, consider a minimal prototype health number in the existing overlay.

Keep it primitive:

- `HP 86 / 100`
- No health bar art.
- No death screen.
- No healing items.

Reason:

- It helps test enemy damage without adding survival systems.

### 3. Keep Basic Attack And Death As Current Quality Bar

Recent passes improved:

- Basic Attack swing readability.
- Basic Attack hit readability.
- Enemy death readability.

Use these as the bar for Cleave, Stomp, Rage, dash, and loot feedback before any content expansion.

### 4. Add More Edit-Mode Tests

The pure combat model has low-risk coverage opportunities:

- Health clamps invalid max health.
- Health ignores zero/negative damage.
- Cooldown ignores negative ticks.
- Zero-duration cooldown can restart immediately.

These are cheap tests and do not require scene or Unity play mode.

### 5. Avoid Scope Expansion

Still do not add:

- Companions
- PvP
- Multiplayer
- Full inventory
- Class trees
- Story systems
- Open world
- Monetization
- Asset pack imports
- Blender assets

## Technical Watchlist

### Runtime Find Calls

The prototype uses runtime `FindAnyObjectByType` and `GameObject.Find` in several helper scripts.

This is acceptable for Prototype 0.1 helper behavior, especially overlays and runtime patchers, but should not become the architecture for production systems.

Later cleanup target:

- Keep runtime patchers in prototype-only scripts.
- Prefer explicit scene references or prefab wiring for production systems.

### Input Action Growth

The Input System architecture is working.

Watch for:

- Keeping actual combat actions abstract.
- Avoiding direct `Input.GetKey`.
- Making playtest-only actions like `ResetSandbox` clearly marked as prototype utilities.

### Scene Serialization

Unity scene instances can keep old serialized values even after script defaults change.

This already mattered for enemy death delay.

Future rule:

- If a new default must affect existing scene objects, enforce a safe runtime minimum or regenerate/save the scene.

## Recommended Morning Order

1. Press Play.
2. Let enemies hit the player.
3. Verify player-hit feedback.
4. Press `R` to reset.
5. Run through the full combat loop once:
   - Move
   - Dash
   - Basic Attack
   - Cleave
   - Stomp
   - Rage
   - Kill enemies
   - Pick up Echo Axe
   - Cleave shockwave
6. If all works, mark Prototype 0.1.9 verified in `docs/15_CURRENT_BUILD_STATUS.md`.

## Next Best Feature After 0.1.9

The next smallest useful improvement is a minimal player health readout in the existing prototype overlay.

Reason:

- It makes enemy damage testable.
- It does not require player death, healing, inventory, UI polish, or progression.

Suggested scope:

- Add `HP current / max` to the top-left overlay.
- Read from the existing player `Damageable`.
- Keep it plain text.
- Do not add player death behavior yet.
