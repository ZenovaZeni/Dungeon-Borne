# Dungeonborn Project Docs

This folder is the source of truth for the game. Before Codex, Claude Code, Unity MCP, or any coding agent touches the project, have it read these docs first.

## Current Working Concept

A mobile-first low-poly action RPG where the player descends through dungeon chapters, evolves classes through branching paths, finds loot that changes skills, and eventually brings their build into arena-style competition.

## Development Rule

Do not build the whole game at once.

Build the smallest fun version first:

1. One room
2. One hero
3. Three enemies
4. Four skills
5. One loot drop
6. One class evolution choice
7. Mobile controls
8. Controller-ready input architecture

If movement, combat, skill use, and loot are not fun in one room, more content will not fix it.

## Tooling Direction

Recommended engine: Unity

Recommended art style: low-poly stylized fantasy

Recommended platform target: mobile-first, with PC/editor testing during development

Recommended asset workflow:
- Unity for gameplay
- Blender for custom/modified 3D assets
- Remotion/Hyperframe for marketing videos, trailers, chapter intro videos, or motion-comic style story assets
- Codex/Claude Code for iterative coding, architecture, refactors, and implementation plans
- Unity MCP for editor-connected automation once the base Unity project exists
- Blender MCP for asset generation/editing once Blender is installed and asset needs are clear

## What This Game Is Not

- Not open-world in v1
- Not multiplayer in v1
- Not a huge MMO
- Not realistic AAA graphics
- Not a grid tactics game
- Not a roguelike that deletes all progress
- Not pay-to-win

## What This Game Is

- Low-poly mobile action RPG
- Fast combat
- Deep progression
- Strong class identity
- Gear-driven buildcraft
- Modular dungeon chapters
- PvP-ready combat design, but PvE-first
