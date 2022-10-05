using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public abstract class PlayerRole : MonoBehaviourPun
{
    public enum PlayerRoles
    {
        Crewmate = 0,
        Imposter = 1,
    }

    public static float InteractRadius
    {
        get => GameManager.Instance.PlayerInteractRadius;
        set => GameManager.Instance.PlayerInteractRadius = value;
    }

    [SerializeField]
    PlayerInteractionUI interactionUI;

    public Player PlayerOwner { get; set; }

    public PlayerInteractionUI InteractionUI { get => interactionUI; set => interactionUI = value; }

    void Start()
    {
        interactionUI.LocalPlayer = PlayerOwner;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        LocalUpdate();
    }


#if DEBUG || UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    void OnDrawGizmos()
    {
        if (GameManager.Instance != null && GameManager.DEBUG_MODE)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, InteractRadius);
            Gizmos.color = Color.white;
        }
    }
#endif

    protected abstract void LocalUpdate();

    public abstract PlayerRoles GetPlayerRole();
}
