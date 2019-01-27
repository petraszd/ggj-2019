using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Target;
    public Transform AdditionalTarget;

    public int MoveSpeed;
    public float StoppingDistance;
    public float TreeSenseRadius;
    public float MinDistToTree;
    public float MaxDistToTree;

    float DistanceToOtherTrees;

    Vector2 MoveDir;
    Vector2 MoveDirOffset;
    Vector2 MoveDirAfterTarget;

    private Rigidbody2D RB;
    private TreeManager TreeManagerCode;

    void Start()
    {
        TreeManagerCode = TreeManager.GetInstance();

        RB = gameObject.GetComponent<Rigidbody2D>();
        if (TreeManagerCode.IsTreeUncotrolledByEnemy())
        {
            Target = TreeManagerCode.GetRandomTreeTransformUncontrolledByEnemy();
            MoveDir = RB.position - (Vector2)Target.position;
        }
    }

    void Update()
    {
        if (Mathf.Abs(RB.position.x) > 40 || Mathf.Abs(RB.position.y) > 30)
        {
            Destroy(gameObject);
        }

        if (Target != null)
        {
            if (AdditionalTarget == null)
            {
                DistanceToOtherTrees = 1000;
                foreach (Transform t in TreeManagerCode.Trees)
                {
                    if (Vector2.Distance(RB.position, t.position) < Vector2.Distance(RB.position, Target.position) && Vector2.Distance(RB.position, t.position) < DistanceToOtherTrees && Vector2.Distance(RB.position, t.position) < TreeSenseRadius && t.GetComponent<TreeController>().Owner != TreeOwnerType.Enemy)
                    {
                        AdditionalTarget = t;
                        DistanceToOtherTrees = Vector2.Distance(RB.position, t.position);
                    }
                }
            }

            if (AdditionalTarget != null)
            {
                /// Looking for additional trees along the way
                if (Vector2.Distance(RB.position, AdditionalTarget.position) >= StoppingDistance)
                {
                    /// Go to a tree
                    MoveDir = RB.position - (Vector2)AdditionalTarget.position;
                    MoveDirOffset = new Vector2(-MoveDir.y, MoveDir.x).normalized * Random.Range(MinDistToTree, MaxDistToTree);
                    RB.position = Vector2.MoveTowards(RB.position, MoveDirOffset + new Vector2(AdditionalTarget.position.x, AdditionalTarget.position.y), Time.deltaTime * MoveSpeed);
                }
                else
                {
                    /// Tree reached
                    AdditionalTarget = null;
                }
            }
            else if (Target.GetComponent<TreeController>().Owner == TreeOwnerType.Enemy)
            {
                /// Go away
                RB.MovePosition(-MoveDirAfterTarget.normalized * MoveSpeed * Time.deltaTime);
            }
            else if (Vector2.Distance(RB.position, Target.position) >= StoppingDistance)
            {
                /// Go to a tree
                MoveDir = RB.position - (Vector2)Target.position;
                MoveDirOffset = new Vector2(-MoveDir.y, MoveDir.x).normalized * Random.Range(MinDistToTree, MaxDistToTree);
                RB.position = Vector2.MoveTowards(RB.position, MoveDirOffset + new Vector2(Target.position.x, Target.position.y), Time.deltaTime * MoveSpeed);
                MoveDirAfterTarget = RB.position - (MoveDirOffset + (Vector2)Target.position);
            }
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

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Target;
    public Transform AdditionalTarget;

    public int MoveSpeed;
    public float StoppingDistance;
    public float TreeSenseRadius;
    public float MinDistToTree;
    public float MaxDistToTree;

    float DistanceToOtherTrees;

    TreeManager TreeManagerCode;
    GameObject TreeManagerGO;

    Vector2 MoveDir;
    Vector2 MoveDirOffset;
    Vector2 MoveDirAfterTarget;

    void Start()
    {
        TreeManagerGO = GameObject.FindGameObjectWithTag("Tree Manager");
        TreeManagerCode = TreeManagerGO.GetComponent<TreeManager>();
        if (TreeManagerCode.TreesForEnemies.Count != 0)
        {
            Target = TreeManagerCode.TreesForEnemies[Random.Range(0, TreeManagerCode.TreesForEnemies.Count)];
            MoveDir = transform.position - Target.position;
        }
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 20 || Mathf.Abs(transform.position.y) > 15)
        {
            Destroy(gameObject);
        }

        if (Target != null)
        {
            if (AdditionalTarget == null)
            {
                DistanceToOtherTrees = 1000;
                foreach (Transform t in TreeManagerCode.Trees)
                {
                    if (Vector2.Distance(transform.position, t.position) < Vector2.Distance(transform.position, Target.position) && Vector2.Distance(transform.position, t.position) < DistanceToOtherTrees && Vector2.Distance(transform.position, t.position) < TreeSenseRadius && t.GetComponent<TreeController>().Owner != -1)
                    {
                        AdditionalTarget = t;
                        DistanceToOtherTrees = Vector2.Distance(transform.position, t.position);
                    }
                }
            }

            if (AdditionalTarget != null)
            {
                /// Looking for additional trees along the way
                if (Vector2.Distance(transform.position, AdditionalTarget.position) >= StoppingDistance)
                {
                    /// Go to a tree
                    MoveDir = transform.position - AdditionalTarget.position;
                    MoveDirOffset = new Vector2(-MoveDir.y, MoveDir.x).normalized * Random.Range(MinDistToTree, MaxDistToTree);
                    transform.position = Vector2.MoveTowards(transform.position, MoveDirOffset + new Vector2(AdditionalTarget.position.x, AdditionalTarget.position.y), Time.deltaTime * MoveSpeed);
                }
                else
                {
                    /// Tree reached
                    AdditionalTarget = null;
                }
            }
            else if (Target.GetComponent<TreeController>().Owner == -1)
            {
                /// Go away
                transform.Translate(-MoveDirAfterTarget.normalized * MoveSpeed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, Target.position) >= StoppingDistance)
            {
                /// Go to a tree
                MoveDir = transform.position - Target.position;
                MoveDirOffset = new Vector2(-MoveDir.y, MoveDir.x).normalized * Random.Range(MinDistToTree, MaxDistToTree);
                transform.position = Vector2.MoveTowards(transform.position, MoveDirOffset + new Vector2(Target.position.x, Target.position.y), Time.deltaTime * MoveSpeed);
                MoveDirAfterTarget = transform.position - (new Vector3(MoveDirOffset.x, MoveDirOffset.y, 0) + Target.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        /// Tree sense circle
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, TreeSenseRadius);
    }
}
*/
