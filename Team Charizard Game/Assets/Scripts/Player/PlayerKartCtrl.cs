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
    //riferimento al kart
    private Transform kart;
    //riferimento al Rigidbody del giocatore
    private Rigidbody kartRb;
    //riferimento all'Animator del kart
    private Animator kartAnim;

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

    [SerializeField]
    private float maxSteerInDrift = 1.5f, //indica il valore massimo di sterzata in drift verso la direzione in cui si sta driftando
        minSteerInDrift = 0.5f, //indica il valore massimo di sterzata in drift verso la direzione opposta in cui si sta driftando
        maxDriftSteer = 20, //indica la rotazione massima di sterzamento in drift
        driftSteerSpeed = 8, //indica la velocità di sterzamento in drift
        steerResistSpeed = 30, //indica a quale velocità di movimento il kart deve iniziare a sterzare più lentamente
        maxResist = 4, //indica quanta resistenza viene applicata allo sterzamento ad alta velocità
        minResist = 1.5f; //indica quanta resistenza viene applicata allo sterzamento a bassa velocità

    //indica la direzione in cui si sta sterzando
    private float steerDirection;
    //variabili di stato del kart
    private bool driftRight, //indica che sta driftando verso destra
        driftLeft, //indica che sta driftando verso sinistra
        isSliding; //indica che sta strisciando per il drift

    [Header("Ground Detection")]
    //indica quanto distante deve essere il terreno per indicare che si sta toccando per terra
    [SerializeField]
    private float groundDistance = 0.75f;
    //indica che si sta toccando per terra
    private bool touchingGround;

    //VARIABILI DRIFT
    [Header("Drift")]
    //indica quanto il drift influenzi l'andamento del kart
    [SerializeField]
    private float outwardsDriftForce = 50000;
    //indica per quanto tempo il giocatore sta rimanendo in drift
    private float driftTime;

    [SerializeField]
    private float minSpeedForDrift = 40, //indica a quale velocità si può driftare
        driftFirstStageTimer = 1.5f, //indica dopo quanto tempo dall'inizio del drift inizia il primo stadio del boost
        driftSecondStageTimer = 4, //indica dopo quanto tempo dall'inizio del drift inizia il secondo stadio del boost
        driftFinalStageTimer = 7; //indica dopo quanto tempo dall'inizio del drift inizia l'ultimo stadio del boost

    [Header("Boost")]
    //indica per quanto tempo ancora il giocatore deve rimanere in boost
    public float BoostTime;
    //indica in quanto tempo il kart raggiunge la velocità massima di boost
    [SerializeField]
    private float boostAcceleration = 1;

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

    //indica se il giocatore sta volando in glide
    //public bool GLIDER_FLY;


    private void Awake()
    {
        //ottiene il riferimento al kart
        kart = transform.GetChild(0);
        //ottiene il riferimento al Rigidbody del giocatore
        kartRb = GetComponent<Rigidbody>();
        //ottiene il riferimento all'Animator del kart
        kartAnim = GetComponent<Animator>();
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

        //se si sta driftando verso sinistra, e non destra...
        if (driftLeft && !driftRight)
        {
            //...ricalcola la direzione verso cui si sta sterzando, dando più rotazione se va sta sterzando a sinistra o meno se sta sterzando a destra...
            steerDirection = Input.GetAxis("Horizontal") < 0 ? -maxSteerInDrift : -minSteerInDrift;
            //...ruota il kart nell'asse Y fino ad arrivare al valore massimo impostato per il drift sinistro
            kart.localRotation = Quaternion.Lerp(kart.localRotation, Quaternion.Euler(0, -maxDriftSteer, 0), driftSteerSpeed * Time.deltaTime);

            //...se si sta scivolando e si sta toccando terra, al kart viene aggiunta una forza acceleratrice verso la parte opposta(forza centrifuga)
            if (isSliding && touchingGround)
                kartRb.AddForce(transform.right * outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);

        }
        //altrimenti, se si sta driftando verso destra, e non sinistra...
        else if (driftRight && !driftLeft)
        {
            //...ricalcola la direzione verso cui si sta sterzando, dando più rotazione se va sta sterzando a destra o meno se sta sterzando a sinistra...
            steerDirection = Input.GetAxis("Horizontal") > 0 ? maxSteerInDrift : minSteerInDrift;
            //...ruota il kart nell'asse Y fino ad arrivare al valore massimo impostato per il drift destro
            kart.localRotation = Quaternion.Lerp(kart.localRotation, Quaternion.Euler(0, maxDriftSteer, 0), driftSteerSpeed * Time.deltaTime);

            //...se si sta scivolando e si sta toccando terra, al kart viene aggiunta una forza acceleratrice verso la parte opposta(forza centrifuga)
            if (isSliding && touchingGround)
                kartRb.AddForce(transform.right * -outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);
        } //altrimenti non si sta driftando verso nessun lato, quindi ripotra il kart alla rotazione iniziale
        else
        {
            kart.localRotation = Quaternion.Lerp(kart.localRotation, Quaternion.Euler(0, 0f, 0), maxDriftSteer * Time.deltaTime);
        }

        //since handling is supposed to be stronger when car is moving slower, we adjust steerAmount depending on the real speed of the kart, and then rotate the kart on its y axis with steerAmount

        //calcola quanto si può sterzare in base alla velocità a cui il giocatore sta andando(maggiore la velocità, maggiore la resistenza allo sterzamento)
        float steerAmount = (realSpeed > steerResistSpeed) ? realSpeed / maxResist * steerDirection 
                                                           : realSpeed / minResist * steerDirection;

        //infine, calcola e ruota il kart nella direzione in cui si sta sterzando
        Vector3 steerDirVect = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount, transform.eulerAngles.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirVect, steerSpeed * Time.deltaTime);

    }

    private void GroundNormalRotation()
    {
        //crea un RayCast
        RaycastHit hit;
        //fa partire il raycast dal centro del kart e lo fa andare verso sotto, se entro la distanza impostata c'è qualcosa...
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundDistance))
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
            kartAnim.SetTrigger("Hop");

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
        if (Input.GetKey(KeyCode.V) && touchingGround && currentSpeed > minSpeedForDrift && Input.GetAxis("Horizontal") != 0)
        {
            //...aumenta il tempo in cui stiamo continuando il drift...
            driftTime += Time.deltaTime;

            //particle effects (sparks)

            //se siamo in drift da meno di tot secondi per il drift medio...
            if (driftTime >= driftFirstStageTimer && driftTime < driftSecondStageTimer)
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

                    }

                }
            }
            //altrimenti, se siamo in drift da più di tot secondi per il drift iniziale ma non abbastanza da essere al boost finale...
            if (driftTime >= driftSecondStageTimer && driftTime < driftFinalStageTimer)
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
            if (driftTime >= driftFinalStageTimer)
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
        if (!Input.GetKey(KeyCode.V) || realSpeed < minSpeedForDrift)
        {
            //comunica che non si sta più driftando...
            driftLeft = false;
            driftRight = false;
            isSliding = false;


            //...da il boost in base a quanto tempo siamo stati in drift...
            if (driftTime > driftFirstStageTimer && driftTime < driftSecondStageTimer)
            {
                BoostTime = 0.75f; //boost debole
            }
            if (driftTime >= driftSecondStageTimer && driftTime < driftFinalStageTimer)
            {
                BoostTime = 1.5f; //boost medio

            }
            if (driftTime >= driftFinalStageTimer)
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
            currentSpeed = Mathf.Lerp(currentSpeed, boostSpeed, boostAcceleration * Time.deltaTime);
            Debug.Log(currentSpeed);
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
    /// <summary>
    /// Comunica all'intero script che si sta scivolando(si sta driftando). Questo metodo viene richiamato dall'Animator
    /// </summary>
    public void SetSliding() { isSliding = true; }

}
