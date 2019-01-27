using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private static float EPSILON_NEAR_TREE_PISSING = 0.1f;
    private static float NEAR_TREE = 7.0f;
    private static YieldInstruction PISS_TIMER = new WaitForSeconds(1.0f);

    public int MoveSpeed;
    public float StoppingDistance;
    public float MinDistToTree;
    public float MaxDistToTree;
    public ParticleSystem m_particles;
    public Transform m_spritesTransform;

    bool m_isRunning = false;
    bool m_isPissing = false;
    Vector2 m_target;
    Vector2 m_direction;

    Vector2 MoveDirAfterTarget;

    private Rigidbody2D RB;
    private TreeManager TreeManagerCode;
    private Animator m_animator;

    void OnEnable()
    {
        TreeManager.OnTreeMarkedByEnemy += OnTreeMarkedByEnemy;
    }

    void OnDisable()
    {
        TreeManager.OnTreeMarkedByEnemy -= OnTreeMarkedByEnemy;
    }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        TreeManagerCode = TreeManager.GetInstance();
    }

    void FixedUpdate()
    {
        if (!m_isRunning && !m_isPissing) {
            return;
        }

        Vector2 direction;
        if (m_isPissing) {
            direction = m_target - RB.position;
            direction.Normalize();
            float distance = Vector2.Distance(m_target, RB.position);
            if (distance < EPSILON_NEAR_TREE_PISSING) {
                m_isRunning = false;
            }
        } else {
            direction = m_direction;
            direction.Normalize();
        }

        if (m_isRunning) {
            Vector3 scale = m_spritesTransform.localScale;
            var shape = m_particles.shape;
            Vector3 particlesPosition = shape.position;
            if (direction.x > 0.0f) {
                particlesPosition.z = -0.1f;
                shape.position = particlesPosition;

                scale.x = Mathf.Abs(scale.x);
            } else if (direction.x < 0.0f) {
                // Somehow it works with both -0.1f. No time to find out why
                particlesPosition.z = -0.1f;
                shape.position = particlesPosition;

                scale.x = -Mathf.Abs(scale.x);
            }
            m_spritesTransform.localScale = scale;
            RB.MovePosition(RB.position + direction * Time.fixedDeltaTime * MoveSpeed);
        }
    }


    public void BeginRunning()
    {
        if (TreeManagerCode.IsTreeUncotrolledByEnemy())
        {
            m_target = TreeManagerCode.GetRandomTreeTransformUncontrolledByEnemy().position;
            Vector2 direction = m_target - RB.position;
            m_target += new Vector2(-direction.y, direction.x) * Random.Range(MinDistToTree, MaxDistToTree);
            m_direction = m_target - RB.position;
            m_isRunning = true;
        }
    }

    private void OnTreeMarkedByEnemy(Vector2 treePos, int index)
    {
        if (Vector2.Distance(treePos, RB.position) < NEAR_TREE) {
            StartCoroutine(StartPissing(treePos, index));
        }
    }

    IEnumerator StartPissing(Vector2 treePos, int index)
    {
        AudioManager.PlayPiss();
        m_target = treePos;
        TreeManagerCode.LockTree(index);
        m_isPissing = true;
        while(m_isRunning) {
            yield return null;
        }

        m_animator.SetBool("Is_Pissing", true);
        m_particles.Play();
        yield return PISS_TIMER;
        m_isPissing = false;
        m_isRunning = true;
        m_animator.SetBool("Is_Pissing", false);
        TreeManagerCode.UnlockTree(index);
    }
}

