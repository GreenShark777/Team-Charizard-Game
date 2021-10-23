//Si occupa del movimento e controlli del giocatore e il suo kart
//Ispirato dal video di Ishaan35:https://www.youtube.com/watch?v=q0cUClufuKE&t=16s
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKartCtrl : MonoBehaviour
{
    //RIFERIMENTI
    [Header("References")]
    //riferimenti alle ruote del kart
    [SerializeField]
    private Transform frontRightTire = default;
    [SerializeField]
    private Transform frontLeftTire = default,
        backRightTire = default,
        backLeftTire = default;

    //riferimenti ai figli delle ruote frontali
    private Transform childFrontSxTire,
        childFrontDxTire;

    //riferimenti ai contenitori dei particellari di drift...
    [SerializeField]
    private Transform leftDrift, //...sinistro...
        rightDrift; //...e destro
    //PROVARE A FARE CON UN SOLO RIFERIMENTO

    //colori che i particellari di drift devono avere in base allo stadio del drift
     [SerializeField]
    private Color drift1, //inizio drift
        drift2, //drift medio
        drift3; //drift lungo

    //riferimento al particellare di boost dopo un drift
    [SerializeField]
    private Transform boostPS = default;

    //riferimento al Rigidbody del giocatore
    private Rigidbody kartRb;

    //VARIABILI DI MOVIMENTO
    [Header("Movement")]
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
    //indica la velocità attuale del giocatore
    private float currentSpeed = 0;
    //indica la velocità reale a cui il giocatore sta andando(nel caso che collide con un muro o è rallentato da qualcosa)
    private float realSpeed;

    //VARIABILI DI STERZAMENTO DEL KART
    [Header("Kart Steer")]
    //indica quanto velocemente ruotano le ruote del kart
    [SerializeField]
    private float steerSpeed = 3;
    //indica la direzione in cui si sta sterzando
    private float steerDirection;
    //variabili di stato del kart
    private bool driftRight, //indica che sta driftando verso destra
        driftLeft, //indica che sta driftando verso sinistra
        isSliding, //indica che sta strisciando per il drift
        touchingGround; //indica che sta toccando per terra

    //VARIABILI DRIFT
    [Header("Drift")]
    //indica quanto il drift influenzi l'andamento del kart
    [SerializeField]
    private float outwardsDriftForce = 50000;

    private float driftTime;

    //VARIABILI DI STERZAMENTO DELLE RUOTE
    [Header("Tire Steer")]
    //indica quanto velocemente le ruote ruotano verso la direzione in cui devono puntare
    [SerializeField]
    private float tireSteerSpeed = 5;
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

    public bool GLIDER_FLY;

    public float BoostTime;


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
        //controlla la direzione verso cui deve andare il kart in base agli Input del giocatore
        Steer();
        //controlla la rotazione che il kart deve avere in base all'inclinazione del terreno(controlla anche se si sta toccando terra o meno)
        GroundNormalRotation();
        //controlla se il giocatore sta tenendo premuto il tasto di drift e cambia il suo movimento di conseguenza
        Drift();
        //controlla se 
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
        //ottiene la direzione in cui il giocatore sta sterzando, che potrà solo essere uno di questi 3 numeri: -1, 0, 1
        steerDirection = Input.GetAxisRaw("Horizontal");

        //Vector3 steerDirVect; //this is used for the final rotation of the kart for steering

        //float steerAmount;


        if (driftLeft && !driftRight)
        {
            steerDirection = Input.GetAxis("Horizontal") < 0 ? -1.5f : -0.5f;
            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, -20f, 0), 8f * Time.deltaTime);


            if (isSliding && touchingGround)
                kartRb.AddForce(transform.right * outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);
        }
        else if (driftRight && !driftLeft)
        {
            steerDirection = Input.GetAxis("Horizontal") > 0 ? 1.5f : 0.5f;
            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 20f, 0), 8f * Time.deltaTime);

            if (isSliding && touchingGround)
                kartRb.AddForce(transform.right * -outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
        }

        //since handling is supposed to be stronger when car is moving slower, we adjust steerAmount depending on the real speed of the kart, and then rotate the kart on its y axis with steerAmount
        float steerAmount = realSpeed > 30 ? realSpeed / 4 * steerDirection : steerAmount = realSpeed / 1.5f * steerDirection;

        //infine, calcola e ruota il kart nella direzione in cui si sta sterzando
        Vector3 steerDirVect = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount, transform.eulerAngles.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirVect, 3 * Time.deltaTime);

    }

    private void GroundNormalRotation()
    {
        //crea un RayCast
        RaycastHit hit;
        //fa partire il raycast dal centro del kart e lo fa andare verso sotto, se entro la distanza impostata c'è qualcosa...
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.75f)) //0.75 E' TROPPO POCO, AUMENTARE
        {
            //...ruota il kart in base alla pendenza dell'oggetto su cui si è...
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 7.5f * Time.deltaTime);
            //...e comunica che si sta toccando terra
            touchingGround = true;

        } //altrimenti, comunica che non si sta toccando terra
        else { touchingGround = false; }

    }

    private void Drift()
    {
        //se si preme il tasto di drift mentre si è per terra...
        if (Input.GetKeyDown(KeyCode.V) && touchingGround)
        {

            //...fa partire l'animazione di inizio drift...
            //transform.GetChild(0).GetComponent<Animator>().SetTrigger("Hop");

            //controlla se si sta sterzando verso destra...
            if (steerDirection > 0)
            {
                driftRight = true;
                driftLeft = false;
            } //...o verso sinistra
            else if (steerDirection < 0)
            {
                driftRight = false;
                driftLeft = true;
            }
        }
        //se si sta continuando a tenere premuto il tasto, siamo per terra, la nostra velocità non è sotto il minimo e si sta ancora sterzando...
        if (Input.GetKey(KeyCode.V) && touchingGround /*&& currentSpeed > 40*/ && Input.GetAxis("Horizontal") != 0)
        {
            //...aumenta il tempo in cui stiamo continuando il drift...
            driftTime += Time.deltaTime;

            Debug.Log("DriftTime = " + driftTime);
            //particle effects (sparks)

            //se siamo in drift da meno di tot secondi per il drift medio...
            if (driftTime >= 1.5 && driftTime < 4)
            {
                //...cambia il colore degli effetti particellari di drift al colore 1(quello di inizio)...
                for (int i = 0; i < leftDrift.childCount; i++)
                {
                    ParticleSystem DriftPS = rightDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>(); //right wheel particles
                    ParticleSystem.MainModule PSMAIN = DriftPS.main;

                    ParticleSystem DriftPS2 = leftDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>(); //left wheel particles
                    ParticleSystem.MainModule PSMAIN2 = DriftPS2.main;

                    PSMAIN.startColor = drift1;
                    PSMAIN2.startColor = drift1;

                    //...e, se non sono partiti, fa partire gli effetti particellari di drift
                    if (!DriftPS.isPlaying && !DriftPS2.isPlaying)
                    {
                        DriftPS.Play();
                        DriftPS2.Play();
                        Debug.Log("Fatti partire PS di drift");
                    }

                }
            }
            //altrimenti, se siamo in drift da più di tot secondi per il drift iniziale ma non abbastanza da essere al boost finale...
            if (driftTime >= 4 && driftTime < 7)
            {
                //drift color particles

                //...cambia il colore degli effetti particellari di drift al colore 2(quello di mezzo)...
                for (int i = 0; i < leftDrift.childCount; i++)
                {
                    ParticleSystem DriftPS = rightDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule PSMAIN = DriftPS.main;
                    ParticleSystem DriftPS2 = leftDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule PSMAIN2 = DriftPS2.main;
                    PSMAIN.startColor = drift2;
                    PSMAIN2.startColor = drift2;


                }

            }
            //altrimenti siamo in drift da abbastanza per poter...
            if (driftTime >= 7)
            {
                //...cambiare il colore degli effetti particellari di drift al colore 3(quello finale)...
                for (int i = 0; i < leftDrift.childCount; i++)
                {

                    ParticleSystem DriftPS = rightDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule PSMAIN = DriftPS.main;
                    ParticleSystem DriftPS2 = leftDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule PSMAIN2 = DriftPS2.main;
                    PSMAIN.startColor = drift3;
                    PSMAIN2.startColor = drift3;

                }
            }
        }
        //se non si sta più premendo il tasto di drift o siamo sotto la soglia minima di velocità per continuare il boost...
        if (!Input.GetKey(KeyCode.V) || realSpeed < 40)
        {
            //comunica che non si sta più driftando...
            driftLeft = false;
            driftRight = false;
            isSliding = false;


            //...da il boost in base a quanto tempo siamo stati in drift...
            if (driftTime > 1.5 && driftTime < 4)
            {
                BoostTime = 0.75f; //boost debole
            }
            if (driftTime >= 4 && driftTime < 7)
            {
                BoostTime = 1.5f; //boost medio

            }
            if (driftTime >= 7)
            {
                BoostTime = 2.5f; //boost finale

            }

            //...riporta a 0 il tempo in cui siamo stati in drift...
            driftTime = 0;
            //...e spegne tutti i particellari di drift
            for (int i = 0; i < rightDrift.transform.childCount; i++)
            {
                ParticleSystem DriftPS = rightDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>(); //right wheel particles
                ParticleSystem.MainModule PSMAIN = DriftPS.main;

                ParticleSystem DriftPS2 = leftDrift.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>(); //left wheel particles
                ParticleSystem.MainModule PSMAIN2 = DriftPS2.main;

                
                DriftPS.Stop();
                DriftPS2.Stop();
                
            }
        }

    }

    private void Boost()
    {
        //continua a diminuire il tempo di boost
        BoostTime -= Time.deltaTime;
        //se il tempo di boost non è ancora a 0 o meno...
        if (BoostTime > 0)
        {
            //...attiva i particellari di boost, nel caso non lo siano già
            for (int i = 0; i < boostPS.childCount; i++)
            {
                if (!boostPS.GetChild(i).GetComponent<ParticleSystem>().isPlaying)
                {
                    boostPS.GetChild(i).GetComponent<ParticleSystem>().Play();
                }

            }
            
            //maxSpeed = boostSpeed;

            //...e cambia la velocità attuale dandogli come velocità massima quella di boost
            currentSpeed = Mathf.Lerp(currentSpeed, boostSpeed, 1 * Time.deltaTime);

        } //altrimenti, essendo finito il boost...
        else
        {
            //...ferma tutti i particellari di boost, se non lo sono già
            for (int i = 0; i < boostPS.childCount; i++)
            {
                if(boostPS.GetChild(i).GetComponent<ParticleSystem>().isPlaying) boostPS.GetChild(i).GetComponent<ParticleSystem>().Stop();
            }
            //maxSpeed = boostSpeed - 20;
        }

    }

    private void TireSteer()
    {
        //se si preme la freccia sinistra per andare a sinistra, le ruote frontali ruoteranno verso sinistra
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, maxLeftSteer, 0), tireSteerSpeed * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, maxLeftSteer, 0), tireSteerSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, maxRightSteer, 0), tireSteerSpeed * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, maxRightSteer, 0), tireSteerSpeed * Time.deltaTime);
        }
        else
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, startWheelsYRotation, 0), tireSteerSpeed * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, startWheelsYRotation, 0), tireSteerSpeed * Time.deltaTime);
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
