using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public AudioClip battleMusic;
    public AudioClip bossMusic;
    private AudioClip lastMusic;

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
        /*enemy.GetComponent<Enemy>().health = enemy.GetComponent<Enemy>().maxHealth;

        Color c = enemy.transform.Find("Body").GetComponent<Renderer>().material.color;
        Color.RGBToHSV(c, out float H, out float S, out float V);
        c = Color.HSVToRGB(H, (float)enemy.GetComponent<Enemy>().health / (float)enemy.GetComponent<Enemy>().maxHealth, V);
        enemy.transform.Find("Body").GetComponent<Renderer>().material.color = c;*/

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

            if(Player.Instance.health <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void AttackEnemy(Enemy enemy, int damage)
    {
            SetItems(false);
            attacked = true;

        if(enemies.Count == 1)
        {
            enemy.health -= damage;
        }
        else
        {
            enemy.health -= Mathf.RoundToInt(damage * (enemies.Count - 1) * multiEnemyDamageMulti);
        }


        Color c = enemy.transform.Find("Body").GetComponent<Renderer>().material.color;
        Color.RGBToHSV(c, out float H, out float S, out float V);
        c = Color.HSVToRGB(H, (float)enemy.health / (float)enemy.maxHealth, V);
        enemy.transform.Find("Body").GetComponent<Renderer>().material.color = c;

            if(enemy.health < 0)
            {
                RemoveEnemy(enemy.gameObject);
                enemy.killEnemy();
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

        lastMusic = MusicManager.Instance.currentSong;

        foreach(GameObject enemy in enemies)
        {
            if(enemy.GetComponent<Enemy>().boss == true)
            {
                MusicManager.Instance.ChangeMusic(battleMusic);
                break;
            }
            else
            {
                MusicManager.Instance.ChangeMusic(battleMusic);
            }
        }
    }

    IEnumerator nextTurn()
    {
        yield return new WaitForSeconds(timeBetweenTurns);

        if(battling == true)
        {
            //if it was the player's turn
            if (currentTurnGo == Player.Instance.gameObject)
            {
                //the first enemy attacks
                currentEnemy = 0;
                currentTurnGo = enemies[0];
                EnemyTurn();
            }
            else
            {

                //add one to enemy count
                currentEnemy++;
                //if there is a next enemy
                if (currentEnemy <= enemies.Count - 1)
                {
                    currentTurnGo = enemies[currentEnemy];
                    EnemyTurn();
                }
                //else it's the player's turn
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
    }

    public void EndBattle()
    {
        battling = false;
        SetItems(true);
        attacked = false;

        MusicManager.Instance.ChangeMusic(lastMusic);
    }
}
