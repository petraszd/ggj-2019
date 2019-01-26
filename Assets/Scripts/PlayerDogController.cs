using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerDogController : MonoBehaviour
{
    private static string ANIM_BLEND_IDLE_RUNNING = "Blend_Idle_Running";
    private static float RATIO_WHEN_RUNNING_IN = 0.3f;
    private static float STAND_STILL_EPSILON = 0.2f;
    private static float SWITICH_BETWEAN_ANIMS_SPEED = 8.0f;

    public GirlController m_girl = null;
    public float m_radius = 2.0f;
    public float m_speed = 1.0f;
    public Transform m_spritesTransform;

    private Rigidbody2D m_girlRigidbody;
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Vector2 m_target;
    private Camera m_camera;
    private bool m_isRunning;
    private float m_runningBlendValue = 0.0f;

    public float Radius
    {
        get { return m_radius; }
    }

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_isRunning = false;
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
        UpdateAnimatorBlendTrees();

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
            if (!m_isRunning) {
                m_isRunning = true;
                OnStartRunning();
            }
        } else {
            if (m_isRunning) {
                m_isRunning = false;
                OnStopRunning();
            }
        }
    }

    void UpdateAnimatorBlendTrees()
    {
        if (m_isRunning && m_runningBlendValue <= 1.0f) {
            m_runningBlendValue += Time.deltaTime * SWITICH_BETWEAN_ANIMS_SPEED;
        } else if (!m_isRunning && m_runningBlendValue > 0.0f) {
            m_runningBlendValue -= Time.deltaTime * SWITICH_BETWEAN_ANIMS_SPEED;
        }

        m_animator.SetFloat(ANIM_BLEND_IDLE_RUNNING, m_runningBlendValue);
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

    void OnStartRunning()
    {
        m_runningBlendValue = 0.0f;

        Vector2 current = m_rigidbody.position;
        Vector2 next = m_target;

        if (current.x > next.x) {
            Vector3 scale = m_spritesTransform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            m_spritesTransform.localScale = scale;
        } else {
            Vector3 scale = m_spritesTransform.localScale;
            scale.x = Mathf.Abs(scale.x);
            m_spritesTransform.localScale = scale;
        }
    }

    void OnStopRunning()
    {
        m_runningBlendValue = 1.0f;
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
