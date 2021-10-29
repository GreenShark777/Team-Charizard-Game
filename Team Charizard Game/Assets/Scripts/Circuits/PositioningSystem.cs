//Si occupa di far vedere le posizioni di tutti 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositioningSystem : MonoBehaviour
{
    //array di riferimenti agli animator delle immagini per il posizionamento
    private Animator[] posImagesAnims = new Animator[4];
    //array di riferimenti al RectTransform delle immagini per il posizionamento
    private RectTransform[] posImagesRect = new RectTransform[4];
    //array di posizioni in cui devono andare le immagini
    [SerializeField]
    private Vector2[] posImagesPositions = new Vector2[4];
    //array di indici di immagini di cui scambiare posizioni
    [SerializeField]
    private List<int> recordedUpIndexes = new List<int>();

    private int goUpIndex = -1, //indica l'indice dell'immagine che deve andare sù nelle posizioni
        goDownIndex = -1; //indica l'indice dell'immagine che deve andare giù nelle posizioni

    [SerializeField]
    private float changePosSpeed = 5, //indica la velocità con cui si muovono le immagini per cambiare posizione
        stopChangeTimer = 1; //indica dopo quanto tempo si potrà cambiare nuovamente posizioni

    //indica che si stanno cambiando delle posizioni
    private bool changing = false;

    //indica l'indice della posizione che si vuole cambiare(DEBUG)
    private int i = 0;


    private void Awake()
    {
        //crea un indice
        int index = 0;
        //cicla ogni figlio di quest'oggetto
        foreach (Transform child in transform)
        {
            //cerca di ottenere il riferimento all'Animator del figlio ciclato
            Animator anim = child.GetComponent<Animator>();
            //se il riferimento esiste...
            if (anim)
            {
                //...lo aggiunge all'array di animator...
                posImagesAnims[index] = anim;
                //...e aggiunge all'array di rect quello del figlio ciclato...
                posImagesRect[index] = child.GetComponent<RectTransform>();

                //Vector2 size = Vector2.Scale(posImagesRect[index].rect.size, transform.lossyScale);

                //Rect newRect = new Rect((Vector2)posImagesRect[index].position - (size * 0.5f), size);

                //posImagesPositions[index] = /*posImagesRect[index]*/newRect.position;

                //posImagesPositions[index] = child.position;

                //...infine, incrementa l'indice per il prossimo figlio
                index++;

            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG----------------------------------------------------------------------------------------------------------------------------------------------------------
        if (i >= posImagesAnims.Length - 1) { i = 0; }

        if (Input.GetKeyDown(KeyCode.Mouse0)/* && !changing*/) { ChangePositions(i); /*i++;*/ i = Random.Range(0, 4); }
        //DEBUG----------------------------------------------------------------------------------------------------------------------------------------------------------

        //se si devono cambiare delle posizioni...
        if (changing)
        {
            //...cambia la posizione dell'immagine che deve andare verso sù...
            posImagesRect[goUpIndex].position = Vector3.Lerp(posImagesRect[goUpIndex].position, posImagesPositions[goDownIndex], changePosSpeed * Time.deltaTime);
            //...cambia la posizione dell'immagine che deve andare verso giù
            posImagesRect[goDownIndex].position = Vector3.Lerp(posImagesRect[goDownIndex].position, posImagesPositions[goUpIndex], changePosSpeed * Time.deltaTime);

        }
        
    }

    /// <summary>
    /// Cambia le posizioni delle immagini in base alla posizione ricevuta
    /// </summary>
    /// <param name="checkingPos"></param>
    public void ChangePositions(int checkingPos)
    {
        //se non sta già cambiando delle posizioni...
        if (!changing)
        {
            //...se la posizione ricevuta è sbagliata, lo comunica...
            if (checkingPos >= posImagesAnims.Length - 1) { Debug.LogError("Dato valore troppo alto per posizione di controllo"); }
            //...comunica che si stanno cambiando delle posizioni...
            changing = true;
            //...salva la posizione dell'immagine di cui cambiare posizione verso sù...
            goUpIndex = checkingPos;
            //...salva la posizione dell'immagine di cui cambiare posizione verso giù...
            goDownIndex = checkingPos + 1;
            //...fa partire la coroutine per fermare il cambio di posizione
            StartCoroutine(StopChanging());

        } //altrimenti, salva il valore ottenuto come parametro nella lista
        else { recordedUpIndexes.Add(checkingPos); }

    }
    /// <summary>
    /// Smette di cambiare le posizioni delle immagini dopo aver aspettato dei secondi
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopChanging()
    {
        Debug.Log("HIGH POS: " + goDownIndex);
        //fa partire l'animazione di cambio dell'immagine che deve andare verso giù
        posImagesAnims[goDownIndex].SetTrigger("trigger");
        //aspetta un po'
        yield return new WaitForSeconds(stopChangeTimer);
        //scambia le posizioni nell'array degli animator delle immagini che sono state scambiate
        var animTemp = posImagesAnims[goDownIndex];
        posImagesAnims[goDownIndex] = posImagesAnims[goUpIndex];
        posImagesAnims[goUpIndex] = animTemp;
        //scambia le posizioni nell'array dei RectTransform delle immagini che sono state scambiate
        var rectTemp = posImagesRect[goDownIndex];
        posImagesRect[goDownIndex] = posImagesRect[goUpIndex];
        posImagesRect[goUpIndex] = rectTemp;
        //riporta le variabili indice ad un valore neutro
        goUpIndex = -1;
        goDownIndex = -1;
        //fa in modo che si possa nuovamente cambiare le posizioni delle immagini
        changing = false;
        //se esistono degli indici nella lista, richiama la funzione di cambio posizione con il primo indice disponibile e lo rimuove dalla lista
        if (recordedUpIndexes.Count > 0) { ChangePositions(recordedUpIndexes[0]); recordedUpIndexes.RemoveAt(0); }
        Debug.LogError("Stopped changing");
    }

    private void OnDrawGizmos()
    {

        if (posImagesAnims.Length > 0)
        {

            Gizmos.DrawWireSphere(new Vector3(posImagesPositions[0].x, posImagesPositions[0].y, 0), 3);
            Gizmos.DrawWireSphere(new Vector3(posImagesPositions[1].x, posImagesPositions[1].y, 0), 3);
            Gizmos.DrawWireSphere(new Vector3(posImagesPositions[2].x, posImagesPositions[2].y, 0), 3);
            Gizmos.DrawWireSphere(new Vector3(posImagesPositions[3].x, posImagesPositions[3].y, 0), 3);

        }

    }

}
