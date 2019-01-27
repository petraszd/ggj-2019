using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject[] Enemies;
    public float Cooldown;
    private float CurrentCooldown;
    public int SpawnMinX_Positive;
    public int SpawnMaxX_Positive;
    public int SpawnMinY_Negative;
    public int SpawnMaxY_Positive;

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

            float X = Random.Range(SpawnMinX_Positive, SpawnMaxX_Positive);
            float Y = Random.Range(SpawnMinY_Negative, SpawnMaxY_Positive);

            bool TrueOrFalse = (Random.value > 0.5f);
            if (TrueOrFalse)
            {
                X = -X;
            }

            Vector3 SpawnLocation = new Vector3(X, Y, 0);
            Instantiate(Enemies[Nr], SpawnLocation, Quaternion.identity);
        }
    }
}
