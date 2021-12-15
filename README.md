# COP-3003-FinalProject
This is a game made in Unity with Visual Studio using C#. Source code is found in Proj/Assets/Scripts.

The player plays as a sorcerer who can cast spells to defeat enemies. There are several potions that increase the player's attack that can be found hidden throughout the map, and they can sometimes be acquired by defeating enemies. Gold can be found throughout the map, which can be spent at the glowing circles to heal the player's HP and MP for 10 gold. Each piece of gold gives the player 5 gold, and there is a large bag of gold at the end of the map that is worth 50 gold.

**Project requirements:**

    Bottom of Enemy1: The comments explain inheritence and dynamic dispatch.
  
    Bottom of PlayerMovement: The comments explain the use of generics.
  
    Top of Stats: The comments explain encapsulation.

**Movement Controls:**

The player can move with either WASD or the arrow keys. Press space to jump. While in mid-air, pressing space again will allow the player to hover. This makes the player move faster and reduce their fall speed. Use the mouse to move the camera.

**Stats:**

Because the player has the script PlayerMovement, which is a subclass to CharacterStats, the player inherits the members from CharacterStats.
The player has HP, MP, Attack, and Defense stats.
Casting spells consumes MP.
Attack can be increased by collecting potions.
Gold can be found throughout the level. The player simply collides with it to gain it.

**Spells:**

The player can cast 2 spells.

Press "F" to shoot a fire ball forward. This costs 5 MP. This deals damage equal to the player's attack stat minus the enemy's defense stat.

Press "I" to surround the player in ice. This costs 10 MP. This is a close-range spell that deals damage equal to half of the player's attack stat, and ignores the enemy's defense. This is useful against the larger enemies with high defense. It can also reflect enemy projectiles, so when an enemy shoots a fireball at you, you can cast the ice spell to shoot it back at the enemy. When this happens, damage will be calculated based on the enemy's attack stat minus their defense stat.

Casting a spell will toggle the able variable in GeneralFunctions to false for a short duration. This is to create a bit of endlag to the player's actions, and the player will only be able to move and perform actions when the able variable is true again.

**Enemies:**

There are 2 types of enemies: skeletons and flame demons.

The skeletons have less health and defense, and upon death have a 25% chance to drop a potion that boosts attack.
Skeletons will chase the player when the player is near them, and they will chase faster when they get very close. The player will take damage upon collision with the skeleton.

The flame demons have more health and defense, and upon death have a 100% chance to drop a potion that boosts attack.
Flame demons shoot fireballs toward the player at regular intervals when the player is near them. These fireballs can be reflected back at the flame demon by using the ice spell at the right time.

The enemies use the classes Enemy1 and Enemy2, which are both subclasses to the same superclass EnemyParent, which is a subclass to CharacterStats.
This allows both Enemy1 and Enemy2 to inherit all members from EnemyParent and from CharacterStats, while adding and overriding other members.

![Screenshot (118)](https://user-images.githubusercontent.com/42978071/146262590-27c3d830-d663-47a4-8090-5849f10ca867.png)

**Code Analysis**

For analyzing my code for issues, I used ReSharper.
![ReSharper_analysis](https://user-images.githubusercontent.com/42978071/146270187-92fbfaa4-6b05-43a9-954e-ff02a56e1a1c.PNG)
Remaining issues detected by ReSharper:

**1. Constraints Violations:** I got the same warning for every file: "Namespace does not correspond to file location, should be 'Assets.Scripts'". I looked at the documentation for this warning, but I still don't understand what the warning means or how to fix it. It seems that it has something to do with the root folder, but I am afraid to mess with that because I am worried that it might break my project.

**2. Language Usage Opportunities:** I got 1 warning saying to "Convert to method call with '?:' expression inside. The documentation suggests something like this as an example: Console.WriteLine(flag ? msg1 : msg2); However, I prefer to keep the simple if statement, because even though it is more lines of code, it is more consistent with the rest of the code in the project.

**3. Potential Code Quality Issues:** I got 4 warnings saying to not compare floats with == or != because of a possible loss of precision when rounding values. I probably could have written the code in this section better to avoid this warning by using gliding as a boolean and setting glideSpd according to a check of the boolean, but it currently works as intended and I would need to rewrite a lot of the logic to how I did this, so I decided to leave it as is. This is definitely something I need to consider in the future when using floats.

**4. Redundancies in Symbol Declaration:** These warnings seem to be about classes and methods not being used, but I am confused why I am getting that issue, because I am sure that they are being used, and deleting those classes and methods would take out chunks of my game.

**5. Spelling Issues:** These are mostly just words in the comments that are not recognized, like "healthbar", "rigidbody", and "endlag". These are fine as they are, but I am not sure if there is a way to remove the warning without deleting the words from the comments, so the warnings remain.

**Bugs:**

There are still two bugs that I have not yet figured out:

1. It doesn't always detect the 'Enter' input when trying to heal at a glowing circle.

The code for this is in proj/Assets/Scripts/PlayerMovement.cs, starting on line 168.

This bug is not game-breaking because it tends to work within a few 'Enter' inputs, but it should be activating 100% of the time when the player presses 'Enter'. This is the only input that doesn't always work despite being written the same way as other input detections. There are checks for whether the player has enough gold and if they are already at full HP and MP, but these seem to be working correctly. It doesn't display the debug message when it doesn't work, so it just isn't getting to that part of the code, which leads me to believe that it might be an issue with the collision detection.

2. Sometimes enemies don't die despite being at less than or equal to 0 health.

The code for this is in proj/Assets/Scripts/Enemy2, starting with line 25.
Could also be associated with proj/Assets/Scripts/EnemyParent or CharacterStats in the die() function.

This bug is a little more game-breaking, because when it happens the enemy just doesn't die. The good news is that it seems fairly repeatable. This bug is only occurring after raising the Attack stat to 14 or so, and it seems to only happen with the flame demon enemy. However, I am unsure of why the Attack stat would be relevant here because the die() function is called when health <= 0, and there is no connection to the Attack Stat aside causing health to be reduced by a different amount. Also, the fireballs that a player shoots at the enemy will start going through the enemy when this happens instead of destroying itself, so it seems that some part of the enemy is being destroyed, to the point that fireballs will no longer collide with the enemy, but the enemy is still visible and can shoot their own fireballs.

**Credits:**

Brackeys' Youtube videos helped a lot with getting started on the movement and camera.

Brackeys - Third Person Movement in Unity https://www.youtube.com/watch?v=4HpC--2iowE

I also followed this Brackeys video to get more help with jumping and gravity:

Brackeys - First Person Movement in Unity - FPS Controller https://youtu.be/_QajrabyTJc?t=898

This video from Adam Konig also helped me get the projectiles working, which can be applied to pretty much any object that needs a force applied to them:

https://www.youtube.com/watch?v=RnEO3MRPr5Y

**Assets downloaded from Unity Asset Store**

These are the textures and prefabs I downloaded from the Unity store to make the game more visually pleasing:

LowlyPoly - Hand Painted Stone Texture

LowlyPoly - Fantasy Treasure Pack Lite

amusedArt - Stone Monster

TeamJoker - Fantasy Monster - Skeleton

TS WORK - Fantasy Monster(Wizard) DEMO 

