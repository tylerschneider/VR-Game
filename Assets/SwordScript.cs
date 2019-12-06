using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Player.Instance.AttackSword(other.GetComponent<Enemy>());
        }
    }
}
