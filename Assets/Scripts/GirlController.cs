using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GirlController : MonoBehaviour
{
  static Color COLOR_NORMAL_POINT = Color.magenta;
  static Color COLOR_CONNECTION = Color.cyan;
  static Color COLOR_CURRENT_POINT = Color.red;

  [Header("Tracel Path")]
  public int m_currentPoint;
  private int m_previousPoint = -1;

  public Vector2[] m_points;
  public PointConnection[] m_connections;

  [Space(10)]
  [Header("Various")]
  public float m_speed;

  private Rigidbody2D m_rigidbody;

  void Awake()
  {
    m_rigidbody = GetComponent<Rigidbody2D>();
    Debug.Assert(m_rigidbody != null);
  }

  void FixedUpdate()
  {
    Vector2 target = m_points[m_currentPoint];
    Vector2 direction = target - m_rigidbody.position;
    float distance = direction.magnitude;
    if (distance < 0.2f) {
      int temp = m_currentPoint;
      m_currentPoint = ChooseNewPoint();
      m_previousPoint = temp;
    }
    direction.Normalize();
    m_rigidbody.MovePosition(m_rigidbody.position + direction * m_speed);
  }

  int ChooseNewPoint()
  {
    List<int> connectedPoints = new List<int>();
    for (int i = 0; i < m_connections.Length; ++i) {
      PointConnection conn = m_connections[i];

      if (conn.index0 == m_currentPoint && conn.index1 != m_previousPoint) {
        connectedPoints.Add(m_connections[i].index1);
      } else if (conn.index1 == m_currentPoint && conn.index0 != m_previousPoint) {
        connectedPoints.Add(m_connections[i].index0);
      }
    }

    return connectedPoints[Random.Range(0, connectedPoints.Count)];
  }

  // Gizmos For Debuging
  // -------------------
  void OnDrawGizmosSelected()
  {
    for (int i = 0; i < m_connections.Length; ++i) {
      GizmosDrawConnection(m_connections[i]);
    }

    for (int i = 0; i < m_points.Length; ++i) {
      GizmosDrawPoint(i);
    }
  }

  void GizmosDrawPoint(int index)
  {
    if (index == m_currentPoint) {
      Gizmos.color = COLOR_CURRENT_POINT;
    } else {
      Gizmos.color = COLOR_NORMAL_POINT;
    }
    Gizmos.DrawWireSphere(m_points[index], 0.4f);
  }

  void GizmosDrawConnection(PointConnection conn)
  {
    Vector2 a = m_points[conn.index0];
    Vector2 b = m_points[conn.index1];

    Gizmos.color = COLOR_CONNECTION;
    Gizmos.DrawLine(a, b);
  }
}
