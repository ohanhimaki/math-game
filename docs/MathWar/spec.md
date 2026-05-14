# MathWar

## 🧠 Overview

This feature introduces a new browser-based math strategy game into the existing Blazor WebAssembly application. The game is a **skill-based, real-time decision game** where the player guides a numeric "army" through a sequence of gates.

Each gate presents mathematical operations (e.g. `x2`, `+30`, `/2`), and the player must quickly choose the better option under time pressure.

The core gameplay emphasizes:

* Mental math speed
* Decision-making under uncertainty
* Simple but scalable mechanics

This game will be implemented as a **new page/component** within the existing app.

---

## 🎮 Core Gameplay

### Game Loop

1. Player starts with an initial value (e.g. `100`)
2. The game presents a sequence of gates (e.g. 5–7)
3. Each gate has **2 choices** (math operations)
4. Player must choose within a time limit (e.g. 2 seconds)
5. The selected operation is applied to the current value
6. Continue until all gates are completed
7. Final value determines score and rating

---

## ❗ Design Constraints (IMPORTANT)

* DO NOT show calculated results of gates (player must compute mentally)
* DO NOT auto-select best options
* DO NOT highlight correct answers (in MVP)
* Keep gameplay skill-based, not automated

---

## 🧱 Architecture

### High-Level Components

* `MathWarPage.razor` → main page
* `GameEngine` → game state & logic
* `GateGenerator` → creates gates
* `GameTimer` → handles timing
* `ScoreCalculator` → end scoring

---

## 📦 Data Models

### GameState

```csharp
public class GameState
{
    public int CurrentValue { get; set; }
    public int CurrentGateIndex { get; set; }
    public List<Gate> Gates { get; set; } = new();
    public bool IsGameOver { get; set; }
    public DateTime GateStartTime { get; set; }
}
```

---

### Gate

```csharp
public class Gate
{
    public Operation OptionA { get; set; }
    public Operation OptionB { get; set; }
}
```

---

### Operation

```csharp
public class Operation
{
    public OperationType Type { get; set; }
    public double Value { get; set; }

    public int Apply(int input)
    {
        return Type switch
        {
            OperationType.Add => input + (int)Value,
            OperationType.Subtract => input - (int)Value,
            OperationType.Multiply => (int)(input * Value),
            OperationType.Divide => (int)(input / Value),
            _ => input
        };
    }
}
```

---

### OperationType

```csharp
public enum OperationType
{
    Add,
    Subtract,
    Multiply,
    Divide
}
```

---

## 🎲 Gate Generation

### Goals

* Ensure meaningful decisions (no obvious always-best choices)
* Avoid impossible or trivial sequences
* Keep numbers readable for mental math

---

### Rules (MVP)

* Gate count: 5–7
* Each gate has 2 operations
* Allowed operations:

  * `+10–50`
  * `-10–30`
  * `x1.5`, `x2`
  * `/2`

---

### Example Generator

```csharp
public class GateGenerator
{
    private Random _random = new();

    public List<Gate> Generate(int count)
    {
        var gates = new List<Gate>();

        for (int i = 0; i < count; i++)
        {
            gates.Add(new Gate
            {
                OptionA = RandomOperation(),
                OptionB = RandomOperation()
            });
        }

        return gates;
    }

    private Operation RandomOperation()
    {
        var type = (OperationType)_random.Next(0, 4);

        return type switch
        {
            OperationType.Add => new Operation { Type = type, Value = _random.Next(10, 50) },
            OperationType.Subtract => new Operation { Type = type, Value = _random.Next(10, 30) },
            OperationType.Multiply => new Operation { Type = type, Value = _random.Next(15, 21) / 10.0 }, // 1.5–2.0
            OperationType.Divide => new Operation { Type = type, Value = 2 },
            _ => throw new Exception()
        };
    }
}
```

---

## ⏱️ Timer System

* Each gate has a time limit (e.g. 2 seconds)
* If player does not choose:

  * either auto-pick random
  * or apply penalty (decide during implementation)

---

## 🖥️ UI Requirements

### Display

* Current value (large, centered)
* Two gate options (left/right)
* Countdown timer (visual)

---

### Interaction

* Click or keyboard:

  * Left = Option A
  * Right = Option B

---

### Important

* DO NOT display result previews
* Only show operation (e.g. `x2`, `+30`)

---

## 🏁 End Game & Scoring

### After last gate:

* Show final value
* Show rating (Bronze / Silver / Gold)

---

### Rating Calculation

1. Simulate:

   * Minimum possible value
   * Maximum possible value

2. Calculate thresholds:

```csharp
bronze = min + 0.4 * (max - min)
silver = min + 0.7 * (max - min)
gold   = min + 0.9 * (max - min)
```

---

## 💾 Persistence (LocalStorage)

Store:

* High score
* Best rating

---

### Example

```csharp
public class SaveData
{
    public int HighScore { get; set; }
    public string BestRating { get; set; }
}
```

---

## 🔧 Implementation Tasks

### Phase 1 – Core

* [ ] Create page `MathWarPage`
* [ ] Implement `GameState`
* [ ] Implement `Operation.Apply`
* [ ] Implement `GateGenerator`
* [ ] Render current value + 2 options
* [ ] Handle player input

---

### Phase 2 – Game Flow

* [ ] Implement gate progression
* [ ] Add timer per gate
* [ ] Handle timeout behavior
* [ ] End game logic

---

### Phase 3 – Scoring

* [ ] Simulate min/max paths
* [ ] Implement rating thresholds
* [ ] Display results screen

---

### Phase 4 – Polish

* [ ] Add animations (optional)
* [ ] Improve UI layout
* [ ] Add restart button
* [ ] Save high score to localStorage

---

## 🚀 Future Extensions (NOT MVP)

* Visual representation (balls, colors)
* Roguelike upgrades
* Hint system
* Difficulty scaling
* Daily challenge mode

---

## ✅ Definition of Done

* Player can complete a full run
* Decisions require mental math
* No automation or hints in MVP
* Game works fully client-side
* Runs inside existing Blazor WASM app

---
