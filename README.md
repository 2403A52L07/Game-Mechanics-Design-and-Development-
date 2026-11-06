# 3D Forest Runner – Village Theme Game

Unity/C# starter source pack for the GMDD course project at SR University.

## Included scripts
- `RunnerController.cs` – automatic forward movement, three lanes, and jump.
- `GameManager.cs` – run state, coin count, score/distance HUD, game over, pause, restart, and best-distance saving.
- `CoinPickup.cs` – coin/relic trigger; set `value` to 1 for a coin or 5 for an emerald.
- `ObstacleCollision.cs` – ends the run when the Player enters an obstacle trigger.
- `CollectibleRotate.cs` – rotates collectible objects.
- `SegmentSpawner.cs` – spawns road segment prefabs ahead and removes old segments.

## Add to Unity
1. Back up your Unity project.
2. Copy the `Assets/Scripts` folder into your project's `Assets` folder.
3. Wait for Unity to compile and check the Console.
4. Add a `CharacterController` and `RunnerController` to the player.
5. Set the player's Tag to `Player`.
6. Create a GameObject named `GameManager`, attach `GameManager.cs`, and assign the player and optional UI references.
7. Coin and obstacle prefabs need Collider components with **Is Trigger** enabled.
8. Add `CoinPickup.cs` to coin prefabs. Use value 1 for gold and 5 for emerald.
9. Add `ObstacleCollision.cs` to obstacle prefabs.
10. For endless road spawning, create equal-length road segment prefabs and assign them to `SegmentSpawner`.

## Controls
- A / Left Arrow: move left
- D / Right Arrow: move right
- Space: jump

## Important
- This is starter code to integrate into the existing game, not the complete Unity project or a verified copy of the team's current source.
- The controller uses Unity's classic Input API. If the project is set to the new Input System only, enable **Active Input Handling: Both** in Player Settings or convert the input code.
- The GameManager uses Unity UI `Text`. If the project uses TextMeshPro, change those fields to `TMP_Text` and add `using TMPro;`.
- Segment prefabs must have a consistent pivot and length. Set `segmentLength` to the real segment length.
- Ensure trigger callbacks work with your CharacterController/collider setup and test all scripts in Play Mode.
- The spawner only creates road segments. Obstacles and collectibles must be placed in the segment prefabs or handled by another spawner.

## Team
D. Pavan · R. Nikhil Kumar · A. Sandeep

Course: Game Mechanics, Design and Development (GMDD)  
Institution: SR University
