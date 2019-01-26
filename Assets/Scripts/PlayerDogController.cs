using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDogController : MonoBehaviour
{
    private static float RATIO_WHEN_RUNNING_IN = 0.5f;
    private static float STAND_STILL_EPSILON = 0.2f;

    public GirlController m_girl = null;
    public float m_radius = 2.0f;
    public float m_speed = 1.0f;

    private Rigidbody2D m_girlRigidbody;
    private Rigidbody2D m_rigidbody;
    private Vector2 m_target;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (m_girl == null) {
            m_girl = GameObject.Find("Girl").GetComponent<GirlController>();
        }

        Debug.Assert(m_girl != null);
        m_girlRigidbody = m_girl.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        m_target = CalculateRealTarget();
        Vector2 direction = m_target - m_rigidbody.position;
        float distance = direction.magnitude;
        direction.Normalize();
        Vector2 delta = direction * m_speed * Time.fixedDeltaTime;

        if (distance > STAND_STILL_EPSILON) {
            m_rigidbody.MovePosition(m_rigidbody.position + delta);
        }
    }

    Vector2 CalculateRealTarget()
    {
        Vector2 girlPos = m_girlRigidbody.position;
        Vector2 dogPos = m_rigidbody.position;

        if (Vector2.Distance(girlPos, dogPos) > m_radius) {
            Vector2 direction = girlPos - dogPos;
            direction.Normalize();

            return girlPos - direction * (m_radius * RATIO_WHEN_RUNNING_IN);
        }

        return m_target;
    }

    // Gizmos For Debuging
    // -------------------
    void OnDrawGizmosSelected()
    {
        if (m_girl) {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(m_girlRigidbody.position, m_radius);
            Gizmos.DrawWireSphere(m_target, 0.3f);
        }
    }
}
