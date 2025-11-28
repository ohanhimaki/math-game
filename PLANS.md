# Project Plans

This document outlines the development plans for the Math Game project.

## Overall Goal: Expand Game Modes

The objective is to create new game modes, specifically focusing on a Spotify Playlist Quiz Game.

## Spotify Playlist Quiz Game (CSV-based Approach)

**Status: ✅ Fully Implemented and Production Ready - Complete HITSTER-style gameplay**

This game mode allows players to guess the release year of songs from a Spotify playlist. The game is created by uploading a CSV file exported from tools like [Chosic Spotify Playlist Exporter](https://www.chosic.com/spotify-playlist-exporter/). Includes full HITSTER mechanics: ryöstö cards (challenge tokens), decade guessing for beginners, and artist/song name bonus guessing.

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
    *   The `RunQuizGame.razor` component is used to play the game (~1,190 lines).
    *   Spotify Web and Desktop buttons allow players to listen to songs.
    *   QR code with Christmas Red theme colors for easy mobile scanning.
    *   Clean, modern UI with gradient result dialogs (green/red).
    *   Artist/song guess checkbox integrated into result dialog.
    *   Full HITSTER gameplay mechanics implemented.

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
*   ✅ **Number keys (0-9)**: Jump directly to placement position (0=first position, 9=last/nearest)
*   ✅ **Arrow keys (←/→)**: Navigate between placement positions
*   ✅ **Space/Enter**: Confirm placement selection
*   ✅ **Space**: Close result dialog and advance to next player
*   ✅ **W key**: Open Spotify Web
*   ✅ **D key**: Open Spotify Desktop
*   ✅ Visual position indicators (0-9 white labels) on placement buttons
*   ✅ Visual keyboard hints on buttons (small white chips)
*   ✅ High contrast white indicator on selected position
*   ✅ Always-active keyboard controls (no toggle needed)
*   ✅ JavaScript interop with retry logic for reliable initialization

#### Visual Improvements
*   ✅ QR code with MudBlazor Primary color theme (violet) → Christmas Red
*   ✅ QR code in white paper frame for better contrast
*   ✅ Keyboard shortcut indicators on all interactive buttons
*   ✅ Numbered position indicators (0-9) for verbal callouts ("SPOT 3!")
*   ✅ Large answer text (Typo.h3) in result dialog for better visibility
*   ✅ Large answer value (Typo.h3, bold) matching text size

### Technical Implementation Notes:

**JavaScript Integration:**
*   `keyboard.js` - Global keyboard event handler
*   Loaded before Blazor to ensure availability
*   Supports Space, Enter, Arrow keys, Number keys (0-9), W, D
*   Proper cleanup on component disposal (IAsyncDisposable)

**Component Structure:**
*   Type-safe generic component `RunQuizGame<T>`
*   DotNetObjectReference for JS interop callbacks
*   Retry logic (5 attempts) for JS function availability
*   Proper error handling with JSException

### Future Enhancements:

#### Priority
*   ✅ **Ryöstö** - Implemented 2025-11-28
    - Each player starts with configurable number of ryöstö cards (default: 2)
    - **Ohita kappale (Skip song)**: Use 1 ryöstö card to skip the current song and draw a new one
    - **Haasta (Challenge)**: Use 1 ryöstö card to challenge opponent's placement. If correct, challenger gets the card
    - **Vaihda korttiin (Trade for card)**: Use 3 ryöstö cards to get the current card without guessing
    - **Ansaitse lisää**: 
      - Players earn 1 ryöstö card for every 5 correct answers
      - Players can shout artist + song before placing - checkbox in result dialog to award card
    - UI shows ryöstö card count for each player
    - Challenge mode: Other players can challenge active player's placement before card is revealed
    - Visual indicators for challenge mode (red buttons, status chips)
*   **PWA support** - Progressive Web App to persist game state (continue if browser crashes)
*   **Dynamic preset listing** - Auto-generate preset list at build time (JSON index of wwwroot/spotify-quizzes/)
*   **Sound effects** - Optional audio feedback for correct/wrong answers (with toggle)
*   **CSV template download** - Provide example CSV template for custom quiz creation

#### Completed
*   ✅ Pelisääntöjen valinta alussa - Implemented 2025-11-28
    - Toggle for enabling/disabling ryöstö cards (challenge system)
    - Adjustable initial ryöstö card count (0-10) when enabled
    - Toggle for enabling/disabling decade guessing for single card
    - Both rules default to enabled for full HITSTER experience
    - All UI elements conditionally rendered based on settings
*   ✅ Vuosikymmen-arvaus kun vain 1 kortti - Implemented 2025-11-28
    - When player has only 1 card, they must guess the decade instead of exact placement
    - System dynamically scans all cards in game to determine available decades
    - Large decade buttons shown (e.g., "1980-luku", "1990-luku", etc.)
    - Yellow warning message explains the special rule
    - Placement buttons hidden during decade guessing
    - Challenge functionality disabled when active player has 1 card
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
*   ✅ **QR code dark mode support** - QR code adapts to dark/light theme (Christmas Red on dark/light background)

### Recent Updates (2025-11-27):
*   ✅ QR code dark mode support - Christmas Red on black (dark) or white (light)
*   ✅ Dark mode AppBar changed to Christmas Red
*   ✅ Darker green secondary color in dark mode (#0d4020)
*   ✅ Added "Parhaat joulubiisit.csv" to preset quizzes
*   ✅ **Number key shortcuts (0-9)** - Jump to placement positions with fallback to nearest
*   ✅ **Visual position indicators** - White numbered labels on placement buttons
*   ✅ **Larger answer display** - Answer text and value increased to Typo.h3 (bold)
*   ✅ **High contrast selection** - Selected placement position shows in white

### Recent Updates (2025-11-28):
*   ✅ **Ryöstökortit** - Skip song (1 card), Challenge opponent (1 card), Trade for card (3 cards)
*   ✅ **Decade guessing when 1 card** - Player must guess decade instead of placement, dynamic decade detection from all cards in game
*   ✅ **Ryöstö card earning** - Players earn 1 ryöstö card every 5 correct answers (works in all modes)
*   ✅ **Artist & song guessing** - Players shout artist + song during turn, checkbox in result dialog to award ryöstö card
*   ✅ **Game rule configuration** - Toggle ryöstö cards and decade guessing at game start

## Generic Quiz Game (Non-Spotify)

**Status: 🔄 Scaffolded, Needs Development**

The generic quiz game framework already exists (`QuizOrderGame.razor`, `CsvQuizCreator.razor`) but needs enhancement:

*   Support for different quiz types (not just Spotify/music)
*   Generic CSV parser for any comparable data
*   Custom value types (dates, numbers, text)
*   CSV template system for easy quiz creation




