# Petrichor: Apotheosis

[Русская версия](README.ru.md)

**Petrichor: Apotheosis** is a persistent meta-progression mod for **Risk of Rain 2**. It extends long-term character growth between runs while preserving the core roguelike loop.

> Current version: **0.8.0** — public rebrand of Petrichor Legacy 0.7.1.

## Core systems

- **Legacy** — the first ten meta-levels form an early progression route before class development.
- **7 classes** — Guardian, Slayer, Pathfinder, Seeker, Arcanist, Technomancer and Commander.
- **21 specializations** — three distinct specializations for every class.
- **Arsenal** — choose persistent starting items inside a limited capacity budget. Class and specialization capstone rewards are separate and do not consume Arsenal capacity.
- **Mastery** — infinite class progression with diminishing returns, personal discipline milestones and class-wide effective-rank milestones.
- **Perfection** — a long-term three-path constellation: Triumph, Endurance and Expedition.
- **Prestige** — automatic infinite progression every 100 meta-levels.
- **Special monsters** — rarity tiers, safe combat items, compact overhead indicators and mechanical affixes that change positioning, target priority and combat timing.
- **Persistent progression** — meta level, XP, talents, Mastery, Perfection and Arsenal selections persist between runs.

## Progression

`Legacy (1–10) → Class → Specialization → Mastery (41+) → Perfection (every 25 levels) → Prestige (every 100 levels) → ∞`

### Mastery

Each class has four endless disciplines. Raw ranks use diminishing returns:

- 1–25: 100%
- 26–50: 75%
- 51–100: 50%
- 101–200: 30%
- 201+: 20%

Each discipline also has personal thresholds at **10 / 25 / 50 / 100** ranks. Class-wide milestones are based on the sum of **effective** ranks, preventing a single-stat dump from being the fastest route through the whole class.

### Perfection

Perfection grants one point every 25 levels. Its three paths can be mixed:

- **Triumph** — bosses, special monsters and offensive tempo.
- **Endurance** — recovery, barriers and survival.
- **Expedition** — Arsenal capacity, meta-XP and economy.

### Prestige

Prestige is automatic: every 100 meta-levels grants one Prestige rank. A five-step cycle repeats indefinitely, giving restrained long-term bonuses to special-monster XP, starting gold, special-monster gold, boss/special damage and occasional Arsenal capacity.

## Special monsters

Special monsters use rarity tiers separate from vanilla `EliteDef`. Higher rarities gain more combat items and affixes, but difficulty is intentionally shifted away from pure health inflation toward mechanics such as teleports, delayed attacks, group support, phase windows, barriers and positional threats.

Their overhead UI stays compact: item icons and affix codes are displayed in a single row with tooltips rather than large text blocks.

## Installation

1. Install BepInEx 5 for Risk of Rain 2.
2. Download a packaged release or build the project.
3. Copy the `PetrichorApotheosis` folder to `BepInEx/plugins/`.
4. Launch the game and press **M** to open the progression interface.

Current target environment:

- Risk of Rain 2 `1.5.0#1224`
- BepInEx `5.4.21`
- `RiskOfRain2.GameLibs 1.5.0-r.1`
- `MMHOOK.RoR2 2026.10.8`

R2API is not required. Newtonsoft.Json is bundled in packaged builds.

## Save compatibility

The public name changed from **Petrichor Legacy** to **Petrichor: Apotheosis** in 0.8.0.

For compatibility, the internal BepInEx GUID remains:

`com.petrichorlegacy.metaprogression`

The existing save directory also remains `PetrichorLegacy`. This is intentional so existing 0.7.x progression continues to load after the rebrand.

## Multiplayer status

The progression layer is currently host/single-player authoritative. Full synchronization of all special-monster affix presentation for non-host clients remains future work.

## Building

```bash
dotnet restore src/PetrichorApotheosis/PetrichorApotheosis.csproj
dotnet build src/PetrichorApotheosis/PetrichorApotheosis.csproj -c Release --no-restore
```

GitHub Actions automatically builds and packages the mod on pushes and pull requests.

## Development status

The project is under active development. Balance, UI, enemy affixes and long-term progression may change as gameplay testing continues.

## License

No open-source license has been selected yet. All rights are reserved unless a license is added later.
