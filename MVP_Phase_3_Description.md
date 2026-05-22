**Phase 3 — Repair Systems & Multiplayer Sync (MVP)**

This document explains what was done in Phase 3 of the MVP checklist. It's written for someone who does not know Unity or game development. The goal is to describe the fundamental concepts, the reason for each step, and what the system achieves.

**Overview**
- **Goal:** Implement a simple, networked repair interaction so two players can collaboratively fix a single repairable object (for example, a damaged wall) and receive a reward.
- **Why it matters:** This demonstrates the core gameplay loop (inspect, repair, reward), proves multiplayer synchronization, and validates save/load and progression systems.

**Key Concepts (plain English)**
- **Game Engine:** Unity is the tool that runs the game world. It organizes everything into scenes (levels) and objects.
- **Scene:** A scene is like a single level or room. It contains objects, lighting, cameras, and UI used while playing.
- **GameObject:** The basic item in a scene (e.g., a wall, a player, a hammer). Think of it as a container that can hold behavior and visuals.
- **Component:** A small building block that you attach to a GameObject to give it functionality (movement, rendering, physics, custom scripts). Components follow the "composition over inheritance" approach.
- **Prefab:** A reusable template of a GameObject with components and settings. Prefabs let you spawn identical items (like repairable walls or tools) consistently.
- **Script / MonoBehaviour:** A small program written in C# that runs inside Unity, attached as a component to a GameObject to implement interactive logic (for example, the logic that applies repair progress when a player uses a tool).
- **Input System:** The code that captures player input (mouse/keyboard or controller) and turns it into actions like "use tool" or "look around".
- **Raycast Interaction:** A typical interaction model where the game shoots an invisible line from the player’s viewpoint to detect which object they're looking at and wants to interact with.
- **Netcode / Networking:** Systems that let multiple players connect and share a single game world. They synchronize important state (who is where, repair progress, object ownership) between machines.

**What we implemented in Phase 3 (high-level steps and why)**

1) Repairable Object and Visual Progress
- **What:** Added a repairable object (example: a damaged wall) with a visible progress indicator that updates as players work.
- **Why:** The visual feedback makes the repair feel real and lets players coordinate their efforts. It also serves as the single authoritative piece of gameplay state that must be shared across the network.

2) Player Tool and Interaction
- **What:** Implemented a simple tool (one action: repair) and an interaction method using raycasts. When a player aims at the repairable object and presses the use key, the tool applies repair progress.
- **Why:** Tools and interactions are the player's primary control over the world. Raycast interactions are reliable and simple to explain: the player looks at an object and performs an action.

3) Shared Repair Progress & Authority
- **What:** Designed the repair progress to be a single authoritative value owned by the server (or host). Clients send requests ("I repaired for X amount") and the server validates and updates the shared progress.
- **Why:** Server-authoritative designs prevent cheating and ensure consistent state across clients. It also simplifies conflict resolution when multiple players act on the same object simultaneously.

4) Network Synchronization
- **What:** Networked the key fields: current repair progress, whether the repair is complete, and a compact event for completion (to trigger reward logic and visual effects). Only necessary values are synchronized to reduce bandwidth.
- **Why:** Multiplayer requires keeping important state in sync. Sending only the minimal data reduces lag and network noise.

5) Simple Reward Payout
- **What:** When the repair reaches completion, the system triggers a payout/reward routine that updates the players' shared money/progression state and displays confirmation to players.
- **Why:** Completing a contract must have immediate, visible consequences (money, progression) to complete the gameplay loop and validate the economy integration.

6) Save/Load Hooks
- **What:** Added hooks so completed repairs and player progression can be saved as simple JSON during development. The save system records minimal state: which repairs are completed and the player’s balance/progression.
- **Why:** Persistence proves progress across sessions and is essential for an MVP that includes progression and house flipping eventually.

**Implementation Notes (non-technical explanations)**
- **Why use prefabs for repair spots and tools:** Prefabs let designers place multiple identical repair spots or spawn tools without re-implementing logic. This keeps the project consistent and fast to iterate on.
- **Why server authority matters:** If every player decided the state locally, players could disagree (one player sees the object repaired, another doesn't). The server owning the state makes the game consistent for everyone.
- **Why keep visuals and logic separate:** Visual effects (dust clearing, changing textures) are presentation; the repair progress is game logic. Separating them makes it easier to change visuals later without breaking gameplay.

**Simple user stories demonstrated by Phase 3**
- "Two players aim at a damaged wall and use their tools; the wall's repair bar fills for both players." 
- "When the bar reaches full, both players see a completion animation and their shared balance increases." 
- "If a player disconnects and returns later, completed repairs remain fixed because the save system recorded the result."

**What this does not cover (out of scope for Phase 3)**
- Advanced replication optimizations and rollback networking.
- Full anti-cheat systems beyond server authority.
- Polished UI/UX and art polish beyond basic feedback.

**Where to look next (developer-oriented)**
- Continue Phase 4 items: expand repair types, add more tools, and refine the economy and UI.
- Add automated tests for save/load, repair progress edge cases, and networking reconciliation.

---

If you want, I can:
- convert this to the project’s preferred doc name and link it from the MVP checklist, or
- add file-level references (scripts or prefabs) from the repo into this markdown.
