# AGENTS.md

# Contractor Simulator

Repository:
:contentReference[oaicite:0]{index=0}

---

# Project Overview

Contractor Simulator is a multiplayer co-op construction, repair, and house flipping simulator built in Unity.

Players operate a contracting business where they:
- accept renovation contracts
- inspect and estimate jobs
- repair and remodel houses
- negotiate with customers
- upgrade tools and vehicles
- improve their business reputation
- buy and flip houses for profit
- specialize in multiple construction trades

The game focuses heavily on:
- cooperative multiplayer gameplay
- satisfying repair mechanics
- progression and economy systems
- realistic contractor workflows
- modular repair/build systems

The project is currently in MVP/prototype phase.

---

# Core Gameplay Loop

1. Accept contract
2. Drive to property
3. Inspect damage
4. Give estimate / negotiate price with customer
5. Gather materials and tools
6. Repair / clean / build
7. Pass inspection
8. Earn money
9. Upgrade business
10. Buy & flip houses
11. Unlock larger and more difficult contracts

The gameplay loop should feel:
- satisfying
- progression-focused
- multiplayer-friendly
- systemic and replayable

---

# MVP Scope

The MVP intentionally has a small scope.

Current MVP goals:
- One small map
- One playable house
- 1–2 repair mechanics
- 2-player co-op
- Basic money system
- Save/load functionality
- One truck
- Basic tools

The MVP should prioritize:
- proving the core gameplay loop
- multiplayer synchronization
- interaction feel
- fast iteration speed

The MVP should NOT prioritize:
- massive content volume
- advanced graphics
- procedural generation
- large open worlds
- advanced AI systems

---

# Current Project Goals

## Completed
- [x] Install latest stable Unity LTS
- [x] Create Git repository

## In Progress
- [ ] Set up folders
- [ ] Install packages
- [ ] Configure input system
- [ ] Configure URP/HDRP
- [ ] Create prototype scene

---

# Primary Vertical Slice Goal

The first playable vertical slice should support:

- Two players connected
- Shared multiplayer house
- One repairable wall
- One repair tool
- Shared repair progress
- Reward payout
- Basic save/load
- One complete repair contract

This is the highest current development priority.

---

# Key Features

## Multiplayer Co-op
The game is designed primarily around cooperative gameplay.

Requirements:
- synchronized repair systems
- synchronized tools
- synchronized interactions
- multiplayer-safe save systems
- shared objectives/contracts

Preferred architecture:
- server authoritative where practical
- lightweight replication
- modular networked interactions

---

## Upgradable Tools

Players can improve:
- repair speed
- durability
- efficiency
- quality output

Examples:
- better drills
- higher quality hammers
- faster paint rollers
- improved electrical tools

---

## Player-Owned Home

Players own a personal house/base where they:
- remodel rooms
- display upgrades
- socialize with co-op players
- test materials/tools
- store vehicles/equipment

This should become a long-term progression anchor.

---

## Increasing Contract Difficulty

Contracts scale over time:
- larger houses
- more repair types
- tighter budgets
- customer expectations
- inspection strictness

---

## Multiple Trade Systems

The game combines multiple contractor professions:
- electrical
- plumbing
- framing
- drywall
- flooring
- painting
- demolition
- cleanup

Systems should remain modular so new trades can be added later.

---

## Material Quality System

Materials have quality levels.

Higher quality:
- increases inspection success
- improves customer satisfaction
- increases resale value

Lower quality:
- cheaper
- increases customer complaints
- may fail inspections
- may damage reputation

This system is central to the economy and progression loop.

---

## Economy System

The economy should feel:
- realistic
- progression-driven
- risk/reward focused

Includes:
- material costs
- labor profits
- business upgrades
- property value
- customer satisfaction
- inspections
- contract negotiation

Avoid oversimplified arcade economy systems.

---

# Missions / Contracts

Contracts should eventually include:
- inspections
- deadlines
- optional objectives
- budget restrictions
- material restrictions
- customer preferences

Contracts should support multiplayer participation naturally.

---

# Core Assets

## Player Character
- first-person controller
- multiplayer synchronized
- interactable hands/tools
- customizable later

---

# Materials / Visual Assets

The game requires:
- many flooring options
- many paint colors
- drywall materials
- wood stud materials
- repair decals
- dirt/grime textures
- damage states

Player-interactable objects should support:
- separate textures/material states
- visible repair progress
- material swapping

---

# Core Tools

Initial tools:
- Drill
- Hammer
- Nail
- Screw
- Pliers
- Wrench

Future tools:
- paint roller
- circular saw
- demolition hammer
- pressure washer
- tile cutter

Tools should:
- support upgrades
- synchronize in multiplayer
- provide satisfying feedback
- use modular interaction systems

---

# Unity Version

Recommended:
- Unity LTS
- URP (Universal Render Pipeline)

Core technologies:
- C#
- Unity Input System
- Netcode for GameObjects

---

# Architecture Principles

Core principles:
- modular systems
- composition over inheritance
- scalable multiplayer-safe architecture
- data-driven gameplay
- avoid hardcoded values
- maintain readability and iteration speed

The project should remain:
- easy to extend
- easy to debug
- easy for AI agents to modify safely

---

# Folder Structure Philosophy

Important directories:

## Assets/_Project/
Contains all project-specific gameplay code and systems.

## Assets/_Project/Scripts/
Contains all gameplay logic.

## Assets/_Project/ScriptableObjects/
Contains configurable gameplay data.

## Assets/Scenes/
Contains Unity scenes.

## Assets/Prefabs/
Contains reusable prefab assets.

Avoid placing gameplay logic outside `_Project`.

---

# Coding Standards

## General Rules

Prefer:
- small focused classes
- explicit responsibilities
- readable code
- modular systems

Avoid:
- giant god objects
- hidden dependencies
- unnecessary abstractions
- premature optimization

---

# Naming Conventions

## Classes
Use:
```csharp
PlayerMovement
RepairTask
HammerTool
NetworkRepairable
```

Avoid:
```csharp
ManagerScript
ThingHandler
RepairStuff
```

---

## Interfaces
Prefix interfaces:
```csharp
IInteractable
IDamageable
ISaveable
```

---

## Serialized Fields
Use:
```csharp
[SerializeField] private float repairSpeed;
```

---

## ScriptableObjects
Suffix:
```csharp
ToolDataSO
JobDataSO
MaterialDataSO
```

---

## Prefabs
Prefix:
```text
PF_Player
PF_Hammer
PF_RepairSpot
PF_Truck
PF_StarterHouse
```

---

# Interaction System

The interaction system is foundational.

Requirements:
- raycast interaction
- interface-driven
- multiplayer-safe
- reusable across systems

Example:
```csharp
public interface IInteractable
{
    void Interact(PlayerInteraction player);
}
```

---

# Repair System

Repair systems should:
- support multiple repair types
- synchronize over network
- expose visual progress
- remain modular/extensible

Repair categories:
- drywall
- painting
- flooring
- plumbing
- electrical
- framing

---

# Networking Rules

Multiplayer stability is critical.

Requirements:
- explicit ownership rules
- synchronized repair progress
- synchronized tool usage
- synchronized object states
- deterministic interactions where practical

Avoid:
- hidden local-only state
- unnecessary RPC spam
- client-authoritative critical systems

---

# Save System

Save/load should support:
- player progression
- inventory
- house states
- completed contracts
- business upgrades
- player-owned house customization

Preferred save format:
- JSON during development

---

# Performance Guidelines

Construction simulators become CPU-heavy quickly.

Prioritize:
- event-driven systems
- minimal Update() usage
- object pooling
- lightweight colliders
- efficient network replication

Avoid:
- excessive physics checks
- expensive per-frame allocations
- unnecessary network syncs

---

# Scene Rules

Scenes should remain lightweight.

Scenes should contain:
- layout
- baked lighting
- environment setup

Avoid embedding gameplay state directly into scenes.

Preferred scenes:
- Bootstrap
- MainMenu
- Lobby
- TestScene
- StarterHouse

---

# Bootstrap Scene

The Bootstrap scene initializes:
- managers
- networking
- save systems
- audio systems
- persistent systems

Only one bootstrap entry point should exist.

---

# Managers

Managers should remain small and focused.

Allowed examples:
- GameManager
- AudioManager
- SaveManager
- SceneLoader
- NetworkManager

Avoid:
- giant multi-purpose managers
- unrelated responsibilities combined together

---

# ScriptableObject Usage

Use ScriptableObjects for:
- tool stats
- material quality
- contract definitions
- house data
- upgrade trees
- economy tuning

Avoid hardcoded gameplay values.

---

# AI Agent Instructions

When generating code:
- prioritize readability
- maintain multiplayer compatibility
- avoid breaking prefab references
- preserve save compatibility where practical
- prefer modular systems
- avoid unnecessary dependencies

When creating systems:
- keep systems isolated
- use interfaces where practical
- support future extensibility

When modifying gameplay:
- preserve the core gameplay loop
- preserve multiplayer synchronization
- preserve data-driven architecture

---

# Recommended Initial Development Order

1. Folder structure
2. Input system
3. FPS player controller
4. Multiplayer connection
5. Interaction raycast
6. Pickup system
7. Repairable wall
8. Repair tool
9. Shared repair synchronization
10. Economy reward
11. Save/load system
12. Basic contract flow

---

# Git Rules

DO:
- commit small focused changes
- use descriptive commit names
- maintain clean prefabs
- organize scenes carefully

DO NOT:
- commit Library/
- commit Temp/
- commit generated builds

Use Unity .gitignore.

---

# Long-Term Vision

Possible future systems:
- advanced house flipping economy
- property purchasing
- customer reputation
- specialized contractor roles
- advanced demolition
- vehicles/driving
- Steam multiplayer
- matchmaking
- Steam Workshop/modding
- dedicated servers
- procedural contracts

Architecture should remain scalable enough to support future expansion without major rewrites.

---