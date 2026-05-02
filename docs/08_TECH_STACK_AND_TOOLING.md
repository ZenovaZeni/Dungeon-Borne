# Tech Stack and Tooling

## Recommended Engine

Unity.

Reasons:
- strong mobile support
- huge asset ecosystem
- good low-poly asset availability
- lots of tutorials and examples
- C# scripting works well with AI coding agents
- Unity Input System supports mobile/controller-style abstraction
- better path for IAP/ads/analytics if the game becomes real

## Project Setup

Create the Unity project first.

Recommended project type:
- Unity 3D URP template
- low-poly/stylized rendering
- Android build support installed
- iOS support later if needed

Install early:
- Input System
- Cinemachine
- TextMeshPro
- Addressables later, not immediately
- mobile joystick/control package or custom implementation

## AI Agent Workflow

Codex/Claude should read docs before coding.

Suggested init instruction:

"Read all markdown files in /docs before making changes. Treat them as the source of truth. Do not expand scope unless explicitly asked. Build the smallest playable version first."

## MCP Setup Order

1. Install Unity
2. Create/open Unity project
3. Put these docs in /docs
4. Initialize git repo
5. Add simple README and project structure
6. Install Unity MCP if using an MCP-capable assistant
7. Confirm the agent can inspect/create scripts/assets/scenes
8. Build first combat sandbox

## Blender Setup

Blender is recommended but not the first blocker.

Use Blender for:
- editing low-poly asset packs
- making simple weapons
- making props
- making modular dungeon pieces
- rendering story panels
- preparing assets for Unity

Use Blender MCP after:
- Blender is installed
- you know what asset you need
- the Unity prototype has a basic style target

## Remotion/Hyperframe

Use for:
- trailers
- chapter intro videos
- motion-comic story sequences
- social media ads
- marketing demos

Do not make the core game dependent on them.
