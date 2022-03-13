# Pre-Rendered Background in Unity
A sample Unity project to handle camera movement on pre-rendered background games, also known as 'Ken Burns' effect. Thanks to the old unity [wiki](http://wiki.unity3d.com/index.php?title=OffsetVanishingPoint) for this amazing trick.

## Specification
* Unity 2019.4
* Any modern version, really (5.0+)

## How to Play
* Open Main scene 
* Use WASD to move the character around

## Setup
### Camera
There are 2 cameras in the scene. One is the main camera, responsible for what you see. The other is used to convert character's world position to screen point.
```
float playerOnScreenPosX = secondaryCamera.WorldToScreenPoint(playerTransform.position).x / Screen.width;
float playerOnScreenPosY = secondaryCamera.WorldToScreenPoint(playerTransform.position).y / Screen.height;
```

__Min/MaxCamOffset__: The min/max offset the camera can pan. The value can be simulated by ticking `IsDebug` value on the inspector then tweak the `X` and `Y` value.

__Min/MaxScreenPoint__: The min/max character's position in screen point. How far you allow your character to be on the edge of the screen is the value you put on these variables. Start by ticking `IsDebug` and see the value of `PlayerScreenPos`.

These variables are important to track the character's movement. By predefining the value of `CamOffset` and `ScreenPoint`, we can then calculate character's screen point relative to the cam offset and pan accordingly.

### Invisible Object
3D object on the scene needs to be invisible while still maintaining its collider. To achieve this, use special material called `InvisibleMat` on the obscuring object and attach `Obscurable` component to the obscured object.
