# Work Hour Counter – Agent Context

Implementation guide for coding agents. Product pitch and user-facing install: [`README.md`](README.md).

---

## Orientation

| Question | Answer |
| -------- | ------ |
| What is this? | Classic **.NET Framework 4.7.2** WinForms app that tracks work in 15-minute slots |
| Entry / UI | `Work Hour Counter/Program.cs` → `MainForm` |
| Domain model | `DataUnit` — one calendar day + 96 binary quarter flags |
| Persistence | Plain-text `.dat` lines (`dd/MM/yyyy` + `0`/`1` string); Open / Save / Save As |
| Toolchain | Visual Studio + MSBuild (non-SDK-style `.csproj`) |
| Tests | None yet — do not invent a test harness unless asked |
| Platform | **Windows only** (WinForms); do not assume Linux/`dotnet` SDK builds |

```text
.
├── Work Hour Counter.sln
├── Work Hour Counter/
│   ├── Program.cs
│   ├── MainForm.cs              # UI logic, I/O, export
│   ├── MainForm.Designer.cs     # designer surface (partial)
│   ├── DataUnit.cs              # day record + totals helpers
│   ├── App.config
│   ├── watch.ico
│   └── Properties/              # AssemblyInfo, Settings, Resources
├── README.md
└── AGENTS.md
```

CI/semver thin workflows live under [`.github/`](.github/) when present (see open/merged PRs adopting [`danielnachumdev/actions`](https://github.com/danielnachumdev/actions) `@latest`). How to adopt: [`HOW-TO-USE.md`](https://github.com/danielnachumdev/actions/blob/v1/HOW-TO-USE.md).

---

## Develop

Open the solution in Visual Studio on Windows, or:

```bat
msbuild "Work Hour Counter.sln" /p:Configuration=Debug
msbuild "Work Hour Counter.sln" /p:Configuration=Release
```

Output: `Work Hour Counter\bin\<Configuration>\Work Hour Counter.exe`.

| Task | Notes |
| ---- | ----- |
| UI edits | Prefer keeping designer + code-behind in sync (`MainForm.Designer.cs` / `.resx`) |
| Day math / `.dat` parse | `DataUnit` |
| Grid click, save, export | `MainForm` |
| NuGet | None — framework references only (`System.Windows.Forms`, etc.) |

Do **not** migrate to SDK-style / .NET (Core/5+) WinForms unless explicitly requested; this repo targets Framework 4.7.2.

---

## Code style & domain notes

- Namespace: `Work_Hour_Counter`. Assembly name / exe: `Work Hour Counter` (spaces).
- Grid: `labels[quarter, hour]` — `quarter` 0–3, `hour` 0–23; selected = red, unselected = gray.
- `cPacket` / `SortedData` hold the in-memory day list mirrored to the open `.dat` path.
- Export aggregates from `SortedData` around the current day index (`IndexOfcPacket`); radio buttons pick day/week/month/year depth.
- Avoid hardcoding personal machine paths in dialogs or docs (use neutral placeholders).

---

## PRs, CI, and releases

1. Branch off `main`; open a PR into `main`.
2. Attach **exactly one** semver label: `noBump` | `bumpPatch` | `bumpMinor` | `bumpMajor`.
3. Prefer required checks **`CI / CI`** and **`Semver / Semver`** once thin workflows from `danielnachumdev/actions` are on `main`.
4. Docs-only / tooling-only: use `noBump`.
5. Dependabot PRs stay on `noBump`.
6. Change shared CI/semver behavior in **`danielnachumdev/actions`**, not by re-vendoring here.

There is no PyPI/npm publish path; this is a desktop exe. Version attributes today live in `Properties/AssemblyInfo.cs` (`1.0.0.0`).

---

## Sensitive / ignored

Do not commit secrets, API keys, or personal paths/PII in issues, PRs, or docs.

Prefer leaving build outputs and IDE state out of new commits: `bin/`, `obj/`, `.vs/`. Historical copies may already exist in the tree — do not expand them; clean-up belongs in a dedicated PR if requested.
