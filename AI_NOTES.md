# AI_NOTES.md

## AI Tools Used
- GitHub Copilot
- ChatGPT (via VS Code Copilot Chat)

## Most Helpful Prompts
1. "Design a .NET backend to read dates from a file, parse multiple formats, and handle invalid dates gracefully."
2. "How to fetch and cache weather data from Open-Meteo API in C#?"
3. "Show me how to build a Blazor WebAssembly UI that calls a backend API and displays data in a table."

## Example of Incorrect AI Suggestion
- Copilot suggested using DateTime.Parse for all date formats, which failed for some custom formats (e.g., 'Jul-13-2020'). I detected this by testing with the provided dates and seeing parse errors. I corrected it by using DateTime.TryParseExact with multiple formats and a fallback to DateTime.TryParse.

## Where I Wrote Code Myself
- I wrote the CORS configuration and error handling logic manually to ensure the backend API could be accessed by the Blazor frontend and that all error cases were handled gracefully. I also manually structured the solution folders and project setup for clarity and maintainability.

## Reasoning
AI tools accelerated boilerplate and repetitive code, but I reviewed, tested, and adjusted logic for edge cases, configuration, and integration to ensure robustness and clarity.
