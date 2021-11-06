using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingCollider : MonoBehaviour, IGiveDamage
{

    [SerializeField]
    private float dmg = default;


    public float GiveDamage()
    {

        return dmg;

    }

}
