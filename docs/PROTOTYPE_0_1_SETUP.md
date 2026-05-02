# Prototype 0.1 Unity Setup

This folder is Unity-adoptable source, not proof that Unity Hub has created a valid project yet.

## Create / Open In Unity

1. Open Unity Hub.
2. Add or create a Unity 3D/URP project at this repository root.
3. Let Unity resolve `Packages/manifest.json`.
4. Confirm these packages are installed:
   - Input System: `com.unity.inputsystem`
   - TextMeshPro: `com.unity.textmeshpro`
   - Unity UI: `com.unity.ugui`
   - Test Framework: `com.unity.test-framework`
5. If Unity prompts to enable the new Input System backend, accept it and restart the editor.

## Generate The Combat Sandbox

After Unity finishes importing:

1. Use the editor menu: `Dungeonborn > Prototype 0.1 > Generate Combat Sandbox Scene`.
2. Open `Assets/_Dungeonborn/Scenes/CombatSandbox_Prototype_0_1.unity`.
3. Press Play.

## Controls

Keyboard / mouse:

- Move: `WASD`
- Basic Attack: left mouse
- Dash: `Space`
- Cleave: `Q`
- Stomp: `E`
- Rage placeholder: `F`
- Reset sandbox: `R`

Controller:

- Move: left stick
- Basic Attack: right trigger
- Dash: east face button
- Cleave: west face button
- Stomp: north face button
- Rage placeholder: right shoulder

Generated mobile placeholders:

- Left on-screen stick maps to left stick.
- Right-side buttons map to the same gamepad-style actions.

## Prototype Loop

- Player can move, dash, basic attack, Cleave, Stomp, and use a Rage placeholder.
- Skeleton Grunt and Brute chase and melee attack.
- Archer chases into range and shoots projectiles.
- Enemies have health and can die.
- Damage numbers spawn on hits.
- Brute drops Echo Axe.
- Picking up Echo Axe enables Cleave shockwaves.

## Placeholder Boundaries

See `TODO_PROTOTYPE_0_1.md` for known placeholders and explicit non-goals.
