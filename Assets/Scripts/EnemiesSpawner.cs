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

            float X = Random.Range(-15, 15);
            float Y = Random.Range(7, 10);

            bool TrueOrFalse = (Random.value > 0.5f);
            if (TrueOrFalse)
            {
                Y = -Y;
            }

            Vector3 SpawnLocation = new Vector3(X, Y, 0);
            Instantiate(Enemies[Nr], SpawnLocation, Quaternion.identity);
        }
    }
}
