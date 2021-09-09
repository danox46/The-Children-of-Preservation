# The-Children-of-Preservation
A 2D Platformer with physic-based skills, combat system, and enemy AI

This was my first Unity project. I decided to include a very wide scope of features for exploration. This means that most features are superficial. 
However, there's some interesting stuff going on with the core mechanic.

I chose an existing book to base the game on. And the magic system of the book required blue lines that originated at the character's chest and pointed to a metal object. 
Those lines also needed to be clickable for the activation of the skill, they needed to be updated on realtime with the movement of both the character and the objects, 
and they needed to appear and dissapear when the metallic object entered and exited the range.

The ALControler calculates the correct angle, and the size and offsets of the clickable trigger collider.
https://github.com/danox46/The-Children-of-Preservation/blob/master/Assets/My%20assets/Characters/ALControler.cs

The ALSpawmwer controls the instantiation and destruction of new lines as needed.
https://github.com/danox46/The-Children-of-Preservation/blob/master/Assets/My%20assets/Characters/ALSpawmer.cs

The game also has basic enemy NPCs. Beacuse of the movility from the main character I integrated a double "Visivility range" system.
This sistem makes it harder for the NPC to initially "See" the character in it's simulated frontal vision range, but it expands in combat mode to a larger full circular range.
While it is posible for the player to get out of the vision range during combat, making the enemy "lose track" of the player and returning to it's designated patrol.

This was my first implementation of an NPC AI. It uses a regular state machine implemented as an interface to control the instructions being sent to the NPC.
https://github.com/danox46/The-Children-of-Preservation/tree/master/Assets/My%20assets/Characters/Kolos/KolosInterface

I'm currently working on updating this prototype. Starting with the visual assets. Watch it for more.
