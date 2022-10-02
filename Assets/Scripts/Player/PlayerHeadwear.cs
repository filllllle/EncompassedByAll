using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHeadwear : MonoBehaviourPunCallbacks
{
    [SerializeField]
    SpriteRenderer playerSprite;

    [SerializeField]
    SpriteRenderer headwearSprite;

    Sprite HeadwearController
    {
        get => headwearSprite.sprite;
        set
        {
            headwearSprite.sprite = value;
        }
    }

    string headwearName;


    void Start()
    {

        if (photonView.IsMine)
        {
            ExitGames.Client.Photon.Hashtable entries = new ExitGames.Client.Photon.Hashtable();
            entries.Add("headgear", "sus");
            photonView.Owner.SetCustomProperties(entries);
        }
    }

    void Update()
    {
        headwearSprite.flipX = playerSprite.flipX;
    }


    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (photonView.Owner == targetPlayer && changedProps.ContainsKey("headgear"))
        {
            object txt;
            if(changedProps.TryGetValue("headgear", out txt))
            {
                headwearName = txt as string;
                Debug.Log("got " + headwearName);

                HeadwearController = Resources.Load<Sprite>("Hats/" + headwearName);
            }
        }
    }
}
