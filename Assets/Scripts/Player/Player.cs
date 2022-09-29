using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TextMeshProUGUI playerNameText;

    [SerializeField]
    PlayerMovement playerMovement;

    [SerializeField]
    PlayerAnimationManager playerAnimationManager;

    public Photon.Realtime.Player PunPlayer { get => photonView.Controller; }

    void Start()
    {
        playerNameText.text = EncompassedByAll.Instance.PlayerName;
        playerNameText.text = PunPlayer.NickName;

        GameManager.AddPlayer(this);
    }

    void OnDestroy()
    {
        GameManager.RemovePlayer(this);
    }

    void Update()
    {

    }
}
