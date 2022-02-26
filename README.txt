Hello Communix,

This is my solution to you unity exercise: Pang game.
In the spirit of the times I decided to rename it "Stop Covid", so you're playing
a brave square trying to stop the covid virus at different places around the world.

== Overview 

The solution meets all the manadatory requirements and some optional ones.
- There are 3 distinct levels, soundtrack and SFX.
- The game is ready for unique weapon types with finite ammo, rate of fire, 
  and maximum number of projectiles in flight, but only 1 weapon is implemented now.
- The game is almost ready for 2-player local multiplayer but I didn't have time to finish it.
- Unfortunately no score board either.

I think I can easily implement the rest of the bonus tasks, but I only worked 3 nights 
and friday noon on the game, and I preferred to concentrate on polishing the mechanics 
and making sure there are no bugs.

== Code architecture
Some words about the code design and architecture.
Early on I decided on the following points:
- The solution will be composed of multiple scenes even though this is a small game and can fit in a single scene.
- I'll have one scene that is always loaded with the game managers and audio source to play
  background music. This way the game managers can keep global state, and the audio source
  can play music seamlessly even when switching scenes.
- Every part of the game should work by itself to facilitate testing.
  I wanted to be able to start any level and jump right into the action.
- Game objects should not be linked to one another directly; messages are better.
  I chose messages over dependency injection because I have more experience with this technique.

That's pretty much all the design I had. The rest of the game grew organically.
I started making the player, working on movement and collision. Then the balls movement and collision,
then digressed to work on the main menu and startup.
I then added graphics, sound, shots and other stuff in no particular order.

The only thing I was strict about was making sure everything is standalone.
The player and balls can work in any scene with no outside information.

== Mechanics
I didn't use Unity's physics system except to detect collisions. I think the original game had very
simple and predictable movements which you can only get with simplified physics that you do yourself.
There are a only handful of layers (walls, ball, player, shots) and 1 tag (enemy; which I never used...)

Note:
The collision handling took longer than expected. I thought I'd make a nice general movement
controller that can work on curved surfaces and slopes. Big mistake. I wasted too much time.
I settled on a much simpler design: separate the X and Y axes and move in each one separately.

== Input
One of the requirements was that the game should work on PC with keyboard, 
and mobile phones with touch screen. That means the input should be decoupled from
player control logic so I made the InputSystem class that represents input feedback
from different devices and combines to exposes the input to the rest of the game.

== Music
There are 2 tracks of background music. The main menu music is played from the "Root" scene,
and that's actually my "loading.." music also. The other track is played in the level scenes so 
in theory you can have different tracks in each level.
To switch the music I tied each audio source to a mixer group and I switch mixer snapshots.
That gives me a nice blending effect !

== SFX
Only a single sound effect is in the game.
I didn't place much importance on this.

== Graphics
There is no fancy or custom graphics, just stock images downloaded freely.
Only notable thing is that the game can adapt to different aspect ratios.
- The background image is scaled to cover the screen
- The walls are positioned by the camera object to always be at the sides.
- The UI is made for landscape mode with Unity's layout groups.
