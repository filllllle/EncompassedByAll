using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickGamePanel : MonoBehaviour
{
    [SerializeField]
    Task ownerTask;

    [SerializeField]
    Button greenButton;

    [SerializeField]
    Button redButton;

    [SerializeField]
    Button closeButton;

    [SerializeField]
    TextMeshProUGUI screenText;

    GameObject lastFirstSelectedGameObject;

    bool gameStarted = false;

    // This is the amount of seconds before player is prompted to press red button
    float timing1 = 2;
    float timing2 = 1;


    // This is the time span that the button can be pressed in...
    float pressTimingMin;
    float pressTimingMax;

    bool sucessFirstPass = false;

    private void Start()
    {
        greenButton.onClick.AddListener(GreenButtonPressed);
        redButton.onClick.AddListener(RedButtonPressed);
    }

    void Update()
    {

    }

    void GreenButtonPressed()
    {
        gameStarted = true;
        screenText.text = "Press red soon\n";

        pressTimingMin = Time.realtimeSinceStartup + timing1;
        pressTimingMax = pressTimingMin + 0.7f;
        sucessFirstPass = false;
        StartCoroutine("PrintToScreen");
    }

    IEnumerator PrintToScreen()
    {
        while(gameStarted)
        {
            screenText.text = string.Concat(screenText.text, ".");

            if(Time.realtimeSinceStartup > pressTimingMin && Time.realtimeSinceStartup < pressTimingMax)
            {
                screenText.text = string.Concat(screenText.text, "\nNow");
                break;
                StopAllCoroutines();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    void RedButtonPressed()
    {
        if (Time.realtimeSinceStartup > pressTimingMin && Time.realtimeSinceStartup < pressTimingMax && !sucessFirstPass)
        {
            screenText.text = "Success!\n Press red again soon\n";

            pressTimingMin = Time.realtimeSinceStartup + timing2;
            pressTimingMax = pressTimingMin + 0.7f;
            sucessFirstPass = true;
            StartCoroutine("PrintToScreen");
        }
        else if(sucessFirstPass)
        {
            screenText.text = "Computer rebooted";
            ownerTask.SetAsResolved();

            Invoke("Hide", 1);
        }
        else
        {
            screenText.text = "Timing wrong! Start again";
            gameStarted = false;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        lastFirstSelectedGameObject = GameManager.Instance.EventSystem.firstSelectedGameObject;
        GameManager.Instance.EventSystem.firstSelectedGameObject = gameObject;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        GameManager.Instance.EventSystem.firstSelectedGameObject = lastFirstSelectedGameObject;
    }
}
