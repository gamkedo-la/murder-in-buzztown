using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmproComponent;
    [SerializeField] string[] _dialogue;
    [SerializeField] private float _textSpeed;
    private int currentIndex;
    // Start is called before the first frame update

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
        StartDialogue();
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

    void StartDialogue()
    {
        currentIndex = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in _dialogue[currentIndex].ToCharArray())
        {
            tmproComponent.text += c;
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
            gameObject.SetActive(false);
        }
    }
}
