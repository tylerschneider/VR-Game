using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public List<GameObject> enemies;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy.gameObject);
        enemy.transform.parent = gameObject.transform;
    }

    public void RemoveEnemey(Enemy enemy)
    {
        enemies.Remove(enemy.gameObject);
        enemy.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
