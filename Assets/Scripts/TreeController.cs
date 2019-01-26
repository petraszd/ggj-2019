using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public int Owner = 0;

    public Color EnemyControlledColor;
    public Color NeutralColor;
    public Color PlayerControlledColor;

    void OnTriggerEnter2D (Collider2D col)
    {
        string Tag = col.tag;

        /// Change sprites?
        switch (Tag)
        {
            case "Enemy":
                //gameObject.GetComponent<SpriteRenderer>().color = EnemyControlledColor;
                Owner = -1;
                break;
            case "Player":
                //gameObject.GetComponent<SpriteRenderer>().color = PlayerControlledColor;
                Owner = 1;
                break;
        }
    }
}
