using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewmateRole : PlayerRole
{    
    void Start()
    {
        
    }

    public override PlayerRoles GetPlayerRole()
    {
        return PlayerRoles.Crewmate;
    }

    protected override void LocalUpdate()
    {

    }

}
