//Si occupa dell'uso e ottenimento degli oggetti da parte del giocatore
using System.Collections;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    //costante che indica il numero di oggetti esistenti nel gioco
    private const int N_ITEMS = 5;
    //indica se il giocatore ha un oggetto
    private bool hasItem = false;
    
    [SerializeField]
    private Transform itemsContainer = default, //riferimento al contenitore degli oggetti
        itemsUseSpawn = default; //riferimento al punto di spawn degli oggetti quando vengono usati

    //array di riferimenti agli oggetti che il giocatore può usare
    //[SerializeField]
    private GameObject[] itemsGameobject = new GameObject[N_ITEMS];
    //riferimento al contenitore degli sprite degli oggetti
    [SerializeField]
    private Transform spritesContainer = default;
    //array di riferimenti agli sprite degli oggetti
    //[SerializeField]
    private GameObject[] itemsSprites = new GameObject[N_ITEMS];
    //riferimento all'Animator della UI degli oggetti
    [SerializeField]
    private Animator itemUIAnim = default;
    //indica l'oggetto ottenuto dal giocatore
    private int obtainedItemIndex;
    
    [SerializeField]
    private float chooseTimer = 4, //indica quanto tempo deve passare dall'inizio dell'animazione di scelta alla sua fine
        boxReactivationTimer = 1; //indica dopo quanto tempo una cassa oggetti viene riattivata


    // Start is called before the first frame update
    private void Awake()
    {
        //ottiene i riferimenti agli oggetti che il giocatore può usare quando li ottiene e li disattiva
        for (int i = 0; i < N_ITEMS; i++) { itemsGameobject[i] = itemsContainer.GetChild(i).gameObject; itemsGameobject[i].SetActive(false); }
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
        //DEBUG_______OTTIENE OGGETTO SPECIFICO
        if (Input.GetKeyDown(KeyCode.F1)) { StartCoroutine(getItem()); obtainedItemIndex = 0; Debug.Log("Debug Fagiolo Bomba"); }
        if (Input.GetKeyDown(KeyCode.F2)) { StartCoroutine(getItem()); obtainedItemIndex = 1; Debug.Log("Debug Missile"); }
        if (Input.GetKeyDown(KeyCode.F3)) { StartCoroutine(getItem()); obtainedItemIndex = 2; Debug.Log("Debug Suonofono"); }
        if (Input.GetKeyDown(KeyCode.F4)) { StartCoroutine(getItem()); obtainedItemIndex = 3; Debug.Log("Debug Silenziatore"); }
        if (Input.GetKeyDown(KeyCode.F5)) { StartCoroutine(getItem()); obtainedItemIndex = 4; Debug.Log("Debug Cubo Elettrico"); }

    }

    private void OnTriggerEnter(Collider other)
    {
        //se si collide con una cassa oggetto, la cassa viene disabilitata per un po' e il giocatore ottiene un oggetto casuale
        if (other.CompareTag("ItemBox")) { StartCoroutine(UsedUpCD(other.gameObject)); }

    }

    /// <summary>
    /// Si occupa delle tempistiche dallo scontro con la cassa oggetti all'ottenimento dell'oggetto casuale
    /// </summary>
    /// <returns></returns>
    public IEnumerator getItem()
    {
        //se non si ha già un oggetto...
        if (!hasItem)
        {
            //...prende casualmente un indice che indicherà l'oggetto ottenuto dal giocatore...
            obtainedItemIndex = Random.Range(0, N_ITEMS);
            //...porta l'oggetto usato alla posizione di spawn per l'uso...
            itemsGameobject[obtainedItemIndex].transform.position = itemsUseSpawn.position;
            //...fa partire l'animazione di scelta della UI di oggetto ottenuto...
            itemUIAnim.SetBool("ChoosingItem", true);
            //...aspetta un po'...
            yield return new WaitForSeconds(chooseTimer);
            //...ferma l'animazione di scelta oggetti della UI...
            itemUIAnim.SetBool("HasItem", true);
            //...mostra lo sprite dell'oggetto ottenuto...
            ShowObtainedItemSprite();
            //...e comunica che il giocatore ha un oggetto e non può ottenerne altri fino a quando non lo usa
            hasItem = true;
            Debug.Log("Oggetto ottenuto: " + obtainedItemIndex);
        }

    }
    /// <summary>
    /// Si occupa di richiamare la funzione dell'oggetto ottenuto quando viene premuto il tasto di uso
    /// </summary>
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
            //...porta l'oggetto usato alla posizione di spawn per l'uso...
            itemsGameobject[obtainedItemIndex].transform.position = itemsUseSpawn.position;
            //...e attiva l'oggetto usato...
            itemsGameobject[obtainedItemIndex].SetActive(true);
            //...richiama l'interfaccia da oggetto dell'oggetto ottenuto per attivarlo
            itemsGameobject[obtainedItemIndex].GetComponent<IUsableItem>().UseThisItem();
            Debug.Log("Oggetto usato: " + obtainedItemIndex);
        }

    }
    /// <summary>
    /// Si occupa della tempistica di riattivazione della cassa oggetti
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private IEnumerator UsedUpCD(GameObject itemBox)
    {
        //ottiene come riferimento il collider della cassa oggetti ottenuta
        //Collider itemBoxColl = other.GetComponent<BoxCollider>();

        //disattiva il collider della cassa oggetti
        //itemBoxColl.enabled = false;

        //disattiva la cassa oggetti
        itemBox.SetActive(false);


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
        yield return new WaitForSeconds(boxReactivationTimer);
        //infine, riattiva la cassa oggetti
        itemBox.SetActive(true);

        //infine, riattiva il collider della cassa oggetti previamente usata
        //itemBoxColl.enabled = true;

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
