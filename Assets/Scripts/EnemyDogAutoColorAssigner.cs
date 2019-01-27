using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDogAutoColorAssigner : MonoBehaviour
{
    public SpriteRenderer[] m_renderers;

    void Awake()
    {
        SpriteRenderer r;
        Color c = Random.ColorHSV(0.05f, 0.2f, 0.0f, 0.3f, 0.4f, 1.0f, 1.0f, 1.0f);
        for (int i = 0; i < m_renderers.Length; ++i) {
            r = m_renderers[i];
            r.color = c;
        }
    }
}
