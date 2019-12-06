using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : Enemy
{
    public override void Attack1()
    {
        BattleManager.Instance.AttackPlayer(this, attackDamage[0]);
        BattleManager.Instance.EndTurn();
    }
}
