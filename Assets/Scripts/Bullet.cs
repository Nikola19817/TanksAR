using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    // OBJECT REFERENCES
    [SerializeField]
    public GameObject explosionParticle;
    public GameObject smokeParticle;
    GameObject smokeEffect;

    public GameObject playerWhoShot;
    // BULLET STATS
    public float damage = 20f;
    public float speed = 10f;
    public float radius = 1f;
    public float explosionDelay = 2f;

    // MOVEMENT VARIABLES
    Vector3 bulletVelocity;
    float predictionStepsPerFrame = 6f;

    // EXPLOSION VARIABLES
    float countdown;
    bool hasExploded = false;

    private void Start()
    {
        countdown = explosionDelay;
        bulletVelocity = this.transform.forward * speed;
        smokeEffect=Instantiate(smokeParticle, this.transform.position, this.transform.rotation);
    }
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
        }
        if(smokeEffect.GetComponent<ParticleSystem>().particleCount == 0 && hasExploded)
        {
            Destroy(smokeEffect);
            Destroy(gameObject);
        }
        
        MovePerFrame();
        
    }

    // Calculates trajectory and moves the bullet
    void MovePerFrame()
    {
        if (hasExploded)
            return;
        
        Vector3 point1 = this.transform.position;
        float stepSize = 1.0f / predictionStepsPerFrame;
        for (float step = 0; step < 1; step += stepSize)
        {
            bulletVelocity += Physics.gravity * stepSize * Time.deltaTime;
            Vector3 point2 = point1 + bulletVelocity * stepSize * Time.deltaTime;

            Ray ray = new Ray(point1, point2 - point1);
            if (Physics.Raycast(ray, (point2 - point1).magnitude) && !hasExploded)
            {
                Explode();
            }

            point1 = point2;
        }
        this.transform.LookAt(point1);
        this.transform.position = point1;
        smokeEffect.transform.LookAt(2 * smokeEffect.transform.position - point1);
        smokeEffect.transform.position = point1;
    }
    // Explodes on collision or after a time period, deals damage to players
    public void Explode()
    {
        Instantiate(explosionParticle, this.transform.position, Quaternion.identity, this.transform);
        hasExploded = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        List<GameObject> tanks = new List<GameObject>();

        foreach (Collider item in colliders)
        {
            if(item.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GameObject temp=null;
                if (item.name == "Cev")
                    temp = item.transform.parent.parent.parent.parent.gameObject;
                
                else if (item.name == "Glava")
                    temp = item.transform.parent.parent.gameObject;
                
                else if (item.tag.Equals("Player"))
                    temp = item.gameObject;
                
                if(temp != null)
                    tanks.Add(temp);
            }
        }
        GameObject player1 = GameObject.Find("PlayerOne");
        GameObject player2 = GameObject.Find("PlayerTwo");

        if (tanks.Contains(player1) && tanks.Contains(player2))
        {
            if (playerWhoShot.name == player1.name)
            {
                player1.GetComponent<PlayerStats>().TakeDamage(damage);
                player2.GetComponent<PlayerStats>().TakeDamage(damage);
            }
            else if (playerWhoShot.name == player2.name)
            {
                player2.GetComponent<PlayerStats>().TakeDamage(damage);
                player1.GetComponent<PlayerStats>().TakeDamage(damage);
            }
        }
        else
        {
            if (tanks.Contains(player1))
                player1.GetComponent<PlayerStats>().TakeDamage(damage);
            else if (tanks.Contains(player2))
                player2.GetComponent<PlayerStats>().TakeDamage(damage);
        }
        smokeEffect.GetComponent<ParticleSystem>().Stop();
    }    
}
