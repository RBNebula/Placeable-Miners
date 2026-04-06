# Placeable Miners

Allows `Auto-Miner Mk1`, `Rapid Auto-Miner`, and `Heavy AutoMiner` to be placed anywhere while still snapping correctly to vanilla miner nodes when used on them.

Project layout:

1. `PlaceableMiners.cs` contains plugin bootstrap only.
2. `Core/` contains runtime and miner-placement logic.
3. `Patches/` contains thin Harmony patch classes.
4. `Utilities/` contains version-safe node ghost helpers.

What it does:

1. Removes the normal placement-node restriction for the three miner variants.
2. Tries to borrow the ore definition from the nearest compatible vanilla miner node.
3. Disables the vanilla "moved miner spot" recovery popup and auto-pack behavior for those miners.

Install:

1. Copy `PlaceableMiners.dll` into `BepInEx/plugins/PlaceableMiners/`
