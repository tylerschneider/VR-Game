using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : Enemy
{
    public override void Attack1()
    {
        transform.Translate(new Vector3(0, 0.1f, 0));
        StartCoroutine(Down());
        BattleManager.Instance.AttackPlayer(this, attackDamage[0]);
        BattleManager.Instance.EndTurn();
    }

    IEnumerator Down()
    {
        yield return new WaitForSeconds(0.1f);
        {
            transform.Translate(new Vector3(0, -0.1f, 0));
        }
    }
}
