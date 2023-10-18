using UnityEngine;
using UnityEngine.UI;

public class SteamManager : MonoBehaviour
{
    const float MAX_STEAM = 1;
    const float REGAIN_STEAM_AFTER = 2.0f;
    const float STEAM_REGAIN_RATIO = 0.001f;
    float _currentSteam = MAX_STEAM;
    float _regainSteamTimer = REGAIN_STEAM_AFTER;
    [SerializeField] Image fillImg;

    private void Update()
    {
        if (_regainSteamTimer >= REGAIN_STEAM_AFTER && _currentSteam < MAX_STEAM)
        {
            _currentSteam += STEAM_REGAIN_RATIO;
            fillImg.fillAmount = _currentSteam;
        }
        if (_regainSteamTimer < REGAIN_STEAM_AFTER)
        {
            _regainSteamTimer += Time.deltaTime;
        }
    }

    public void UseSteam(float amount)
    {
        if (_currentSteam - amount < 0) return; // This validation should be done outside of here
        _regainSteamTimer = 0f;
        _currentSteam -= amount;
        fillImg.fillAmount = _currentSteam;
    }
}
