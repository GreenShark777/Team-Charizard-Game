//Si occupa della UI di posizionamento dei kart(nuovo)
using UnityEngine;
using UnityEngine.UI;

public class NewPositioningSystem : MonoBehaviour
{
    //riferimento al testo che indica la posizione del giocatore
    [SerializeField]
    private Text playerPosText = default;
    //array di riferimenti agli animator delle immagini per il posizionamento
    private Animator[] posImagesAnims = new Animator[4];
    //array di riferimenti al RectTransform delle immagini per il posizionamento
    private RectTransform[] posImagesRect = new RectTransform[4];
    //array di posizioni in cui devono andare le immagini
    [SerializeField]
    private Transform[] posImagesPositions = new Transform[4];
    //array di suffissi per i numeri di posizione
    [SerializeField]
    private string[] numberSuffix = default;


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
                //...infine, incrementa l'indice per il prossimo figlio
                index++;

            }

        }

    }
    /// <summary>
    /// Cambia le posizioni dei kart nella UI in base alle posizioni controllate dal checkpoint che richiama questa funzione
    /// </summary>
    /// <param name="calculatedPositions"></param>
    public void SetPositions(int[] calculatedPositions)
    {
        //cicla ogni immagine di posizioni
        for (int i = 0; i < 4; i++)
        {
            //cambia la posizione dell'immagine nella posizione calcolata
            posImagesRect[i].position = posImagesPositions[calculatedPositions[i] - 1].position;

        }
        //infine, cambia il testo che indica la posizione del giocatore
        ChangePlayerPosText(calculatedPositions[3] - 1);

    }
    /// <summary>
    /// Cambia il testo che indica la posizione del giocatore
    /// </summary>
    private void ChangePlayerPosText(int playerPosIndex)
    {
        //cambia il testo del giocatore con il nuovo indice di posizione del giocatore
        playerPosText.text = (playerPosIndex + 1) + numberSuffix[playerPosIndex];

    }

    private void OnDrawGizmos()
    {

        if (posImagesAnims.Length > 0)
        {

            Gizmos.DrawWireSphere(new Vector3(posImagesPositions[0].position.x, posImagesPositions[0].position.y, 0), 3);
            Gizmos.DrawWireSphere(new Vector3(posImagesPositions[1].position.x, posImagesPositions[1].position.y, 0), 3);
            Gizmos.DrawWireSphere(new Vector3(posImagesPositions[2].position.x, posImagesPositions[2].position.y, 0), 3);
            Gizmos.DrawWireSphere(new Vector3(posImagesPositions[3].position.x, posImagesPositions[3].position.y, 0), 3);

        }

    }

}
