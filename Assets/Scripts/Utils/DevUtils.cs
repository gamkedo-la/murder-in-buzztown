using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevUtils : MonoBehaviour
{
    [SerializeField] GameObject _bossCam;
    [SerializeField] GameObject _followCam;

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
}
