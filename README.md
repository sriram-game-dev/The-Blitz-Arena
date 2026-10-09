# The Blitz Arena

An AR target-practice mini-game for Android, built in Unity with AR Foundation.
Scan a surface, tap to place the arena, then shoot energy-filled targets with a cannon that fires from your phone camera.

## Demo

- **Screen recording:** `[add Google Drive link]`
- **Shader Graph screenshot:** `[add Google Drive link]`

---

## Gameplay

1. Move your phone to scan the floor or a table. A grid shows the detected plane.
2. **Tap the screen** to place the Game Base on the surface.
3. Three energy targets float above the base.
4. Press **FIRE** to shoot a projectile from the camera.
5. Hit a target to destroy it (+10 score).
6. Clear all 3 targets to see **WAVE CLEARED**, then press **Restart** to play again.

---

## Features

### Part 1: The Arena (AR Interaction)
- Horizontal plane detection with AR Foundation
- Tap-to-place using `ARRaycastManager` (`PlaneWithinPolygon`)
- Base placed with the raycast hit **pose**, so it sits flat on the surface with no tilt or floating
- `ARAnchor` added on placement so the base stays fixed in the real world
- Placement happens once only; the plane grid is hidden and plane detection stops afterwards
- Taps on UI (FIRE button) do not trigger placement

### Part 2: The Juice (Shader Graph and VFX)
- **Custom Shader Graph** for the targets (no standard materials), URP Unlit:
  - **Scrolling gradient noise** for a flowing energy look
  - **Fresnel rim light** with HDR color for glow under Bloom
  - **Vertex displacement** (sine wobble along normals) so targets feel volatile
  - Exposed properties: colors, rim power, noise scale, scroll speed, wobble amount, speed, and frequency
- **Particle explosion** when a target is destroyed
- Custom **plane grid shader** for the detected-surface visualizer
- **Muzzle flash** and **projectile trail** effects
- Bloom post-processing with HDR

### Part 3: The Game Loop (C#)
- Main Camera acts as a cannon; a UI **FIRE** button spawns a projectile moving forward
- Projectile uses a `Rigidbody` with continuous collision detection
- **Object pooling** (`UnityEngine.Pool.ObjectPool`) for projectiles
- **Hit:** target is destroyed with VFX and sound, score +10
- **Miss:** projectile is returned to the pool after flying more than 5 m
- Game Base spawns 3 floating targets as its children
- Score counter UI and **WAVE CLEARED** message when all targets are cleared
- Sound effects for firing and explosions

---

## Tech Stack

| | |
|---|---|
| Engine | Unity `[6000.3.24f1 LTS]` |
| Render pipeline | Universal Render Pipeline (URP) |
| AR | AR Foundation + Google ARCore XR Plugin |
| UI | TextMeshPro |
| Platform | Android (ARCore-supported device) |

---

## Project Structure

```
Assets/
└── Blitz Arena/
    ├── Scenes/            # Scene_Ar_0.0.0 (main AR scene)
    ├── Script/
    │   ├── AR/            # Placement.cs (tap-to-place, anchor, plane lock)
    │   ├── Gameplay/      # Cannon.cs, Projectile.cs, Target.cs, TargetBob.cs
    │   └── Managers/      # GameManager.cs (score, waves), UiManager.cs (UI state),
    │                      # ResourceManager.cs (shared references)
    ├── ShaderGraph/       # EnergyTarget (target shader), PlaneMaterial (plane visualizer)
    ├── ShaderMaterial/    # Materials built from the shaders, extra URP materials,
    │                      # particle materials (explosion, muzzle flash, trail)
    ├── Prefab/            # AR Default Plane, Target, Projectile, Explosion,
    │                      # MuzzleFlash, Object1
    ├── EFX/               # Sound effects (muzzle flash, explosion)
    ├── Environment/
    │   └── Podium/        # 3D podium model for the Game Base
    └── UI/
        ├── Font/          # AR Techni (sci-fi font)
        ├── Images/        # Background images
        └── Shapes/        # UI shapes
```

---

## Shader Graph Overview

`EnergyTarget` has three effect groups:

| Group | Nodes | Result |
|---|---|---|
| **Scrolling Noise** | Position (Object) + Time x ScrollSpeed → Gradient Noise → Saturate → Power → Lerp (BaseColor / EnergyColor) | Flowing energy pattern, seamless on a sphere |
| **Fresnel Rim** | Fresnel Effect x RimColor (HDR) | Glowing edge |
| **Vertex Wobble** | Sine(Position.y x Frequency + Time x Speed) x Amount x Normal + Position → Vertex Position | Surface ripples and wobbles |

---

## Getting Started

### Requirements
- Unity `[version]` with **Android Build Support** (OpenJDK and Android SDK and NDK)
- An **ARCore-supported Android phone** with Google Play Services for AR installed
- USB debugging enabled

### Run the project
1. Clone the repo:
   ```bash
   git clone https://github.com/sriram-game-dev/The-Blitz-Arena.git
   cd The-Blitz-Arena
   ```
2. Open the project in Unity Hub with the matching Unity version.
3. Open the main scene in `Assets/Scenes/`.
4. **File → Build Profiles / Build Settings → Android → Switch Platform.**
5. Connect your phone, select it under **Run Device**, and click **Build And Run**.

### Android settings used
- Minimum API Level: 30+
- Scripting Backend: IL2CPP, Target Architecture: ARM64
- Graphics API: OpenGLES3
- XR Plug-in Management → Android → **ARCore** enabled
- URP Renderer: **AR Background Renderer Feature** added
- URP asset: HDR enabled, Global Volume with Bloom

> AR features (camera feed, plane detection) do not work in the Unity Editor. Test on a real device.

---

## Controls

| Action | Input |
|---|---|
| Place arena | Tap the screen on a detected surface |
| Shoot | Tap the **FIRE** button |
| Play again | Tap **Restart** after WAVE CLEARED |

---

## Author

`[Your Name]` · GitHub: [@sriram-game-dev](https://github.com/sriram-game-dev) · `[email / LinkedIn]`
