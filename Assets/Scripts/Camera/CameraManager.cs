using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] _allCams;
    [Header("Lerp controls for fall/jump")]
    [SerializeField] private float _fallLerpAmount;
    [SerializeField] private float _fallYlerpTime;
    public float _fallSpeedYDampingChangeThreshold;

    public bool IsLerpingYDamping { get; private set;}
    public bool LerpedFromPlayerFalling { get; set;}

    private Coroutine _lerpYCoroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _transposer;

    private float _normYLerpAmount;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null) instance = this;

        for (int i = 0; i < _allCams.Length; i++)
        {
            if (_allCams[i].enabled)
            {
                _currentCamera = _allCams[i];
                _transposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        _normYLerpAmount = _transposer.m_YDamping;
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = _transposer.m_YDamping;
        float endDampAmount = 0f;

        if(isPlayerFalling)
        {
            endDampAmount = _fallLerpAmount;
            LerpedFromPlayerFalling = true;
        }

        else
        {
            endDampAmount = _normYLerpAmount;
        }

        float elapsedTime = 0f;
        while(elapsedTime < _fallYlerpTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYlerpTime));
            _transposer.m_YDamping = lerpedAmount;
            yield return null;
        }

        IsLerpingYDamping = false;
    }
}
