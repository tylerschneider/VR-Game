using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if (BattleManager.Instance.battling == true && BattleManager.Instance.currentTurnGo == Player.Instance.gameObject && BattleManager.Instance.attacked == false)
            {
                BattleManager.Instance.AttackEnemy(other.GetComponent<Enemy>(), Player.Instance.swordDamage);

                BattleManager.Instance.EndTurn();
            }
        }
    }
}
