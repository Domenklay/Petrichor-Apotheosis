# Petrichor: Apotheosis

[Русская версия](README.ru.md)

**Petrichor: Apotheosis** is an experimental persistent meta-progression mod for **Risk of Rain 2**. It adds long-term progression between runs without replacing the core roguelike loop.

> Current development version: **0.8.0** (rebrand of Petrichor Legacy 0.7.1)

## Core systems

- **Legacy** — the first ten levels form an early progression route before class development.
- **7 classes** — Guardian, Slayer, Pathfinder, Seeker, Arcanist, Technomancer and Commander.
- **21 specializations** — three specializations for every class.
- **Arsenal** — choose persistent starting items within a limited capacity budget. Class and specialization capstone rewards do not consume Arsenal capacity.
- **Mastery** — infinite class-specific progression with diminishing returns, discipline milestones and class-wide milestones.
- **Perfection** — a long-term three-path constellation: Triumph, Endurance and Expedition.
- **Prestige** — automatic infinite progression every 100 meta-levels.
- **Special monsters** — rarity tiers, combat items, compact overhead indicators and mechanical affixes that change how enemies are fought.
- **Persistent progression** — meta level, XP, talents, Mastery, Perfection and Arsenal selections persist between runs.

## Progression structure

`Legacy (1–10) → Class → Specialization → Mastery (41+) → Perfection (every 25 levels) → Prestige (every 100 levels) → ∞`

## Installation

1. Install BepInEx 5 for Risk of Rain 2.
2. Build the project or download a packaged release.
3. Copy the `PetrichorApotheosis` folder into `BepInEx/plugins/`.
4. Launch the game and press **M** to open the progression interface.

The project currently targets Risk of Rain 2 `1.5.0#1224`, BepInEx `5.4.21`, `RiskOfRain2.GameLibs 1.5.0-r.1` and `MMHOOK.RoR2 2026.10.8`.

## Save compatibility

The public mod name changed from **Petrichor Legacy** to **Petrichor: Apotheosis** in version 0.8.0. For compatibility, the internal BepInEx GUID and the legacy save directory are intentionally kept unchanged. Existing progression should therefore continue to load after the rebrand.

## Multiplayer status

The progression layer is currently host/single-player authoritative. Full client synchronization of every special-monster affix indicator is still an area for further work.

## Building

```bash
dotnet restore src/PetrichorApotheosis/PetrichorApotheosis.csproj
dotnet build src/PetrichorApotheosis/PetrichorApotheosis.csproj -c Release --no-restore
```

GitHub Actions also builds and packages the mod automatically.

## Development status

The mod is under active development. Balance values, UI layout, enemy affixes and long-term progression are expected to continue changing based on gameplay testing.

## License

No license has been selected yet. All rights are reserved unless a license is added later.
