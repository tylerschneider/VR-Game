using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : Enemy
{
    public override void Attack1()
    {
        transform.Translate(new Vector3(0, 0.1f, 0));

        BattleManager.Instance.AttackPlayer(this, attackDamage[0]);
        BattleManager.Instance.EndTurn();
    }
}
