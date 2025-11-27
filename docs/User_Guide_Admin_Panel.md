# Guida Utente - CoreDisplay Admin Panel

Benvenuto nel Pannello di Amministrazione di CoreDisplay. Questa guida ti accompagnerà passo dopo passo nella gestione della tua flotta di schermi digitali.

---

## 1. Dashboard & Monitoraggio 📊

La Dashboard è il tuo centro di controllo. Appena accedi, vedrai una panoramica immediata dello stato della flotta.

### Interpretare lo Stato dei Dispositivi
Ogni dispositivo è rappresentato da una "Card" con un indicatore colorato:
*   🟢 **Online**: Il dispositivo funziona correttamente e ha comunicato negli ultimi 2 minuti.
*   🔴 **Offline**: Il dispositivo non comunica da più di 5 minuti. Controlla connessione internet o alimentazione.
*   🟡 **Errore**: Il dispositivo segnala problemi hardware (es. disco pieno, CPU > 90%).

### Azioni Rapide
Sulla card di ogni dispositivo trovi dei pulsanti per azioni immediate:
*   🔄 **Riavvia**: Invia un comando di riavvio al dispositivo.
*   📷 **Screenshot**: Richiede un'istantanea di cosa sta mostrando lo schermo in quel momento.
*   ⚙️ **Config**: Aggiorna la configurazione remota.

---

## 2. Gestione Contenuti (Media & Playlist) 🎬

### Caricare Nuovi Media
1.  Naviga nella sezione **Media Library** dal menu laterale.
2.  Clicca sul pulsante blu **"Carica Media"** in alto a destra.
3.  Trascina i tuoi file (Immagini .jpg/.png o Video .mp4) nell'area tratteggiata.
4.  Attendi il completamento della barra di caricamento. Il sistema verificherà automaticamente l'integrità del file.

### Creare una Playlist
1.  Vai su **Playlists**.
2.  Clicca **"Nuova Playlist"**.
3.  Dai un nome alla playlist (es. "Promo Inverno 2025").
4.  Trascina i media dalla libreria alla timeline in basso.
5.  Imposta la durata per le immagini (default: 10 secondi). I video useranno la loro durata naturale.
6.  Clicca **Salva**.

---

## 3. Pianificazione (Scheduling) 📅

Una volta creata la playlist, decidi quando e dove mostrarla.

### Caso d'Uso: Campagna Settimanale
*Obiettivo: Mostrare la "Promo Inverno" su tutti gli schermi dell'ingresso, dalle 9:00 alle 18:00, dal Lunedì al Venerdì.*

1.  Vai su **Schedules**.
2.  Clicca **"Nuova Schedulazione"**.
3.  **Target**: Seleziona il gruppo "Ingresso" (o singoli dispositivi).
4.  **Contenuto**: Seleziona la playlist "Promo Inverno 2025".
5.  **Orario**:
    *   Seleziona "Custom Time".
    *   Imposta Start: 09:00, End: 18:00.
    *   Giorni: Spunta Lun, Mar, Mer, Gio, Ven.
6.  Clicca **Pubblica**.
    *   *Nota: I dispositivi scaricheranno il contenuto entro pochi minuti.*

---

## 4. Risoluzione Problemi (Troubleshooting) 🛠️

**Il dispositivo risulta "Offline" ma lo schermo è acceso.**
*   Verifica che il cavo di rete/Wi-Fi sia connesso.
*   Assicurati che il servizio "CoreDisplay Agent" sia in esecuzione su Windows.

**I contenuti non si aggiornano.**
*   Controlla lo spazio libero su disco nella Dashboard. Se è pieno, il dispositivo non può scaricare nuovi file.
*   Verifica che non ci siano errori di rete nel log del dispositivo.
