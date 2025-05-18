# Restaurant App

O aplicatie desktop pentru gestionarea unui restaurant cu comenzi online, realizată folosind WPF, C# și SQL Server.

## Cerinte

- .NET 7.0 SDK
- SQL Server 2022
- Visual Studio 2022 (recomandat)

## Instalare

1. Clonati repository-ul:
```bash
git clone https://github.com/yourusername/RestaurantApp.git
```

2. Creati baza de date folosind scriptul SQL din `RestaurantApp/Database/CreateDatabase.sql`

3. Configurati string-ul de conexiune in `RestaurantApp/Config/appsettings.json`

4. Deschideti solutia in Visual Studio si restaurati pachetele NuGet

5. Compilati si rulati aplicatia

## Functionalitati

- Vizualizare meniu restaurant
- Cautare in meniu dupa diverse criterii
- Comanda online pentru clienti autentificati
- Generare rapoarte pentru angajati
- Gestionare stoc si cantitati
- Sistem de reduceri si livrare

## Structura Proiectului

- `Models/` - Clasele de model pentru entitatile din baza de date
- `ViewModels/` - Clasele pentru logica de prezentare (MVVM)
- `Views/` - Interfetele utilizator (XAML)
- `Services/` - Serviciile de business logic
- `Repositories/` - Accesul la date
- `Data/` - Contextul bazei de date
- `Commands/` - Implementari pentru comenzi
- `Config/` - Fisiere de configurare
- `Database/` - Scripturi SQL