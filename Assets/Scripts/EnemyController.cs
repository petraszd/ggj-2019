using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Target;
    public int MoveSpeed;
    public float StoppingDistance;
    public GameObject[] TreesGO;
    public Transform[] Trees;

    void Start()
    {
        TreesGO = GameObject.FindGameObjectsWithTag("Tree");
        Trees = new Transform[TreesGO.Length];
        for (int i = 0; i < TreesGO.Length; i++)
        {
             Trees[i] = TreesGO[i].GetComponent<Transform>();
        }
        FindTarget();
    }
    
    void Update()
    {
        if (Target != null)
        {
            if (Vector2.Distance(transform.position, Target.position) >= StoppingDistance)
            {
                /// Go to a tree
                transform.position = Vector2.MoveTowards(transform.position, Target.position, Time.deltaTime * MoveSpeed);
                Target.transform.LookAt(Target);
            }
            else
            {
                /// Mark the tree
                Target.GetComponent<TreeController>().ChangeColor(-1);

                /// Find a new tree
                FindTarget();
            }
        }
    }

    void FindTarget ()
    {
        int Nr = -1;
        float Dist = -1;
        for (int i = 0; i < Trees.Length; i++)
        {
            if (Trees[i] == Target || Trees[i].GetComponent<TreeController>().Owner == -1)
            {

            }
            else if (Vector2.Distance(transform.position, Trees[i].position) < Dist || Dist == -1)
            {
                Dist = Vector2.Distance(transform.position, Trees[i].position);
                Nr = i;
            }
        }

        if (Nr != -1)
        {
            Target = Trees[Nr];
        }
        else
        {
            Target = null;
        }
    }
}
