using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class DebugSecretRoomDocks : MonoBehaviour
{
    [SerializeField] ChangeCamera changeCameraZone;
    private bool isBridgeOut = false;

    private void Update() 
    {
        if (isBridgeOut) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (!changeCameraZone.GetIsPlayerInZone()) return;
            GetComponent<PlayableDirector>().Play();
            isBridgeOut = true;
        }
    }
}
