//Si occupa del movimento e controlli del giocatore e il suo kart
//Ispirato dal video di Ishaan35:https://www.youtube.com/watch?v=q0cUClufuKE&t=16s
using UnityEngine;

public class PlayerKartCtrl : MonoBehaviour
{
    //RIFERIMENTI
    [Header("References")]
    
    /*
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
    */

    //riferimenti al contenitore dei particellari di drift
    [SerializeField]
    private Transform driftsPSContainer = default;
    //colori che i particellari di drift devono avere in base allo stadio del drift
    [SerializeField]
    private Color weakDriftColor = default, //inizio drift
        mediumDriftColor = default, //drift medio
        finalDriftColor = default; //drift lungo

    //riferimento al particellare di boost dopo un drift
    [SerializeField]
    private Transform boostPS = default;
    //riferimento al kart
    private Transform kart;
    //riferimento al Rigidbody del giocatore
    private Rigidbody kartRb;
    //riferimento all'Animator del kart
    private Animator kartAnim;
    //riferimento al manager delle cinematiche
    [SerializeField]
    private CinematicsManager cm = default;

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
    //indica per quanto tempo deve rimanere in aria il giocatore per ricevere un boost
    [SerializeField]
    private float onAirTimerForBoost = 2;
    //timer che indica per quanto tempo il giocatore è rimasto in aria
    private float notOnGroundTimer;
    //indica per quanto tempo il kart deve rimanere in boost dopo essere atterrato da un luogo alto
    [SerializeField]
    private float highLandBoostTime = 1.25f;
    //indica l'indice del layer del terreno da controllare
    [SerializeField]
    private LayerMask groundLayer = 8;
    //indica quanto velocemente si deve ruotare il giocatore per mettere le ruote del kart sempre per terra anche quando il terreno è inclinato
    [SerializeField]
    private float rotationSpeed = 7.5f;

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
    //indica in quanto tempo il kart raggiunge la velocità massima di boost
    [SerializeField]
    private float boostAcceleration = 1;

    /*
    //indica quanto velocemente il giocatore deve ruotare nella direzione in cui il katr è rivolto durante un boost
    [SerializeField]
    private float onBoostRotationSpeed = 10;
    */

    //indicano le durate di boost...
    [SerializeField]
    private float weakBoostTime = 0.75f, //...quando si sta poco tempo in drift...
        mediumBoostTime = 1.5f, //...quando si sta abbastanza in drift...
        finalBoostTime = 2.5f; //...e quando si sta molto in drift

    //indica per quanto tempo ancora il giocatore deve rimanere in boost
    private float boostTime;

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
    //private float startWheelsYRotation;

    //VARIABILI PER IL SALTO
    [Header("Jump")]
    //indica quanto in alto salterà il giocatore
    [SerializeField]
    private float jumpForce = 20;
    //indica se il giocatore ha già effettuato un salto o meno
    private bool jumped = false;
    //indica se il giocatore deve aspettare l'inizio della corsa(e deve caricare il boost iniziale)
    [SerializeField]
    private bool prepareToBegin = true;

    [Header("Start Boost Charge")]
    //indica quanto ha caricato il boost il giocatore
    private float chargedBoost = 0;
    
    [SerializeField]
    private float maxBoostCharge = 2.5f, //indica il valore massimo che la carica di boost può avere
        chargeBoostIncreaseRate = 0.1f; //indica quanto velocemente si ricarica il boost iniziale


    private void Awake()
    {
        //ottiene il riferimento al kart
        kart = transform.GetChild(0);
        //ottiene il riferimento al Rigidbody del giocatore
        kartRb = GetComponent<Rigidbody>();
        //ottiene il riferimento all'Animator del kart
        kartAnim = GetComponent<Animator>();

        /*
        //ottiene il riferimento ai figli delle ruote frontali
        childFrontDxTire = frontRightTire.GetChild(0);
        childFrontSxTire = frontLeftTire.GetChild(0);
        //ottiene la rotazione nell'asse Y delle ruote frontali
        startWheelsYRotation = frontRightTire.localEulerAngles.y;
        */
    }

    private void Update()
    {
        //se non è in atto il countdown dela corsa, effettua i vari controlli
        if (!prepareToBegin)
        {
            //controlla se il giocatore preme un bottone per muovere il kart
            KartMovement();
            //controlla la direzione verso cui deve andare il kart in base agli Input del giocatore
            Steer();
            //controlla la rotazione che il kart deve avere in base all'inclinazione del terreno(controlla anche se si sta toccando terra o meno)
            GroundCheck();
            //controlla se il giocatore sta tenendo premuto il tasto di drift e cambia il suo movimento di conseguenza
            Drift();
            //controlla se 
            Boost();

            //controlla la direzione verso cui devono ruotare le ruote del kart
            //TireSteer();

            //controlla se il giocatore vuole saltare e, se preme il tasto, si occupa del salto
            Jump();
            //se la corsa è finita...
            if (FinishLine.RaceFinished)
            {
                //...disattiva questo script, impedendo al giocatore di muoversi...
                enabled = false;
                //...attiva la cinematica di fine gara...
                cm.ToNextCinematic(true);

                //ATTIVA LO SCRIPT DEL GIOCATORE PER FARLO MUOVERE CON AI

            }

        } //altrimenti, controlla se il giocatore sta premendo il tasto per caricare il boost iniziale
        else { StartBoostCharge(); }

    }
    /// <summary>
    /// Si occupa del movimento del kart tramite Input del giocatore
    /// </summary>
    private void KartMovement()
    {
        //ottiene la velocità reale in cui il kart si sta muovendo
        realSpeed = transform.InverseTransformDirection(kartRb.velocity).z;
        //ottiene il movimento che il giocatore intende fare(1 : destra, 0 : niente, -1 : sinistra)
        float movementDir = Input.GetAxisRaw("Vertical");
        //se il giocatore preme il tasto d'accelerazione, la sua velocità aumenterà per tutto il tempo fino ad arrivare alla velocità massima
        if (movementDir > 0) { currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * acceleration); }
        //se il giocatore preme il tasto di decelerazione, la sua velocità diminuirà fino ad arrivare alla velocità massima in retromarcia
        else if (movementDir < 0) { currentSpeed = Mathf.Lerp(currentSpeed, maxBackwardsSpeed, Time.deltaTime); }
        //altrimenti, se non sta premendo nessuno dei 2 tasti, continua a decelerare fino a fermarsi
        else { currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * deceleration); }
        //crea un vettore per calcolare la velocità che il kart dovrà avere
        Vector3 newVel = transform.forward * currentSpeed;
        //aggiorna il valore y a quello attuale del kart(altrimenti perderemmo la forza di gravità che agisce sul kart)
        newVel.y = kartRb.velocity.y;
        //aggiorna la velocity al nuovo valore ottenuto
        kartRb.velocity = newVel;

    }
    /// <summary>
    /// Controlla se si sta sterzando e, se si sta sterzando, ruota il kart e cambia la direzione in cui il giocatore deve andare
    /// </summary>
    private void Steer()
    {
        //ottiene la direzione in cui il giocatore sta sterzando, che potrà solo essere uno di questi 3 numeri: -1, 0, 1
        steerDirection = Input.GetAxisRaw("Horizontal");
        //indica la nuova rotazione da dare al kart
        Quaternion newKartRotation;
        //indica la rotazione attuale del kart prima dell'aggiornamento di rotazione
        Quaternion actualKartRotation = kart.localRotation;
        //se si sta driftando verso sinistra, e non destra...
        if (driftLeft && !driftRight)
        {
            //...ricalcola la direzione verso cui si sta sterzando, dando più rotazione se va sta sterzando a sinistra o meno se sta sterzando a destra...
            steerDirection = Input.GetAxis("Horizontal") < 0 ? -maxSteerInDrift : -minSteerInDrift;
            //...ruota il kart nell'asse Y fino ad arrivare al valore massimo impostato per il drift sinistro
            newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, -maxDriftSteer, 0), driftSteerSpeed * Time.deltaTime);
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
            newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, maxDriftSteer, 0), driftSteerSpeed * Time.deltaTime);
            //...se si sta scivolando e si sta toccando terra, al kart viene aggiunta una forza acceleratrice verso la parte opposta(forza centrifuga)
            if (isSliding && touchingGround)
                kartRb.AddForce(transform.right * -outwardsDriftForce * Time.deltaTime, ForceMode.Acceleration);

        } //altrimenti non si sta driftando verso nessun lato, quindi ripotra il kart alla rotazione iniziale
        else { newKartRotation = Quaternion.Lerp(actualKartRotation, Quaternion.Euler(0, 0f, 0), maxDriftSteer * Time.deltaTime); }
        //dopo tutti i controlli e calcoli, aggiorna la rotazione del kart
        kart.localRotation = newKartRotation;
        //calcola quanto si può sterzare in base alla velocità a cui il giocatore sta andando(maggiore la velocità, maggiore la resistenza allo sterzamento)
        float steerAmount = (realSpeed > steerResistSpeed) ? realSpeed / maxResist * steerDirection 
                                                           : realSpeed / minResist * steerDirection;

        //infine, calcola e ruota il kart nella direzione in cui si sta sterzando
        Vector3 steerDirVect = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount, transform.eulerAngles.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirVect, steerSpeed * Time.deltaTime);

    }
    /// <summary>
    /// Controlla se il giocatore sta toccando per terra e, se lo è, ruota il suo kart in base alla pendenza del terreno
    /// </summary>
    private void GroundCheck()
    {
        //crea un RayCast
        RaycastHit hit;
        //fa partire il raycast dal centro del kart e lo fa andare verso sotto, se entro la distanza impostata c'è del terreno...
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundDistance, groundLayer))
        {
            //...ruota il kart in base alla pendenza dell'oggetto su cui si è...
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * 
                transform.rotation, rotationSpeed * Time.deltaTime);
            //se si è rimasti in aria per abbastanza tempo, il giocatore riceve un boost...
            if (notOnGroundTimer >= onAirTimerForBoost) { /*boostTime = highLandBoostTime;*/ SetBoostTime(highLandBoostTime); }
            //Debug.Log(notOnGroundTimer);
            //...viene resettato il timer di caduta...
            notOnGroundTimer = 0;
            //...se non si stava toccando terra, comunica che si può nuovamente saltare...
            if (!touchingGround) { jumped = false; }
            //...e, comunica che si sta toccando terra
            touchingGround = true;

        } //altrimenti...
        else
        {
            //...comunica che non si sta toccando terra...
            touchingGround = false;
            //...e aumenta il tempo in cui si sta rimanendo in aria
            notOnGroundTimer += Time.deltaTime;
        
        }

    }
    /// <summary>
    /// Controlla se il giocatore sta driftando e, se lo è, si occupa della sua fisica in questo stato
    /// </summary>
    private void Drift()
    {
        //se si preme il tasto di drift mentre si è per terra...
        if (Input.GetButtonDown("Drift") && touchingGround)
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

        } //altrimenti, se non si è a terra...
        else if (!touchingGround)
        {
            //...vengono fermati tutti i particellari di drift
            for (int i = 0; i < driftsPSContainer.transform.childCount; i++)
            {

                ParticleSystem DriftPS = driftsPSContainer.GetChild(i).GetComponent<ParticleSystem>();
                DriftPS.Stop();

            }

        }
        //se si sta continuando a tenere premuto il tasto, siamo per terra, la nostra velocità non è sotto il minimo e si sta ancora sterzando...
        if (Input.GetButton("Drift") && touchingGround && currentSpeed > minSpeedForDrift && Input.GetAxis("Horizontal") != 0)
        {
            //...aumenta il tempo in cui stiamo continuando il drift...
            driftTime += Time.deltaTime;
            //...crea una variabile che indica il colore che il particellare di drift deve avere...
            Color newPSColor = Color.gray;
            //se siamo in drift da meno di tot secondi per il drift medio, il colore di drift cambia al colore di drift debole
            if (driftTime >= driftFirstStageTimer && driftTime < driftSecondStageTimer) { newPSColor = weakDriftColor; }
            //se siamo in drift da più di tot secondi per il drift iniziale ma non abbastanza da essere al boost finale, il colore di drift cambia al colore di drift medio
            if (driftTime >= driftSecondStageTimer && driftTime < driftFinalStageTimer) { newPSColor = mediumDriftColor; }
            //se siamo in drift da abbastanza tempo, il colore di drift cambia al colore di drift finale
            if (driftTime >= driftFinalStageTimer) { newPSColor = finalDriftColor; }
            //...dopo tutti i controlli...
            for (int i = 0; i < driftsPSContainer.childCount; i++)
            {
                //...ottiene il riferimento al particellare di drift del figlio ciclato nel contenitore di particellari...
                ParticleSystem driftPS = driftsPSContainer.GetChild(i).GetComponent<ParticleSystem>();
                ParticleSystem.MainModule PSMAIN = driftPS.main;
                //...cambia il colore degli effetti particellari di drift al colore ottenuto...
                PSMAIN.startColor = newPSColor;
                //...e, se non è partito, fa partire gli effetti particellari di drift
                if (!driftPS.isPlaying) { driftPS.Play(); }

            }

        }
        //se non si sta più premendo il tasto di drift o siamo sotto la soglia minima di velocità per continuare il boost...
        if (!Input.GetButton("Drift") || realSpeed < minSpeedForDrift)
        {
            //comunica che non si sta più driftando...
            driftLeft = false;
            driftRight = false;
            isSliding = false;

            //...da il boost in base a quanto tempo siamo stati in drift...
            if (driftTime > driftFirstStageTimer && driftTime < driftSecondStageTimer)
            {  /*boostTime = weakBoostTime;*/ SetBoostTime(weakBoostTime); } //boost debole
            if (driftTime >= driftSecondStageTimer && driftTime < driftFinalStageTimer)
            { /*boostTime = mediumBoostTime;*/ SetBoostTime(mediumBoostTime); } //boost medio
            if (driftTime >= driftFinalStageTimer)
            { /*boostTime = finalBoostTime;*/ SetBoostTime(finalBoostTime); } //boost forte

            //...riporta a 0 il tempo in cui siamo stati in drift...
            driftTime = 0;
            //...e spegne tutti i particellari di drift
            for (int i = 0; i < driftsPSContainer.transform.childCount; i++)
            {

                ParticleSystem DriftPS = driftsPSContainer.GetChild(i).GetComponent<ParticleSystem>();
                DriftPS.Stop();

            }

        }

    }
    /// <summary>
    /// Controlla se il giocatore deve essere in boost e, se lo è, cambia la velocità massima a cui può andare a quella di boost
    /// </summary>
    private void Boost()
    {
        //continua a diminuire il tempo di boost
        boostTime -= Time.deltaTime;
        //se il tempo di boost non è ancora a 0 o meno...
        if (boostTime > 0)
        {
            //...attiva i particellari di boost, nel caso non lo siano già
            for (int i = 0; i < boostPS.childCount; i++)
            {

                ParticleSystem cycledBoostPS = boostPS.GetChild(i).GetComponent<ParticleSystem>();

                if (!cycledBoostPS.isPlaying) { cycledBoostPS.Play(); }

            }
            //...e cambia la velocità attuale dandogli come velocità massima quella di boost
            currentSpeed = Mathf.Lerp(currentSpeed, boostSpeed, boostAcceleration * Time.deltaTime);

            //...infine, ruota lentamente la direzione in cui il giocatore deve andare in base alla direzione in cui il kart è rivolto
            //transform.rotation = Quaternion.Lerp(transform.rotation, kart.rotation, onBoostRotationSpeed * Time.deltaTime);

            //Debug.Log(currentSpeed);
        } //altrimenti, essendo finito il boost...
        else
        {
            //...ferma tutti i particellari di boost, se non lo sono già
            for (int i = 0; i < boostPS.childCount; i++)
            {

                ParticleSystem cycledBoostPS = boostPS.GetChild(i).GetComponent<ParticleSystem>();

                if (cycledBoostPS.isPlaying) { cycledBoostPS.Stop(); }

            }

        }

    }

    /*
    /// <summary>
    /// Si occupa di ruotare le ruote del kart del giocatore
    /// </summary>
    private void TireSteer()
    {
        //crea un vettore che indicherà la rotazione che le ruote frontali dovranno avere
        Vector3 newTireRotation = frontLeftTire.localEulerAngles;
        //se si preme la freccia sinistra per andare a sinistra, le ruote frontali ruoteranno verso sinistra
        if (steerDirection < 0)
        { newTireRotation = Vector3.Lerp(newTireRotation, new Vector3(0, maxLeftSteer, 0), tireSteerSpeed * Time.deltaTime); }
        //se si preme la freccia destra per andare a destra, le ruote frontali ruoteranno verso destra
        else if (steerDirection > 0)
        { newTireRotation = Vector3.Lerp(newTireRotation, new Vector3(0, maxRightSteer, 0), tireSteerSpeed * Time.deltaTime); }
        //se non si sta premendo nessun tasto direzionale, quindi riporta le ruote alla rotazione originale
        else
        { newTireRotation = Vector3.Lerp(newTireRotation, new Vector3(0, startWheelsYRotation, 0), tireSteerSpeed * Time.deltaTime); }
        //dopo tutti i calcoli e controlli, assegna la nuova rotazione alle ruote frontali
        frontLeftTire.localEulerAngles = newTireRotation;
        frontRightTire.localEulerAngles = newTireRotation;
        //Debug.Log(frontLeftTire.localEulerAngles + " : " + newTireRotation);

        //tire spinning

        //se si supera la velocità minima per far ruotare le ruote, le fa ruotare in base alla velocità corrente
        if (currentSpeed > minSpeedForSpinning)
        {
            childFrontSxTire.Rotate(0, 90 * Time.deltaTime * currentSpeed * spinSpeed, 0);
            childFrontDxTire.Rotate(0, 90 * Time.deltaTime * currentSpeed * spinSpeed, 0);
            backLeftTire.Rotate(-90 * Time.deltaTime * currentSpeed * spinSpeed, 0, 0);
            backRightTire.Rotate(-90 * Time.deltaTime * currentSpeed * spinSpeed, 0, 0);

        } //altrimenti, ruoteranno a velocità reale
        else
        {
            childFrontSxTire.Rotate(0, 90 * Time.deltaTime * realSpeed * spinSpeed, 0);
            childFrontDxTire.Rotate(0, 90 * Time.deltaTime * realSpeed * spinSpeed, 0);
            backLeftTire.Rotate(-90 * Time.deltaTime * realSpeed * spinSpeed, 0 , 0);
            backRightTire.Rotate(-90 * Time.deltaTime * realSpeed * spinSpeed, 0, 0);

        }

    }
    */

    /// <summary>
    /// Fa saltare il giocatore, se preme il tasto di salto
    /// </summary>
    public void Jump()
    {
        //se viene premuto il tasto di salto...
        if (Input.GetButtonDown("Jump") && touchingGround && !jumped)
        {
            //...rimuove ogni forza che agisce sul kart...
            kartRb.velocity = Vector3.zero;
            //...il giocatore salterà un po' in aria...
            kartRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            //...e verrà comunicato che è stato effettuato un salto
            jumped = true;

        }

    }
    /// <summary>
    /// Comunica all'intero script che si sta scivolando(si sta driftando). Questo metodo viene richiamato dall'Animator
    /// </summary>
    public void SetSliding() { isSliding = true; }
    /// <summary>
    /// Ritorna il tempo di boost
    /// </summary>
    /// <returns></returns>
    public float GetBoostTime() { return boostTime; }
    /// <summary>
    /// Permette di impostare il tempo di boost
    /// </summary>
    /// <param name="newBoostTime"></param>
    private void SetBoostTime(float newBoostTime) { boostTime = newBoostTime; }
    /// <summary>
    /// Ritorna la velocità reale a cui la macchina sta andando
    /// </summary>
    /// <returns></returns>
    public float GetRealSpeed() { return realSpeed; }
    /// <summary>
    /// Carica il boost iniziale quando il giocatore preme il tasto per andare avanti
    /// </summary>
    private void StartBoostCharge()
    {
        //se il giocatore preme(non tiene premuto) il tasto per andare avanti...
        if (Input.GetButtonDown("Vertical"))
        {
            //...ricarica il boost iniziale fino al massimo consentito...
            chargedBoost = Mathf.Clamp(chargedBoost + chargeBoostIncreaseRate, 0, maxBoostCharge);
            //...e cicla ogni particellare di drift nel contenitore in modo da...
            for (int i = 0; i < driftsPSContainer.childCount; i++)
            {
                //... ottenerne riferimento temporaneo...
                ParticleSystem driftPS = driftsPSContainer.GetChild(i).GetComponent<ParticleSystem>();
                //...attivarlo se non lo è già...
                if (!driftPS.isPlaying) { driftPS.Play(); }
                //...ottenere riferimento al suo MainModule...
                ParticleSystem.MainModule driftPSMain = driftPS.main;
                //...e cambiarne il colore in base a quanto è stato caricato fino ad ora il boost iniziale
                if (chargedBoost >= finalBoostTime) { driftPSMain.startColor = finalDriftColor; }
                else if (chargedBoost >= mediumBoostTime) { driftPSMain.startColor = mediumDriftColor; }
                else if (chargedBoost >= weakBoostTime) { driftPSMain.startColor = weakDriftColor; }

            }
            Debug.Log("Charged Boost: " + chargedBoost);
        }

    }
    /// <summary>
    /// Si occupa di ciò che deve accadere quando la gara inizia
    /// </summary>
    public void RaceBegun()
    {
        //se il boost iniziale caricato è maggiore di almeno il minore dei boost...
        if (chargedBoost >= weakBoostTime)
        {
            //...fa partire un boost che dura quanto il boost caricato
            boostTime = chargedBoost;

        }
        //infine, comunica allo script che la gara è cominciata
        prepareToBegin = false;

    }
    /// <summary>
    /// Ferma il kart e resetta le sue variabili di movimento. Se viene passato false, il Rigidbody diverrà kinematico e non sarà affetto da gravità e collisioni
    /// </summary>
    /// <param name="affected"></param>
    public void IsAffectedByGravity(bool affected)
    {
        //se il kart deve essere affetto da gravità il Rigidbody del giocatore rimane invariato, altrimenti diventa kinematico
        kartRb.isKinematic = !affected;
        //se non deve essere affetto da gravità, rimuove ogni forza che agisce su esso
        /*if (!affected) {*/ kartRb.velocity = Vector3.zero; //}
        //resetta il timer che controlla se fare boost una volta arrivati per terra
        notOnGroundTimer = 0;
        //fa in modo che il kart non abbia boost, ni caso di rimasugli
        boostTime = 0;
        //resetta la velocità a cui il kart stava andando
        realSpeed = 0;
        currentSpeed = 0;
        //evita che si attivi il boost, nel caso il gioactore stesse driftando quando stava cadendo
        driftTime = 0;

    }
    /*USARE, INVECE DI QUESTO, "IsAffectedByGravity"(quello qua sopra)
    public void STOOOOp()//by rob
    {
        kartRb.velocity = Vector3.zero;
    }*/

    private void OnDrawGizmos()
    {
        //mostra fin dove arriva il RayCast per il controllo che si sta toccando a terra
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z));

    }


}
