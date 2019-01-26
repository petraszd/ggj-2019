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
    
    float DistanceToOtherTrees;

    TreeManager TreeManagerCode;
    GameObject TreeManagerGO;

    Vector2 MoveDir;
    public GameObject RestartMenu;

    void Start()
    {
        TreeManagerGO = GameObject.FindGameObjectWithTag("Tree Manager");
        TreeManagerCode = TreeManagerGO.GetComponent<TreeManager>();
        if (TreeManagerCode.TreesForEnemies.Count != 0)
        {
            Target = TreeManagerCode.TreesForEnemies[Random.Range(0, TreeManagerCode.TreesForEnemies.Count)];
            MoveDir = (transform.position - Target.position).normalized;
        }
    }
    
    void Update()
    {
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
                    transform.position = Vector2.MoveTowards(transform.position, AdditionalTarget.position, Time.deltaTime * MoveSpeed);
                    MoveDir = (transform.position - AdditionalTarget.position).normalized;
                    //transform.LookAt(AdditionalTarget);
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
                transform.Translate(-MoveDir * MoveSpeed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, Target.position) >= StoppingDistance)
            {
                /// Go to a tree
                transform.position = Vector2.MoveTowards(transform.position, Target.position, Time.deltaTime * MoveSpeed);
                MoveDir = (transform.position - Target.position).normalized;
                //transform.LookAt(Target);
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
