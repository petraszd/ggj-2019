using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GirlController : MonoBehaviour
{
    private PathManager m_path;

    public float m_speed;
    public int m_currentPoint;
    private int m_previousPoint = -1;

    private Rigidbody2D m_rigidbody;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        Debug.Assert(m_rigidbody != null);
    }

    void Start()
    {
        m_path = PathManager.GetInstance();
    }

    void FixedUpdate()
    {
        Vector2 target = m_path.Points[m_currentPoint];
        Vector2 direction = target - m_rigidbody.position;
        float distance = direction.magnitude;
        if (distance < 0.2f) {
            int temp = m_currentPoint;
            m_currentPoint = ChooseNewPoint();
            m_previousPoint = temp;
        }
        direction.Normalize();
        Vector2 delta = direction * m_speed * Time.fixedDeltaTime;
        m_rigidbody.MovePosition(m_rigidbody.position + delta);
    }

    int ChooseNewPoint()
    {
        List<int> connectedPoints = new List<int>();
        for (int i = 0; i < m_path.Connections.Length; ++i) {
            PointConnection conn = m_path.Connections[i];

            if (conn.index0 == m_currentPoint && conn.index1 != m_previousPoint) {
                connectedPoints.Add(m_path.Connections[i].index1);
            } else if (conn.index1 == m_currentPoint && conn.index0 != m_previousPoint) {
                connectedPoints.Add(m_path.Connections[i].index0);
            }
        }

        return connectedPoints[Random.Range(0, connectedPoints.Count)];
    }
}
