# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

### Added
- Placeholder for upcoming features

---

## [0.0.1] - 2025-05-27 [Pre-alpha]

### 🆕 Added
- Initial support for D&D 3.5e character sheet creation
- Race selection system with multilingual translation via JSON
- Translation structure for menus (`menu/`) and race names (`races/`)
- Login screen (`FrmLogin`) and preferences screen for units and language
- Initial SQL script (`HeroForgeDb.sql`) and EF LocalDB integration
- Random generation routines for:
  - Name
  - Age and height
  - Hair style
- Wild Shape support groundwork (WIP)
- CONTRIBUTING.md with full contributor workflow
- LocalizationHelper to support translated message boxes
- `messages.json` for multilingual error/message handling

### 🔧 Changed / Improved
- Form1 logic and comboBox population with localized values
- Improved handling of height and weight with validation feedback
- Adjusted form closing behavior to fully terminate app with `Environment.Exit(0)`
- Removed obsolete HTTP listener and related cleanup logic
- README.md updated with setup, translation, and contribution instructions

### 🐞 Fixed
- Race file not found error now uses translated messages
- Race loading logic now checks file existence gracefully
- Menu JSON added to output with correct build action

### 🗃️ Organizational
- JSON file structure cleanup and resource management
- Gitignore improvements
- Removed accidentally committed `.zip` archive
