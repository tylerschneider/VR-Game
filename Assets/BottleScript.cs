using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BottleScript : MonoBehaviour
{
    public float healAmount = 50;
    public float breakVelocity;
    public BottleCollider healCollider;
    public ParticleSystem healParticles;
    public GameObject brokenBottle;
    public TextMeshPro amount;

    private GameObject newBottle;

    private void Start()
    {
        amount.text = healAmount.ToString();
    }
    private void Update()
    {
        if(!transform.Find("Cork") && transform.Find("Potion"))
        {
            if (Vector3.Dot(transform.up, Vector3.down) > 0 && !healParticles.isPlaying)
            {
                healParticles.Play();
                StartCoroutine(PourPotion());
            }
            else if(Vector3.Dot(transform.up, Vector3.down) < 0 && healParticles.isPlaying)
            {
                healParticles.Stop();
                StopAllCoroutines();
            }
        }
        else if(transform.Find("Cork") && transform.Find("Potion"))
        {
            healParticles.Stop();
            StopAllCoroutines();
        }

        amount.text = healAmount.ToString();
    }

    IEnumerator PourPotion()
    {
        yield return new WaitForSeconds(0.1f);

        healAmount -= 1;

        if(healCollider.hitting == true)
        {
            Player.Instance.Heal(1);
        }

        if(healAmount <= 0)
        {
            Destroy(transform.Find("Potion").gameObject);
        }
        else
        {
            StartCoroutine(PourPotion());
        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player" && other.relativeVelocity.magnitude > breakVelocity && newBottle == null)
        {
            newBottle = Instantiate(brokenBottle, transform.position, transform.rotation);
            newBottle.GetComponentInChildren<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            newBottle.GetComponentInChildren<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
            newBottle.GetComponentInChildren<Rigidbody>().AddExplosionForce(1000, other.GetContact(0).point, 10);

            if(!gameObject.transform.Find("Cork"))
            {
                Destroy(newBottle.transform.Find("Cork").gameObject);
            }

            newBottle.GetComponent<AudioSource>().Play();
            newBottle.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.3f);

            Destroy(gameObject);
        }
    }
}
