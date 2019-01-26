using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform Target;
    public int MoveSpeed;
    public int StoppingDistance;
    public int FollowDistance;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Vector2.Distance(transform.position, Target.position) >= FollowDistance)
        {
            /// Idle
        }
        else if (Vector2.Distance(transform.position, Target.position) >= StoppingDistance)
        {
            /// Follow
            transform.position = Vector2.MoveTowards(transform.position, Target.position, Time.deltaTime * MoveSpeed);
            Target.transform.LookAt(Target);
        }
        else
        {
            /// Combat starts
        }
    }
}
