using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmproComponent;
    public string[] _dialogue;
    //Another day, another dead person.
    //That's just how things are here in Buzztown
    [SerializeField] private float _textSpeed;
    [SerializeField] private PlayerController _player;
    private int currentIndex;

    [SerializeField] GameObject[] allDialogueBoxGameObjects;

    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject buzztownPoliceman;
    BuzztownPoliceScript buzztownPolicemanScript;

    private PlayerInputs _inputs;
    private void Awake()
    {
        _inputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        _inputs.Player.Jump.performed += HandleDialoguePress;
        _inputs.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        _inputs.Player.Jump.Disable();
    }

    void Start()
    {
        tmproComponent.text = string.Empty;
        buzztownPolicemanScript = buzztownPoliceman.GetComponent<BuzztownPoliceScript>();
    }

    void HandleDialoguePress(InputAction.CallbackContext obj)
    {
        if (tmproComponent.text == _dialogue[currentIndex])
        {
            NextDialogue();
        }
        else
        { // Don't wait and fill all the text
            StopAllCoroutines();
            tmproComponent.text = _dialogue[currentIndex];
        }
    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        currentIndex = 0;
        _player.TakeAwayControl();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        int characterCount = 0;

        foreach (char c in _dialogue[currentIndex].ToCharArray())
        {
            tmproComponent.text += c;

            characterCount++;

            if (characterCount % 2 == 0)
            {
                AudioManager.Instance.PlayEffect(AudioManager.Instance.dialogueBlipAudioClip);
            }

            yield return new WaitForSeconds(_textSpeed);
        }
    }

    void NextDialogue()
    {
        if (currentIndex < _dialogue.Length - 1)
        {
            currentIndex++;
            tmproComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            _player.ReturnControl();
            tmproComponent.text = string.Empty;
            gameObject.SetActive(false);
            gameManager.GetComponent<GameManager>().gameState = "gameplay";
        }
    }

    public IEnumerator WaitToFadeInOpeningDialogueBox()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeIn());
        StartDialogue();
    }

    IEnumerator FadeIn()
    {
        GameObject lastUI_Object = allDialogueBoxGameObjects[allDialogueBoxGameObjects.Length - 1].gameObject;
        Color lastImageColor = lastUI_Object.GetComponent<Image>().color;

        while (lastImageColor.a < 1)
        {
            for (int i = 0; i < allDialogueBoxGameObjects.Length - 1; i++)
            {
                Color currentImageColor = allDialogueBoxGameObjects[i].gameObject.GetComponent<Image>().color;
                currentImageColor.a += 0.25f * Time.deltaTime;
                Color newImageColor = currentImageColor;
                allDialogueBoxGameObjects[i].gameObject.GetComponent<Image>().color = newImageColor;
            }
            yield return null; // Wait until the next frame
        }

    }
}
