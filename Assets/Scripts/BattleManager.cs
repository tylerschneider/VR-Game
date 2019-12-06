using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public bool battling = false;
    public bool attacked = false;

    public List<GameObject> enemies;

    public float timeBetweenTurns = 2f;

    public float multiEnemyDamageMulti = 1.5f;

    public GameObject currentTurnGo;
    public int currentEnemy;

    public GameObject itemSword;
    public GameObject itemSwordUsed;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        enemy.transform.parent = gameObject.transform;

        if(enemies.Count == 1)
        {
            StartBattle();
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        enemy.transform.parent = null;

        if(enemies.Count == 0)
        {
            EndBattle();
        }
    }

    public void AttackPlayer(Enemy enemy, int damage)
    {
        if (battling == true && currentTurnGo == enemy.gameObject)
        {
            Player.Instance.Damage(damage);
        }
    }

    public void AttackEnemy(Enemy enemy, int damage)
    {
        if(battling == true && currentTurnGo == Player.Instance.gameObject && attacked == false)
        {
            SetItems(false);
            attacked = true;

            enemy.health -= Mathf.RoundToInt(damage * enemies.Count * multiEnemyDamageMulti);
            if(enemy.health < enemy.maxHealth)
            {
                RemoveEnemy(enemy.gameObject);
                enemy.killEnemy();
            }
        }
    }

    public void EndTurn()
    {
        if (battling == true)
        {
            StartCoroutine(nextTurn());
        }
    }

    public void SetItems(bool active)
    {
        if(active)
        {
            itemSword.SetActive(true);
            itemSwordUsed.SetActive(false);
        }
        else
        {
            itemSword.SetActive(false);
            itemSwordUsed.SetActive(true);
        }

    }

    public void StartBattle()
    {
        battling = true;
        attacked = false;

        currentTurnGo = Player.Instance.gameObject;
    }

    IEnumerator nextTurn()
    {
        yield return new WaitForSeconds(timeBetweenTurns);

        if(battling == true)
        {
            if (currentTurnGo == Player.Instance.gameObject)
            {
                currentEnemy = 0;
                currentTurnGo = enemies[0];
                EnemyTurn();
            }
            else
            {
                currentEnemy++;
                if (currentEnemy <= enemies.Count - 1)
                {
                    currentTurnGo = enemies[currentEnemy];
                    EnemyTurn();
                }
                else
                {
                    currentTurnGo = Player.Instance.gameObject;
                    SetItems(true);
                    attacked = false;
                }
            }
        }
    }

    public void EnemyTurn()
    {
        currentTurnGo.GetComponent<Enemy>().Attack();

        if (battling == true)
        {
            StartCoroutine(nextTurn());
        }
    }

    public void EndBattle()
    {
        battling = false;
        SetItems(true);
        attacked = false;
    }
}
