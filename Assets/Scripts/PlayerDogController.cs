using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerDogController : MonoBehaviour
{
    public delegate void EventPlayerDogPulled();
    public static event EventPlayerDogPulled OnPlayerDogPulled;

    private static YieldInstruction PISS_TIMER = new WaitForSeconds(1.0f);
    private static string ANIM_BLEND_IDLE_RUNNING = "Blend_Idle_Running";
    private static float RATIO_WHEN_RUNNING_IN = 0.3f;
    private static float STAND_STILL_EPSILON = 0.2f;
    private static float SWITICH_BETWEAN_ANIMS_SPEED = 8.0f;

    public GirlController m_girl = null;
    public float m_radius = 2.0f;
    public float m_speed = 1.0f;

    [HeaderAttribute("Child Objects")]
    public Transform m_spritesTransform;
    public ParticleSystem m_particles;
    public Transform m_targetIcon;

    private Rigidbody2D m_girlRigidbody;
    private Rigidbody2D m_rigidbody;
    private Animator m_animator;
    private Vector2 m_target;
    private Camera m_camera;
    private bool m_isRunning;
    private bool m_isPissing;
    private float m_runningBlendValue = 0.0f;
    private TreeManager m_treeManager;

    public float Radius
    {
        get { return m_radius; }
    }

    void OnEnable()
    {
        TreeManager.OnTreeMarkedByPlayer += OnStartPissingOnTree;
    }

    void OnDisable()
    {
        TreeManager.OnTreeMarkedByPlayer -= OnStartPissingOnTree;
    }

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_isRunning = false;
        m_isPissing = false;

        // Hack to prevent 2d fighting
        m_targetIcon.parent = null;
    }

    void Start()
    {
        if (m_girl == null) {
            m_girl = GameObject.Find("Girl").GetComponent<GirlController>();
        }

        Debug.Assert(m_girl != null);
        m_girlRigidbody = m_girl.GetComponent<Rigidbody2D>();
        m_camera = Camera.main;

        m_target = m_girlRigidbody.position + Vector2.right * m_radius * 0.5f;

        m_treeManager = TreeManager.GetInstance();
    }

    void Update()
    {
        UpdateAnimatorBlendTrees();

        m_targetIcon.position = m_target;

        if (Input.GetMouseButtonDown(0)) {
            HandlePointInput(Input.mousePosition);
        }
        for (int i = 0; i < Input.touches.Length; ++i) {
            Touch t = Input.touches[i];
            HandlePointInput(t.position);
        }
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

        if (m_isRunning) {
            UpdateSpritesScaleX();
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
        if (m_isPissing) {
            return;
        }
        Vector2 pos = m_camera.ScreenToWorldPoint(screenPosition);

        m_target = pos;
    }

    Vector2 CalculateRealTarget()
    {
        Vector2 girlPos = m_girlRigidbody.position;
        Vector2 dogPos = m_rigidbody.position;

        if (!m_isPissing && !IsWithinRadius()) {
            Vector2 direction = girlPos - dogPos;
            direction.Normalize();

            EmitPlayerDogPulled();
            return girlPos - direction * (m_radius * (1.0f - RATIO_WHEN_RUNNING_IN));
        }

        return m_target;
    }

    void OnStartRunning()
    {
        m_runningBlendValue = 0.0f;
    }

    void OnStopRunning()
    {
        m_runningBlendValue = 1.0f;
    }

    void OnStartPissingOnTree(Vector2 treePosition, int treeIndex)
    {
        StartCoroutine(StartPissing(treePosition, treeIndex));
    }

    IEnumerator StartPissing(Vector2 treePosition, int treeIndex)
    {
        AudioManager.PlayPiss();
        m_target = treePosition;

        m_isPissing = true;
        m_treeManager.LockTree(treeIndex);
        m_girl.IsWaiting = true;

        while (true) {
            yield return null;
            if (!m_isRunning) {
                break;
            }
        }

        m_animator.SetBool("Is_Pissing", true);
        m_particles.Play();

        yield return PISS_TIMER;
        m_girl.IsWaiting = false;
        m_treeManager.UnlockTree(treeIndex);
        m_animator.SetBool("Is_Pissing", false);
        m_isPissing = false;
    }

    void EmitPlayerDogPulled()
    {
        if (OnPlayerDogPulled != null) {
            OnPlayerDogPulled();
        }
    }

    void UpdateSpritesScaleX()
    {
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
