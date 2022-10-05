using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterRole : PlayerRole
{
    float killCooldown;

    void Start()
    {
        
    }

    public override PlayerRoles GetPlayerRole()
    {
        return PlayerRoles.Imposter;
    }

    protected override void LocalUpdate()
    {}
}
