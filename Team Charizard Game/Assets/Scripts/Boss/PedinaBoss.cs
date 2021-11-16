//Si occupa del comportamento della pedina che il boss lancia
using System.Collections;
using UnityEngine;

public class PedinaBoss : MonoBehaviour
{
    //riferimento al Rigidbody della pedina da lanciare
    private Rigidbody pedinaRb = default;
    //riferimento all'Animator della pedina da lanciare
    private Animator pedinaAnim;
    //riferimento alla Mesh della pedina
    private GameObject pedinaMesh;
    //riferimento al parent iniziale della pedina
    private Transform previousParent;
    //riferimento alla rotazione che la pedina deve avere all'inizio del lancio
    private Quaternion startRotation;
    //indica quanto deve passare dopo il lancio per essere resettato allo stato originale
    [SerializeField]
    private float afterThrownTimer = 2;


    private void Awake()
    {
        //ottiene il riferimento al Rigidbody della pedina da lanciare
        pedinaRb = GetComponent<Rigidbody>();
        //ottiene il riferimento all'Animator della pedina da lanciare
        pedinaAnim = GetComponent<Animator>();

        pedinaMesh = transform.GetChild(0).gameObject;
        //ottiene il riferimento al parent iniziale della pedina
        previousParent = transform.parent;
        //ottiene il riferimento alla rotazione iniziale della pedina
        startRotation = transform.localRotation;

    }
    /// <summary>
    /// Fa comparire la pedina, per prepararla al lancio
    /// </summary>
    public void Spawn() { pedinaAnim.SetBool("Spawn", true); Debug.Log("Spawned"); }
    /// <summary>
    /// Lancia la pedina verso la direzione indicata(o la riporta allo stato originale)
    /// </summary>
    /// <param name="thrown"></param>
    /// <param name="throwVelocity"></param>
    public void Thrown(bool thrown, Vector3 throwVelocity)
    {
        //se la pedina viene lanciata, la pedina non sarà più figlia del suo parent iniziale, altrimenti lo sarà
        transform.parent = thrown ? null : previousParent;
        //fa partire l'animazione di lancio della pedina che attiverà la sua fisica
        pedinaAnim.SetBool("Spawn", false);
        pedinaAnim.SetBool("Launched", thrown);
        //disabilita l'Animator della pedina per evitare che la pedina non cambi posizione dopo il lancio
        pedinaAnim.enabled = !thrown;
        //lancia la pedina verso la direzione specificata dal parametro
        pedinaRb.velocity = throwVelocity;
        //se la pedina è stata lanciata, avvia il timer per riportarla allo stato originale
        if (thrown) { StartCoroutine(AfterThrownCD()); }

    }
    /// <summary>
    /// Riporta allo stato originale la pedina dopo aver aspettato del tempo dopo il lancio
    /// </summary>
    /// <returns></returns>
    private IEnumerator AfterThrownCD()
    {
        //aspetta un po'
        yield return new WaitForSeconds(afterThrownTimer);
        //riporta allo stato originale la pedina
        ResetPedinaState();

    }

    /// <summary>
    /// Riporta la pedina al suo stato originale
    /// </summary>
    private void ResetPedinaState()
    {

        pedinaMesh.SetActive(false);
        
        Thrown(false, Vector3.zero);

        pedinaRb.angularVelocity = Vector3.zero;

        transform.localRotation = startRotation;
    
    }

}
