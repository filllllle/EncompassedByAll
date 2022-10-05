using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class RulesUI : MonoBehaviourPunCallbacks
{

    [SerializeField]
    TMP_InputField impostersInputField;

    [SerializeField]
    TMP_InputField killCooldownInputField;

    [SerializeField]
    TMP_InputField tasksInputField;

    private void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            impostersInputField.interactable = false;
            killCooldownInputField.interactable = false;
            tasksInputField.interactable = false;
        }
        else
        {
            OnValueChange("hej");

            impostersInputField.onValueChanged.AddListener(OnValueChange);
            killCooldownInputField.onValueChanged.AddListener(OnValueChange);
            tasksInputField.onValueChanged.AddListener(OnValueChange);
        }
    }

    void OnValueChange(string change)
    {

        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();

        roomProperties.Add("impostercount", impostersInputField.text);
        roomProperties.Add("killcooldown", killCooldownInputField.text);
        roomProperties.Add("taskcount", tasksInputField.text);

        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            bool icb = propertiesThatChanged.TryGetValue("impostercount", out object ic);
            bool kcb = propertiesThatChanged.TryGetValue("killcooldown", out object kc);
            bool tcb = propertiesThatChanged.TryGetValue("taskcount", out object tc);

            if(icb)
            {
                impostersInputField.text = ic as string;
            }
            if(kcb)
            {
                killCooldownInputField.text = kc as string;
            }
            if(tcb)
            {
                tasksInputField.text = tc as string;
            }

        }
    }
}
