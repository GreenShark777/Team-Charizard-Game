//Si occupa della gestione dell'abilità del giocatore
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    //riferimento allo Slider che indica la carica dell'abilità del giocatore
    [SerializeField]
    private SliderChange abilitySlider = default;
    //indica il valore corrente della carica dell'abilità del giocatore
    [SerializeField]
    private float charge = 100;
    //indica la carica massima della carica del giocatore
    private float maxCharge;


    private void Start()
    {
        //ottiene la carica massima
        maxCharge = charge;
        //imposta il valore massimo dello slider della carica dell'abilità al valore massimo
        abilitySlider.ChangeSliderMaxValue(maxCharge);
        //imposta il valore corrente dello slider dell'abilità al valore massimo
        abilitySlider.ChangeSliderValue(maxCharge);

    }
    /// <summary>
    /// Ricarica di un po' l'abilità del giocatore
    /// </summary>
    public void Recharge()
    {

    }

}
