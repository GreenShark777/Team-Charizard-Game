//Si occupa di ruotare il tachimetro in base alla velocità a cui il kart del giocatore sta andando
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    //riferimento al controller del kart
    [SerializeField]
    private PlayerKartCtrl kartCtrl = default;
    //riferimento al pivot della lancetta del tachimetro
    private Transform needlePivot;
    
    [SerializeField]
    private float startPosition = 145, //indica la rotazione mainima che il tachimetro può avere
        endPosition = -145, //indica la rotazione massima che il tachimetro può avere
        speedOffset = 90; //indica quanto viene diminuita la velocità reale del kart per dare i valori giusti nel tachimetro

    //indica la posizione centrale della lancetta
    private float centerPosition;


    private void Awake()
    {
        //ottiene il riferimento al pivot della lancetta del tachimetro
        needlePivot = transform.GetChild(0);
        //calcola la posizione centrale del tachimetro
        centerPosition = startPosition - endPosition;

    }

    private void FixedUpdate()
    {
        //ruota la lancetta del tachimetro in base alla velocità reale del kart
        NeedleRotationUpdate(kartCtrl.GetRealSpeed());

    }
    /// <summary>
    /// Ruota il tachimetro in base al parametro di velocità del veicolo ricevuto
    /// </summary>
    /// <param name="vehicleSpeed"></param>
    private void NeedleRotationUpdate(float vehicleSpeed)
    {
        //calcola il valore in cui la velocità del kart sarà nel tachimetro
        float speedOnSpeedometer = Mathf.Abs(vehicleSpeed) / speedOffset;
        //ruota la lancetta in base al valore calcolato
        needlePivot.eulerAngles = new Vector3(0, 0, (startPosition - speedOnSpeedometer * centerPosition));

    }

}
