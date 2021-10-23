﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;

    [SerializeField]
    private float camBoostOffsetY = 0,
        camBoostOffsetZ = 0;

    private PlayerKartCtrl playerScript;

    private Vector3 originalCamPos;
    private Vector3 boostCamPos;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<PlayerKartCtrl>();

        originalCamPos = transform.GetChild(0).localPosition;

        boostCamPos = new Vector3(originalCamPos.x, originalCamPos.y + camBoostOffsetY, originalCamPos.z + camBoostOffsetZ);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = player.position + offset;
        
        if (!playerScript.GLIDER_FLY)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(player.rotation.x, Mathf.Clamp(player.rotation.y, -2, 2),
                player.rotation.z, player.rotation.w), 3 * Time.deltaTime); //normal

        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, player.eulerAngles.y, 0), 3 * Time.deltaTime); //we only want the camera rotate on the y axis. Not the x or z axis since when the player is turning in the air while gliding, the player is tilting a lot, so we do not want the camera to tilt sideways too.
        }

        if (playerScript.BoostTime > 0)
        {
            transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, boostCamPos, 3 * Time.deltaTime);
        }
        else
        {
            transform.GetChild(0).localPosition = Vector3.Lerp(transform.GetChild(0).localPosition, originalCamPos, 3 * Time.deltaTime);
        }
        
    }
}