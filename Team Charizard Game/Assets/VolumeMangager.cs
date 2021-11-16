using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeMangager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer master = default;
  [SerializeField]
    private string volumeToChange=default;//incrementare su unity di uno
    [SerializeField]
    private int volumeOffSet = default;



    [SerializeField]
    private GameObject[] taccheteVolume = default;
  

    private int volume = 29, maxvolume= 30;
    void Start()
    {

        maxvolume = taccheteVolume.Length-1;

    }

   public void CambiaVolume(bool incrementa)
   {
        //taccheteVolume[volume].SetActive(incrementa);
        if (incrementa)
        {

            volume++;

        }
        volume = Mathf.Clamp(volume, 0, maxvolume);
        taccheteVolume[volume].SetActive(incrementa);
        if (!incrementa)
        {
            volume--;

        }
        //volume = Mathf.Clamp(volume, 0, maxvolume);
        //taccheteVolume[volume].SetActive(incrementa);
        master.SetFloat(volumeToChange, volume+volumeOffSet);


   }

}
