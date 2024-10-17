# Manga Scraper

Questo progetto personale è stato sviluppato con l'intento di esercitarmi su diverse tecnologie. L'obiettivo principale è quello di acquisire e archiviare in un database una vasta collezione di manga, effettuando scraping da un sito web. I manga vengono memorizzati con tutte le informazioni pertinenti e le immagini dei relativi capitoli. Inoltre è in fase di sviluppo la UI tramite la quale sarà possibile consultare i manga.

Mi piacerebbe ricevere feedback sull'implementazione e sulla struttura del progetto; qualsiasi consiglio sarà molto apprezzato. Se desiderate discuterne ulteriormente, sentitevi liberi di contattarmi via email o su LinkedIn, utilizzando i contatti presenti nel mio profilo.

>**Nota**: **Attualmente sono alla ricerca di nuove opportunità lavorative**.

## Tecnologie utilizzate

- **ASP.NET Core**, **Entity Framework Core** e **SQL Server** per la gestione del backend e del database.
- **Selenium**, per eseguire lo scraping su pagine con contenuti dinamici.
- **Hangfire** per eseguire operazioni di lunga durata, come lo scraping, in background.
- **Blazor** per creare la **UI**.
- **ImageSharp** per il ridimensionamento e la gestione delle immagini.

## Ultime aggiunte

> **Nota**: **Il primo punto nell'elenco rappresenta l'ultima aggiunta al progetto**.

- Creata una **UI** con **Blazor** per la consultazione dei fumetti ottenuti tramite scraping, la quale è ancora in fase di sviluppo.

- Creata una **Class Library** per condividere i model tra le due **API** e per implementare Entity Framework in un punto condivisibile tra diversi servizi.

- Creata un'**API** per l'ottenimento dei dati dal database, comprese le immagini, da utilizzare nella UI. Ho effettuato questa scelta per mantenere divise le funzioni di scraping in un'**API** e le funzioni di lettura dal database e comunicazione con il front-end in un'altra **API**, quindi per mantenere una separazione delle responsabilità.

- **Aggiornamento dei dati**: se il database è già popolato e ci sono nuove uscite sul sito, queste verranno aggiunte automaticamente durante l'esecuzione. Poiché non esiste una pagina che mostra solo le nuove uscite, è necessario riciclare l'archivio e confrontare i dati con quelli presenti nel database. Ora non è più necessaria la distinzione tra il metodo `Operate()` (che veniva usato per un database vuoto) e `Update()` (per aggiornare i dati esistenti).

- Creata un'**API** per gestire la comunicazione con il database, separando così la console app dal contatto diretto con il database.


## Future implementazioni

- Implementazione di **Autenticazione** e **Autorizzazione**.
