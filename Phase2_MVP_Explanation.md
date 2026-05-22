# Phase 2 — Core Systems Foundation (Explained for Non‑Unity Developers)

This document explains what was implemented during Phase 2 of the MVP checklist and why those steps matter. It is written for someone who does not know Unity or game development. The goal is to describe the fundamental concepts, what was built, and the practical reason behind each decision.

**Quick links**
- Phase 2 checklist: [MVP_CHECKLIST.md](MVP_CHECKLIST.md)

**High-level summary**
Phase 2 sets up the foundational systems the rest of the game depends on: application bootstrap, global managers, the player input and movement systems, an interaction framework, and a save/load foundation. Think of Phase 2 as wiring the game’s plumbing and electrical systems so features built later (multiplayer, repair mechanics, UI, scenes) can plug into a stable, predictable base.

---

**Core Unity concepts (quick primer)**
- Scene: A named collection of objects that make up a level, menu, or part of the game. Scenes are loaded and unloaded at runtime.
- GameObject: The basic container in Unity. Everything you see or interact with in a scene is a GameObject.
- Component: A piece of behavior or data you attach to a GameObject (for example: a physics collider, a render mesh, or a custom script). Unity uses composition: GameObjects gain functionality by having components attached.
- Prefab: A saved template of one or more GameObjects and components. Prefabs let you reuse and instantiate objects at runtime (e.g., player, tools, furniture).
- Script (C#): Custom code attached as a component that defines game behavior.
- ScriptableObject: A Unity asset type for storing data (settings, tool stats, contract definitions) independent of scenes or instances.

If those concepts are unfamiliar: imagine a Scene is a stage, GameObjects are props, Components are the individual properties of props (paint, movable wheels), Prefabs are prop blueprints, and Scripts are the instruction manuals for how props behave.

---

**What we implemented in Phase 2 and why**

1) Bootstrap and Manager Systems
- What: A small `Bootstrap` scene and a set of global manager systems (GameManager, SceneLoader, SaveManager, AudioManager, SettingsManager). These managers are created early and persist across scene loads.
- Unity details: Managers are MonoBehaviour scripts attached to small GameObjects placed into the Bootstrap scene. They set themselves as singletons (one global instance) and use `DontDestroyOnLoad()` so they remain active when switching scenes.
- Why: Games need some globally accessible services (session state, audio routing, save operations). Centralizing them avoids duplicating code in every scene and provides a single place to coordinate things like starting a game, loading the starter house, or saving progress.
- Practical effect: When the application starts, Bootstrap loads first and guarantees that the rest of the game (scenes, players, tools) can find and call the same GameManager or SaveManager APIs.

2) Input and Player Setup
- What: A first‑person player controller and an input action asset (movement, look, interact, use tool, menus). The controller reuses Starter Assets patterns but is integrated into our project structure under `Assets/_Project/Scripts/Player/`.
- Unity details: Input is handled using Unity’s Input System package which uses an asset (a JSON-like configuration) mapping keys/buttons to named actions. The player controller is a script component that reads those named actions each frame (or via events) and moves the player’s Character or Camera accordingly.
- Why: A predictable input system separates the concept of “what the player wants to do” from “how the game implements it.” This makes it easy to add gamepad support later or remap keys without touching movement code. Reusing a standard first‑person controller saves time and gives solid, tested movement for playtesting.
- Practical effect: Players can walk, look around, and press a single button to interact with objects. Input events are routed through a small set of player input scripts so features (like tools or menus) can subscribe to them.

3) Interaction Framework
- What: A minimal, reusable interaction system centered on an `IInteractable` interface (or equivalent base component) and a raycast-driven player interaction routine, plus an interaction prompt UI.
- Unity details: The player casts a short invisible ray forward; if it intersects an object that implements `IInteractable` (a component on a GameObject), the player sees a UI prompt (e.g., “Press E to use”) and can call `Interact()` on that object. Core code lives under `Assets/_Project/Scripts/Interaction/`.
- Why: Standardizing interactions keeps mechanics consistent. Every interactable object (pickup, tool, repair panel) implements the same interface so the player code doesn’t need to know specifics about each object type.
- Practical effect: Adding a new interactive thing (like a tool or a repair spot) is straightforward: attach an interactable component and implement the interface. The player will automatically be able to interact with it.

4) Save System Foundation
- What: A save data model, a JSON serialization layer, and file utilities for reading/writing saves. We implemented a basic SaveManager API and platform-aware file location handling.
- Unity details: Save data is represented as plain C# data classes (POCOs). We use JSON (via Unity’s JsonUtility or a library like Newtonsoft JSON) to serialize and deserialize these objects to disk. The SaveManager exposes simple calls like `SaveGame()` and `LoadGame()` and fires events when saves complete.
- Why: Saves are essential to persist player money, house/repair state, and contract progress. Implementing a simple, robust save layer early prevents later features from inventing inconsistent save formats.
- Practical effect: The game can persist the player’s currency and the repair progress of objects between play sessions. Save events are available so UI or gameplay systems can show “Saved” feedback.

---

**How this supports later phases**
- The manager and bootstrap systems provide a stable environment for the multiplayer lobby and network manager in Phase 3.
- The input and interaction frameworks let repair tools and house objects hook in cleanly in Phase 4 without reworking player code.
- The save system ensures progress and contract state can be tested and validated across play sessions.

---

**Where to look in the repository**
- Phase 2 checklist and scope: [MVP_CHECKLIST.md](MVP_CHECKLIST.md)
- Bootstrap scene location: [Assets/Scenes/Bootstrap/](Assets/Scenes/)
- Player and interaction scripts: [Assets/_Project/Scripts/Player/](Assets/_Project/Scripts/Player/) and [Assets/_Project/Scripts/Interaction/](Assets/_Project/Scripts/Interaction/)
- Save system code: [Assets/_Project/Scripts/SaveSystem/](Assets/_Project/Scripts/SaveSystem/)

(If the folders are empty placeholders, they are still intentionally created to hold the described systems.)

---

**Next steps and recommendations**
- Phase 3 (Networking) should build on the manager patterns here and register the networked player prefab with the NetworkManager.
- Keep interfaces small and testable (for example `IInteractable`, `ISaveable`) to make unit testing and future expansion easier.
- Add short developer docs for each manager’s API (e.g., `GameManager.StartSession()`, `SaveManager.SaveGame()`) so contributors can use them without reading the full implementation.

---

If you want, I can:
- Expand this into separate per-system markdown pages (Input, Interaction, Save) under `Assets/_Project/Docs/`.
- Generate small API snippets for the managers to include in developer docs.

