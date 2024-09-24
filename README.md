# Manga Scraper

Questo progetto personale è stato sviluppato con l'intento di esercitarmi su diverse tecnologie. L'obiettivo principale è quello di acquisire e archiviare in un database una vasta collezione di manga, effettuando scraping da un sito web. I manga vengono memorizzati con tutte le informazioni pertinenti e le immagini dei relativi capitoli.

Mi piacerebbe ricevere feedback sull'implementazione e sulla struttura del progetto; qualsiasi consiglio sarà molto apprezzato. I dubbi che ho al momento sul progetto sono elencati sotto. Se desiderate discuterne ulteriormente, sentitevi liberi di contattarmi via email o su LinkedIn, utilizzando i contatti presenti nel mio profilo.

>**Nota**: **Attualmente sono alla ricerca di nuove opportunità lavorative**.

## Tecnologie utilizzate

- **ASP.NET Core**, **Entity Framework Core** e **SQL Server** per la gestione del backend e del database.
- **Selenium**, per eseguire lo scraping su pagine con contenuti dinamici.
- **Hangfire** per eseguire operazioni di lunga durata, come lo scraping, in background.

## Ultime aggiunte

> **Nota**: **Il primo punto nell'elenco rappresenta l'ultima aggiunta al progetto**.

- **Aggiornamento dei dati**: se il database è già popolato e ci sono nuove uscite sul sito, queste verranno aggiunte automaticamente durante l'esecuzione. Poiché non esiste una pagina che mostra solo le nuove uscite, è necessario riciclare l'archivio e confrontare i dati con quelli presenti nel database. Ora non è più necessaria la distinzione tra il metodo `Operate()` (che veniva usato per un database vuoto) e `Update()` (per aggiornare i dati esistenti).

- Creata un'**API** per gestire la comunicazione con il database, separando così la console app dal contatto diretto con il database.

- Aggiunti **endpoint** in previsione della futura implementazione di una UI.

## Problemi attuali

Di seguito i problemi riscontrati nel progetto:

- **Hangfire**: in alcune occasioni, esegue il job più volte senza motivo apparente. Questo comportamento è casuale: a volte il job viene messo in coda due volte, altre quattro, o si avvia nuovamente dopo 5-10 minuti. Alcuni utenti hanno riscontrato lo stesso problema e hanno trovato workaround, ma non una soluzione definitiva. Riavviando l'applicazione, il problema spesso scompare.
  [Link alla discussione sul problema](https://github.com/HangfireIO/Hangfire/issues/1025)

- **RISOLTO!** --> Problema con **Selenium**: dopo un certo tempo di esecuzione (10-20 minuti o più), Selenium si blocca e lancia l'eccezione seguente:

    ```text
    OpenQA.Selenium.WebDriverException: The HTTP request to the remote WebDriver server for URL http://localhost:51696/session/.../click timed out after 60 seconds.
            ---> System.Threading.Tasks.TaskCanceledException: The request was canceled due to the configured HttpClient.Timeout of 60 seconds elapsing.
            ---> System.TimeoutException: The operation was canceled.
            ---> System.Threading.Tasks.TaskCanceledException: The operation was canceled.
            ---> System.IO.IOException: Unable to read data from the transport connection: Operazione di I/O terminata a causa dell'uscita dal thread oppure della richiesta di un'applicazione.
            ---> System.Net.Sockets.SocketException (995): Operazione di I/O terminata a causa dell'uscita dal thread oppure della richiesta di un'applicazione.
        --- End of inner exception stack trace ---
    ```

	Il problema si verifica nei metodi `GetManga/Manhwa/OneshotChapterImages()`, solitamente al momento del click. Nei log, sembra che il click avvenga correttamente e la pagina del WebDriver cambi, ma Selenium non rileva il cambiamento e continua ad aspettare, fino al timeout.

	Un esempio del metodo problematico:

	```C#
	for (int j = 0; j < nPagine; j++)
	{
    	//Avviene qualche blocco ogni tanto, credo dovuto al server che si blocca per le troppe richieste consecutive, quindi applico una logica di retry
    	int retries = 3; // Numero di tentativi di retry
    	int delay = 1000; // Tempo di ritardo iniziale in millisecondi

    	for (int z = 0; z < retries; z++)
    	{
        	try
        	{
            	//Estraggo l'immagine
            	string urlImg = _selenium.GetAttributeOfElementByClassName("img-fluid", "src", imgDiv);

            	//Salvo l'immagine nell'apposita cartella, chiamando il file con: nomeDelManga + numeroVolume + numeroCapitolo + numeroImmagineCapitolo.jpg
            	await DownloadImgs(urlImg, chapterFolder.FullName + $"\\{newMangaName}" + $"Capitolo{i + 1}_img{j + 1}.jpg"); //<-- **QUESTO METODO NON LANCIA ECCEZIONE**

            	capitolo.ImgPositions.Add(new ImagePosition(chapterFolder.FullName + $"\\{newMangaName}" + $"Capitolo{i + 1}_img{j + 1}.jpg", capitolo.Id));

            	_selenium.FindElementByClassName("page-next", driver).Click();

            	_logger.LogInformation("SUCCESSO: Try {z}", z);

            	break; // Esco dal ciclo di retry se il download ha avuto successo
        	}
        	catch (Exception ex)
        	{
            	_logger.LogError("ERRORE: Ottenimento immagine fallito nel try {z}. {ex}", z, ex); //<-- **QUESTO METODO LANCIA QUESTO MESSAGGIO MA ANCHE IL RETRY FALLISCE COME SI VEDE DALL'ECCEZIONE SUCCESSIVA**
        	}

        	if(z == retries - 1)
        	{
            	throw new Exception("ERRORE: Retry esauriti per l'ottenimento dell'immagine.");
        	}

        	// Ritardo tra un tentativo e l'altro con backoff esponenziale
        	await Task.Delay(delay);
        	delay *= 2; // Aumento il ritardo con backoff esponenziale

        	_selenium.Refresh(driver);
    	}
	}
	```


## Dubbi sul progetto

Il progetto non è concluso. Ciò che verrà aggiunto è elencato nella sezione sotto. I dubbi al momento sono questi:

1. Seguendo i **SOLID principles**, bisogna astrarre le dipendenze e così è stato fatto. Questo però ha portato a un'estesa lista di metodi nell'interfaccia `ISeleniumService`, dato che i metodi se non presenti in quell'interfaccia non possono essere utilizzati poi nella classe `MangaScraperService`. In una futura estensione del progetto (cosa molto improbabile data la natura dell'interfaccia e della classe), sarebbe problematico implementare tutti i metodi dell'interfaccia nella nuova classe.

2. Uso **Hangfire** per la prima volta, non so se ho strutturato il design attorno ad esso in maniera corretta.

3. Valuto l'utilizzo di **MediatR** con il pattern **CQRS**, anche per la futura **UI**.


## Future implementazioni

- Verrà implementata una **UI** per la consultazione dei manga.
- Probabile implementazione di **Autenticazione** e **Autorizzazione**.
