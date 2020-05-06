using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public GameObject hitScreen;
    public GameObject pauseScreen;

    public GameObject healthBand;
    public GameObject manaBand;
    public GameObject healthText;
    public GameObject manaText;

    public int health = 100;
    public int maxHealth = 100;
    public int mana = 200;
    public int maxMana = 200;

    public float lowHealthAmount = 0.3f;

    public int swordDamage = 10;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        UpdateBand();
    }

    public void Heal(int healAmount)
    {
        if (health < maxHealth)
        {
            if (health + healAmount >= maxHealth)
            {
                health = maxHealth;
                SoundManager.Instance.playPowerupSound();
            }
            else
            {
                health += healAmount;
            }

            if(hitScreen.activeSelf == true && health >= maxHealth * lowHealthAmount)
            {
                hitScreen.SetActive(false);
            }

            UpdateBand();
        }
    }

    public void Damage(int damageAmount)
    {
        if (health > 0)
        {
            if (health - damageAmount <= 0)
            {
                health = 0;
                SoundManager.Instance.playDeadSound();
            }
            else
            {
                health -= damageAmount;
                SoundManager.Instance.playPowerdownSound();
            }

            UpdateBand();
        }

        transform.Find("OVRCameraRig/TrackingSpace/LocalAvatar/hand_left").transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        transform.Find("OVRCameraRig/TrackingSpace/LocalAvatar/hand_right").transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        hitScreen.SetActive(true);

        StartCoroutine(StopDamage());
    }

    IEnumerator StopDamage()
    {
        yield return new WaitForSeconds(0.3f);
        transform.Find("OVRCameraRig/TrackingSpace/LocalAvatar/hand_left").transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
        transform.Find("OVRCameraRig/TrackingSpace/LocalAvatar/hand_right").transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;

        if(health > maxHealth * lowHealthAmount)
        {
            hitScreen.SetActive(false);
        }
    }

    public void UpdateBand()
    {
        healthBand.transform.localRotation = Quaternion.Euler(0, (150f - (health * (105f / maxHealth))), 0);

        healthText.GetComponent<TextMeshPro>().SetText("Health: " + health.ToString());
    }

    private void Update()
    {
        if(health == 0)
        {
            //SceneChanger.Instance.LoadScene(3);
        }

        if(OVRInput.GetUp(OVRInput.RawButton.Start))
        {
            if(pauseScreen.activeSelf == true)
            {
                //Time.timeScale = 1;
                pauseScreen.SetActive(false);
            }
            else
            {
                //Time.timeScale = 0;
                pauseScreen.SetActive(true);
            }
        }
    }
}
