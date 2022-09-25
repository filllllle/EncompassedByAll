using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TextMeshProUGUI playerNameText;

    void Start()
    {
        playerNameText.text = EncompassedByAll.Instance.PlayerName;
    }

    void Update()
    {

    }
}
