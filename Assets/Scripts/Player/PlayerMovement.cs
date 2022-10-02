using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerMovement : MonoBehaviourPun
{

    Player player;

    [SerializeField]
    float movementSpeed;

    RaycastHit2D[] moveCaster;

    void Start()
    {
        player = GetComponent<Player>();

        if (photonView.IsMine)
        {
            moveCaster = new RaycastHit2D[20];
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector2 moveVector = (Vector2.up * v + Vector2.right * h) * movementSpeed * Time.deltaTime;
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 futurePosition = currentPosition + moveVector;

        CapsuleCollider2D col = player.PlayerCollider;
        Vector2 origin = col.bounds.center;
        Vector2 size = col.size;

        Debug.DrawLine(origin, origin + size);
        Debug.DrawLine(origin, new Vector2(origin.x + size.x, origin.y - size.y));

        Debug.DrawLine(origin, origin + moveVector.normalized);



        int hits = col.Cast(moveVector.normalized, moveCaster, moveVector.magnitude + 0.1f, true);
        if (hits == 0)
        {
            transform.Translate(moveVector);
        }
        else
        {
            Vector2 horizontal = Vector2.right * h * movementSpeed * Time.deltaTime;
            Vector2 vertical = Vector2.up * v * movementSpeed * Time.deltaTime;
            hits = col.Cast(horizontal.normalized, moveCaster, horizontal.magnitude + 0.1f, true);

            if (hits == 0)
            {
                transform.Translate(horizontal);
            }
            else
            {
                hits = col.Cast(vertical.normalized, moveCaster, vertical.magnitude + 0.1f, true);

                if (hits == 0)
                {
                    transform.Translate(vertical);
                }
            }
        }
    }
}
