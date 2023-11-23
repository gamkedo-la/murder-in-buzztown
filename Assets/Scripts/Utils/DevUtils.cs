using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevUtils : MonoBehaviour
{
    [SerializeField] GameObject _bossCam;
    [SerializeField] GameObject _followCam;
    [SerializeField] GameObject _player;
    [SerializeField] TextMeshProUGUI tmproComponent;
    private bool isInBossRoom = false;

    public void SwitchToBossCam()
    {
        _followCam.SetActive(false);
        _bossCam.SetActive(true);
    }

    public void SwitchToFollowCam()
    {
        _bossCam.SetActive(false);
        _followCam.SetActive(true);
    }

    public void TeleportToBossRoom()
    {
        if (isInBossRoom)
        {
            _player.transform.position = new Vector3(-14f, 2, 0f);
            tmproComponent.text = "TP to Boss";
            SwitchToFollowCam();
        }
        else
        {
            _player.transform.position = new Vector3(-338f, 2, 0f);
            tmproComponent.text = "TP to Start";
            SwitchToBossCam();
        }
    }
}
