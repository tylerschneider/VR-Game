using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BandScript : MonoBehaviour
{
    public GameObject healthBand;
    public GameObject manaBand;
    public GameObject healthText;
    public GameObject manaText;

    public Transform bandAnchor;

    public int health = 100;
    public int maxHealth = 100;
    public int mana = 200;
    public int maxMana = 200;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = bandAnchor;
        transform.position = bandAnchor.position;
        transform.rotation = bandAnchor.rotation;

        healthBand = this.gameObject.transform.GetChild(1).gameObject;
        manaBand = this.gameObject.transform.GetChild(2).gameObject;
        healthText = this.gameObject.transform.GetChild(3).gameObject;
        manaText = this.gameObject.transform.GetChild(4).gameObject;

        UpdateBand();
    }

    public void Heal()
    {
        if(health < maxHealth)
        {
            if(health + 5 >= maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += 5;
            }

            UpdateBand();
        }
    }

    public void Damage()
    {
        if (health > 0)
        {
            if (health - 5 <= 0)
            {
                health = 0;
            }
            else
            {
                health -= 5;
            }

            UpdateBand();
        }
    }

    public void UpdateBand()
    {
        healthBand.transform.localRotation = Quaternion.Euler(0, (150f - (health * (105f / maxHealth))), 0);
       
        healthText.GetComponent<TextMeshPro>().SetText("Health: " + health.ToString());
    }

    void Update()
    {

    }
}
