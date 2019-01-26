using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerDogController : MonoBehaviour
{
    private static string ANIM_STATE_IS_RUNNING = "Is_Running";
    private static string ANIM_STATE_IS_PISSING = "Is_Pissing";
    private static float RATIO_WHEN_RUNNING_IN = 0.3f;
    private static float STAND_STILL_EPSILON = 0.2f;

    public GirlController m_girl = null;
    public float m_radius = 2.0f;
    public float m_speed = 1.0f;

    private Rigidbody2D m_girlRigidbody;
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Vector2 m_target;
    private Camera m_camera;

    public float Radius
    {
        get { return m_radius; }
    }

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (m_girl == null) {
            m_girl = GameObject.Find("Girl").GetComponent<GirlController>();
        }

        Debug.Assert(m_girl != null);
        m_girlRigidbody = m_girl.GetComponent<Rigidbody2D>();
        m_camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            HandlePointInput(Input.mousePosition);
        }
        // TODO: touch detection
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
            m_animator.SetBool(ANIM_STATE_IS_RUNNING, true);
        } else {
            m_animator.SetBool(ANIM_STATE_IS_RUNNING, false);
        }
    }

    void HandlePointInput(Vector2 screenPosition)
    {
        Vector2 pos = m_camera.ScreenToWorldPoint(screenPosition);

        if (IsWithinRadius(pos)) {
            m_target = pos;
        }
    }

    Vector2 CalculateRealTarget()
    {
        Vector2 girlPos = m_girlRigidbody.position;
        Vector2 dogPos = m_rigidbody.position;

        if (!IsWithinRadius()) {
            Vector2 direction = girlPos - dogPos;
            direction.Normalize();

            return girlPos - direction * (m_radius * (1.0f - RATIO_WHEN_RUNNING_IN));
        }

        return m_target;
    }

    bool IsWithinRadius()
    {
        return IsWithinRadius(m_rigidbody.position);
    }

    bool IsWithinRadius(Vector2 position)
    {
        return Vector2.Distance(m_girlRigidbody.position, position) <= m_radius;
    }

    // Gizmos For Debuging
    // -------------------
    void OnDrawGizmos()
    {
        if (m_girl) {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(m_girlRigidbody.position, m_radius);
            Gizmos.DrawWireSphere(m_target, 0.3f);
        }
    }
}
