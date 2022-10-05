using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterRole : PlayerRole
{
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
