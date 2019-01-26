using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GirlController : MonoBehaviour
{
    private static float MIN_DISTANCE_BEFORE_CHOOSING_NEW_DIRECTION = 0.2f;
    private PathManager m_path;

    public PlayerDogController m_dog;
    public float m_speed;
    public int m_currentPoint;
    private int m_previousPoint = -1;

    [HeaderAttribute("Hierarchy")]
    public Transform m_spritesTransform;

    private Rigidbody2D m_rigidbody;
    private Rigidbody2D m_dogRigidbody;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        m_path = PathManager.GetInstance();
        if (m_dog == null) {
            m_dog = GameObject.Find("Player_Dog").GetComponent<PlayerDogController>();
        }
        Debug.Assert(m_dog != null);
        m_dogRigidbody = m_dog.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 target = m_path.Points[m_currentPoint];
        Vector2 direction = target - m_rigidbody.position;
        float distance = direction.magnitude;
        if (distance < MIN_DISTANCE_BEFORE_CHOOSING_NEW_DIRECTION) {
            int temp = m_currentPoint;
            m_currentPoint = ChooseNewPoint();
            UpdateSpritesScaleX(temp, m_currentPoint);
            m_previousPoint = temp;
        }
        direction.Normalize();
        Vector2 delta = direction * m_speed * Time.fixedDeltaTime;
        m_rigidbody.MovePosition(m_rigidbody.position + delta);
    }

    int ChooseNewPoint()
    {
        int[] indexes = GetConnectedPointIndexes();
        Vector2 dogPos = m_dogRigidbody.position;
        Vector2 girlPos = m_rigidbody.position;
        float[] distances = new float[indexes.Length];

        for (int i = 0; i < indexes.Length; ++i) {
            Vector2 point = m_path.Points[indexes[i]];
            Vector2 direction = point - girlPos;
            direction.Normalize();
            distances[i] = Vector2.Distance(dogPos, girlPos + direction * m_dog.Radius);
        }

        int dogPrefersIndex = Random.Range(0, indexes.Length);
        for (int i = 0; i < indexes.Length; ++i) {
            float distance = distances[i];
            if (distance < distances[dogPrefersIndex]) {
                dogPrefersIndex = i;
            }
        }

        return indexes[dogPrefersIndex];
    }

    void UpdateSpritesScaleX(int currentIndex, int nextIndex)
    {
        Vector2 current = m_path.Points[currentIndex];
        Vector2 next = m_path.Points[nextIndex];

        if (current.x > next.x) {
            Vector3 scale = m_spritesTransform.localScale;
            scale.x = Mathf.Abs(scale.x);
            m_spritesTransform.localScale = scale;
        } else {
            Vector3 scale = m_spritesTransform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            m_spritesTransform.localScale = scale;
        }
    }

    int[] GetConnectedPointIndexes()
    {
        List<int> result = new List<int>();
        for (int i = 0; i < m_path.Connections.Length; ++i) {
            PointConnection conn = m_path.Connections[i];

            if (conn.index0 == m_currentPoint && conn.index1 != m_previousPoint) {
                result.Add(m_path.Connections[i].index1);
            } else if (conn.index1 == m_currentPoint && conn.index0 != m_previousPoint) {
                result.Add(m_path.Connections[i].index0);
            }
        }
        return result.ToArray();
    }
}
