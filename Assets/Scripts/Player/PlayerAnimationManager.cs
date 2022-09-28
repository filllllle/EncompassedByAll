using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimationManager : MonoBehaviourPun
{
    [SerializeField]
    SpriteRenderer playerSpriteRenderer;

    [SerializeField]
    Animator playerAnimator;

    int animatorSpeedParameterId;
    int animatorDeadParameterId;
    int animatorFacingLeftParameterId;

    void Start()
    {
        if(playerAnimator == null)
        {
            Debug.LogErrorFormat("Player {0} is missing Animator.", gameObject.name);
        }

        animatorSpeedParameterId = Animator.StringToHash("Speed");
        animatorDeadParameterId = Animator.StringToHash("IsDead");
        animatorFacingLeftParameterId = Animator.StringToHash("FacingLeft");
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        playerAnimator.SetFloat(animatorSpeedParameterId, h * h + v * v);
        
        if (h != 0)
        {
            bool faceLeft = h < 0;
            playerAnimator.SetBool(animatorFacingLeftParameterId, faceLeft);

            playerSpriteRenderer.flipX = faceLeft;
        }
    }
}
