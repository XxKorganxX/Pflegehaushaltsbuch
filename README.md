# Pflegehaushaltsbuch

Pflegehaushaltsbuch ist eine Windows-Desktopanwendung zur Verwaltung von Klienten, Betreuern, Assistenzdaten, Verwahrgeld, Kassenbewegungen, Dokumentlayouts und Auswertungen im Pflege- und Verwaltungsumfeld. Die Anwendung ist historisch gewachsen, wird aktuell auf .NET Framework 4.8 betrieben und setzt auf Windows Forms mit eigenen Controls, mehrsprachige `.resx`-Ressourcen und mehrere Datenbank-Backends.

## Funktionsueberblick

- Klienten, Betreuer, Assistenzpersonen und Stammdaten verwalten
- Barbestand, Bankbestand, Verwahrgeld und Kassenbuchungen erfassen
- Umbuchungen, Auszahlungen, Einzahlungen, Stornos und Journal-Eintraege abbilden
- Benutzer, Rechte und Administratorfunktionen verwalten
- Datenbanken erstellen, verbinden, aktualisieren, sichern und wiederherstellen
- SQL Server, MySQL und SQLite als Datenbankziele unterstuetzen
- Dokumente, Quittungen, Buchungslisten und Auswertungen drucken oder exportieren
- Layouts fuer Druckdokumente bearbeiten, importieren, exportieren und zuruecksetzen
- Fristen, Termine, Monatskalender und Auswertungen anzeigen
- Mehrsprachige Oberflaechen ueber neutrale und lokalisierte `.resx`-Dateien bereitstellen

## Technischer Steckbrief

| Bereich | Stand |
| --- | --- |
| Anwendungstyp | Windows Desktop / WinForms |
| Ziel-Framework | .NET Framework 4.8 |
| Projektformat | klassisches MSBuild-Projekt mit `packages.config` |
| Hauptassembly | `Pflegehaushaltsbuch.exe` |
| Einstiegspunkt | `Program.Main()` startet `Forms.MDI` |
| UI-Technologie | Windows Forms, eigene `FormControls`, einzelne WPF-Komponenten |
| Datenbanken | Microsoft SQL Server, MySQL, SQLite |
| Lokalisierung | `.resx` mit Satellitenassemblies fuer `de`, `de-DE` und `en` |
| Tests | MSTest-Projekt fuer Passwortschutz, DPAPI-Schutz und SQL-Helfer |

## Architektur

Die Anwendung ist in mehrere fachliche und technische Bereiche aufgeteilt.

### Anwendungseinstieg

`Program.cs` initialisiert die Anwendung, setzt Standardpfade, Farben, Schriften, UI-Kompatibilitaet und Kulturinformationen. Ist in den Einstellungen keine Sprache gesetzt, verwendet die Anwendung die installierte UI-Kultur des Betriebssystems. Danach wird die MDI-Hauptoberflaeche gestartet.

Wichtige Startaufgaben:

- Dokumentpfad unter `Eigene Dokumente\Pflegehaushaltsbuch` vorbereiten
- gespeicherte Benutzereinstellungen bei Bedarf aktualisieren
- globale Farb- und Font-Einstellungen setzen
- `CurrentCulture` und `CurrentUICulture` anhand der App-Einstellung oder OS-Kultur setzen
- `Forms.MDI` als Hauptfenster starten

### Datenbankschicht

Die Datenbanklogik basiert auf `Databases.SQLBase`. Diese abstrakte Basisklasse definiert die gemeinsamen Operationen fuer Verbindungen, Adapter, Buchungen, Journalisierung, Updates, Backups und Wiederherstellung. Die konkreten Implementierungen liegen in:

- `Databases.SQL` fuer Microsoft SQL Server
- `Databases.MySQL` fuer MySQL
- `Databases.SQLITE` fuer SQLite

Zentrale Aufgaben der Datenbankschicht:

- Verbindung testen und herstellen
- Datenbanken erstellen oder loeschen
- Tabellen und Views ueber Adapter fuellen
- Aenderungen aus `DataTable`-Instanzen zurueckschreiben
- Buchungen fuer Barbestand, Bankbestand, Klientenbuch und Buero-/Kassenbuch erzeugen
- Datenbankversionen aktualisieren
- SQL-Skripte aus dem `Version`-Ordner ausfuehren
- Backups erzeugen und wiederherstellen
- Benutzer und Datenbankrechte verwalten

Die fachlichen Selects und Spaltennamen werden ueber Enums in `SQLBase` gebuendelt. Dadurch verwenden Formulare und Dialoge keine frei verteilten Tabellen- oder Spaltennamen.

### Fachliche Datenmodelle

Der Ordner `Data` enthaelt zentrale Daten- und Hilfsklassen:

- `Company` verwaltet Firmen- und Kontaktdaten inklusive E-Mail-Validierung.
- `User` beschreibt lokale Benutzerinformationen.
- `ID_Client_Data` kapselt Klienten-IDs und zugehoerige Namen.
- `DocumentLayer` beschreibt serialisierbare Dokumentseiten und Seitennummern.
- `Printing` verwaltet Drucklayouts, Variablen, Layoutimport, Layoutexport und Layout-Reset.
- `Excel` kapselt Excel-bezogene Export- und Interop-Funktionen.
- `Licensing` prueft Lizenzdaten gegen die Datenbank.

Die Unterordner `Data\Graphics` und `Data\Print` bilden die Druck- und Layoutlogik ab. `GraphicsItem` ist die gemeinsame Basis fuer grafische Elemente wie Text, Linien, Rechtecke, Bilder und Tabellen. `PrintBase` stellt den gemeinsamen Druckablauf bereit, waehrend `Quittance` die Quittungslogik darauf aufsetzt.

### Oberflaeche

Die Fenster liegen im Ordner `Forms`. Viele davon erben von `Pflegehaushaltsbuch.FormControls.Form`, damit Farben, Schriftgroessen, Layoutverhalten und gemeinsame UI-Funktionen zentral gesteuert werden.

Wichtige Formularbereiche:

- `MainMenuForm`, `MDI` und `AdministrationForm` fuer Navigation und Hauptbereiche
- `UserLoginForm`, `CreationUserForm`, `ChangeUserForm` und `UserManagerForm` fuer Benutzerverwaltung
- `ClientsForm`, `AdvisorForm`, `AssistantsForm` und zugehoerige Dialoge fuer Stammdaten
- `BookForm`, `CashForm`, `BankForm`, `OfficeCashForm` und Buchungsdialoge fuer Finanzbewegungen
- `DatabaseManagerForm`, `DatabaseFileForm`, `DatabaseServerConnectForm`, `DatabaseUpdateForm` und `CreateSQLUser` fuer Datenbankverwaltung
- `DocumentsForm`, `DesignForm`, `LayoutManager`, `PageSettingsForm` und Druckdialoge fuer Dokumentlayouts
- `DeadLinesForm`, `ChangeDeadlineForm`, `StatisticsForm` und Kalenderdialoge fuer Auswertungen und Fristen

Die Dialoge unter `Forms\Dialoge` decken Erstellen, Bearbeiten, Importieren, Drucken, Zuruecksetzen und spezielle Workflows wie Rueckzahlungen oder Datenbankupdates ab.

### Eigene Controls

Der Ordner `FormControls` erweitert Standard-WinForms-Controls um ein konsistentes Verhalten fuer Darstellung, Farben, Eingaben und Layouts.

Beispiele:

- `Form` als gemeinsame Basis fuer Anwendungsfenster
- `Button`, `Label`, `TextBox`, `ComboBox`, `ListBox`, `DataGridView` und weitere Wrapper um Standardcontrols
- `DateTimeBox` und `NumericUpDown` mit Property-Change-Unterstuetzung
- `Layout` und `GeometryControl` fuer visuelle Layout- und Zeichenelemente
- `PrintPreviewDialog` fuer angepasste Druckvorschau
- `MessageBox` als eigene Dialogbasis
- `EmbededForm` fuer eingebettete Fensterdarstellung

Zusaetzlich existiert mit `WPFControls\UserTextBox.xaml.cs` eine WPF-basierte TextBox-Komponente fuer spezielle Eingabe- und Fokuslogik.

### Sicherheit

Die Anwendung enthaelt zwei moderne Sicherheitsbausteine fuer sensible Daten:

- `PasswordHasher` verwendet PBKDF2-SHA256 mit Salt und Iterationen fuer neue Passwort-Hashes.
- Legacy-MD5-Hashes werden weiterhin erkannt, koennen aber ueber `NeedsRehash` als migrationsbeduerftig markiert werden.
- `CredentialProtector` schuetzt lokal gespeicherte Zugangsdaten ueber Windows DPAPI und kennzeichnet geschuetzte Werte mit einem Versionspraefix.

Diese Funktionen sind durch Tests abgedeckt.

### Lokalisierung

Mehrsprachigkeit wird ueber `.resx`-Dateien umgesetzt. Die neutralen Ressourcen enthalten die Standardtexte, lokalisierte Dateien enthalten die jeweilige Sprache.

Beispiele:

- `Errorcodes.resx` enthaelt neutrale Errorcode-Texte und erzeugt `Errorcodes.Designer.cs`.
- `Errorcodes.de.resx` enthaelt deutsche Uebersetzungen und erzeugt keinen eigenen Designer.
- `EnumResources.resx` enthaelt neutrale Enum-Anzeigetexte und erzeugt `EnumResources.Designer.cs`.
- `EnumResources.de.resx` enthaelt deutsche Uebersetzungen und erzeugt keinen eigenen Designer.
- Formularressourcen liegen als `FormName.resx`, `FormName.de.resx` und teilweise `FormName.en.resx` vor.

Beim Build erzeugt `BuildSatelliteAssemblies.ps1` Satellitenassemblies fuer `de`, `de-DE` und `en`. Die Anwendung verwendet beim Start entweder die explizit gespeicherte Sprache oder, wenn diese leer ist, die installierte UI-Kultur des Betriebssystems.

Regeln fuer neue Resource-Eintraege:

1. Neue Keys immer in der neutralen `.resx` anlegen.
2. Uebersetzungen in den jeweiligen Sprachspalten oder Sprachdateien ergaenzen.
3. Nur neutrale `.resx`-Dateien duerfen einen `Designer.cs` erzeugen.
4. Lokalisierte Dateien wie `.de.resx` duerfen keinen `Generator` und kein `LastGenOutput` haben.

## Wichtige Abhaengigkeiten

Die NuGet-Pakete werden ueber `packages.config` verwaltet. Wichtige Bibliotheken sind:

- Entity Framework 6
- Microsoft.Data.SqlClient
- MySqlConnector
- System.Data.SQLite
- PDFsharp
- Microsoft Office Interop fuer Excel und Outlook
- System.Text.Json und moderne Microsoft.Extensions-Pakete
- System.Resources.Extensions fuer Ressourcenerzeugung im aktuellen Build

## Build

Voraussetzungen:

- Windows
- .NET Framework 4.8 Developer Pack
- Visual Studio oder MSBuild Build Tools
- NuGet-Paketwiederherstellung fuer `packages.config`

Mit der Solution-Datei:

```powershell
dotnet build Pflegehaushaltsbuch.slnx
```

Alternativ mit MSBuild:

```powershell
msbuild Pflegehaushaltsbuch.csproj /p:Configuration=Debug /p:Platform="AnyCPU"
```

Beim Build werden die lokalisierten Satellitenassemblies automatisch erzeugt.

## Tests

Das Testprojekt liegt unter `Pflegehaushaltsbuch.Tests` und verwendet MSTest.

Aktuell abgedeckt:

- PBKDF2-Passwort-Hashing und Passwortpruefung
- Ablehnung falscher Passwoerter
- Legacy-MD5-Kompatibilitaet und Rehash-Erkennung
- DPAPI-Schutz und Wiederherstellung von Zugangsdaten
- Ablehnung ungueltiger Credential-Formate
- SQL-Server-Identifier-Quoting
- SQL-Server-Connection-String fuer SQL-Login und Windows-Login

Tests ausfuehren:

```powershell
dotnet test Pflegehaushaltsbuch.slnx
```

Nach einem bereits erfolgreichen Build:

```powershell
dotnet test Pflegehaushaltsbuch.slnx --no-build
```

## Projektstruktur

```text
.
|-- Data/                         Fachliche Datenmodelle, Druckdaten und Layoutobjekte
|-- Databases/                    SQLBase sowie SQL Server-, MySQL- und SQLite-Implementierungen
|-- FormControls/                 Eigene WinForms-Control-Basis und UI-Erweiterungen
|-- Forms/                        Hauptfenster, Verwaltungsfenster und Dialoge
|-- Pflegehaushaltsbuch.Tests/    MSTest-Projekt
|-- Properties/                   Anwendungseinstellungen, Assemblyinfos und Ressourcen
|-- Resources/                    Eingebettete Ressourcen wie Schriften
|-- Tools/                        Hilfsklassen wie Druckfunktionen
|-- Version/                      Eingebettete SQL-Update-Skripte
|-- WPFControls/                  Einzelne WPF-basierte UI-Komponenten
|-- BuildSatelliteAssemblies.ps1  Erzeugt lokalisierte Satellitenassemblies
|-- Pflegehaushaltsbuch.csproj    Hauptprojekt
|-- Pflegehaushaltsbuch.slnx      Solution-Datei
```

## Entwicklungsnotizen

- Designer-Dateien werden nicht manuell gepflegt.
- Lokalisierte `.resx`-Dateien duerfen keine eigenen Designer-Dateien erzeugen.
- Neue Datenbankupdates gehoeren als eingebettete SQL-Skripte in den `Version`-Ordner.
- Zugangsdaten sollen nur geschuetzt gespeichert werden.
- Neue Passwortspeicherung soll PBKDF2 verwenden; MD5 ist nur noch Legacy-Kompatibilitaet.
- `bin`, `obj`, `.vs`, lokale Datenbanken, temporaere Dateien und private Zugangsdaten gehoeren nicht in die Versionsverwaltung.

## Aktueller Modernisierungsstand

Das Projekt ist weiterhin eine klassische .NET-Framework-WinForms-Anwendung. Bereits modernisiert wurden unter anderem Passwort-Hashing, lokaler Credential-Schutz, Lokalisierungsaufbau, Build-Stabilitaet und ein fokussiertes Testprojekt. Weitere sinnvolle Schritte waeren eine breitere Testabdeckung fuer Datenbank- und Buchungslogik, eine klare Trennung von UI und Fachlogik sowie perspektivisch eine Migration auf ein neueres .NET-Ziel.

