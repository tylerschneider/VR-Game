using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public int coinPitch = 70;
    public int maxCoinPitch = 90;
    public float coinTimer = 5;
    private float time = 0;
    private int coinStartPitch;

    private ChuckScript chuckScript;
    private ChuckMainInstance chuck;
    void Start()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        chuckScript = GetComponent<ChuckScript>();
        chuck = GetComponent<ChuckMainInstance>();

        coinStartPitch = coinPitch;
    }

    public void playPowerupSound()
    {
        chuck.RunCode(chuckScript.Script + chuckScript.powerup + @"powerup(40, 3, 50);");
    }

    public void playPowerdownSound()
    {
        //goes from 1 to 0
        float health = Player.Instance.health;
        float maxHealth = Player.Instance.maxHealth;
        //goes from 120 to 20 speed
        float speed = ((health / maxHealth) * 100) + 20;

        //go from 60 to 20
        float pitch = (((health / maxHealth)) * 10) + 40;

        chuck.RunCode(chuckScript.Script + chuckScript.powerup + @"powerup(" + pitch + ", -1, " + speed + ");");
    }

    public void playMysterySound()
    {
        chuck.RunCode(chuckScript.Script + chuckScript.mystery + @"mystery(40, 10, 350);");
    }

    public void playBossSound()
    {
        chuck.RunCode(chuckScript.Script + chuckScript.boss + @"boss(40, 150, 25);");
    }

    public void playDeadSound()
    {
        chuck.RunCode(chuckScript.Script + chuckScript.dead + @"dead();");
    }

    public void playCoinSound()
    {
        chuck.RunCode(chuckScript.Script + chuckScript.coin + @"coin(" + coinPitch + ", 100);");
        if (coinPitch < maxCoinPitch)
        {
            coinPitch++;
            time = 0;
        }
    }

    void Update()
    {
        if(coinPitch > coinStartPitch)
        {
            time += Time.deltaTime;

            if (time > coinTimer)
            {
                coinPitch = coinStartPitch;
                time = 0;
            }
        }

    }
}
