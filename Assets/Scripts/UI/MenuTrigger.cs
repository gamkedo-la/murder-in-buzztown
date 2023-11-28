using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MenuTrigger : MonoBehaviour
{

    [SerializeField] float triggerDuration = 3f;
    [SerializeField] TextMeshProUGUI display;

    bool inTrigger;
    float currentTriggerTime;
    public UnityEvent TriggerEvent;
    bool triggerFired = false;

    // Start is called before the first frame update
    void Start()
    {
        display.enabled = false;
        currentTriggerTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inTrigger)
        {
            return;
        }

        if(triggerFired){
            return;
        }

        currentTriggerTime += Time.deltaTime;
        
        if(display != null){
            display.text = GetDisplayText();
        }

        if (currentTriggerTime > triggerDuration)
        {
            TriggerEvent.Invoke();
            triggerFired = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        inTrigger = true;
        if(display == null){
            return;
        }
        display.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        inTrigger = false;
        currentTriggerTime = 0;
        triggerFired = false;
        if(display == null){
            return;
        }
        display.enabled = false;
        display.text = GetDisplayText();
    }

    private string GetDisplayText(){
        if(currentTriggerTime > triggerDuration){
            return "0";
        }
        return Mathf.CeilToInt(triggerDuration - currentTriggerTime) + "";
    }
}
