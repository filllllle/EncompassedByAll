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
    TextMeshProUGUI playerName;

    [SerializeField]
    PlayerHeadwear headwearSocket;

    List<Task> playerTasks;

    Camera playerCamera;

    public CapsuleCollider2D PlayerCollider { get => playerCollider; }

    public Rigidbody2D PlayerRigidbody { get => playerRigidbody; }

    public Camera PlayerCamera { get => playerCamera; }

    public PlayerHeadwear HeadwearSocket { get => headwearSocket; }

    public Photon.Realtime.Player PunPlayer { get => photonView.Controller; }

    void Start()
    {
        playerNameText.transform.SetParent(GameManager.Instance.WorldUI.transform);

        playerNameText.text = EncompassedByAll.Instance.PlayerName;
        playerNameText.text = PunPlayer.NickName;

        GameManager.AddPlayer(this);

        photonView.Owner.TagObject = gameObject;


        if(photonView.IsMine)
        {
            playerCamera = GameManager.Instance.MainCamera;
        }
    }

    void OnDestroy()
    {
        Destroy(playerNameText);
        GameManager.RemovePlayer(this);
    }

    void Update()
    {
        playerNameText.transform.position = transform.position + Vector3.up * 1.0f;

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


        PlayerRole playerRole = null;

        switch (role)
        {
            case PlayerRole.PlayerRoles.Crewmate:
                playerRole = gameObject.AddComponent<CrewmateRole>();
                break;
            case PlayerRole.PlayerRoles.Imposter:
                playerRole = gameObject.AddComponent<ImposterRole>();
                break;
            default:
                break;
        }

        if(playerRole)
        {
            playerRole.InteractionUI = GameManager.Instance.PlayerUI.GetComponent<PlayerInteractionUI>();
        }
    }
}
