using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject[] Enemies;
    public float Cooldown;
    private float CurrentCooldown;

    void Start()
    {
        CurrentCooldown = Cooldown;
    }
    
    void Update()
    {
        if (CurrentCooldown >= 0)
        {
            CurrentCooldown -= Time.deltaTime;
        }
        else
        {
            CurrentCooldown = Cooldown;
            int Nr = Random.Range(0, Enemies.Length);
            Vector3 SpawnLocation = new Vector3(Random.Range (-10, 10), Random.Range(-4, 4), 0);
            Instantiate(Enemies[Nr], SpawnLocation, Quaternion.identity);
        }
    }
}
