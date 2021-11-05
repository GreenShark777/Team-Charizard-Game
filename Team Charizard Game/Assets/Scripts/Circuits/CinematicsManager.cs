//Si occupa di gestire le cinematiche del circuito ad inizio scena
using System.Collections.Generic;
using UnityEngine;

public class CinematicsManager : MonoBehaviour
{
    //riferimento allo script che da inizio al countdown per iniziare la corsa
    [SerializeField]
    private RaceStartCD raceStartCD = default;
    //array di tutte le cinematiche di questo circuito
    [SerializeField]
    private CircuitCinematics[] allCircuitCinematics = default;
    //lista di tutte le cinematiche di fine gara
    [SerializeField]
    private List<CircuitCinematics> allEndRaceCinematics = default;
    //lista di tutte le cinematiche di fine gara non ancora ciclate
    private List<CircuitCinematics> toCycleEndRaceCinematics = new List<CircuitCinematics>();
    //riferimento alla telecamera del giocatore
    [SerializeField]
    private Transform cam = default;
    //riferimento allo script che costringe la telecamera a seguire il giocatore
    private CameraFollow cf;
    //riferimento alla posizione iniziale della telecamera
    private Vector3 camStartPosition;
    //riferimento alla rotazione iniziale della telecamera
    private Quaternion camStartRotation;
    //indica la cinematica a cui siamo arrivati
    private int cinematicIndex = 0;
    //riferimento alla Coroutine per l'ultima cinematica che è stata fatta partire
    private Coroutine startedCinematic;


    private void Start()
    {
        //ottiene il riferimento allo script che costringe la telecamera a seguire il giocatore
        cf = cam.GetComponentInParent<CameraFollow>();
        //disabilita lo script che costringe la telecamera a seguire il giocatore
        cf.enabled = false;
        //ottiene la posizione e rotazione iniziali della telecamera
        camStartPosition = cam.localPosition;
        camStartRotation = cam.rotation;
        //fa partire la prima cinematica
        ToNextCinematic(false);
        //copia l'array di cinematiche di fine gara
        foreach (CircuitCinematics cinematic in allEndRaceCinematics) { toCycleEndRaceCinematics.Add(cinematic); }

    }

    private void Update()
    {
        //se il giocatore preme la barra spaziatrice...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //...ferma la cinematica che era stata fatta partire...
            StopCoroutine(startedCinematic);
            allCircuitCinematics[cinematicIndex - 1].enabled = false;
            //...aumenta al massimo l'indice...
            cinematicIndex = allCircuitCinematics.Length;
            //...e salta tutte le cutscene(facendo così partire il countdown per iniziare la gara)
            StartRaceCountdown();
            
        }
        
    }

    /// <summary>
    /// Fa partire la cinematica immediatamente successiva
    /// </summary>
    /// <param name="endRace"></param>
    public void ToNextCinematic(bool endRace)
    {
        //disabilita lo script che costringe la telecamera a seguire il giocatore, se è ancora attivo per qualche motivo
        if(cf.enabled) cf.enabled = false;
        //se non devono essere le cinematiche di fine gara, fa partire le cinematiche di inizio gara
        if (!endRace)
        {
            //se l'indice di cinematiche non è oltre il range di cinematiche in lista...
            if (cinematicIndex < allCircuitCinematics.Length)
            {
                //...fa partire la prossima cinematica e ne prende riferimento...
                startedCinematic = StartCoroutine(allCircuitCinematics[cinematicIndex].StartCinematic(cam));
                //...e incrementa l'indice per la prossima cinematica
                cinematicIndex++;

            } //altrimenti, fa partire il countdown per iniziare la gara
            else { StartRaceCountdown(); }

        }
        else //altrimenti, bisogna far partire quelli nell'array delle cinematiche di fine gara, quindi...
        {
            //...prende un indice casuale tra 0 e il numero di cinematiche rimanenti...
            int n = Random.Range(0, cinematicIndex);
            //Debug.Log(n);
            //...fa partire la cinematica scelta casualmente...
            StartCoroutine(toCycleEndRaceCinematics[n].StartCinematic(cam));
            //...diminuisce l'indice di cinematiche disponibili...
            cinematicIndex--;
            //...rimuove dalla lista la cinematica appena partita...
            toCycleEndRaceCinematics.RemoveAt(n);
            //...e, se si è arrivati all'ultima cinematica...
            if (cinematicIndex == 0)
            {
                //...riporta l'indice e la lista ai valori originali(facendo così ripartire il ciclo)
                cinematicIndex = allEndRaceCinematics.Count;
                foreach (CircuitCinematics cinematic in allEndRaceCinematics) { toCycleEndRaceCinematics.Add(cinematic); }
            
            }
            
        }

    }
    /// <summary>
    /// Prepara la scena per far partire il countdown di inizio gara
    /// </summary>
    private void StartRaceCountdown()
    {
        //se questo script è ancora attivo, viene fatto partire il countdown
        if (enabled)
        {
            //riporta la telecamera alla sua posizione e rotazione iniziali
            cam.localPosition = camStartPosition;
            cam.rotation = camStartRotation;
            //fa partire il countdown per iniziare la gara
            raceStartCD.enabled = true;
            //riabilita lo script che costringe la telecamera a seguire il giocatore
            cf.enabled = true;
            //prepara l'indice di cinematiche per il ciclo di cinematiche di fine gara
            cinematicIndex = allEndRaceCinematics.Count;
            //disabilita questo script
            enabled = false;

        }

    }

}
