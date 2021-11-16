//Si occupa di cambiare i valori dello slider di cui è componente
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderChange : MonoBehaviour
{
    //riferimento allo slider di cui bisogna cambiare il valore
    private Slider slider;

    [SerializeField]
    private float sliderValueChangeRate = 5, //indica quanto velocemente la barra della vita deve cambiare
        fasterSliderChangeRate = 50, //indica quanto velocemente la barra della vita deve cambiare quando deve essere più veloce
        acceptableDist = 0.1f; //indica quanto vicino deve almeno essere lo slider della vita per smettere di cambiare

    private float startSliderChangeRate, //indica il valore iniziale del valore di aumento dello slider della vita
        targetValue; //indica il valore a cui bisogna arrivare

    //riferimento al metodo da richiamare una volta raggiunto il valore obiettivo
    public delegate void MethodToRecall();

    private void Awake()
    {
        //ottiene il riferimento allo slider di cui bisogna cambiare il valore
        slider = GetComponent<Slider>();
        //ottiene il valore iniziale del valore di aumento dello slider della vita
        startSliderChangeRate = sliderValueChangeRate;

    }

    /// <summary>
    /// Permette di cambiare il valore massimo dello slider
    /// </summary>
    /// <param name="newMaxValue"></param>
    public void ChangeSliderMaxValue(float newMaxValue) { slider.maxValue = newMaxValue; }
    /// <summary>
    /// Permette di cambiare il valore dello slider
    /// </summary>
    /// <param name="newValue"></param>
    public void ChangeSliderValue(float newValue) { slider.value = newValue; }
    /// <summary>
    /// Rende più veloce il cambio di valore fino a quando non arriva di nuovo al valore obiettivo
    /// </summary>
    public void FastenChangeRate() { sliderValueChangeRate = fasterSliderChangeRate; }
    /// <summary>
    /// Cambia lentamente il valore dello slider, fino ad arrivare al valore obiettivo
    /// </summary>
    /// <param name="increment"></param>
    /// <param name="targetValue"></param>
    /// <returns></returns>
    public IEnumerator ChangeSliderValueSlowly(bool increment, float newTargetValue, MethodToRecall method = null)
    {
        //se si possono chiamare altre coroutine, fa tutti i vari controlli e calcoli
        if (gameObject.activeInHierarchy)
        {
            //aggiorna il valore obiettivo
            targetValue = newTargetValue;
            //cambia il valore dello slider della vita per pareggiare la vita attuale del giocatore
            slider.value += (sliderValueChangeRate * (increment ? Time.deltaTime : -Time.deltaTime));
            //aspetta la fine del frame
            yield return new WaitForEndOfFrame();
            //Debug.Log("Dist: " + (Mathf.Abs(slider.value - targetValue)) + " -> To Increment? " + increment + " || val: " + slider.value + " target: " + targetValue);
            //se la differenza tra il valore nello slider e la vita del giocatore è ancora troppo grande, continua a cambiare il valore dello slider
            if (Mathf.Abs(slider.value - targetValue) >= acceptableDist) { StartCoroutine(ChangeSliderValueSlowly(targetValue > slider.value, targetValue, method)); yield break; }
            //altrimenti, si è arrivati al valore obiettivo, quindi...
            else
            {
                //...se il riferimento al metodo da richiamare non è nullo, richiama quella funzione...
                if (method != null) { method(); }
                //...e se la velocità di cambio del valore non è uguale a quella iniziale, la riporta al valore iniziale
                if (sliderValueChangeRate != startSliderChangeRate) { sliderValueChangeRate = startSliderChangeRate; }

            }

        }

    }
    /// <summary>
    /// Ritorna il valore attuale dello slider
    /// </summary>
    /// <returns></returns>
    public float GetSliderValue() { return slider.value; }

}
