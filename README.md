# WeatherApp

A full-stack .NET solution to fetch, store, and display historical weather data using the Open-Meteo API.

## Project Structure

```
WeatherApp/
├── Backend/
│   └── WeatherApp.Api/         # .NET 6+ Web API backend
├── FrontEnd/
│   └── WeatherApp.UI/          # Blazor WebAssembly frontend
├── Shared/
│   ├── dates.txt               # Input dates (various formats)
│   └── weather-data/           # Cached weather data as JSON
├── AI_NOTES.md                 # AI usage documentation
└── README.md                   # This file
```

## Backend (WeatherApp.Api)
- Reads and parses dates from `Shared/dates.txt` (supports multiple formats, handles invalid dates).
- Fetches historical weather data for each valid date from the Open-Meteo API (Dallas, TX).
- Stores results as JSON in `Shared/weather-data/` (avoids repeat API calls).
- Exposes a REST endpoint: `GET /api/weather` returning all results (date, min/max temp, precipitation, error/status).
- Handles network/API failures, empty/missing data, and invalid dates gracefully.

### How to Run Backend
1. Open a terminal in the `Backend/WeatherApp.Api` directory.
2. Run:
   ```
   dotnet run
   ```
3. The API will be available at (for example): `http://localhost:5098/api/weather`

## Frontend (WeatherApp.UI)
- Blazor WebAssembly SPA.
- Calls the backend API to load weather data.
- Displays data in a table (date, min/max temp, precipitation, error/status).
- Shows loading and error states.
- (Optional) Supports sorting/filtering (see code for details).

### How to Run Frontend
1. Open a terminal in the `FrontEnd/WeatherApp.UI` directory.
2. Run:
   ```
   dotnet run
   ```
3. Open the provided local URL in your browser (e.g., `http://localhost:5291`).

## Assumptions
- Both backend and frontend are run locally.
- Backend API URL is set in the frontend code (update if ports differ).
- No authentication required.

## API Reference
- `GET /api/weather` — Returns a list of weather data entries (see backend code for structure).

## AI Usage
See `AI_NOTES.md` for details on how AI tools were used in this project.
