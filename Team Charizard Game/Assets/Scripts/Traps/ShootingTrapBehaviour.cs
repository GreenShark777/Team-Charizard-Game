//Si occupa del comportamento delle trappole che sparano un proiettile continuamente
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrapBehaviour : MonoBehaviour
{
    //riferimento al proiettile da sparare
    private Transform projectile;
    //riferimento al rigidbody del proiettile
    private Rigidbody projectileRb;

    //riferimento al Rigidbody del proiettile
    //private Rigidbody projectileRb = default;

    //lista di tutti i proiettili
    private List<Transform> projectiles = new List<Transform>();
    //riferimento alla posizione in cui il proiettile deve essere quando deve essere sparato
    private Vector3 startProjectilePos;

    [SerializeField]
    private float projectileSpeed = 2, //indica quanto velocemente deve andare il proiettile
        reshootTimer = 2; //indica quanto tempo deve passare prima che questa trappola possa nuovamente sparare

    //indica se questa trappola che spara usa più proiettili o meno
    [SerializeField]
    private bool multipleProjectiles = true;
    //indica quale proiettile nella lista bisogna sparare
    private int shootIndex = -1;


    private void Awake()
    {
        //ottiene il riferimento al primo figlio della trappola che può essere o un proiettile o un contenitore di proiettili
        Transform firstChild = transform.GetChild(0);
        //se questa trappola usa più proiettili, ottiene il riferimento a tutti i proiettili nel contenitore
        if (multipleProjectiles) { foreach (Transform childProjectile in firstChild) { projectiles.Add(childProjectile); } }
        //altrimenti, ottiene il riferimento all'unico proiettile da sparare e il suo Rigidbody
        else { projectile = firstChild; projectileRb = firstChild.GetComponent<Rigidbody>(); }
        //ottiene la posizione in cui il proiettile deve essere quando deve essere sparato
        startProjectilePos = firstChild.position;
        //se la velocità impostata del proiettile è maggiore di 0, viene impostata al valore negativo del valore
        if (projectileSpeed > 0) { projectileSpeed = -projectileSpeed; }

    }

    void Start()
    {
        //fa partire la coroutine di sparo
        StartCoroutine(Shoot());
        
    }

    private IEnumerator Shoot()
    {
        //crea un riferimento locale al proiettile da sparare
        Transform toShoot;
        //se non spara più proiettili, prende come riferimento l'unico proiettile
        if (!multipleProjectiles) { toShoot = projectile; }
        //altrimenti...
        else
        {
            //...incrementa l'indice...
            shootIndex++;
            //...se l'indice sfora dal limite della lista, lo riporta a 0...
            if (shootIndex >= projectiles.Count) { shootIndex = 0; }
            //...e prende come riferimento il proiettile all'indice specificato
            toShoot = projectiles[shootIndex];

        }
        //ottiene il riferimento al Rigidbody del proiettile da sparare
        Rigidbody toShootRb = (!multipleProjectiles) ? projectileRb : toShoot.GetComponent<Rigidbody>();
        //resetta ogni forza che agisce sul proiettile
        toShootRb.velocity = Vector3.zero;
        //riporta il proiettile alla posizione iniziale
        toShoot.position = startProjectilePos;
        //spinge il proiettile verso la direzione in cui deve essere sparato
        toShootRb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

        /*
        //resetta ogni forza che agisce sul proiettile
        projectileRb.velocity = Vector3.zero;
        //riporta il proiettile alla posizione iniziale
        projectile.position = startProjectilePos;
        //spinge il proiettile verso la direzione in cui deve essere sparato
        projectileRb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
        */

        yield return new WaitForSeconds(reshootTimer);
        //fa ripartire la stessa coroutine, facendo in modo che il loop non si interrompa mai
        StartCoroutine(Shoot());

    }

}
