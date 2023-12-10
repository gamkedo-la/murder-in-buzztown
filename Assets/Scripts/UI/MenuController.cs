using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject _creditsPanel;
    bool _isPanelActive = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isPanelActive)
        {
            _creditsPanel.SetActive(false);
            _isPanelActive = false;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);

    }

    public void LoadOptions()
    {
        if (!_isPanelActive)
        {
            _creditsPanel.SetActive(true);
            _isPanelActive = true;
        }
    }
}
