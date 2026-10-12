# The Blitz Arena 🎯

**The Blitz Arena** is an Android Augmented Reality (AR) target-practice mini-game built with **Unity 6**, **AR Foundation**, and **Google ARCore**. Scan a real-world surface, place the arena, and fire a cannon at floating energy targets.

## 📸 Project Screenshots

> Screenshots are loaded from the repository's `Project Detail` folder. To make these image links work, use the filenames below for your portrait screenshots, or update the paths to match your existing filenames.

<p align="center">
  <img src="Project%20Detail/01-ar-scanning.png" alt="AR surface scanning" width="220">
  &nbsp;&nbsp;
  <img src="Project%20Detail/02-ar-placement.png" alt="Arena placement in AR" width="220">
  &nbsp;&nbsp;
  <img src="Project%20Detail/03-gameplay.png" alt="Cannon gameplay and targets" width="220">
</p>

<p align="center">
  <em>AR scanning · Arena placement · Gameplay</em>
</p>

## 🎮 Gameplay

1. Move your phone to scan a floor or table.
2. Tap a detected surface to place the arena.
3. Aim using the phone's camera view.
4. Tap **FIRE** to launch a projectile at an energy target.
5. Earn **10 points** for each successful hit.
6. Destroy all three targets to clear the wave, then restart to play again.

## ✨ Features

- **AR plane detection:** Detects horizontal surfaces using AR Foundation.
- **Tap-to-place arena:** Places the game base at the detected surface pose.
- **World anchoring:** Uses an `ARAnchor` to keep the placed base stable.
- **Energy target shader:** Shader Graph effects including animated noise, glowing Fresnel edges, and vertex wobble.
- **Gameplay VFX:** Projectile trails, muzzle flashes, and target-destruction effects.
- **Scoring and wave completion:** Adds 10 points per hit and displays a wave-cleared state after all three targets are destroyed.
- **Projectile pooling:** Reuses projectiles with `UnityEngine.Pool.ObjectPool`.
- **Audio feedback:** Firing and explosion sound effects.

## 🛠 Tech Stack

| Component | Technology |
|---|---|
| Game engine | Unity `6000.3.24f1 LTS` |
| Render pipeline | Universal Render Pipeline (URP) |
| AR framework | AR Foundation |
| Android AR provider | Google ARCore XR Plugin |
| UI | TextMeshPro |
| Language | C# |
| Platform | Android (ARCore-supported devices) |

## 🚀 Getting Started

### Requirements

- Unity `6000.3.24f1 LTS`
- Unity Android Build Support, including Android SDK, NDK, and OpenJDK
- An ARCore-supported Android phone
- Google Play Services for AR installed on the phone
- USB debugging enabled if deploying over USB

### Open and run the project

1. Clone the repository:

   ```bash
   git clone https://github.com/sriram-game-dev/The-Blitz-Arena.git
   cd The-Blitz-Arena
   ```

2. Open the project in Unity Hub using the Unity version listed above.
3. Open the main AR scene:

   `Assets/Blitz Arena/Scenes/Scene_Ar_0.0.0`

4. In Unity, open **File → Build Profiles**, select **Android**, and switch platform if needed.
5. Confirm ARCore is enabled under **XR Plug-in Management → Android**.
6. Connect an ARCore-supported Android phone and select it as the Run Device.
7. Select **Build And Run**.

### Android build configuration

- Minimum API Level: `30+`
- Scripting Backend: `IL2CPP`
- Target Architecture: `ARM64`
- Graphics API: `OpenGLES3`
- AR provider: Google ARCore XR Plugin
- URP: Ensure the AR Background Renderer Feature is configured.
- Post-processing: HDR and Bloom are used for the energy effects.

> **Note:** AR camera passthrough and plane detection require a supported physical device; they cannot be fully tested in the standard Unity Editor Game view.

## 🕹 Controls

| Action | Input |
|---|---|
| Scan a surface | Move the phone to view the floor or table |
| Place the arena | Tap a detected surface |
| Fire | Tap the **FIRE** button |
| Play again | Tap **Restart** after the wave is cleared |

## 📁 Key Project Structure

```text
Assets/
└── Blitz Arena/
    ├── Scenes/          # Main AR scene
    ├── Script/
    │   ├── AR/          # AR placement and anchoring
    │   ├── Gameplay/    # Cannon, projectile, target logic
    │   └── Managers/    # Game state, UI, shared references
    ├── ShaderGraph/     # Energy target and plane grid shaders
    ├── ShaderMaterial/  # Materials and VFX materials
    ├── Prefab/          # Targets, projectiles, explosions, plane
    ├── EFX/             # Sound effects
    ├── Environment/     # Arena base and environment assets
    └── UI/              # Fonts, images, and UI shapes

Project Detail/           # Project screenshots and presentation images
```

## 🧪 Testing Checklist

- [ ] Plane detection works on a supported Android device.
- [ ] The arena places flat on the detected surface and remains anchored.
- [ ] UI taps do not accidentally place or move the arena.
- [ ] Firing, projectile collision, and target destruction work correctly.
- [ ] Each successful hit adds 10 points.
- [ ] Clearing all three targets displays the wave-completion state.
- [ ] Restart allows the intended next gameplay session.
- [ ] The Android build launches and runs on the target device.

## 👤 Author

**Sriram S**

- GitHub: [@sriram-game-dev](https://github.com/sriram-game-dev)
- Portfolio: [sriram-game-dev.github.io/profile](https://sriram-game-dev.github.io/profile/)

## 📄 Project Information

- **Project duration:** 7–9 October 2026
- **Platform:** Android AR
- **Repository:** [The-Blitz-Arena](https://github.com/sriram-game-dev/The-Blitz-Arena)
