using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWell : MonoBehaviour
{
    private Player m_player = null;

    public void AffectPlayer(Player player)
    {
        m_player = player;
    }
}
