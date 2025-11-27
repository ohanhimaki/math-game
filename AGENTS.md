# AGENTS.md - Project Context for AI Agents

**Last Updated:** 2025-11-27  
**Project:** MathGame.Web - Multi-Game Web Application  
**Technology Stack:** Blazor WebAssembly, C#, MudBlazor, .NET 9

---

## Project Overview

MathGame.Web is a Blazor WebAssembly application that hosts multiple interactive quiz games. The primary focus is on the **Spotify Playlist Quiz Game**, where players guess the release year of songs from Spotify playlists.

**Key Statistics:**
- ~1,841 lines of code (C# + Razor)
- 7 Razor pages
- 4 Razor components
- 17 C# classes/services
- 2 JavaScript interop files

---

## Architecture

### Technology Stack
- **Frontend:** Blazor WebAssembly (C# in browser)
- **UI Framework:** MudBlazor (Material Design components)
- **State Management:** Scoped services with DI
- **Data Format:** CSV files (exported from Spotify playlists)
- **JS Interop:** Custom keyboard handlers

### Project Structure
```
MathGame.Web/
├── Pages/                    # Routable pages
│   ├── SpotifyQuizPlayer.razor    # Main game entry point
│   ├── CsvQuizCreator.razor       # Alternative creator
│   ├── QuizOrderGame.razor        # Order-based quiz
│   ├── MathGamePage.razor         # Math game mode
│   └── Home.razor                 # Landing page
├── Components/               # Reusable components
│   ├── RunQuizGame.razor          # Core game component (~780 lines)
│   ├── RunMathGame.razor          # Math game component
│   ├── SelectGame.razor           # Game mode selector
│   └── ShowMathGameNumber.razor   # Math display
├── Models/
│   ├── Quiz/                 # Quiz domain models
│   │   ├── Quiz.cs               # Quiz<T>, QuizItem<T>
│   │   ├── QuizGame.cs           # Game state, Player<T>
│   │   ├── QuizFactory.cs        # Factory for game creation
│   │   ├── CsvQuizParser.cs      # CSV → Quiz<int> parser
│   │   └── SpotifyTrack.cs       # CSV record mapping
│   ├── Spotify/              # Spotify API models (legacy)
│   └── Game/                 # Math game models
├── Services/
│   ├── SpotifyService.cs     # Spotify API client (legacy)
│   └── QuizService.cs        # Quiz management
├── wwwroot/
│   ├── js/
│   │   ├── keyboard.js       # Global keyboard event handler
│   │   └── download.js       # File download helper
│   └── spotify-quizzes/      # Preset CSV quiz files
│       ├── suomi-musaa.csv
│       ├── hitster-suomi.csv
│       └── Parhaat joulubiisit.csv
├── Program.cs                # DI configuration
└── appsettings.json          # Configuration
```

---

## Core Features

### 1. Spotify Playlist Quiz Game (Primary Feature)

**Game Flow:**
1. User selects a preset quiz from dropdown OR uploads custom CSV file
2. Preset quizzes loaded from `wwwroot/spotify-quizzes/` via HttpClient
3. CSV is parsed into `QuizItem<int>` objects (song name, artist, year, Spotify URI)
4. Players configure initial cards per player (1-10, default: 1)
5. Players take turns placing cards in chronological order
6. System validates placement and awards points for correct answers

**Key Components:**
- **SpotifyQuizPlayer.razor**: Entry point, preset selector & file upload
- **RunQuizGame<T>.razor**: Generic game runner (supports any comparable type)
- **CsvQuizParser**: Parses CSV with columns: Song, Artist, Album Date, Spotify Track Id

**Game Mechanics:**
- **Players**: Multiple players/teams supported (comma-separated names)
- **Initial Cards**: Configurable 1-10 cards per player at game start
- **Cards**: Each player builds a timeline of correctly placed songs
- **Validation**: System checks if new card fits between adjacent cards
- **Smart Placement**: Buttons hidden between cards with same value (no point guessing between same years)
- **Failed Cards**: Tracked separately for review
- **Winner Celebration**: Shows leaderboard with celebration song

**User Interactions:**
- **Preset Quizzes**: Radio button to select from `wwwroot/spotify-quizzes/*.csv`
- **Custom Upload**: Alternative radio option for own CSV files
- **Placement Selection**: Click + icons between cards or use keyboard shortcuts
- **Two-click confirmation**: First click selects, second confirms (keyboard: select position then Space/Enter)
- **Keyboard shortcuts**:
  - `0-9`: Jump to placement position (0=first, 9=last or closest available)
  - `←/→`: Navigate placement positions
  - `Space/Enter`: Confirm placement / Close dialog / Next player
  - `W`: Open Spotify Web
  - `D`: Open Spotify Desktop
- **Visual Position Indicators**: White numbered labels (0-9) shown on placement buttons for quick reference
- **Value Editing**: Game master can correct release years in result dialog
- **Spotify Integration**: QR codes + buttons to play songs
- **Search Helper**: Button to Google search "release date {artist} - {song}"
- **Presentation Mode**: Toggle to hide card names from non-active players (TV presentation)

### 2. Winner Celebration Feature

**Configuration** (appsettings.json):
```json
{
  "CelebrationSong": {
    "TrackId": "0E00H4qhjNmcNsuR4GQv25"
  }
}
```

**Features:**
- Button on each player card: "Juhli voittajaa!"
- Dialog shows all players ordered by score
- Winner highlighted with gold gradient and trophy icon 🏆
- Links to play celebration song (Karri Koira - Suomen mestari)

### 3. UI/UX Features

**Visual Design:**
- MudBlazor Material Design components
- **Christmas Theme**:
  - Primary: Christmas Red (#c41e3a) - Used in AppBar and accents
  - Secondary: Christmas Green (#165b33 light, #0d4020 dark)
  - Dark mode: Red AppBar with dark backgrounds
  - Light mode: Red AppBar with light backgrounds
- QR codes with dynamic colors (Christmas Red on black/white based on mode)
- Gradient dialogs: Green for correct, Red for wrong

**Responsive Layout:**
- 3-column grid: Quiz info, Current song, QR code
- Scrollable player list (max-height: 60vh) - important for TV/projector display
- Horizontal scrolling card timeline per player
- Active player highlighted with border + chip
- Top info and bottom buttons always visible

**Presentation Mode:**
- Toggle switch: "Piilota kappaleiden nimet muilta pelaajilta"
- When enabled: Only active player sees card names
- Other players see only years (perfect for TV display)
- Prevents spoilers when displaying on shared screen

**Accessibility:**
- Visual keyboard hints (small white chips on buttons)
- Numbered position indicators (0-9) for quick verbal reference ("SPOT 3!")
- Always-active keyboard controls with number key shortcuts
- Color-coded feedback (green/red)
- Large text sizes for TV viewing (150px cards, Typo.h3 for answer text/values)
- High contrast white indicator on selected placement position

---

## Key Technical Implementation Details

### Generic Type System
```csharp
public class QuizGame<T> where T : IComparable<T>
public class QuizItem<T> where T : IComparable<T>
public class Player<T> where T : IComparable<T>
```
- Supports any comparable value type (int for years, string for text, etc.)
- Type-safe comparison logic

### CSV Parsing
**Expected Format:**
```csv
Song,Artist,Album Date,Spotify Track Id
Song Name,Artist Name,YYYY-MM-DD,trackid123
```

**Parsing Logic:**
- Extracts year from `Album Date` (first 4 chars)
- Constructs text as `"{Artist} - {Song}"`
- Builds Spotify URI as `spotify:track:{Spotify_Track_Id}`

### JavaScript Interop

**keyboard.js:**
- Global event listener for keyboard shortcuts
- DotNetObjectReference for C# callbacks
- Prevents default behavior for game keys
- Proper cleanup on component disposal

**Integration:**
```csharp
@inject IJSRuntime JSRuntime
@implements IAsyncDisposable

protected override async Task OnAfterRenderAsync(bool firstRender)
{
    _dotNetRef = DotNetObjectReference.Create(this);
    await JSRuntime.InvokeVoidAsync("setupKeyboardListener", _dotNetRef);
}

[JSInvokable]
public void HandleKeyPress(string key) { /* ... */ }

public async ValueTask DisposeAsync()
{
    await JSRuntime.InvokeVoidAsync("removeKeyboardListener");
    _dotNetRef?.Dispose();
}
```

### State Management

**Game State:**
- `CurrentItem`: Active question card
- `CurrentPlayerIndex`: Turn tracking
- `AvailableQuestions`: Remaining cards
- `Players[].Cards`: Correctly placed cards (sorted)
- `Players[].FailedCards`: Incorrect guesses

**Dialog States:**
- `_showResultDialog`: Answer feedback
- `_showFailedHistoryDialog`: Review failed cards
- `_showWinnerDialog`: Celebration screen
- `_isEditingValue`: Value correction mode

**Placement Selection:**
- `_activePlacementKey`: Currently selected position
- `_currentPlacementIndex`: Keyboard navigation index
- `_availablePlacements`: List of valid placement positions

---

## Configuration

### appsettings.json
```json
{
  "AppBasePath": "/test/",
  "CelebrationSong": {
    "TrackId": "0E00H4qhjNmcNsuR4GQv25"
  }
}
```

### Dependency Injection (Program.cs)
```csharp
builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<SpotifyService>();
builder.Services.AddScoped<CsvQuizParser>();
builder.Services.AddScoped<QuizFactory>();
builder.Services.AddMudServices();
```

---

## Development History & Design Decisions

### Security Considerations
- **CSV-based approach**: Avoids exposing Spotify API credentials in public client
- **No server-side API calls**: All processing in browser
- **No authentication required**: Public-facing game

### Evolution
1. **Phase 1**: Direct Spotify API integration (abandoned for security)
2. **Phase 2**: CSV-based approach (current implementation)
3. **Phase 3**: Enhanced UX with keyboard shortcuts and visual polish

### Completed Features (from PLANS.md)
- ✅ CSV file upload and parsing
- ✅ Preset quiz selector from wwwroot/spotify-quizzes/
- ✅ Configurable initial cards per player (1-10)
- ✅ Multi-player support
- ✅ Spotify integration (Web + Desktop links)
- ✅ QR codes for mobile scanning with dark mode support
- ✅ Answer value editing by game master
- ✅ Full keyboard navigation
- ✅ Failed cards history tracking
- ✅ Winner celebration dialog with leaderboard
- ✅ Google search helper for release dates
- ✅ Smart placement (no buttons between same values)
- ✅ Scrollable player list for better screen space
- ✅ Presentation mode (hide card names toggle)
- ✅ Christmas theme with red/green colors

---

## Common Patterns & Conventions

### Naming
- **Finnish UI text**: "Juhli voittajaa!", "Oikein!", "Väärin!"
- **Component parameters**: PascalCase with `[Parameter]` attribute
- **Private fields**: `_camelCase` with underscore prefix

### Code Style
- **Minimal comments**: Code is self-documenting
- **Compact syntax**: Blazor inline C# with `@` prefix
- **MudBlazor naming**: `Typo.h5`, `Color.Primary`, `Variant.Filled`

### Error Handling
- **Snackbar notifications**: For user-facing errors
- **Console.WriteLine**: For debugging
- **Try-catch**: Parsing errors return null, UI shows fallback

---

## Testing & Deployment

### Build
```bash
dotnet build
```

### Run (Development)
```bash
cd MathGame.Web
dotnet run
```

### Deployment
- Blazor WebAssembly outputs to `wwwroot`
- Static files can be hosted on any web server
- Current deployment: `/test/` path (see `AppBasePath`)

---

## Working with AI Agents

### Project Context
- **Language**: C# 12, .NET 9, Blazor WebAssembly
- **UI Framework**: MudBlazor (prefer MudComponents over raw HTML)
- **State**: Component-level state, no global state management
- **Data flow**: Props down, events up (Blazor standard)

### When Making Changes
1. **Read PLANS.md first**: Understand completed vs. pending features
2. **Preserve working code**: Only modify what's necessary
3. **Follow Finnish UI text**: Keep user-facing text in Finnish
4. **Test keyboard shortcuts**: Ensure interop still works
5. **Update PLANS.md**: Mark completed features with ✅

### Common Tasks
- **Adding new quiz type**: Extend `QuizItem<T>` generic, create parser
- **Adding preset quiz**: Place CSV in `wwwroot/spotify-quizzes/` and add filename to `_presetQuizzes` list in SpotifyQuizPlayer.razor
- **UI changes**: Use MudBlazor components, maintain responsive layout
- **Keyboard shortcuts**: Update `keyboard.js` and `HandleKeyPress()`
- **Configuration**: Add to `appsettings.json`, inject `IConfiguration`

### Files to Know
- **RunQuizGame.razor**: ~800 lines, core game logic - tread carefully
- **SpotifyQuizPlayer.razor**: Preset selector and file upload logic
- **CsvQuizParser.cs**: CSV → Quiz<int> conversion
- **QuizGame.cs**: Game state, player logic, initial cards configuration
- **keyboard.js**: Global keyboard event handling

---

## External Dependencies

### Required Tools
- [Chosic Spotify Playlist Exporter](https://www.chosic.com/spotify-playlist-exporter/)
  - Exports playlists to CSV with required columns
  - Free to use, no authentication needed
- **Optional**: Users can upload custom CSV files following the same format

### NuGet Packages
- MudBlazor (UI components)
- CsvHelper (CSV parsing)
- Microsoft.AspNetCore.Components.WebAssembly

---

## Known Issues & Limitations

1. **Spotify Links**: Desktop URIs may not work on all platforms
2. **CSV Format**: Tightly coupled to Chosic export format
3. **Browser Compatibility**: Requires modern browser with WebAssembly support
4. **Preset List**: Hardcoded in component, not dynamically scanned from filesystem

---

## Future Considerations

- Dynamic preset quiz discovery (scan wwwroot/spotify-quizzes/ automatically)
- Alternative CSV formats support
- Offline play support (PWA)
- Score persistence (local storage)
- Game replay/history
- Additional game modes (math games already scaffolded)
- Sound effects for game actions

---

## Quick Reference Commands

### Find a feature
```bash
grep -r "Juhli voittajaa" MathGame.Web/
```

### Count game components
```bash
find MathGame.Web/Components -name "*.razor" | wc -l
```

### Check configuration
```bash
cat MathGame.Web/appsettings.json
```

---

**For AI Agents:** This document provides comprehensive context about the project. Always reference PLANS.md for feature status. Maintain Finnish UI text and MudBlazor design patterns. The RunQuizGame component is the most complex (~780 lines) - make surgical changes only.
