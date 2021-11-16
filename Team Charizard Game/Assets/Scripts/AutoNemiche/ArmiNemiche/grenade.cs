using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour, IGiveDamage
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float explosionSpeed;
    [SerializeField]
    private Collider explosionCollider;
    [SerializeField]
    private ParticleSystem explosion;
    [SerializeField]
    private float dmg;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(countDown());
        // aggiunge una forza al gameOBj per lanciarla
        this.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator explode()
    {
        explosion.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("CattoBOMBAAA");
            //other.GetComponent<enemyCarHealth>().slowDown(3);
            StartCoroutine(explode());
        }

        if (other.CompareTag("Player"))
        {

            other.GetComponent<PlayerHealth>().ChangeHealth(-3);
            StartCoroutine(explode());
        }


    }

    IEnumerator countDown()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(explode());


    }

    public float GiveDamage()
    {
        return dmg;
    }
}
