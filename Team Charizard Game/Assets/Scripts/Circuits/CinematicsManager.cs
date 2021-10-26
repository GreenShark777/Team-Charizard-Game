//Si occupa di gestire le cinematiche del circuito ad inizio scena
using UnityEngine;

public class CinematicsManager : MonoBehaviour
{
    //riferimento allo script che da inizio al countdown per iniziare la corsa
    [SerializeField]
    private RaceStartCD raceStartCD = default;
    //array di tutte le cinematiche di questo circuito
    [SerializeField]
    private CircuitCinematics[] allCircuitCinematics = default;
    //riferimento alla telecamera del giocatore
    [SerializeField]
    private Transform cam = default;
    //riferimento alla posizione iniziale della telecamera
    private Vector3 camStartPosition;
    //indica la cinematica a cui siamo arrivati
    private int cinematicIndex = 0;
    //riferimento alla Coroutine per l'ultima cinematica che è stata fatta partire
    private Coroutine startedCinematic;


    private void Start()
    {
        //disabilita lo script che costringe la telecamera a seguire il giocatore
        cam.GetComponentInParent<CameraFollow>().enabled = false;
        //ottiene la posizione iniziale della telecamera
        camStartPosition = cam.localPosition;
        //fa partire la prima cinematica
        ToNextCinematic();

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
    public void ToNextCinematic()
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
    /// <summary>
    /// Prepara la scena per far partire il countdown di inizio gara
    /// </summary>
    private void StartRaceCountdown()
    {
        //se questo script è ancora attivo, viene fatto partire il countdown
        if (enabled)
        {
            //riporta la telecamera alla sua posizione iniziale
            cam.localPosition = camStartPosition;
            //fa partire il countdown per iniziare la gara
            raceStartCD.enabled = true;
            //riabilita lo script che costringe la telecamera a seguire il giocatore
            cam.GetComponentInParent<CameraFollow>().enabled = true;
            //disabilita questo script
            enabled = false;

        }

    }

}
