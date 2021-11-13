using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class enemyCarHealth : MonoBehaviour
{
    [SerializeField]
    private float health; //variabile da assegnare


    private float startHealth;
    
    private float healthRef 
    {

        get { return health; }

        set
        {
            if(healthRef >3)
            {
                sparks.SetActive(false);
                sparks2.SetActive(false);
                
            }

            health = value;

            if (healthRef >= 2 && healthRef < 3)
            {
                part1.transform.parent = null;
                part1.isKinematic = false;

                //attiva un gameObject (in questo caso sparks)
                sparks.SetActive(true);

            }
            

            if (healthRef < 2 && healthRef >= 1)
            {
                //attiva un gameObject (in questo caso sparks2)
                sparks2.SetActive(true);

            }

            if (healthRef <= 1)
            {
                isKillable = true;
                
                
                lifeBar.sprite = explosion;

                lifeBar.fillAmount += 5;


            }

            if(healthRef < -3f)
            {

                Debug.Log("explode");

            }

            

        }
    
    } //variabile che cambierà
    [SerializeField]
    private GameObject sparks;
    [SerializeField]
    private GameObject sparks2;
    [SerializeField]
    private Image lifeBar;
    [SerializeField]
    private Rigidbody part1;
    [SerializeField]
    private Sprite explosion;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator carAnimator;
    [SerializeField]
    private Transform kart;

    private Vector3 rightKartPosition;
    bool isKillable;
    float actualSpeed;
    Animator anim;
    bool isExecuted;
    private void Start()
    {
        rightKartPosition = kart.localPosition;
        carAnimator.enabled = false;
        actualSpeed = agent.speed;
        //imposta starthealt = al valore di health in modo da avere sempre un riferimento alla salute massima
        startHealth = health;
        //prende automaticamente il riferimento all animator
        anim = lifeBar.GetComponent<Animator>();
        healthRef = health; //imposto la variabile uguale a quella assegnata nell inspector
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(slowDown(3));

            StartCoroutine(showLifeBar());
            //diminuisce la vita di 1
            healthRef -= 1;
            //diminuisce il fill amount di 03 per cambiare lo slider
            
        }

        if (Input.GetKeyDown(KeyCode.G) && isKillable == false)
        {
            //diminuisce il fill amount di 03 per cambiare lo slider
            lifeBar.fillAmount -= 0.3f;
        }

        if (healthRef < 1 && isKillable)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 20)
            {
                //parte animazione fadeOUT
                anim.SetBool("fade", true);
                if (Input.GetKeyDown(KeyCode.K) && isExecuted == false)
                {
                    StartCoroutine(Esecuzione());
                    Debug.Log("esecuzione");

                }

            }
            else
            {
                //parte animazione fadeIN
                anim.SetBool("fade", false);

            }


        }



    }


    IEnumerator showLifeBar()
    {
        //fa partire l animazione di fadeIN
        anim.SetBool("fade", true);
        //aspetta 2 secondi
        yield return new WaitForSeconds(2);
        //fa partire l animazione di fadeOUT
        anim.SetBool("fade", false);
        


    }

    public IEnumerator slowDown(float dmg)
    {
        healthRef -= dmg;
        //imposta la velocità a 0
        agent.speed = 0;
        yield return new WaitForSeconds(1f);

        //rimette la velocità a quella iniziale
        agent.speed = actualSpeed;


    }


    IEnumerator Esecuzione()
    {
        //rende la variabile true per evitare che possa essere richiamata più volte
        isExecuted = true;
        //attiva l animator
        carAnimator.enabled = true;
        //passaggio da idle alla substateMachine
        carAnimator.SetBool("death", true);
        //setta questa variabile random, in modo da far partire un animazione random
        carAnimator.SetInteger("deathIndex", Random.Range(0, 4));
        //aspetta 3 secondi
        yield return new WaitForSeconds(1.5f);
        
        carAnimator.SetInteger("deathIndex", Random.Range(0, 4));
        //rende la variaible falsa per tornare all idle
        carAnimator.SetBool("death", false);
        //disattiva l animator
        carAnimator.enabled = false;
        //permette di ripetere tutto da capo
        isExecuted = false;
        //riposiziona il kart alla giusta posizione
        kart.localPosition = rightKartPosition;

        Debug.Log("fineEsecuzione");


    }


}
