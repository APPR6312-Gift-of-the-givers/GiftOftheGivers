# Gift of the Givers — POE Part 1 Prototype (ASP.NET Core / C#)

A functional prototype for APPR6312 POE Part 1. Built with ASP.NET Core MVC, C#, and
ASP.NET Core Identity. Uses EF Core's **InMemory** database provider (explicitly allowed
by the POE brief for the prototype), so it runs immediately — no SQL Server install needed.

## What's functional

- **Home page** — logo, nav, hero, feature cards.
- **Login / Register** — real authentication via ASP.NET Core Identity (hashed passwords,
  cookie auth, role-based redirects). Donors can self-register through the Register tab.
- **Volunteer registration** — form saves to the database; skill chips are multi-select
  checkboxes; confirmation page on success.
- **Donations** — one-time or recurring, currency + amount selection, saves to the
  database, generates a certificate number, and shows a Thank You page with it.
- **Employee Dashboard** *(role: Employee)* — live stats (active projects, updates posted,
  volunteers registered, donations this month), a field-update feed, recent volunteers
  and recent donations tables, and a "Post Update" form that writes to the database.
- **Donor Portal** *(role: Donor)* — shows the logged-in donor's own donation history and
  totals, pulled live from the database.

## Demo credentials

| Role     | Email             | Password     |
|----------|-------------------|--------------|
| Employee | employee@gotg.org | Employee123! |
| Donor    | donor@gotg.org    | Donor123!    |

New donor accounts can also be created on the fly via the Register tab.

## How to run it

You'll need the [.NET 8 SDK](https://dotnet.microsoft.com/download) installed.

**Visual Studio:** open `GiftOfTheGivers.sln`, press F5 (or Ctrl+F5).

**VS Code / command line:**
```bash
cd GiftOfTheGivers.Web
dotnet restore
dotnet run
```
Then open the URL shown in the terminal (usually `https://localhost:7xxx`).

> First run downloads NuGet packages, so you'll need an internet connection once. After
> that it runs fully offline.

## Project structure

```
GiftOfTheGivers.Web/
├── Controllers/       Home, Account, Volunteer, Donation, Employee, Donor
├── Models/             ApplicationUser, Volunteer, Donation, ReliefProject, FieldUpdate
├── Data/               ApplicationDbContext, SeedData (roles, demo users, sample projects)
├── Views/               Razor views matching the Figma mockups
├── wwwroot/             site.css (green/gold GOTG branding), site.js
└── Program.cs           Identity + EF Core InMemory setup, seeding, routing
```

## Notes for the write-up

- Data resets on restart — this is intentional for a prototype and is called out in the
  POE brief as acceptable ("You may save this into SQL Server or in memory").
- To move this toward the full system: swap `UseInMemoryDatabase(...)` in `Program.cs`
  for `UseSqlServer(connectionString)`, add an `Azure SQL` connection string, add EF Core
  migrations, and publish the project to Azure App Services.
- Passwords are hashed by ASP.NET Core Identity (not plain text) — worth mentioning in
  your evidence/screenshots section as a security strength of using ASP.NET Core.
