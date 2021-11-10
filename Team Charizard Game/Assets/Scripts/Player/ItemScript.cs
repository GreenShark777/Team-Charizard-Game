using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    //costante che indica il numero di oggetti esistenti nel gioco
    private const int N_ITEMS = 5;
    //indica se il giocatore ha un oggetto
    private bool hasItem = false;
    //riferimento al contenitore degli oggetti
    [SerializeField]
    private Transform itemsContainer = default;
    //array di riferimenti agli oggetti che il giocatore può usare
    [SerializeField]
    private GameObject[] itemsGameobject = new GameObject[N_ITEMS];
    //riferimento al contenitore degli sprite degli oggetti
    [SerializeField]
    private Transform spritesContainer = default;
    //array di riferimenti agli sprite degli oggetti
    [SerializeField]
    private GameObject[] itemsSprites = new GameObject[N_ITEMS];

    //riferimento all'immagine di cui bisogna cambiare lo sprite per comunicare al giocatore l'oggetto ottenuto
    //[SerializeField]
    //private  Image yourSprite = default;

    //riferimento all'Animator della UI degli oggetti
    [SerializeField]
    private Animator itemUIAnim = default;
    //indica l'oggetto ottenuto dal giocatore
    private int obtainedItemIndex;
    //indica quanto tempo deve passare dall'inizio dell'animazione di scelta alla sua fine
    [SerializeField]
    private float chooseTimer = 4;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene i riferimenti agli oggetti che il giocatore può usare quando li ottiene
        for (int i = 0; i < N_ITEMS; i++) { itemsGameobject[i] = itemsContainer.GetChild(i).gameObject; }
        //ottiene i riferimenti alle immagini nella UI degli oggetti che il giocatore può usare
        for (int j = 0; j < N_ITEMS; j++) { itemsSprites[j] = spritesContainer.GetChild(j).gameObject; }

    }

    // Update is called once per frame
    void Update()
    {
        //se il giocatore ha un oggetto, controlla se lo vuole usare
        if(hasItem) useItem();


        //DEBUG_______OTTIENE OGGETTO
        else if(Input.GetKeyDown(KeyCode.M)) { StartCoroutine(getItem()); }

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ItemBox")
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false; //question mark

            other.transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false; //box
            other.transform.GetChild(2).GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = false; //box

            other.gameObject.GetComponent<Animator>().SetBool("Enlarge", false); //reset to start process
                                                                                 //StartCoroutine(getItem());
                                                                                 // ItemUIAnim.SetBool("ItemIn", true);
                                                                                 //ItemUiScroll.SetBool("Scroll", true);
            Debug.Log("Triggerato");
            //re-enable
           // yield return new WaitForSeconds(1);
            other.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true; //question mark
            for (int i = 1; i < 3; i++)
            {
                other.transform.GetChild(2).GetChild(i).GetComponent<SkinnedMeshRenderer>().enabled = true; //box
            }
           // other.gameObject.GetComponent<Animator>().SetBool("Enlarge", true);  //show the item box spawning with animation, even though it was already there
            other.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

    }*/
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger");

        //se si collide con una cassa oggetto, la cassa viene disabilitata per un po' e il giocatore ottiene un oggetto casuale
        if (other.CompareTag("ItemBox")) { StartCoroutine(UsedUpCD(other)); }

    }

    public IEnumerator getItem()
    {
        //se non si ha già un oggetto...
        if (!hasItem)
        {
            //...prende casualmente un indice che indicherà l'oggetto ottenuto dal giocatore...
            obtainedItemIndex = Random.Range(0, N_ITEMS);
            //...fa partire l'animazione di scelta della UI di oggetto ottenuto...
            itemUIAnim.enabled = true;
            itemUIAnim.SetBool("ChoosingItem", true);
            //...aspetta un po'...
            yield return new WaitForSeconds(chooseTimer);
            //...ferma l'animazione di scelta oggetti della UI...
            itemUIAnim.SetBool("HasItem", true);

            //itemUIAnim.enabled = false;
            //yield return new WaitForSeconds(0.2f);

            //...mostra lo sprite dell'oggetto ottenuto...
            ShowObtainedItemSprite();
            //...e comunica che il giocatore ha un oggetto e non può ottenerne altri fino a quando non lo usa
            hasItem = true;
            Debug.Log("Oggetto ottenuto: " + obtainedItemIndex);
        }

    }

    private void useItem()
    {
        //se il giocatore preme il tasto di uso oggetti...
        if (Input.GetButtonDown("UseItem"))
        {
            //...comunica che il giocatore non ha più un oggetto...
            hasItem = false;
            //...riporta l'Animator allo stato iniziale...
            itemUIAnim.SetBool("HasItem", false);
            itemUIAnim.SetBool("ChoosingItem", false);
            //itemUIAnim.enabled = false;

            // ItemUIAnim.SetBool("ItemIn", false);
            // ItemUiScroll.SetBool("Scroll", false);
            //itemGameobjects[index].SetActive(false);

            //...attiva l'oggetto ottenuto...
            //itemsGameobject[obtainedItemIndex].SetActive(true);

            //...porta l'oggetto usato alla posizione del giocatore
            itemsGameobject[obtainedItemIndex].transform.position = transform.position;
            Debug.Log("Oggetto usato: " + obtainedItemIndex);
        }

    }

    private IEnumerator UsedUpCD(Collider other)
    {
        //ottiene come riferimento il collider della cassa oggetti ottenuta
        Collider itemBoxColl = other.GetComponent<BoxCollider>();
        //disattiva il collider della cassa oggetti
        itemBoxColl.enabled = false;

        //other.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false; //question mark
        //other.transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false; //box
        //other.transform.GetChild(2).GetChild(2).GetComponent<SkinnedMeshRenderer>().enabled = false; //box
        //other.gameObject.GetComponent<Animator>().SetBool("Enlarge", false); //reset to start process

        //fa partire la coroutine per far ottenere un oggetto al giocatore
        StartCoroutine(getItem());

        // ItemUIAnim.SetBool("ItemIn", true);
        //ItemUiScroll.SetBool("Scroll", true);
        Debug.Log("dentro");
        //re-enable

        //aspetta un po' di tempo
        yield return new WaitForSeconds(1);
        //infine, riattiva il collider della cassa oggetti previamente usata
        itemBoxColl.enabled = true;

        //other.transform.GetChild(1).GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true; //question mark

        /*
        for (int i = 1; i < 3; i++)
        {
            other.transform.GetChild(2).GetChild(i).GetComponent<SkinnedMeshRenderer>().enabled = true; //box
        }
        */

        // other.gameObject.GetComponent<Animator>().SetBool("Enlarge", true);  //show the item box spawning with animation, even though it was already there

    }
    /// <summary>
    /// Mostra solo lo sprite dell'oggetto appena ottenuto
    /// </summary>
    private void ShowObtainedItemSprite()
    {
        //disattiva tutti gli sprite degli oggetti ottenibili dal giocatore tranne quello appena ottenuto
        //for (int index = 0; index < N_ITEMS; index++) { itemsSprites[obtainedItemIndex].SetActive(index == obtainedItemIndex); if (index == obtainedItemIndex) { Debug.LogError(index); } }

        itemUIAnim.SetInteger("ObtainedItem", obtainedItemIndex);

    }

}
