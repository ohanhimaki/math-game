# Project Plans

This document outlines the development plans for the Math Game project.

## Overall Goal: Expand Game Modes

The objective is to create new game modes, specifically focusing on a Spotify Playlist Quiz Game.

## Spotify Playlist Quiz Game (CSV-based Approach)

**Status: Implemented and Ready for Testing**

This game mode allows players to guess the release year of songs from a Spotify playlist. The game is created by uploading a CSV file exported from tools like Chosic.
(add link to spotify playlist exporter: https://www.chosic.com/spotify-playlist-exporter/)

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
    *   An "Open in Spotify" button is displayed for each `QuizItem`, which constructs a direct link to the song on Spotify's website using the parsed "Spotify Track Id". This allows players to listen to the song in their Spotify application or web player.

### Security Considerations:

*   This CSV-based approach is significantly more secure for a public client-side application.
*   It avoids the need to handle Spotify API credentials (Client ID, Client Secret, Access Tokens) directly within the application's code or configuration for real-time API calls.
*   No sensitive Spotify credentials are stored or used, eliminating the risk of exposure in a public GitHub repository.

### Next Steps / Future Enhancements:

*   **Error Handling:** Improve error handling -> When user makes guess and popup appears, let game master change answer of card (year value) and recheck if guess was correct. This is because some songs are from new release albums etc
*   **UI Polish:** Enhance the UI for a better user experience, make guess input not take extra space: use plus icons that need to be activated (then icon turns filled ) and after selecting it again (or keyboard enter/space) it makes guess. this makes it easier for game master to as "you mean tihs? Wna lock?" Also those new plus icons: can those be ilmpelemted so that they dont make line content any wider (so maybe higher z value but horizontal minus padding or something)

### Small changes:

- Make "Correct!/Wrong!" popup screen more clear, maybe use background or big banner with green/red. remove "kuuntele spotifyssa" button from that popup




