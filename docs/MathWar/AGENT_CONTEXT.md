# MathWar – Agent Context

**Last updated:** 2026-04-18  
**Status:** Playable, multi-level system implemented  
**Route:** `/math-war`

---

## What is MathWar?

Arcade lane game inside the Blazor WASM app. Player starts with value `100` and guides it through gates. Each gate row spans 6 lanes and is split into 2–5 segments with different math operations. Player moves left/right between lanes; when a gate reaches the player row, the active lane's segment operation is applied to the value. After the operational gates, 3 trophy gates show Bronze/Silver/Gold thresholds.

---

## Files

```
MathGame.Web/
├── Pages/MathWarPage.razor                  # All UI + game loop
├── Models/MathWar/
│   ├── Gate.cs                              # Gate + GateSegment classes
│   ├── GameState.cs                         # Player lane, current value, gates list
│   ├── Operation.cs                         # Formula (string) + Label (string) + Apply()
│   └── SaveData.cs                          # LocalStorage: HighScore, BestRating, BestLevel
├── Services/MathWar/
│   ├── GateGenerator.cs                     # Generates gates with difficulty scaling
│   └── ScoreCalculator.cs                   # SimulateMinMax, GetThresholds, GetRating
└── wwwroot/js/mathwar.js                    # mathWarLoadSave / mathWarSaveSave (localStorage)
```

**DI registrations** in `Program.cs`:
```csharp
builder.Services.AddScoped<GateGenerator>();
builder.Services.AddScoped<ScoreCalculator>();
```

**Nav link** in `Layout/MainLayout.razor`:
```razor
<MudNavLink Href="math-war">MathWar</MudNavLink>
```

---

## Architecture

### Key models

```csharp
public class Gate {
    List<GateSegment> Segments;  // 2-5 segments partitioning all 6 lanes
    double YPosition;            // % from top; starts negative, scrolls to 100
    bool Applied;
    // Trophy gate fields:
    bool IsThreshold; string? ThresholdEmoji; int ThresholdValue; string? ThresholdColor;
}

public class GateSegment {
    int StartLane; int EndLane;  // 0–5 inclusive
    Operation Operation;
}

public class Operation {
    string Formula;  // NCalcSync expression, 'x' = player value. e.g. "x*2+100"
    string Label;    // Display: x-first → omit x ("+30","×2"); x elsewhere → show it ("100×x")
    int Apply(int input);  // Math.Clamp(result, -1_000_000_000, 1_000_000_000)
}
```

### NCalcSync
Package: `NCalcSync` v5.12.0. Pure interpreter — no `Reflection.Emit`, works in Blazor WASM.
```csharp
var expr = new Expression(Formula);
expr.Parameters["x"] = (double)input;
return (int)Math.Clamp(Convert.ToDouble(expr.Evaluate()), -1e9, 1e9);
```

---

## Game Loop (MathWarPage.razor)

- **Game loop:** `Task.Delay(33ms)` → move gates by `_scrollSpeed = 0.4%` → `CheckCollisions()` → `StateHasChanged()`
- **Player row:** `_playerRowPct = 80.0` (gate applied when `YPosition >= 80`)
- **Lane count:** 6, each `100/6 ≈ 16.67%` wide
- **Keyboard:** `ArrowLeft` / `ArrowRight` on focused `<div tabindex="0">`

### State machine

| Field | Condition |
|-------|-----------|
| `!_started` | Menu screen |
| `_levelComplete` | Between-levels screen (Jatka →) |
| `_state.IsGameOver` | Game over screen |
| else | Game running |

---

## Multi-level system

- **Value carry-over:** `_state.CurrentValue` passed into next level
- **Ideal start:** `_idealStartValue` = max possible output from previous level (capped at 10,000,000)
- **Thresholds** always calculated from `_idealStartValue`, not the player's actual value → compounding penalty for sub-optimal play
- **Fail condition:** player value < Bronze threshold after last gate → game over
- **Score:** value carried + level reached, saved to localStorage

### Difficulty scaling per level (GateGenerator)

| Level | Gate count | Max segments | Extra multipliers |
|-------|-----------|--------------|-------------------|
| 1 | 5 | 2 | ×1.5, ×2 |
| 2 | 6 | 2 | same |
| 3 | 7 | 3 | + ×3 |
| 4 | 8 | 3 | same |
| 5 | 9 | 4 | + ×4 |
| 6+ | 10 | 5 | max |

Add/subtract ranges also scale: `+N` where `N` up to `10 + level*15` (capped 100), `-N` up to `10 + level*10` (capped 60).

---

## Rating thresholds (ScoreCalculator)

```
Bronze = idealMin + 0.50 * (idealMax - idealMin)
Silver = idealMin + 0.75 * (idealMax - idealMin)
Gold   = idealMin + 0.90 * (idealMax - idealMin)
```

Three **trophy gates** appended after operational gates (full 6-lane width, no operation applied, just show medal + threshold value):
- 🥉 Bronze — `lastGateY - 30`
- 🥈 Silver — `lastGateY - 50`  
- 🥇 Gold   — `lastGateY - 70`

---

## Known issues / bugs fixed

- **Int32 overflow** (fixed): `SimulateMax` across levels with ×4 chains → `Apply()` now clamps to ±1,000,000,000; `_idealStartValue` capped at 10,000,000.

---

## Pending ideas / future work

### Number formatting
- Display large values with suffix: `1 000` → plain; `1 000 000` → `1000k`; `1 000 000 000` → `1B`
- Threshold: only apply suffix at ≥ 1,000,000
- Affects: current value display, trophy gate values, end screen, high score

### Operation scaling for large values
- At higher levels / higher values, `+30` becomes insignificant compared to ×4
- Idea: scale add/subtract amounts proportionally to current idealStartValue range
- E.g., if idealStart > 10,000 → add range becomes `+500–2000` instead of `+10–100`
- Or: introduce percentage-based operations (`+10%`, `-20%`) at higher levels

### Other future ideas
- Visual lane highlight when player moves (color pulse)
- Sound effects
- Difficulty selector (Easy / Normal / Hard modifies thresholds %, not gate generation)
- Endless mode without level structure
- Animated gate collision (flash on player)
- Show running "optimal path" score faintly alongside current value

---

## Adding new formula types

To add a new gate operation, only edit `GateGenerator.cs` — no other files needed:

```csharp
// x-first formula (label omits x):
new Operation { Formula = "x*2+100", Label = "×2+100" }

// Non-x-first (label shows x):
new Operation { Formula = "100*x", Label = "100×x" }

// Percentage-based (future):
new Operation { Formula = "x*1.1", Label = "+10%" }
```

---

## Tech notes

- **Blazor WASM constraint:** no `Reflection.Emit` → use `NCalcSync` not `NCalc.LambdaCompilation`
- **UI framework:** MudBlazor (Material Design)
- **Theme:** Christmas Red (`#c41e3a`) primary, dark backgrounds
- **Gate colors:** even segment index = blue-tinted, odd = amber-tinted
