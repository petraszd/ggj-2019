using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public float Cooldown;
    private float CurrentCooldown;

    public Transform m_enemyDogsHolder;

    private EnemyController[] m_enemyDogs;
    private int m_currentIndex;

    void Start()
    {
        if (m_enemyDogsHolder == null) {
            m_enemyDogsHolder = GameObject.Find("Enemy_Dogs").GetComponent<Transform>();
        }
        Debug.Assert(m_enemyDogsHolder != null);

        CurrentCooldown = Cooldown;

        m_enemyDogs = new EnemyController[m_enemyDogsHolder.childCount];
        int index = 0;
        foreach(Transform child in m_enemyDogsHolder) {
            m_enemyDogs[index] = child.GetComponent<EnemyController>();
            index++;
        }
        m_currentIndex = 0;
    }

    void Update()
    {
        if (CurrentCooldown >= 0)
        {
            CurrentCooldown -= Time.deltaTime;
        }
        else if (m_currentIndex < m_enemyDogs.Length)
        {
            CurrentCooldown = Cooldown;
            m_enemyDogs[m_currentIndex].BeginRunning();
            m_currentIndex++;
        }

        // TODO: delay and annouce that enemies are over
    }
}
