//Si occupa del timer di gara
using UnityEngine;
using UnityEngine.UI;

public class RaceTimer : MonoBehaviour
{
    //riferimento al testo da cambiare in base al tempo
    private Text timeSpentText;
    //indicano i numeri che devono esserci nel testo
    private int seconds, //SECONDI
        minutes; //MINUTI
    private float milliseconds; //MILLISECONDI
    //stringa del tempo speso per finire la corsa
    private string timeSpent;


    void Start()
    {
        //ottiene il riferimento al testo da cambiare in base al tempo
        timeSpentText = GetComponent<Text>();

    }

    private void Update()
    {
        //aumenta continuamente i millisecondi
        milliseconds += Time.deltaTime * 100;
        //se si arriva al massimo numero di millisecondi per fare un secondo, incrementa i secondi e riporta a 0 i millisecondi
        if (milliseconds >= 100) { seconds++; milliseconds = 0; }
        //se si arriva al massimo numero di secondi per fare un minuto, incrementa i minuti e riporta a 0 i secondi
        if (seconds >= 60) { minutes++; seconds = 0; }
        //converte i secondi in testo, e aggiunge uno zero dietro nel caso i secondi non siano 10 o più
        string secondsInText = (seconds < 10) ? "0" + seconds : "" + seconds;
        //converte i millisecondi in testo, e aggiunge uno zero dietro nel caso i millisecondi non siano 10 o più
        string millisecondsInText = ((int)milliseconds < 10) ? "0" + (int)milliseconds : "" + (int)milliseconds;
        //aggiorna la stringa in base ai minuti, secondi e millisecondi dall'inizio della gara
        timeSpent = minutes + ":" + secondsInText + "." + millisecondsInText.Substring(0, millisecondsInText.Length - 1);
        //se si arriva al massimo numero consentito di minuti, secondi e millisecondi...
        if (minutes == 99 && seconds == 59 && milliseconds >= 99)
        {
            //...il testo, se esiste, diventa rosso...
            if(timeSpentText) timeSpentText.color = Color.red;
            //...e il timer smette di andare avanti
            enabled = false;

        }
        //se esiste, aggiorna il testo del tempo impiegato
        if(timeSpentText) timeSpentText.text = timeSpent;


        //DEBUG---------------------------------------------------------------------------------------------------------------------------------
        //if (Input.GetKeyDown(KeyCode.K)) { minutes++; }
        //if (Input.GetKeyDown(KeyCode.L)) { seconds++; }

    }
    /// <summary>
    /// Ritorna il testo del tempo che si è impiegato nella gara
    /// </summary>
    /// <returns></returns>
    public string GetRaceTimeText() { return timeSpent; }

    public int GetMinutes() { return minutes; }
    public int GetSeconds() { return seconds; }
    public int GetMilliseconds() { return (int)milliseconds; }

}
