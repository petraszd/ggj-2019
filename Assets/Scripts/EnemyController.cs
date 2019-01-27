using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform AdditionalTarget;

    public int MoveSpeed;
    public float StoppingDistance;
    public float TreeSenseRadius;
    public float MinDistToTree;
    public float MaxDistToTree;

    float DistanceToOtherTrees;

    bool m_hasTarget = false;
    Vector2 m_target;
    Vector2 m_moveDir;
    Vector2 MoveDir
    {
        get { return m_moveDir; }
        set {
            m_moveDir = value;
            m_moveDir.Normalize();
        }
    }

    Vector2 MoveDirAfterTarget;

    private Rigidbody2D RB;
    private TreeManager TreeManagerCode;

    void Start()
    {
        TreeManagerCode = TreeManager.GetInstance();
        RB = gameObject.GetComponent<Rigidbody2D>();
        BeginRunning();
    }

    void FixedUpdate()
    {
        if (!m_hasTarget) {
            return;
        }

        Debug.DrawLine(m_target + Vector2.left, m_target + Vector2.right);
        Debug.DrawLine(m_target + Vector2.up, m_target + Vector2.down);

        RB.MovePosition(RB.position + MoveDir * Time.fixedDeltaTime * MoveSpeed);

    }

    public void BeginRunning()
    {
        if (TreeManagerCode.IsTreeUncotrolledByEnemy())
        {
            m_target = TreeManagerCode.GetRandomTreeTransformUncontrolledByEnemy().position;
            Vector2 direction = m_target - RB.position;
            m_target += new Vector2(-direction.y, direction.x) * Random.Range(MinDistToTree, MaxDistToTree);
            MoveDir = m_target - RB.position;
            m_hasTarget = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (RB == null) {
            return;
        }

        /// Tree sense circle
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(RB.position, TreeSenseRadius);
    }
}

