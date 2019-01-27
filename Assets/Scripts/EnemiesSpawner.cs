using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public delegate void EventTimeRemaining(float time);
    public static event EventTimeRemaining OnTimeRemaining;

    public float Cooldown;
    private float CurrentCooldown;

    public Transform m_enemyDogsHolder;

    private EnemyController[] m_enemyDogs;
    private int m_currentIndex;

    private float m_timeRemaining;

    void Start()
    {
        if (m_enemyDogsHolder == null) {
            m_enemyDogsHolder = GameObject.Find("Enemy_Dogs").GetComponent<Transform>();
        }
        Debug.Assert(m_enemyDogsHolder != null);

        CurrentCooldown = Cooldown;

        m_enemyDogs = new EnemyController[m_enemyDogsHolder.childCount];
        int index = 0;
        foreach (Transform child in m_enemyDogsHolder) {
            m_enemyDogs[index] = child.GetComponent<EnemyController>();
            index++;
        }
        m_currentIndex = 0;

        m_timeRemaining = Cooldown * (m_enemyDogs.Length + 1) + 5.0f;
        StartCoroutine(CountDownToEnd());
    }

    void EmitTimeRemaining()
    {
        if (OnTimeRemaining != null) {
            OnTimeRemaining(m_timeRemaining);
        }
    }

    void Update()
    {
        if (CurrentCooldown >= 0) {
            CurrentCooldown -= Time.deltaTime;
        } else if (m_currentIndex < m_enemyDogs.Length) {
            CurrentCooldown = Cooldown;
            m_enemyDogs[m_currentIndex].BeginRunning();
            m_currentIndex++;
        }
    }

    IEnumerator CountDownToEnd()
    {
        while (true) {
            yield return null;
            m_timeRemaining -= Time.deltaTime;
            if (m_timeRemaining <= 0.0f) {
                m_timeRemaining = 0.0f;
                EmitTimeRemaining();
                break;
            }
            EmitTimeRemaining();
        }

        TreeManager mgr = TreeManager.GetInstance();
        if (mgr.GetNumberOfPlayerTrees() >= mgr.NumToWin) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
        } else {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }
    }
}

