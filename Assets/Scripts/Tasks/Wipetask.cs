using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wipetask : Task
{
    [SerializeField]
    WipeTaskPanel gamePanel;

    protected override void OnStart()
    {

    }

    public override void OnInteract()
    {
        gamePanel.Show();
    }
}
