# Asset Sourcing Rules

## Core Rule

Use placeholder primitives first.

Only import assets when the mechanic they support already works.

## Asset Packs Are Support, Not Architecture

Asset packs may provide:

- Dungeon walls and floors
- Props
- Low-poly characters
- Weapons
- Skill VFX
- UI icons
- Sounds
- Animations

Asset packs should not define:

- Player combat architecture
- Input system architecture
- Skill system
- Loot modifier system
- Class evolution
- Dungeon progression
- Future arena logic

The game's brain stays custom.

## Avoid For Now

Do not import large all-in-one RPG templates into the main project during Prototype 0.1.

They can be useful for reference, but they often bring:

- Unneeded systems
- Conflicting input architecture
- Inventory assumptions
- Quest/story frameworks
- Hard-to-remove dependencies
- Messy folders and scripts

Prototype 0.1 should stay small and understandable.

## Preferred Asset Types Later

After the combat loop is testable, search in this order:

1. Low-poly dungeon environment kit
2. Low-poly fantasy characters
3. Weapon pack
4. Stylized magic VFX pack
5. Mobile RPG UI/icon pack
6. Combat animation pack
7. Fantasy sound effects pack

## Asset Quality Rules

Prefer assets that are:

- Low-poly or stylized
- Mobile-friendly
- Modular
- Readable from an angled top-down camera
- Easy to recolor or customize
- Compatible with current Unity version
- Clear about license terms

Avoid assets that are:

- Realistic high-poly
- Huge and unfocused
- Dependent on paid frameworks
- Visually inconsistent with the style guide
- Built around a different game genre

## Asset Log Requirement

Every imported third-party asset must be tracked.

Create or update an asset log with:

- Asset name
- Source URL
- Creator/publisher
- License
- Price
- Import date
- Unity version compatibility
- Where it is used
- Whether it is placeholder or intended final

Do not import untracked assets.

## Blender Role

Blender is for editing and customizing assets after prototype mechanics work.

Use Blender later for:

- Simple weapon edits
- Low-poly props
- Silhouette cleanup
- Modular dungeon piece adjustments
- Story or marketing renders

Blender is not a blocker for Prototype 0.1.

