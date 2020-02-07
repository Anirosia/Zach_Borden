using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObj : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject enemy02;
    public GameObject zombie;
    public GameObject zombie02;

    public float damage;

    PersonController player_cs;
    AIController enemy_cs;
    AIController enemy02_cs;
    AIController zombie_cs;
    AIController zombie02_cs;

    string _tag;

    void Start()
    {
        player_cs = player.GetComponent<PersonController>();

        enemy_cs = enemy.GetComponent<AIController>();

        enemy02_cs = enemy02.GetComponent<AIController>();

        zombie_cs = zombie.GetComponent<AIController>();

        zombie02_cs = zombie02.GetComponent<AIController>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Zombie01":
                zombie_cs.TakeDamage(damage);
                break;
            case "Zombie02":
                zombie02_cs.TakeDamage(damage);
                break;
            case "Player":
                player_cs.TakeDamage(damage);
                break;
            case "Enemy01":
                enemy_cs.TakeDamage(damage);
                break;
            case "Enemy02":
                enemy02_cs.TakeDamage(damage);
                break;
        }
    }
}
