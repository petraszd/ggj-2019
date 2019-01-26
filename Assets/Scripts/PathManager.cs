using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    private static Color COLOR_CONNECTION = Color.magenta;
    private static Color COLOR_NORMAL_POINT = Color.cyan;

    private static PathManager m_instance;

    public Vector2[] Points;
    public PointConnection[] Connections;

    public static PathManager GetInstance()
    {
        Debug.Assert(m_instance != null);
        return m_instance;
    }

    void Awake()
    {
        if (m_instance == null) {
            m_instance = this;
        } else if (m_instance != this) {
            Destroy(gameObject);
        }
    }

    // Gizmos For Debuging
    // -------------------
    void OnDrawGizmos()
    {
        for (int i = 0; i < Connections.Length; ++i) {
            GizmosDrawConnection(Connections[i]);
        }

        for (int i = 0; i < Points.Length; ++i) {
            GizmosDrawPoint(i);
        }
    }

    void GizmosDrawPoint(int index)
    {
        Gizmos.color = COLOR_NORMAL_POINT;
        Gizmos.DrawWireSphere(Points[index], 0.4f);
    }

    void GizmosDrawConnection(PointConnection conn)
    {
        Vector2 a = Points[conn.index0];
        Vector2 b = Points[conn.index1];

        Gizmos.color = COLOR_CONNECTION;
        Gizmos.DrawLine(a, b);
    }
}
