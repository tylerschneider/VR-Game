using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public bool battling = false;

    public List<GameObject> enemies;

    public float multiEnemyDamageMulti = 1.5f;

    public GameObject currentTurnGo;
    public int currentEnemy;

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

            if (battling == true)
            {
                nextTurn();
            }
        }
    }

    public void AttackEnemy(Enemy enemy, int damage)
    {
        if(battling == true && currentTurnGo == Player.Instance.gameObject)
        {
            enemy.health -= damage;
            if(enemy.health < enemy.maxHealth)
            {
                RemoveEnemy(enemy.gameObject);
                enemy.killEnemy();
            }

            if(battling == true)
            {
                nextTurn();
            }

        }
    }

    public void StartBattle()
    {
        battling = true;

        currentTurnGo = Player.Instance.gameObject;
    }

    public void nextTurn()
    {
        if(currentTurnGo == Player.Instance.gameObject)
        {
            currentTurnGo = enemies[0];
            currentEnemy = 0;
        }
        else
        {
            currentEnemy++;
            if(currentEnemy <= enemies.Count - 1)
            {
                currentTurnGo = enemies[currentEnemy];
            }
            else
            {
                currentTurnGo = Player.Instance.gameObject;
            }
        }
    }

    public void EndBattle()
    {
        battling = false;
    }
}
