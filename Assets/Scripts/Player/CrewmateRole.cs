using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewmateRole : PlayerRole
{
    Collider2D[] interactArray;
    GameObject latestTask;

    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        InteractionUI.UseButton.gameObject.SetActive(true);
        InteractionUI.UseButton.interactable = false;

        interactArray = new Collider2D[10];
        InteractionUI.UseButton.onClick.AddListener(OnUseButtonClick);
        InteractionUI.ReportButton.onClick.AddListener(OnReportButtonClick);
    }

    public override PlayerRoles GetPlayerRole()
    {
        return PlayerRoles.Crewmate;
    }

    protected override void LocalUpdate()
    {
        int hit = Physics2D.OverlapCircleNonAlloc(transform.position, InteractRadius, interactArray);
        GameObject hitTask = null;

        for (int i = 0; i < hit; i++)
        {
            if (interactArray[i].CompareTag("Task"))
            {
                Task task = interactArray[i].GetComponent<Task>();
                if (task && !task.IsResolved)
                    hitTask = interactArray[i].gameObject;
            }
        }

        if (hitTask == null && latestTask != null)
        {
            latestTask.GetComponent<Task>().UnhighlightTask();
            latestTask = null;
            InteractionUI.UseButton.interactable = false;
        }
        else if (hitTask != null)
        {
            latestTask = hitTask;
            latestTask.GetComponent<Task>().HighlightTask();

            InteractionUI.UseButton.interactable = true;
        }
    }

    void OnUseButtonClick()
    {
        latestTask.GetComponent<Task>().Interact();
    }

    void OnReportButtonClick()
    {

    }
}
