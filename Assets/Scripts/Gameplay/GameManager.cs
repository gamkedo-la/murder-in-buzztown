using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private bool _disableConversations;
    [SerializeField] private DialogueManager _dm;
    [SerializeField] private PlayerController _pc;

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
    }

    private void Start()
    {
        if (!_disableConversations)
        {
            _dm.StartDialogue();
        }
    }


}
