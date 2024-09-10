## Manga Scraper

Questo è un progetto personale utilizzato per fare pratica, mi sollevo da ogni responsabilità data dall'utilizzo di questo progetto.

Il progetto consiste nell'ottenere e conservare in un database una lunga lista di manga effettuando scraping di un sito web (di dubbia legalità dato copyright).
Questi manga vengono conservati con tutte le informazioni e tutte le immagini di ogni capitolo del manga.


## Tecnologie utilizzte

Ho deciso di utilizzare Selenium in quanto mi consente di effettuare scraping su pagine dinamiche (JavaScript), ma magari non è la scelta più efficiente e funzionale.
Ho provato ad utilizzare AngleSharp ma ho notato delle limitazioni per lo scraping di pagine dinamiche (forse skill issue da parte mia).


## Ultime aggiunte

Implementata un'API che si occupa di comunicare con il database, per lasciare la console app priva di ogni contatto con il db. Rifletto sull'implementazione di Autenticazione e Autorizzazione.
Implementati endpoint per una futura UI.

## Dubbi sul progetto

Il progetto non è concluso. Ciò che verrà aggiunto è elencato nella sezione sotto.
I dubbi al momento sono alcuni:
 1 - Seguendo i SOLID principles bisogna astrarre le dipendenze e così è stato fatto. Questo però ha portato ad una estesa lista di metodi nell'interfaccia "ISeleniumService", dato che i metodi se non presenti
	 in quell'interfaccia non possono essere utilizzati poi nella classe "MangaScraperService". In una futura estensione del progetto (cosa molto improbabile data la natura dell'interfaccia e della classe ->
	 perchè bisognerebbe creare un'altra classe che implementa Selenium?) diventa problematico implementare tutti i metodi dell'intefaccia nella nuova classe. (Skill issue anche qui? Probabile).

 2 - EntityFramework già è di per se un repository pattern, quindi perchè ho utilizzato quest'ultimo?
	 La risposta è per fare pratica. Credo che comunque invece di utilizzare la classe repository avrei dovuto iniettare direttamente il context all'interno del service, per poi utilizzare i dbset come fossero dei repository.


## Future implementazioni

Al momento il progetto svolge la funzione di riempire un database vuoto con tutta la lista dei manga trovati sul sito (ne sono davvero molti).
In futuro verrà implementata la funzione di aggiornamento che controllerà le nuove uscite sul sito e le aggiungerà al database.
Verrà implementata anche una UI per la consultazione dei manga.