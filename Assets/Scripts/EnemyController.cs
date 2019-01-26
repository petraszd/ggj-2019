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
            MoveDir = transform.position - Target.GetChild(0).transform.position;
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
                    if (Vector2.Distance(transform.position, t.GetChild(0).transform.position) < Vector2.Distance(transform.position, Target.GetChild(0).transform.position) && Vector2.Distance(transform.position, t.GetChild(0).transform.position) < DistanceToOtherTrees && Vector2.Distance(transform.position, t.GetChild(0).transform.position) < TreeSenseRadius && t.GetComponent<TreeController>().Owner != -1)
                    {
                        AdditionalTarget = t;
                        DistanceToOtherTrees = Vector2.Distance(transform.position, t.GetChild(0).transform.position);
                    }
                }
            }

            if (AdditionalTarget != null)
            {
                /// Looking for additional trees along the way
                if (Vector2.Distance(transform.position, AdditionalTarget.GetChild(0).transform.position) >= StoppingDistance)
                {
                    /// Go to a tree
                    MoveDir = transform.position - AdditionalTarget.GetChild(0).transform.position;
                    MoveDirOffset = new Vector2(-MoveDir.y, MoveDir.x).normalized * Random.Range(MinDistToTree, MaxDistToTree);
                    transform.position = Vector2.MoveTowards(transform.position, MoveDirOffset + new Vector2(AdditionalTarget.GetChild(0).transform.position.x, AdditionalTarget.GetChild(0).transform.position.y), Time.deltaTime * MoveSpeed);
                    MoveDir = (transform.position - AdditionalTarget.GetChild(0).transform.position).normalized;
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
                MoveDir = transform.position - Target.GetChild(0).transform.position;
                MoveDirOffset = new Vector2(-MoveDir.y, MoveDir.x).normalized * Random.Range(MinDistToTree, MaxDistToTree);
                transform.position = Vector2.MoveTowards(transform.position, MoveDirOffset + new Vector2(Target.GetChild(0).transform.position.x, Target.GetChild(0).transform.position.y), Time.deltaTime * MoveSpeed);
                MoveDirAfterTarget = transform.position - (new Vector3(MoveDirOffset.x, MoveDirOffset.y, 0) + Target.GetChild(0).transform.position);
                MoveDir = (transform.position - Target.GetChild(0).transform.position).normalized;
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
