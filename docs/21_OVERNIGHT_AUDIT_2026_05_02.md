# Overnight Prototype Audit

Created: 2026-05-02

## Purpose

This note captures long-run audit work after Prototype 0.1.8, followed by verified checkpoints 0.1.9 through 0.1.11 and implemented-but-pending-verification defeat and Android-readiness placeholders.

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

## Recent Checkpoints

Prototype 0.1.9 added and verified player-hit readability.

Prototype 0.1.10 added and verified a minimal prototype HP readout.

Prototype 0.1.11 added and verified the Editor-only scene validation tool.

Prototype 0.1.12 adds a temporary player defeat placeholder and still needs Play Mode verification.

Prototype 0.1.13 adds Build Settings and validator readiness for Android-first testing and still needs Unity Editor verification.

## Important Audit Notes

Some older subagent notes reported missing scene references for projectiles, damage numbers, loot, and shockwave prefabs. The live scene has those references wired now:

- `PlayerCombatController.shockwavePrefab` is assigned.
- `DamageNumberSpawner.damageNumberPrefab` is assigned.
- Enemy `LootDropper.pickupPrefab` references the Echo Axe pickup prefab.
- Enemy `EnemyBrain.projectilePrefab` references the Archer projectile prefab.

Do not re-fix those unless a fresh Play Mode test shows a real regression.

## Smallest High-Value Next Steps

### 1. Verify Player Defeat Placeholder

Test:

- Let Skeleton, Archer, and Brute damage the player.
- Confirm the HP readout updates.
- Confirm reaching zero HP disables player movement/combat.
- Confirm the defeated marker and reset UI appear.
- Confirm keyboard `R` and the touch/click reset button recover the sandbox.

If weak:

- Increase defeated marker/readout clarity slightly.
- Do not add lives, potions, healing, inventory, progression, or a final death screen yet.

### 2. Android Readiness Follow-Up

Prototype 0.1.13 adds the sandbox scene to Build Settings and expands validation.

Still pending later:

- Real Android device or emulator test.
- Safe-area overlap check for the placeholder mobile HUD.
- Thumb reach check for stick/buttons.

Reason:

- It helps test enemy damage without adding survival systems.

### 3. Keep Basic Attack And Death As Current Quality Bar

Recent passes improved:

- Basic Attack swing readability.
- Basic Attack hit readability.
- Enemy death readability.

Use these as the bar for Cleave, Stomp, Rage, dash, and loot feedback before any content expansion.

### 4. Edit-Mode Tests

The pure combat model and prototype assets now have low-risk coverage for:

- Health/cooldown/modifier basics.
- Prototype scene/prefab/ScriptableObject/input asset presence.
- Prototype asset references and expected core components.

These tests do not replace Play Mode feel checks.

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

### Batchmode Test Running

An overnight attempt was made to run Unity EditMode tests with:

```text
Unity.exe -batchmode -projectPath "C:\Josh\Projects\Dungeon Borne" -runTests -testPlatform EditMode
```

Unity refused because the same project was already open in the editor.

Morning option:

- Close the Unity Editor.
- Re-run EditMode tests from batch mode or use the Unity Test Runner window.

Current verification still includes:

- Runtime assembly compiler check passed.
- EditMode test assembly compiler check passed.
- Full Unity Test Runner execution remains pending while the editor is open.

## Recommended Morning Order

1. Run `Dungeonborn > Prototype 0.1 > Validate Combat Sandbox Scene`.
2. Press Play.
3. Let enemies hit the player.
4. Verify player-hit feedback and HP readout.
5. Let enemies defeat the player.
6. Verify the defeat placeholder and reset button.
7. Press `R` to reset.
8. Run through the full combat loop once:
   - Move
   - Dash
   - Basic Attack
   - Cleave
   - Stomp
   - Rage
   - Kill enemies
   - Pick up Echo Axe
   - Cleave shockwave
9. If all works, verify Prototype 0.1.12 and 0.1.13 in `docs/15_CURRENT_BUILD_STATUS.md`.

## Next Verification Target After 0.1.13

The next smallest useful docs-consistent action is Play Mode verification of the player defeat placeholder and Unity Editor verification of Android/validator readiness.

Reason:

- Player-hit feedback and `HP current / max` now make enemy damage testable.
- Prototype 0.1.12 adds temporary defeat behavior that still needs Play Mode confirmation.
- Prototype 0.1.13 adds Build Settings/validator readiness that still needs Editor confirmation.

Suggested scope:

- Let each enemy type damage the player.
- Confirm the HP readout updates.
- Confirm the red hit feedback is visible.
- Confirm reaching zero HP disables player movement/combat.
- Confirm the temporary defeat marker and reset UI appear.
- Confirm `R` reset and touch/click reset recover the sandbox.
- Run the scene validator and confirm it passes.
