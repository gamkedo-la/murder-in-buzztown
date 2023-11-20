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
    [SerializeField] private LifeManager _lifeManager;
    private AudioManager audioManagerScript;
    [SerializeField] GameObject dialogueManager;
    public DialogueManager dialogueManagerScript;

    public string gameState = "first dialogue";

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
        dialogueManagerScript = dialogueManager.GetComponent<DialogueManager>();
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
            // TODO: disabled for testing purposes
            // audioManagerScript.ChangeMusic(audioManagerScript.buzztownThemeAudioClip);
            // dialogueManager.SetActive(true);
            // StartCoroutine(dialogueManagerScript.WaitToFadeInOpeningDialogueBox());
        }

    }

    public void DecreaseLives()
    {
        _lifeManager.DecreaseLives();
    }

    public bool IncreaseLives()
    {
        return _lifeManager.IncreaseLives();
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
