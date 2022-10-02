using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class PlayerRole : MonoBehaviourPun
{
    public static float InteractRadius
    {
        get => GameManager.Instance.PlayerInteractRadius;
        set => GameManager.Instance.PlayerInteractRadius = value;
    }

    void Start()
    {

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

    protected virtual void LocalUpdate()
    {

    }
}
