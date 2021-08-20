====================================
 TIGERFORGE EASY EVENT MANAGER DEMO
====================================

This Demo Scene simply shows how to use Easy Event Manager.

When you play the game, you'll see a cube (Player) moving forward to a file of Coins.
Every time the Player collides with a Coin, he picks it up.
In the Console you should see a text message informing you about the quantity of collected coins.

This example uses the Event Manager system.
Basically, the Coin C# Script emit an Event with a value of 10 every time it catches a collision with the Player.
In the meanwhile, the Player constantly listens to that Event, increasing the number of collected coins.
