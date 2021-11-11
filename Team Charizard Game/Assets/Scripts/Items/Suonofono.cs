//Si occupa del comportamente dell'oggetto suonofono
using System.Collections;
using UnityEngine;

public class Suonofono : MonoBehaviour, IUsableItem
{
    //riferimento all'Animator del suonofono
    private Animator suonofonoAnim;
    //riferimento ai collider dell'oggetto
    private Collider suonofonoColls;

    [SerializeField]
    private float onActivationTimer = 0.5f, //indica dopo quanto tempo, all'attivazione, deve iniziare l'animazione di uso
        deactivateTimer = 1; //indica dopo quanto tempo dall'attivazione dell'attacco, deve essere disattivato quest'oggetto


    private void Awake()
    {
        //ottiene il riferimento all'Animator del suonofono
        suonofonoAnim = GetComponent<Animator>();
        //ottiene il riferimento ai collider dell'oggetto
        suonofonoColls = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        //se i collider dell'oggetto colpiscono un nemico, lo spinge
        if (other.CompareTag("Enemy")) { PushAway(other.transform); }

    }

    public void UseThisItem()
    {
        //fa partire la coroutine d'attacco del suonofono
        StartCoroutine(AttackTiming());

    }

    private IEnumerator AttackTiming()
    {
        //fa partire l'animazione di inizio attacco
        suonofonoAnim.SetBool("StartAttack", true);
        //aspetta un po'
        yield return new WaitForSeconds(onActivationTimer);
        //attiva i collider di attacco dell'oggetto
        suonofonoColls.enabled = true;
        //aspetta un po'
        yield return new WaitForSeconds(deactivateTimer);
        //fa fermare l'animazione dell'oggetto
        suonofonoAnim.SetBool("StartAttack", false);
        //disattiva l'oggetto
        gameObject.SetActive(false);
        //disattiva i collider di attacco dell'oggetto
        suonofonoColls.enabled = false;

    }

    private void PushAway(Transform enemy)
    {

        Debug.Log("Spinto nemico: " + enemy);

    }

}
