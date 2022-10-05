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
    PlayerRole playerRole;

    [SerializeField]
    PlayerAnimationManager playerAnimationManager;

    [SerializeField]
    CapsuleCollider2D playerCollider;

    [SerializeField]
    Rigidbody2D playerRigidbody;

    [SerializeField]
    Canvas playerCanvasUI;

    [SerializeField]
    TextMeshProUGUI playerName;

    [SerializeField]
    PlayerHeadwear headwearSocket;

    Camera playerCamera;

    public CapsuleCollider2D PlayerCollider { get => playerCollider; }

    public Rigidbody2D PlayerRigidbody { get => playerRigidbody; }

    public Camera PlayerCamera { get => playerCamera; }

    public PlayerHeadwear HeadwearSocket { get => headwearSocket; }

    public Photon.Realtime.Player PunPlayer { get => photonView.Controller; }

    void Start()
    {
        playerCanvasUI.gameObject.SetActive(false);

        playerNameText.text = EncompassedByAll.Instance.PlayerName;
        playerNameText.text = PunPlayer.NickName;

        GameManager.AddPlayer(this);

        photonView.Owner.TagObject = gameObject;


        if(photonView.IsMine)
        {
            playerCamera = GameManager.Instance.MainCamera;
            playerCanvasUI.gameObject.SetActive(true);
            playerCanvasUI.worldCamera = PlayerCamera;
        }
    }

    void OnDestroy()
    {
        GameManager.RemovePlayer(this);
    }

    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }  
    }

    void RecieveRole(PlayerRole.PlayerRoles role)
    {
        if (photonView.IsMine)
        {
            Debug.LogFormat("I am {0}", role);
        }

        switch (role)
        {
            case PlayerRole.PlayerRoles.Crewmate:
                gameObject.AddComponent<CrewmateRole>();
                break;
            case PlayerRole.PlayerRoles.Imposter:
                gameObject.AddComponent<ImposterRole>();
                break;
            default:
                break;
        }
    }
}
