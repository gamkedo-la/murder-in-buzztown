using UnityEngine;

public class LifeManager : MonoBehaviour
{
    private int _currentLives = 3;
    private int _maxLives = 4; 

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void IncreaseLives()
    {
        if (_currentLives == _maxLives) return;
        transform.GetChild(_currentLives).GetChild(0).gameObject.SetActive(true);
        _currentLives++;
    }

    public void DecreaseLives()
    {
        
        transform.GetChild(_currentLives - 1).GetChild(0).gameObject.SetActive(false);
        _currentLives--;
        AudioManager.Instance.PlayEffect(AudioManager.Instance.hitAudioClip);

        if (_currentLives < 1)
        {
            Destroy(player);
        }
    }

    public void HandleGinsengTeaPowerup()
    {
        _currentLives = 4;

        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
    }
}
