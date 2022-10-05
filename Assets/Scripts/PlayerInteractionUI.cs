using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{
    Player localPlayer;

    public Player LocalPlayer { get => localPlayer; set => localPlayer = value; }

    // Common Buttons
    [SerializeField]
    Button useButton;
    [SerializeField]
    Button reportButton;

    public Button UseButton { get => useButton; }
    public Button ReportButton { get => reportButton; }


    // Imposter Buttons
    [SerializeField]
    Button killButton;

    public Button KillButton { get => killButton; }

    private void Start()
    {
        
    }

    void Update()
    {

    }
}
