﻿//Si occupa del countdown per far iniziare la gara, e attiva e disattiva i vari script che devono partire solo quando il countdown è terminato
using System.Collections;
using UnityEngine;

public class RaceStartCD : MonoBehaviour
{
    //riferimento allo script che tiene conto del tempo dall'inizio della gara corsa
    [SerializeField]
    private RaceTimer raceTimer = default;
    //riferimento allo script di movimento del kart del giocatore
    [SerializeField]
    private PlayerKartCtrl kartCtrl = default;

    //riferimenti alle mesh dei giudici di gara(i ragazzi semaforo)
    [SerializeField]
    private MeshRenderer redBoyMR = default,
        yellowBoyMR = default,
        greenBoyMR = default;

    //riferimenti ai materiali da dare ai giudici di gara per attivarli
    [SerializeField]
    private Material activeRedBoy = default,
        activeYellowBoy = default,
        activeGreenBoy = default;

    [SerializeField]
    private float startCD = 2, //indica quanto tempo bisogna aspettare prima che il giocatore possa iniziare a caricare il boost iniziale
        timeBetweenActivation = 1, //indica quanto tempo deve passare tra l'attivazione di un giudice di gara ad un altro
        flyUpSpeed = 20; //indica quanto velocemente tutti i giudici nella piattaforma salgono dopo che la corsa inizia

    //indica se la gara è iniziata o meno
    private bool raceBegun = false;


    private void Awake()
    {
        //fa in modo che il giocatore non possa muoversi fino alla fine del countdown
        kartCtrl.enabled = false;
        //si disabilita, per aspettare che la cinematica di inizio gara finisca
        enabled = false;

    }

    private void Start()
    {
        //fa partire la coroutine per il countdown all'inizio della gara
        StartCoroutine(StartRaceCountdown());

    }

    private void FixedUpdate()
    {
        //se la gara è iniziata...
        if (raceBegun)
        {
            //...continua a salire in sù fino a quando i giudici nella piattaforma non saranno più nella visuale del giocatore
            transform.position = new Vector3(transform.position.x, transform.position.y + (flyUpSpeed * Time.deltaTime),
                transform.position.z);

        }

    }

    /// <summary>
    /// Si occupa delle tempistiche di inizio gara
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartRaceCountdown()
    {
        Debug.LogError("Start Countdown");
        //aspetta tot secondi
        yield return new WaitForSeconds(startCD);
        //permette al giocatore di caricare il boost iniziale per la gara
        kartCtrl.enabled = true;
        //aspetta un altro po'
        yield return new WaitForSeconds(startCD / 2);
        //attiva il giudice di gara rosso
        ActivateTrafficLightBoy(0);
        //aspetta un certo intervallo
        yield return new WaitForSeconds(timeBetweenActivation);
        //attiva il giudice di gara giallo
        ActivateTrafficLightBoy(1);
        //aspetta un certo intervallo
        yield return new WaitForSeconds(timeBetweenActivation);
        //attiva il giudice di gara verde
        ActivateTrafficLightBoy(2);
        //attiva il timer di gara
        raceTimer.enabled = true;
        //comunica che la gara è iniziata
        raceBegun = true;
        //infine, comunica al giocatore che la gara è iniziata e potrà guidare
        kartCtrl.RaceBegun();
        Debug.LogError("Race Begun");
        //aspetta un po'
        yield return new WaitForSeconds(startCD);
        //infine, disattiva la piattaforma con i giudici
        gameObject.SetActive(false);
        Debug.LogError("Deactivated");
    }
    /// <summary>
    /// Attiva uno dei giudici di gara in base al valore ricevuto
    /// </summary>
    /// <param name="boyToActivate"></param>
    private void ActivateTrafficLightBoy(int boyToActivate)
    {
        //in base al valore ottenuto, attiva uno dei giudici di gara
        switch (boyToActivate)
        {
            //GIUDICE ROSSO
            case 0: { redBoyMR.material = activeRedBoy; break; }
            //GIUDICE GIALLO
            case 1: { yellowBoyMR.material = activeYellowBoy; break; }
            //GIUDICE VERDE
            case 2: { greenBoyMR.material = activeGreenBoy; break; }
            //VALORE ERRATO
            default: { Debug.LogError("Valore errato, non esiste un altro giudice di gara!"); break; }

        }

    }

}