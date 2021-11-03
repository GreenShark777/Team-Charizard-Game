//Si occupa del collezionabile speciale e di ciò che deve succedere quando viene preso dal giocatore
using UnityEngine;
using UnityEngine.UI;

public class SpecialCollectable : MonoBehaviour
{
    //riferimento all'immagine che indica che si è preso il collezionabile speciale
    [SerializeField]
    private Image specialCollectableSprite = default;
    //indica quanto velocemente deve ruotare il collezionabile
    [SerializeField]
    private float rotationSpeed = 90;


    private void FixedUpdate()
    {
        //ruota il collezionabile continuamente nell'asse y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        //se si collide con il giocatore, viene ottenuto da quest'ultimo
        if (other.CompareTag("Player")) { Obtained(); }

    }

    private void Obtained()
    {
        //fa in modo che lo sprite del collezionabile speciale non sia più ingrigito
        specialCollectableSprite.color = new Color(1, 1, 1, 1);
        //viene disattivato questo collezionabile
        gameObject.SetActive(false);
        Debug.Log("Ottenuto collezionabile pappagalli");
    }

}
