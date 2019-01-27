using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private static float EPSILON_NEAR_TREE_PISSING = 0.1f;
    private static YieldInstruction PISS_TIMER = new WaitForSeconds(1.0f);

    public int MoveSpeed;
    public float StoppingDistance;
    public float MinDistToTree;
    public float MaxDistToTree;
    public ParticleSystem m_particles;

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
        BeginRunning();
    }

    void FixedUpdate()
    {
        if (!m_isRunning && !m_isPissing) {
            return;
        }

        if (m_isPissing) {
            Vector2 direction = m_target - RB.position;
            float distance = Vector2.Distance(m_target, RB.position);
            RB.MovePosition(RB.position + direction * Time.fixedDeltaTime * MoveSpeed);
            if (distance < EPSILON_NEAR_TREE_PISSING) {
                m_isRunning = false;
            }
        } else {
            Vector2 direction = m_direction;
            direction.Normalize();
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
        StartCoroutine(StartPissing(treePos, index));
    }

    IEnumerator StartPissing(Vector2 treePos, int index)
    {
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

