using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera targetCamera;
    private CinemachineVirtualCamera originCamera;
    private bool isPlayerInZone = false;
    private bool isPlayerDying = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isPlayerInZone || isPlayerDying) return;
        isPlayerInZone = true;

        GetCurrentActiveCamera();
        ActivateTargetCamera();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || !isPlayerInZone || isPlayerDying) return;
        isPlayerInZone = false;

        ReactivateOriginCamera();
    }

    public bool GetIsPlayerInZone()
    {
        return isPlayerInZone;
    }

    private void ActivateTargetCamera()
    {
        originCamera.gameObject.SetActive(false);
        targetCamera.gameObject.SetActive(true);
    }

    private void ReactivateOriginCamera()
    {
        if (originCamera == null)
        {
            Debug.Log(
                "The player exits the special camera zone but there is no origin camera to switch back to. " +
                "The player probably did not enter the zone beforehand."
            );
            return;
        }
        
        originCamera.gameObject.SetActive(true);
        targetCamera.gameObject.SetActive(false);
    }

    private void GetCurrentActiveCamera()
    {
        CinemachineVirtualCamera[] allVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (CinemachineVirtualCamera virtualCamera in allVirtualCameras)
        {
            if (virtualCamera.isActiveAndEnabled)
            {
                originCamera = virtualCamera;
                break;
            }
        }
    }
}