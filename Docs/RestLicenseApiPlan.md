# REST-Lizenzierung: Funktionsumfang

Diese Datei ersetzt den entfernten WCF-Lizenzserver als fachliche Notiz fuer die spaetere REST-API.

## Ziele

- Keine WCF-Abhaengigkeit mehr in der Desktopanwendung.
- Kommunikation nur ueber HTTPS.
- Lizenzdaten als JSON austauschen.
- Lizenz lokal speichern und offline pruefen koennen.
- Lizenzdaten serverseitig digital signieren.

## Benoetigte API-Funktionen

### Lizenz registrieren

Endpoint-Idee:

```text
POST /api/licenses/register
```

Eingaben:

- Firma
- E-Mail
- Adresse
- PLZ
- Ort
- Land
- Sprache
- optionale Programmversion

Ausgabe:

- Lizenz-ID
- Status
- Gueltigkeitszeitraum
- signierte Lizenzdaten

### Lizenz abrufen

Endpoint-Idee:

```text
POST /api/licenses/lookup
```

Eingaben:

- Lizenz-ID oder E-Mail/Firma
- optionale Maschinenkennung

Ausgabe:

- Lizenzstatus
- Gueltigkeitszeitraum
- signierte Lizenzdaten
- Fehlermeldung bei ungueltiger Lizenz

### Lizenz aktivieren

Endpoint-Idee:

```text
POST /api/licenses/activate
```

Eingaben:

- Lizenz-ID
- Aktivierungscode
- Maschinenkennung

Ausgabe:

- Aktivierungsstatus
- signierte Lizenzdaten

### Aktivierungscode senden

Endpoint-Idee:

```text
POST /api/licenses/send-activation-code
```

Eingaben:

- E-Mail
- Firma

Ausgabe:

- Versandstatus
- Fehlermeldung

### Feedback oder Supportnachricht senden

Endpoint-Idee:

```text
POST /api/support/messages
```

Eingaben:

- Absender-E-Mail
- Nachricht
- Programmversion

Ausgabe:

- Versandstatus

## Lokale Speicherung in der App

Die App sollte die vom Server signierten Lizenzdaten lokal speichern. Die Pruefung sollte offline moeglich sein.

Empfohlen:

- Server signiert Lizenzdaten mit privatem Schluessel.
- App enthaelt nur den oeffentlichen Schluessel.
- App prueft Signatur, Gueltigkeitszeitraum und optionale Maschinenbindung.

## Spaetere technische Umsetzung

- ASP.NET Core Minimal API
- JSON DTOs
- HTTPS
- Datenbank fuer Lizenzen und Aktivierungen
- Signatur z.B. mit RSA oder ECDSA
- einfache Admin-Oberflaeche oder Admin-Endpoints
