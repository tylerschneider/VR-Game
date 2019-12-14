using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && other.isTrigger == false && GetComponent<GrabbableObject>().m_grabbedBy != null)
        {
            if (BattleManager.Instance.battling == true && BattleManager.Instance.currentTurnGo == Player.Instance.gameObject && BattleManager.Instance.attacked == false)
            {
                BattleManager.Instance.AttackEnemy(other.GetComponent<Enemy>(), Player.Instance.swordDamage);

                BattleManager.Instance.EndTurn();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Debug.Log(collision.relativeVelocity);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log(collision.relativeVelocity);
        }
    }

    private void Update()
    {
        if(GetComponent<GrabbableObject>().m_grabbedBy != null && !GameData.Instance.gotSword)
        {
            GameData.Instance.gotSword = true;
        }
    }
}
