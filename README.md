# project_boost
Project Boost code

Script Descriptions:

ObstaclePusher.cs
This script is attached as a Component to moving obstacles in the game, and the camera in the last level of the game.
Maybe it should be renamed to "Component Pusher"?
It uses a sin function to move the object it's attached to, according to a vector set by the designer.

Rocket.cs
Attached to the Rocket ship.
It handles the user input, moves the rocket around, detects collisions, and manages the current scene.
This script is probably doing too much, and if this project got any bigger it would need to be split into several smaller scripts. 
