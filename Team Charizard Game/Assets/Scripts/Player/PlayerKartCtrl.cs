//Si occupa del movimento e controlli del giocatore e il suo kart
//Ispirato dal video di Ishaan35:https://www.youtube.com/watch?v=q0cUClufuKE&t=16s
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKartCtrl : MonoBehaviour
{
    //RIFERIMENTI
    [Header("References")]
    //riferimento al Rigidbody del giocatore
    private Rigidbody kartRb;
    //riferimenti alle ruote del kart
    [SerializeField]
    private Transform frontRightTire = default,
        frontLeftTire = default,
        backRightTire = default,
        backLeftTire = default;

    //riferimenti ai figli delle ruote frontali
    private Transform childFrontSxTire,
        childFrontDxTire;

    //VARIABILI DI MOVIMENTO
    [Header("Movement")]
    //indica la velocità attuale del giocatore
    private float currentSpeed = 0;
    //indica quanto velocemente il kart arriva a velocità massima
    [SerializeField]
    private float acceleration = 1;
    //indica la velocità massima a cui il giocatore può andare
    [SerializeField]
    private float maxSpeed = default;
    //indica quanto velocemente il giocatore si muove in retromarcia
    [SerializeField]
    private float maxBackwardsSpeed = default;
    [SerializeField]
    //indica quanto veloce andrà il giocatore mentre è in turbo
    private float boostSpeed = default;
    //indica quanto velocemente decelera il kart quando non si preme il tasto per andare avanti
    [SerializeField]
    private float deceleration = default;
    //indica la velocità reale a cui il giocatore sta andando(nel caso che collide con un muro o è rallentato da qualcosa)
    private float realSpeed;

    //VARIABILI DI STERZAMENTO DELLE RUOTE
    [Header("Tire Steer")]
    //indica quanto velocemente le ruote ruotano verso la direzione in cui devono puntare
    [SerializeField]
    private float steerSpeed = 5;
    //indica quanto velocemente le ruote ruotano mentre il kart va avanti o dietro
    [SerializeField]
    private float spinSpeed = 0.5f;
    //indica quanto veloce deve andare il kart per far ruotare le ruote
    [SerializeField]
    private float minSpeedForSpinning = 30;
    //indicano quanto possono ruotare nell'asse Y le ruote del kart verso...
    [SerializeField]
    private float maxRightSteer = 205, //...destra...
        maxLeftSteer = 155; //...e sinistra

    //indica la rotazione Y iniziale delle ruote
    private float startWheelsYRotation;


    private void Awake()
    {
        //ottiene il riferimento al Rigidbody del giocatore
        kartRb = GetComponent<Rigidbody>();
        //ottiene il riferimento ai figli delle ruote frontali
        childFrontDxTire = frontRightTire.GetChild(0);
        childFrontSxTire = frontLeftTire.GetChild(0);
        //ottiene la rotazione nell'asse Y delle ruote frontali
        startWheelsYRotation = frontRightTire.localEulerAngles.y;

    }

    private void FixedUpdate()
    {
        //controlla se il giocatore preme un bottone per muovere il kart
        KartMovement();

        Steer();

        GroundNormalRotation();

        Drift();

        Boost();
        //controlla la direzione verso cui devono ruotare le ruote del kart
        TireSteer();

    }
    /// <summary>
    /// Si occupa del movimento del kart tramite Input del giocatore
    /// </summary>
    private void KartMovement()
    {
        //ottiene la velocità reale in cui il kart si sta muovendo
        realSpeed = transform.InverseTransformDirection(kartRb.velocity).z;
        //se il giocatore preme il tasto d'accelerazione, la sua velocità aumenterà per tutto il tempo fino ad arrivare alla velocità massima
        if (Input.GetKey(KeyCode.W)) { currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * acceleration); }
        //se il giocatore preme il tasto di decelerazione, la sua velocità diminuirà fino ad arrivare alla velocità massima in retromarcia
        else if (Input.GetKey(KeyCode.S)) { currentSpeed = Mathf.Lerp(currentSpeed, maxBackwardsSpeed, Time.deltaTime); }
        //altrimenti, se non sta premendo nessuno dei 2 tasti, continua a decelerare fino a fermarsi
        else { currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * deceleration); }
        //crea un vettore per calcolare la velocità che il kart dovrà avere
        Vector3 newVel = transform.forward * currentSpeed;
        //aggiorna il valore y a quello attuale del kart(altrimenti perderemmo la forza di gravità che agisce sul kart)
        newVel.y = kartRb.velocity.y;
        //aggiorna la velocity al nuovo valore ottenuto
        kartRb.velocity = newVel;

    }

    private void Steer()
    {

    }

    private void GroundNormalRotation()
    {

    }

    private void Drift()
    {

    }

    private void Boost()
    {

    }

    private void TireSteer()
    {
        //se si preme la freccia sinistra per andare a sinistra, le ruote frontali ruoteranno verso sinistra
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, maxLeftSteer, 0), steerSpeed * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, maxLeftSteer, 0), steerSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, maxRightSteer, 0), steerSpeed * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, maxRightSteer, 0), steerSpeed * Time.deltaTime);
        }
        else
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, startWheelsYRotation, 0), steerSpeed * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, startWheelsYRotation, 0), steerSpeed * Time.deltaTime);
        }

        //tire spinning

        if (currentSpeed > minSpeedForSpinning)
        {
            childFrontSxTire.Rotate(0, 90 * Time.deltaTime * currentSpeed * spinSpeed, 0);
            childFrontDxTire.Rotate(0, 90 * Time.deltaTime * currentSpeed * spinSpeed, 0);
            backLeftTire.Rotate(-90 * Time.deltaTime * currentSpeed * spinSpeed, 0, 0);
            backRightTire.Rotate(-90 * Time.deltaTime * currentSpeed * spinSpeed, 0, 0);
        }
        else
        {
            childFrontSxTire.Rotate(0, 90 * Time.deltaTime * realSpeed * spinSpeed, 0);
            childFrontDxTire.Rotate(0, 90 * Time.deltaTime * realSpeed * spinSpeed, 0);
            backLeftTire.Rotate(-90 * Time.deltaTime * realSpeed * spinSpeed, 0 , 0);
            backRightTire.Rotate(-90 * Time.deltaTime * realSpeed * spinSpeed, 0, 0);
        }

    }

}
