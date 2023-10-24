using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private bool _disableConversations;
    [SerializeField] private DialogueManager _dm;
    [SerializeField] private PlayerController _pc;
    [SerializeField] private GameObject audioManager;
    private AudioManager audioManagerScript;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioManagerScript = audioManager.GetComponent<AudioManager>();
    }

    private void Start()
    {
        if (!_disableConversations)
        {
            // TODO: determine how dialogue wil be handled
            _dm.StartDialogue();
        }

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {

            audioManagerScript.ChangeMusic(audioManagerScript.buzztownThemeAudioClip);
        }
        
    }

    private void HandleWinGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("VictoryScreen");
    }

    private void HandleDeath()
    {
        // Add code for what happens when the character dies
    }


}
