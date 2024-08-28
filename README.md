## Manga Scraper

Questo � un progetto personale utilizzato per fare pratica, mi sollevo da ogni responsabilit� data dall'utilizzo di questo progetto.

Il progetto consiste nell'ottenere e conservare in un database una lunga lista di manga effettuando scraping di un sito web (di dubbia legalit� dato copyright).
Questi manga vengono conservati con tutte le informazioni e tutte le immagini di ogni capitolo del manga.


## Tecnologie utilizzte

Ho deciso di utilizzare Selenium in quanto mi consente di effettuare scraping su pagine dinamiche (JavaScript), ma magari non � la scelta pi� efficiente e funzionale.
Ho provato ad utilizzare AngleSharp ma ho notato delle limitazioni per lo scraping di pagine dinamiche (forse skill issue da parte mia).


## Dubbi sul progetto

Il progetto non � concluso. Ci� che verr� aggiunto � elencato nella sezione sotto.
I dubbi al momento sono alcuni:
 1 - Seguendo i SOLID principles bisogna astrarre le dipendenze e cos� � stato fatto. Questo per� ha portato ad una estesa lista di metodi nell'interfaccia "ISeleniumService", dato che i metodi se non presenti
	 in quell'interfaccia non possono essere utilizzati poi nella classe "MangaScraperService". In una futura estensione del progetto (cosa molto improbabile data la natura dell'interfaccia e della classe ->
	 perch� bisognerebbe creare un'altra classe che implementa Selenium?) diventa problematico implementare tutti i metodi dell'intefaccia nella nuova classe. (Skill issue anche qui? Probabile).

 2 - EntityFramework gi� � di per se un repository pattern, quindi perch� ho utilizzato quest'ultimo?
	 La risposta � per fare pratica. Credo che comunque invece di utilizzare la classe repository avrei dovuto iniettare direttamente il context all'interno del service, per poi utilizzare i dbset come fossero dei repository.


## Future implementazioni

Al momento il progetto svolge la funzione di riempire un database vuoto con tutta la lista dei manga trovati sul sito (ne sono davvero molti).
In futuro verr� implementata la funzione di aggiornamento che controller� le nuove uscite sul sito e le aggiunger� al database.
Verr� implementata anche un'API (o una Class Library se si utilizza Blazor?) per comunicare con un'interfaccia utente, anch'essa da creare in futuro. Nel momento in cui verr� creata l'API, tutte le funzioni di
interazione con il database verranno spostate su essa per lasciare la console app e l'interfaccia utente priva di ogni contatto con il db.

