# BudgetPlanner8

BudgetPlanner8 är en WPF-baserad desktopapplikation för budgetplanering, byggd med .NET och Entity Framework Core mot SQL Server. Projektet demonstrerar en tydlig uppdelning mellan presentation, affärslogik och datalager samt följer MVVM-principer för en strukturerad och skalbar arkitektur.

Applikationen är utvecklad som ett praktiskt exempel på hur man bygger en modern desktoplösning med persistens, databasmigreringar och tydlig separation av ansvar.

---

## Översikt

BudgetPlanner8 gör det möjligt att:

- Skapa och hantera budgetposter
- Registrera inkomster och utgifter
- Kategorisera transaktioner
- Lagra data i SQL Server via Entity Framework Core
- Arbeta med en strukturerad MVVM-arkitektur
- Bygga vidare på en tydlig och utbyggbar kodbas

Projektet är uppdelat i separata lager för att skapa en tydlig och professionell struktur som lämpar sig väl för vidareutveckling och underhåll.

---

## Teknikstack

- .NET (WPF)
- Entity Framework Core
- SQL Server
- MVVM-arkitektur
- C#

---

## Projektstruktur

BudgetPlanner8/ ├── BudgetPlanner8.DAL/      # Data Access Layer (DbContext, entiteter, migrationer) ├── BudgetPlanner8.WPF/      # Presentation Layer (Views, ViewModels) ├── BudgetPlanner8.slnx      # Lösningsfil └── README.md

### BudgetPlanner8.DAL
Innehåller:
- Entity-modeller
- DbContext
- Databaslogik
- Migrationer

### BudgetPlanner8.WPF
Innehåller:
- Views (XAML)
- ViewModels
- UI-logik enligt MVVM

---

## Kom igång

### Förutsättningar

- Visual Studio 2022 eller senare
- .NET SDK (version enligt projektfil)
- SQL Server (LocalDB, Express eller full version)
- Entity Framework Core CLI (valfritt men rekommenderat)

### Installation

1. Klona repot

git clone https://github.com/antonlidstroem/BudgetPlanner8.git cd BudgetPlanner8

2. Uppdatera connection string

Öppna konfigurationen (exempelvis appsettings.json eller DbContext-konfigurationen) och ange din SQL Server-anslutning.

3. Skapa databasen

Om projektet använder EF-migreringar:

dotnet ef database update --project .\BudgetPlanner8.DAL\

4. Starta applikationen

Öppna lösningen i Visual Studio och sätt `BudgetPlanner8.WPF` som startup-projekt. Kör applikationen.

---

## Arkitektur och designprinciper

Projektet är uppbyggt enligt följande principer:

- Separation of Concerns
- MVVM (Model-View-ViewModel)
- Tydlig uppdelning mellan UI och dataåtkomst
- Entity Framework som ORM
- Skalbar struktur för vidareutveckling

Detta gör projektet lämpligt som:
- Utbildningsprojekt
- Referensprojekt för WPF + EF Core
- Grund för vidare produktutveckling

---

## Vidareutveckling

Möjliga förbättringar:

- Implementera repository-mönster ovanpå EF
- Lägga till validering med Data Annotations eller FluentValidation
- Skapa enhetstester för affärslogik
- Implementera rapportexport (PDF/CSV)
- Lägga till diagram och visualisering av budgetdata
- CI/CD med GitHub Actions

---

## Bidra

Förslag, förbättringar och pull requests är välkomna.  
Skapa gärna en issue om du hittar en bugg eller har en idé på förbättring.

---

## Licens

Detta projekt är öppet för utbildnings- och demonstrationssyfte.  
Lägg till en specifik licens om projektet ska användas i produktion eller delas vidare offentligt.