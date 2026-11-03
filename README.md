# Work Hour Counter

Windows desktop app (C# WinForms) for tracking work time in **15-minute** increments.

Click quarter-hour cells on a 24-hour grid, save a local `.dat` file, browse days, and export totals for day / week / month / year.

## Requirements

- Windows
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472) (or newer 4.x that can run a 4.7.2 app)
- [Visual Studio](https://visualstudio.microsoft.com/) with the **.NET desktop development** workload (for building from source)

## Build and run

1. Open `Work Hour Counter.sln` in Visual Studio.
2. Build **Debug** or **Release** (Any CPU).
3. Run the project (`F5` / Start), or launch:

   `Work Hour Counter\bin\Release\Work Hour Counter.exe`

Command-line (Developer Command Prompt / MSBuild on PATH):

```bat
msbuild "Work Hour Counter.sln" /p:Configuration=Release
```

## Usage

1. **Open** an existing `.dat` file, or start marking time and **Save As**.
2. Toggle gray/red cells for each 15-minute slot (hour labels select/deselect a whole hour).
3. Use the date picker or **Previous Day** / **Next Day** to move between days (you will be prompted to save if the day changed).
4. **Export** writes a text summary for the selected range (day / week / month / year), optionally including nested day/week/month lines via the checkboxes.

Data stays on disk in the file you choose; the app does not upload timesheets.

### `.dat` line format

One line per day:

```text
dd/MM/yyyy <96 chars of 0 or 1>
```

The 96 bits are hour 0–23 × four quarters (`00`, `15`, `30`, `45`). A `1` means that slot was worked. Total hours for a day = (count of `1`) / 4.

## Agent / contributor notes

See [AGENTS.md](AGENTS.md) for layout, toolchain, and PR conventions.
