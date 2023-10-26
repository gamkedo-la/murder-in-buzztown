using UnityEngine;

public class LifeManager : MonoBehaviour
{
    private int _currentLives = 3;
    private int _maxLives = 3; // Initially this will be 3 but you can increase it to 4

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

        if (_currentLives < 1)
        {
            Destroy(player);
        }
    }
}
