# First Codex / Claude Code Prompt

Use this prompt after creating the Unity project and placing these docs in a /docs folder.

```text
You are helping build a mobile-first low-poly action RPG prototype in Unity.

Before making any changes, read every markdown file in the /docs folder. Treat those docs as the source of truth.

Your goal is NOT to build the full game. Your goal is to create Prototype 0.1: Combat Sandbox.

Build the smallest playable version that proves movement, combat, skills, enemies, and loot can feel good.

Prototype 0.1 requirements:
- angled top-down camera
- test arena room
- mobile-first input architecture using Unity Input System
- keyboard/controller support through the same action map
- placeholder virtual joystick/buttons if mobile UI is not ready
- one Fighter character
- movement
- basic attack
- dash
- cleave skill
- stomp/shield-break skill
- rage/ultimate placeholder
- three simple enemy types: Skeleton Grunt, Archer, Brute
- health/damage/death system
- cooldown handling
- damage numbers
- simple loot drop after enemy death
- one legendary item modifier: Echo Axe makes Cleave send a shockwave

Implementation rules:
- Keep code modular and data-driven.
- Do not hard-code everything into one player script.
- Create clear folders for Scripts, Prefabs, Scenes, ScriptableObjects, UI, Materials, VFX, Art.
- Use placeholder low-poly primitives if no art exists yet.
- Add comments where future systems will connect.
- Do not add multiplayer, open world, monetization, or full inventory yet.
- After implementation, provide a short summary of files created, how to test, and what remains incomplete.

Start by inspecting the project structure, then propose the exact implementation plan, then implement.
```
