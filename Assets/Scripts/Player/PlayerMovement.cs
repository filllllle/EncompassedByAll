using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [SerializeField]
    float movementSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            transform.Translate((Vector2.up * v + Vector2.right * h) * movementSpeed * Time.deltaTime);
        }
    }
}
