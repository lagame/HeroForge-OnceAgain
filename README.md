# HeroForge-OnceAgain

HeroForge-OnceAgain is a character sheet creation software for **D&D 3.5e**.  
It makes the character generation process much simpler, allowing you to create in minutes what previously took hours, and in hours what previously took days!  
Powerful and well-designed, it is based on the design of my dear Heliomance, the famous **HeroForge-Anew**.  
Thanks to that amazing Excel project, I had the inspiration to rebuild it using **C# and Windows Forms**. This way we gained performance and extensibility, allowing anyone to contribute with translations, new content, and improvements.  
The app already includes most of the official D&D 3.5 material, and more content is constantly being added.  
Give it a try and never look back!

---

## 🗂️ Features

- Character creation for **D&D 3.5e**
- Full multiclass and prestige class support
- Dynamic feat and spell selection
- Wild Shape and animal companion support (WIP)
- Multilanguage support (English and Portuguese currently)
- Database-driven content management (JSON + SQL Server LocalDB)
- Excel-style logic with C# performance

---

## 🗄️ Database

This project uses **SQL Server LocalDB** and **Entity Framework Core** for data persistence.

To create the database locally, run the following SQL script:

BancoSQL/HeroForgeDb.sql

---

## ⚙️ Conversion Progress
HeroForge-OnceAgain is based on the original HeroForge-Anew 7.4.0.1 Excel spreadsheet.
The current implementation represents approximately 8% of the total logic and formulas converted into C# code.

We are steadily migrating the spreadsheet’s powerful features into a more robust and extensible codebase, using structured data, object-oriented design, and modern development practices.

💡 Even though only a small portion has been migrated so far, the core architecture is ready to scale, and contributions are welcome!

---

## 🤝 Want to help this project?

There are several areas where you can help:

- **Translations** to other languages (FR, ES, IT...)
- **Data entry**: Add new monsters and creatures for Wild Shape and companion lists
- **Bug reporting** and **feature suggestions**
- **UI improvements** or **theme/skin suggestions**

Right now, the list of creatures available for transformation is pretty limited, and there's almost nothing beyond animals — which makes **Masters of Many Forms** quite sad.  
If you’re willing to help, send me a message and I’ll guide you through the process.