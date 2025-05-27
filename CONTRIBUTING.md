# Contributing to HeroForge-OnceAgain

Thank you for your interest in contributing! This project welcomes collaboration, and we follow a structured Git workflow to ensure a clean, stable codebase.

---

## 🛠 Requirements

- Git installed
- A GitHub account
- Visual Studio (recommended) with .NET Framework 4.8
- Basic understanding of pull requests

---

## 🚀 Contribution Workflow

### 1. Fork the repository

Click the **"Fork"** button on GitHub to create your own copy of the project.

### 2. Clone your fork

```bash
git clone https://github.com/<your-username>/HeroForge-OnceAgain.git
cd HeroForge-OnceAgain
```

### 3. Create a new branch

Use a clear, descriptive name:

```bash
git checkout -b feature/your-feature-name
```

Examples:
- `feature/localization-support`
- `fix/race-dropdown-error`
- `refactor/remove-obsolete-code`

### 4. Make your changes

- Follow the existing code style
- Keep your changes focused and scoped
- Use descriptive commit messages
- If adding a new feature, include translations when applicable

### 5. Commit and push

```bash
git add .
git commit -m "Describe your change"
git push origin feature/your-feature-name
```

### 6. Open a Pull Request

- Go to your fork on GitHub
- Click **"Compare & Pull Request"**
- Provide a summary of your changes
- Reference any related issues (e.g. `Fixes #12`)

---

## 🔍 Review and Merge

- Your pull request will be reviewed by the project maintainer
- You may receive feedback or change requests
- Once approved, it will be merged into the `main` branch

---

## 📂 Project Structure Highlights

- **Messages**: `Resources/Translations/messages.json`
- **Race names**: `Resources/Translations/races/Races.json`
- **Menu labels**: `Resources/Translations/menu/Menu.json`
- **Localization logic**: `LocalizationHelper.cs`

---

## 🤝 Thank you!

Your contributions are valued and appreciated. Feel free to open issues if you find bugs or have suggestions.
