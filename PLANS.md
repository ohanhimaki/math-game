# Project Plans

This document outlines the development plans for the Math Game project.

## Overall Goal: Expand Game Modes

The objective is to create new game modes, specifically focusing on a Spotify Playlist Quiz Game.

## Spotify Playlist Quiz Game (CSV-based Approach)

**Status: ✅ Fully Implemented and Production Ready**

This game mode allows players to guess the release year of songs from a Spotify playlist. The game is created by uploading a CSV file exported from tools like [Chosic Spotify Playlist Exporter](https://www.chosic.com/spotify-playlist-exporter/).

### Implementation Details:

1.  **UI for CSV Upload:**
    *   The `SpotifyQuizPlayer.razor` page has been modified to allow users to upload a CSV file.
    *   Fields for Spotify Playlist ID and Access Token have been removed, replaced by a file input and player name input.

2.  **CSV Parsing:**
    *   A `CsvQuizParser.cs` service has been created to parse the uploaded CSV file.
    *   It reads the CSV (expecting columns like "Song", "Artist", "Album Date", "Spotify Track Id") and maps them to a `Quiz<int>` object.
    *   The `QuizItem<int>` is populated with the song's name, artist, the release year (parsed from "Album Date") as the value, and a Spotify URI (constructed from "Spotify Track Id") for linking.

3.  **Game Creation:**
    *   `SpotifyQuizGameFactory.cs` has been refactored and renamed to `QuizFactory.cs`.
    *   The `QuizFactory` now uses the `CsvQuizParser` to create a `QuizGame<int>` instance from the uploaded CSV content.

4.  **Game UI (`RunQuizGame.razor`):**
    *   The `RunQuizGame.razor` component is used to play the game.
    *   Spotify Web and Desktop buttons allow players to listen to songs.
    *   QR code with MudBlazor theme colors (Primary violet) for easy mobile scanning.
    *   Clean, modern UI with gradient result dialogs (green/red).

### Security Considerations:

*   This CSV-based approach is significantly more secure for a public client-side application.
*   It avoids the need to handle Spotify API credentials (Client ID, Client Secret, Access Tokens) directly within the application's code or configuration for real-time API calls.
*   No sensitive Spotify credentials are stored or used, eliminating the risk of exposure in a public GitHub repository.

### ✅ Completed Features (Nov 2024):

#### Error Handling & Answer Correction
*   ✅ Game master can edit answer values in the result dialog
*   ✅ System automatically rechecks correctness after editing
*   ✅ Cards automatically move between correct/failed lists based on new answer

#### UI Enhancements
*   ✅ Compact plus icons (+) for placement selection (using Secondary green color)
*   ✅ Icons use zero-width positioning to avoid layout shifts
*   ✅ Two-click interaction: first click selects, second click confirms
*   ✅ Proper row alignment with padding for consistent spacing
*   ✅ Bold green/red gradient backgrounds in result dialogs
*   ✅ Large "OIKEIN!"/"VÄÄRIN!" text in result popups
*   ✅ Removed redundant Spotify button from result dialog

#### Keyboard Navigation (Full Implementation)
*   ✅ **Arrow keys (←/→)**: Navigate between placement positions
*   ✅ **Space/Enter**: Confirm placement selection
*   ✅ **Space**: Close result dialog and advance to next player
*   ✅ **W key**: Open Spotify Web
*   ✅ **D key**: Open Spotify Desktop
*   ✅ Visual keyboard hints on buttons (small white chips)
*   ✅ Always-active keyboard controls (no toggle needed)
*   ✅ JavaScript interop with retry logic for reliable initialization

#### Visual Improvements
*   ✅ QR code with MudBlazor Primary color theme (violet)
*   ✅ QR code in white paper frame for better contrast
*   ✅ Keyboard shortcut indicators on all interactive buttons

### Technical Implementation Notes:

**JavaScript Integration:**
*   `keyboard.js` - Global keyboard event handler
*   Loaded before Blazor to ensure availability
*   Supports Space, Enter, Arrow keys, W, D
*   Proper cleanup on component disposal (IAsyncDisposable)

**Component Structure:**
*   Type-safe generic component `RunQuizGame<T>`
*   DotNetObjectReference for JS interop callbacks
*   Retry logic (5 attempts) for JS function availability
*   Proper error handling with JSException

### Future Enhancements:

#### Priority
*   ✅ **QR code dark mode support** - QR code adapts to dark/light theme (Christmas Red on dark/light background)
*   **PWA support** - Progressive Web App to persist game state (continue if browser crashes)
*   **Dynamic preset listing** - Auto-generate preset list at build time (JSON index of wwwroot/spotify-quizzes/)
*   **Sound effects** - Optional audio feedback for correct/wrong answers (with toggle)
*   **CSV template download** - Provide example CSV template for custom quiz creation

#### Completed
*   ✅ Ability to press button on players/teams card to CELEBRATE WINNER (show also all teams ordered)
*   ✅ Show artist + song always as: artist - songname (no songname by artist)
*   ✅ Fix spotify web player link
*   ✅ Configurable initial cards per player (1-10)
*   ✅ Scrollable player list (max-height: 60vh) for better screen utilization
*   ✅ Toggle to hide card names from non-active players (presentation mode)
*   ✅ Hide placement buttons between cards with same value
*   ✅ Preset CSV quiz selector from wwwroot/spotify-quizzes/
*   ✅ Add button in result dialog to search for "release date {artist}-{songname}" to verify release year
*   ✅ Christmas theme with red/green colors and dark mode support

### Recent Updates (2025-11-27):
*   ✅ QR code dark mode support - Christmas Red on black (dark) or white (light)
*   ✅ Dark mode AppBar changed to Christmas Red
*   ✅ Darker green secondary color in dark mode (#0d4020)
*   ✅ Added "Parhaat joulubiisit.csv" to preset quizzes

## Generic Quiz Game (Non-Spotify)

**Status: 🔄 Scaffolded, Needs Development**

The generic quiz game framework already exists (`QuizOrderGame.razor`, `CsvQuizCreator.razor`) but needs enhancement:

*   Support for different quiz types (not just Spotify/music)
*   Generic CSV parser for any comparable data
*   Custom value types (dates, numbers, text)
*   CSV template system for easy quiz creation




