using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class SecretRoomDocksManager : MonoBehaviour
{
    [SerializeField] Lever lever;

    private bool isBridgeOut = false;

    private void Start() 
    {
        lever.activateLever += PlaySecretAnimation;
    }

    private void PlaySecretAnimation()
    {
        GetComponent<PlayableDirector>().Play();
    }
}
