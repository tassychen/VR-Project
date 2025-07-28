# ARM Institute Immersive VR Experience: Automation in Manufacturing

## Overview

This Unity-based virtual reality project was created for the **ARM Institute (Advanced Robotics for Manufacturing)** [https://arminstitute.org/]. Its purpose is to **educate users**—from **manufacturing business owners** to **robotic operators** and  **students** —about the **practical use of automation and collaborative robots (cobots)** in manufacturing environments.

Users interact with robotic arms, assembly lines, tooling systems(end-effectors, Human-machine Interface), and learn:

- How automation improves efficiency and safety  
- The role of collaborative robots (cobots)  
- Key workflows in modern smart manufacturing environments

---

## Demo Video

▶️ **Watch the demo here**: [Demo Video](https://youtu.be/wI4SdyyoUIE)

> ⚠️ For evaluation and private review only. Please do **not redistribute** or publicly list this video.

---

## 🛠️ Technologies & Setup

| Feature              | Details                                      |
|----------------------|----------------------------------------------|
| **Engine**           | Unity 6.0.0 (LTS)                              |
| **Platform**         | Meta Quest (Standalone VR via IL2CPP build)  |
| **XR Toolkit**       | Unity XR Interaction Toolkit (Input, Grab)   |
| **Target Users**     | Manufacturers,  robotics operators, students, |
| **Purpose**          | Inspire awareness and adoption of automation |

---

## Included in This Repository

```plaintext
Assets/
 ├── 3D Models/          # Robotic arms, environment props
 ├── Scripts/            # C# scripts for interactions & scene logic
 ├── XR/                 # XR Rig, interactions
 ├── Audio/              # Instructional sound cues
 └── Scenes/             # Main VR experience scene(s)

Packages/
 └── manifest.json       # Unity dependencies (XR, Input System, etc.)

ProjectSettings/
 └── Various configs      # Input mapping, layers, tags
