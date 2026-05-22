# MVP Checklist for Contractor Simulator

## Overview
This document captures a comprehensive, detailed checklist for implementing the Contractor Simulator MVP. It is derived from the current project vision in `Agents.md` and the folder structure guidance in `FOLDER_STRUCTURE.md`.

The goal is to produce a playable vertical slice with a small map, one house, basic tools, repair mechanics, and 2-player multiplayer.

---

## MVP Core Goals

- [ ] One small map
- [ ] One playable house
- [ ] 1–2 repair mechanics
- [ ] 2-player co-op multiplayer
- [ ] Basic money system
- [ ] Save/load functionality
- [ ] One truck or equivalent contextual vehicle
- [ ] Basic tools
- [ ] Proof of the core gameplay loop
- [ ] Multiplayer synchronization
- [ ] Interaction feel
- [ ] Fast iteration speed

---

## Primary Vertical Slice Goal

The first playable vertical slice should support:

- [ ] Two players connected in the same session
- [ ] Shared multiplayer house instance
- [ ] One repairable wall/repair objective
- [ ] One usable repair tool
- [ ] Shared repair progress visible to all players
- [ ] Reward payout after completion
- [ ] Basic save/load of progression or contract state
- [ ] One complete repair contract flow

---

## Phase 1: Project Setup

### 1.1 Repository and Project Bootstrapping
- [x] Confirm Unity LTS installation and project compatibility (`ConstructionSimulator/ProjectSettings/ProjectVersion.txt` reports `m_EditorVersion: 6000.4.7f1`)
- [x] Create or update `.gitignore` for Unity
- [x] Verify `ConstructionSimulator.slnx` and Unity project files are present
- [x] Ensure `Assets`, `Packages`, `ProjectSettings` are tracked correctly

### 1.2 Folder Structure Setup
- [x] Create `_Project/` root folder under `Assets/`
- [x] Create `Assets/_Project/Gameplay/` and subfolders for Houses, Items, Jobs, Materials, Tools
- [x] Create `Assets/_Project/Networking/` and subfolders for Lobby, Matchmaking, PlayerSync, Replication
- [x] Create `Assets/_Project/Prefabs/` with categories Environment, Furniture, Houses, Interactables, Networking, Player, Tools, UI, Vehicles
- [x] Create `Assets/_Project/Resources/` for runtime-loadable icons, localization, and UI assets
- [x] Create `Assets/_Project/ScriptableObjects/` for data definitions and tuning assets
- [x] Create `Assets/_Project/Scripts/` with subfolders Economy, House, Interaction, Managers, Multiplayer, Player, SaveSystem, Tools, UI
- [x] Create top-level `Assets/Art/`, `Assets/Audio/`, `Assets/Materials/`, `Assets/Models/`, `Assets/Scenes/`, `Assets/Settings/`, `Assets/ThirdParty/`

### 1.3 Package and Engine Configuration
- [x] Install and configure Unity Input System
- [x] Install and configure Netcode for GameObjects
- [x] Install Unity Transport package
- [x] Install TextMeshPro
- [ ] Install Starter Assets - First Person Controller
- [ ] Optionally install Cinemachine, ProBuilder, Multiplayer Tools, DOTween if needed later
- [x] Configure Render Pipeline settings (URP recommended)
- [x] Verify `ProjectSettings/InputManager.asset` and `InputSystem_Actions.inputactions`
- [x] Set up quality and graphics settings for MVP

---

## Phase 2: Core Systems Foundation

### 2.1 Bootstrap and Manager Systems
- [x] Create `Bootstrap` scene under `Assets/Scenes/Bootstrap/`
- [x] Implement bootstrap logic to load persistent managers and network systems
- [x] Create `GameManager` for session flow and global state
- [x] Create `SceneLoader` for scene transitions
- [x] Create `SaveManager` for save/load handling
- [x] Create `AudioManager` for sound playback and mixing
- [x] Create `SettingsManager` for input, audio, and gameplay settings
- [x] Add persistent singleton patterns only where appropriate

### 2.2 Input and Player Setup
- [x] Implement first-person player controller reusing starter assets
- [x] Create player input actions asset for movement, look, interact, use tool, menu
- [x] Build `Player/Input/` systems for centralized input reading
- [x] Create `Player/Movement/` scripts for walking and camera look
- [x] Add mouse/keyboard bindings and optional gamepad support
- [x] Ensure input is modular and reusable across player prefabs

### 2.3 Interaction Framework
- [x] Define `IInteractable` interface or equivalent
- [x] Implement raycast-based interaction logic
- [x] Create interaction prompt UI component
- [x] Build reusable interaction code in `Assets/_Project/Scripts/Interaction/`
- [x] Create base `Interactable` component for pickup, repair, and object interaction
- [x] Add input event hooks for `Interact()` and `UseTool()`

### 2.4 Save System Foundation
- [x] Create save data model structure in `Assets/_Project/Scripts/SaveSystem/Data/`
- [x] Implement serialization layer for JSON save files
- [x] Build file storage utilities for save file management
- [x] Add save location handling and platform file paths
- [x] Build save/load events for game state persistence

---

## Phase 3: Multiplayer Foundation

### 3.1 Networking Architecture
- [x] Set up the Unity `NetworkManager` prefab and scene components
- [x] Configure transport and network settings
- [x] Create `Assets/_Project/Scripts/Networking/` for lobby, match, player sync, replication
- [x] Add default network prefab registration
- [x] Define ownership rules and authority model
- [x] Choose server-authoritative behavior for repair and object state

### 3.2 Lobby and Session Flow
- [x] Create a minimal lobby or connection flow in the Bootstrap scene
- [x] Allow host/client connection for 2 players
- [x] Create UI for join/host and connection status
- [x] Add session start logic for multiplayer gameplay

### 3.3 Networked Player Prefab
- [x] Create network-enabled player prefab under `Assets/_Project/Prefabs/Player/`
- [x] Add movement sync and ownership handling
- [x] Add networked camera and input routing
- [x] Sync player states required for multiplayer interaction
- [x] Implement player spawn/despawn logic

### 3.4 Multiplayer Sync Systems
- [x] Create `NetworkRepairable` component for shared repair state
- [x] Create networked tool usage and ability sync
- [x] Implement networked object interaction replication
- [x] Add efficient state updates and avoid unnecessary RPC spam
- [x] Test synchronization with 2 players in an editor/play mode session

---

## Phase 4: MVP Gameplay Systems

### 4.1 House/Housing Systems
- [ ] Create one playable house layout and scene under `Assets/Scenes/StarterHouse/`
- [ ] Design small house interior/exterior with one repair objective inside
- [ ] Configure colliders, navigation, and environment details
- [ ] Add `House` systems for damage state and repair tracking
- [ ] Implement house value and contract completion state

### 4.2 Repair System
- [ ] Build a modular repair mechanic architecture
- [ ] Support at least one repairable wall or object
- [ ] Design repair progress state, visual effects, and completion thresholds
- [ ] Add repair type metadata (drywall, painting, etc.)
- [ ] Create repair tool interaction with progress accumulation
- [ ] Add shared progress so both players contribute and see updated state
- [ ] Implement repair completion triggers and reward callbacks

### 4.3 Tool System
- [ ] Create a basic tool prefab under `Assets/_Project/Prefabs/Tools/`
- [ ] Implement one repair tool (e.g., hammer, drill, paint tool)
- [ ] Create tool stat/config data via `ToolDataSO`
- [ ] Build tool behavior for use, animations, and sounds
- [ ] Implement tool selection or pickup through interaction
- [ ] Ensure tool usage replicates across network if needed

### 4.4 Contract Flow and Economy
- [ ] Create contract data definitions under `Assets/_Project/ScriptableObjects/Jobs/`
- [ ] Build one example contract with a repair objective and reward
- [ ] Implement contract acceptance, objective tracking, and completion logic
- [ ] Add a basic money/currency system in `Assets/_Project/Scripts/Economy/`
- [ ] Tie reward payout to contract completion
- [ ] Display current money and contract info in UI

### 4.5 Vehicle/Truck Setup
- [ ] Add a simple truck or vehicle representation in the starter environment
- [ ] Use the truck as a narrative/context element for job start/end
- [ ] Optionally support entering/exiting or inventory staging
- [ ] Ensure the vehicle is present but not overcomplicated for MVP

### 4.6 UI and Feedback
- [ ] Create HUD with interaction prompts, objective text, and currency display
- [ ] Add feedback for repair progress and completion
- [ ] Implement tool equip/use UI feedback
- [ ] Add save/load menu or button interface
- [ ] Add multiplayer status indicators and connected player names
- [ ] Add contract acceptance and reward notifications

---

## Phase 5: Content and Polish

### 5.1 Scene and Environment Polish
- [ ] Bake lighting or configure runtime lighting for the starter map
- [ ] Add environment details to support immersion without heavy overhead
- [ ] Use placeholder art or simple geometry where needed
- [ ] Ensure the house is playable and visually clear

### 5.2 Audio and Visual Feedback
- [ ] Add tool sound effects and ambient audio
- [ ] Add impact sounds for repair actions
- [ ] Add UI sounds for contract accept/reward and save/load
- [ ] Add particle or decal feedback for repair progress if feasible

### 5.3 Stability and Performance
- [ ] Reduce costly per-frame operations in core systems
- [ ] Keep `Update()` usage minimal and event-driven where possible
- [ ] Use lightweight colliders and limited physics simulation
- [ ] Validate networked object counts and replication load
- [ ] Confirm MVP runs smoothly on target hardware

### 5.4 Testing and Validation
- [ ] Test one-player gameplay flow from start to finish
- [ ] Test two-player multiplayer flow through connection, repair, and reward
- [ ] Confirm save/load restores relevant state
- [ ] Validate contract completion and payout behavior
- [ ] Check input responsiveness and interaction feel
- [ ] Verify scene transitions and bootstrap behavior

---

## Phase 6: Documentation and Handoff

### 6.1 Developer Documentation
- [ ] Document the folder structure and asset conventions
- [ ] Document the interaction system API and repair component usage
- [ ] Document network ownership and sync rules for MVP
- [ ] Document save file structure and relevant JSON schema
- [ ] Add notes for future expansion and trade system extension

### 6.2 Checklist Review
- [ ] Review all MVP tasks and mark completed items
- [ ] Identify any remaining MVP blockers or unresolved issues
- [ ] Create follow-up backlog items for post-MVP work
- [ ] Verify that the vertical slice meets the primary vertical slice goal
- [ ] Capture a short demo or video of MVP gameplay if useful

---

## Detailed Implementation Checklist

### A. Gameplay Architecture
- [ ] Define core gameplay loop in code and data
- [ ] Keep systems modular and decoupled
- [ ] Avoid giant manager classes and hidden dependencies
- [ ] Use `ScriptableObject` assets for tool stats, contract definitions, and material config
- [ ] Implement reusable interfaces like `IInteractable`, `IDamageable`, `ISaveable`

### B. Repair Mechanic Details
- [ ] Create a `RepairTask` or `RepairObjective` class
- [ ] Add `RepairableComponent` for objects that can be repaired
- [ ] Support repair progress from multiple players simultaneously
- [ ] Add visual state changes for damaged, repairing, and repaired
- [ ] Add a repair speed modifier from tools or player stats
- [ ] Add repair quality or success thresholds if applicable
- [ ] Implement failure cases or incomplete repair feedback

### C. Tool Data and Upgrades
- [ ] Create `ToolDataSO` with fields for damage, speed, durability, quality
- [ ] Add a simple upgrade path if time allows (repair speed increase)
- [ ] Implement tool equip/unequip behavior
- [ ] Keep tool interactions simple for MVP and extensible later

### D. House and Contract Systems
- [ ] Build a house contract system with location, objectives, material cost, reward
- [ ] Add objective completion tracking and UI progress
- [ ] Add contract acceptance and job summary displays
- [ ] Add reward payout and currency update on completion
- [ ] Add optional customer satisfaction or inspection state for later expansion

### E. Multiplayer and Networking
- [ ] Design network ownership model: host-authoritative with client input
- [ ] Sync critical state only: repair progress, tool usage, contract completion
- [ ] Avoid client-side-only state for contract or reward logic
- [ ] Test with two clients in a build or editor play mode
- [ ] Add debug display or logs for network synchronization issues

### F. Save/Load Details
- [ ] Save player money and contract state
- [ ] Save house repair progress and completion state
- [ ] Save current active contract if needed
- [ ] Save basic player data for MVP progression
- [ ] Add load validation and fallback for corrupted files

### G. UI and UX
- [ ] Build minimal HUD with currency, contract status, and repair progress
- [ ] Add interaction text for `E` / `Use` prompts
- [ ] Show connected player count or names in multiplayer
- [ ] Add simple pause menu and save/load buttons
- [ ] Provide clear success/failure feedback for contract outcomes

### H. Asset and Prefab Organization
- [ ] Keep prefabs organized under `Assets/_Project/Prefabs/`
- [ ] Keep ScriptableObjects organized under `Assets/_Project/ScriptableObjects/`
- [ ] Keep scripts organized by feature area
- [ ] Avoid gameplay code outside `_Project/` wherever possible
- [ ] Keep Unity scene content lightweight and focused on MVP

---

## Suggested MVP Delivery Checklist

- [ ] Complete a minimal playable map and starter house scene
- [ ] Complete a minimal networked player that can walk and interact
- [ ] Complete a repairable wall or object and one repair tool
- [ ] Complete a working contract with reward payout
- [ ] Complete a working save/load flow
- [ ] Complete a 2-player session where both players can repair together
- [ ] Complete polish on UI, audio, and feedback for MVP feel
- [ ] Validate all major tasks through playtesting

---

## Notes for Future Expansion (Post-MVP)

- Add more repair types: painting, flooring, plumbing, electrical
- Add more contract complexity: deadlines, optional objectives, budgets
- Add house flipping economy and property buying
- Add upgrade trees for tools, vehicles, and business systems
- Add progression anchor with player-owned home customization
- Add more multiplayer systems: lobby, matchmaking, dedicated server support
- Add material quality system and inspection mechanics
- Add procedural contract generation and risk/reward scaling

---

## Document Maintenance

- [ ] Keep this checklist updated as MVP tasks are completed
- [ ] Split large items into work tickets or issues when needed
- [ ] Use the checklist to track progress and avoid scope creep
- [ ] Reconcile this document with actual code and scene structure regularly
