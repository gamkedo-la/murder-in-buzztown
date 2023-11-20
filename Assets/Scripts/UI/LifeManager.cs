using System.Collections;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    private int _currentLives = 4;
    private int _maxLives = 4;

    private GameObject player;

    private CheckpointManager checkpointManager;
    [SerializeField] float timeToRespawn = 1f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        checkpointManager = FindObjectOfType<CheckpointManager>();
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

        if (_currentLives > 0)
        {
            AudioManager.Instance.PlayEffect(AudioManager.Instance.hitAudioClip);
        }
        else
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        AudioManager.Instance.PlayEffect(AudioManager.Instance.deathAudioClip);
        StartCoroutine(HideAndSpawnPlayerAfterDelay());
    }

    private IEnumerator HideAndSpawnPlayerAfterDelay()
    {
        SetPlayerVisibleStateTo(false);

        yield return new WaitForSeconds(timeToRespawn);
        checkpointManager.SpawnPlayerAtCurrentCheckpoint();

        RefillAllLives();
        SetPlayerVisibleStateTo(true);
    }

    private void SetPlayerVisibleStateTo(bool state)
    {
        player.GetComponent<SpriteRenderer>().enabled = state;
        foreach (Collider2D collider2D in player.GetComponents<BoxCollider2D>())
        {
            collider2D.enabled = state;
        }
    }

    private void RefillAllLives()
    {
        if (_currentLives > 0) return;  // if you fall in a hole and still have life you respawn with the same amount of life you had before falling
        for (int life_idx = 0; life_idx < _maxLives; life_idx++)
        {
            IncreaseLives();
        }
    }

    public void HandleGinsengTeaPowerup()
    {
        _maxLives = 5;
        _currentLives = 5;

        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
    }
}
