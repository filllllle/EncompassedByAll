using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanelUI : MonoBehaviour
{
    public MenuUI MenuUI { get; set; }

    [SerializeField]
    Slider masterVolumeSlider;

    [SerializeField]
    Slider taskVolumeSlider;

    [SerializeField]
    Slider playerVolumeSlider;

    [SerializeField]
    Button acceptButton;

    [SerializeField]
    Button backButton;

    private void Start()
    {
        acceptButton.onClick.AddListener(AcceptButton);
        backButton.onClick.AddListener(BackButton);
    }

    void AcceptButton()
    {

        CloseUI();
    }

    void BackButton()
    {

        CloseUI();
    }

    void CloseUI()
    {
        gameObject.SetActive(false);
        MenuUI.gameObject.SetActive(true);
    }
}
