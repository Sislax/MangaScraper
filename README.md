# Manga Scraper

Questo progetto è stato sviluppato con l'intento di esercitarmi su diverse tecnologie. L'obiettivo principale è quello di acquisire e archiviare in un database una vasta collezione di manga, effettuando scraping da un sito web. I manga vengono memorizzati con tutte le informazioni pertinenti e le immagini dei relativi capitoli. I manga ottenuti sono consultabili tramite la UI, la quale ha funzioni di autenticazione ed autorizzazione tramite i Jwt Tokens.

## Tecnologie utilizzate

- **ASP.NET Core**, **Entity Framework Core** e **SQL Server** per la gestione del backend e del database.
- **ASP.NET Identity** e **Jwt Tokens** per le funzioni di Autenticazione ed Autorizzazione.
- **Selenium**, per eseguire lo scraping su pagine con contenuti dinamici.
- **Hangfire** per eseguire operazioni di lunga durata, come lo scraping, in background.
- **Blazor** per creare la **UI**.
- **ImageSharp** per il ridimensionamento e la gestione delle immagini.

## Struttura del progetto

1. **MangaScraper.Data**:

    Libreria di classi nella quale risiedono tutti gli oggetti di dominio, le entità per ASP.NET Identity e i DTO.
    Implementa Entity Framework Core.

2. **MangaScraper.Api**:

    API che implementa Selenium e Hangfire, nella quale risiede la logica di scraping.

3. **MangaView.Api**:

    API che si occupa di fornire alla UI i DTO.

4. **AuthService.Api**:

    API che implementa le funzioni per l'autenticazione e l'autorizzazione.

5. **MangaView.UI**:

    UI sviluppata in Blazor per la consultazione dei manga ottenuti tramite lo scraping e, se ADMIN, permette di avviare lo scraping.

## Come testare il progetto

1. **Clonare il progetto con il seguente comando tramite terminale**:

```bash
   git clone https://github.com/Sislax/MangaScraper.git
```

2. **Creare due database SQL Server, uno per le entità di dominio e l'altro per le identità.**

3. **Inserire le stringhe di connessione negli appositi progetti**
    - Per il database delle entità di dominio inserire la stringa di connessione nel file *appsettings.json* nella sezione *DefaultConnection* del progetto **MangaScraper.Api** e **MangaView.Api**.
    - Per il database delle identità inserire la stringa di connessione nel file *appsettings.json* nella sezione *DefaultConnection* del progetto **AuthService.Api**.

4. **Effettuare le migration**
    - Impostare come progetto di avvio **MangaScraper.Api** ed eseguire i seguenti comandi tramite terminale: 

    ```powershell
        Add-Migration Init -Context AppDbContext
    ```

    successivamente:

    ```powershell
        Update-Database -Context AppDbContext
    ```
    - Impostare come progetto di avvio **AuthService.Api**

    ```powershell
        Add-Migration Init -Context UserIdentityDbContext
    ```

    successivamente:

    ```powershell
        Update-Database -Context UserIdentityDbContext
    ```

5. **Creare una cartella su disco la quale conterrà tutte le immagini dei manga**
    - Creare la cartella dove si preferisce e inserire il percorso completo della cartella nel file *appsettings.json* nella sezione *FolderForImages* del progetto **MangaScraper.Api**.

6. **Impostare come progetti di avvio multipli i seguenti**:
    1. **MangaScraper.Api**
    2. **MangaView.Api**
    3. **AuthService.Api**
    4. **MangaView.UI**

7. **Come effettuare il login**:
    1. Per accedere come ADMIN usare le seguenti credenziali:
        - Email: admin@test.it
        - Password: 33**DDss

    2. Per accedere come USER usare le seguenti credenziali:
        - Email: user@test.it
        - Password: 33**DDss

8. **Testare lo scraping**
    - Accedere come ADMIN e navigare nella pagina *AdminPage*.
    - Selezionare un numero tra 1 e 176 (Questo andrà ad indicare su quante pagine dell'archivio del sito di riferimento sarà effettuato lo scraping. **CONSIGLIO**: inserire 1 per testare, lo scraping dura molto tempo in quanto per ogni manga esistono numerosi capitoli e per ogni capitolo sono presenti numerose immagini. Ogni pagina dell'archivio contiene 16 manga).
    - Il salvataggio su database avviene dopo aver finito lo scraping di ogni manga. Quindi se si vuole testare l'interfaccia utente per visualizzare le immagini appena ottenute, è possibile interrompere l'esecuzione dopo aver finito lo scraping del primo manga, riavviare e navigare nella UI per vedere il manga con le relative immagini.



