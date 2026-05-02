# Combat and Input Design

## Camera

Use an angled top-down/isometric camera.

Reasons:
- best for mobile
- easier to control
- easier to see enemies
- supports dungeon rooms
- works with low-poly style
- good for future arena PvP

## Mobile Controls

Left side:
- virtual joystick for movement

Right side:
- basic attack
- dash/dodge
- skill 1
- skill 2
- skill 3
- ultimate

Optional later:
- tap enemy to target
- auto-target nearest enemy
- hold attack for heavy attack
- swipe skill button for alternate cast

## Controller Support

Design input through Unity's Input System from day one.

Actions should be abstract, not hard-coded:
- Move
- Aim
- BasicAttack
- Dash
- Skill1
- Skill2
- Skill3
- Ultimate
- Interact
- OpenInventory
- Pause

This allows:
- mobile buttons
- keyboard/mouse
- Bluetooth controller
- Fold/tablet play
- future PC build

## Combat Feel Checklist

Every attack should have:
- startup time
- active hit window
- recovery time
- hit VFX
- hit sound
- damage number
- enemy reaction
- slight hit pause for heavy hits
- optional camera shake

## Target Feel

Fast but readable.

Not slow like old-school click-only ARPGs.

Not chaotic like button-mashing.

The player should feel:
- I dodged that because I saw it coming.
- I used the right skill at the right time.
- My gear changed my combo.
- My class choice matters.
