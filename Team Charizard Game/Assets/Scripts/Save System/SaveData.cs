//Si occupa di salvare i dati di gioco

[System.Serializable]
public class SaveData {

 

    public float savedMasterVolume, //indica il valore del volume generale scelto dal giocatore l'ultima volta che è stato salvato
        savedMusicVolume, //indica il valore del volume della musica scelto dal giocatore l'ultima volta che è stato salvato
        savedSfxVolume; //indica il valore del volume degli effetti sonori scelto dal giocatore l'ultima volta che è stato salvato

    public int savedLanguage = 0; //indica la lingua che è stata messa l'ultima volta dal giocatore
    public int qualita = 0 ,  risoluzione=0;
    public bool fullSchermo;


    public SaveData(GameManag g)
    {
        //aggiorna i dati da salvare in base ai valori dentro GameManag
        savedMasterVolume = g.savedMasterVolume;
        savedMusicVolume = g.savedMusicVolume;
        savedSfxVolume = g.savedSfxVolume;
        savedLanguage = g.savedLanguage;
        qualita = g.qualita;
        risoluzione = g.risoluzione;
        fullSchermo = g.fullSchermo;

    }

}
