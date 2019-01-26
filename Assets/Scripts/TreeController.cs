using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public int Owner = 0;
    public Sprite TreeSprite;

    public Color EnemyControlledColor;
    public Color NeutralColor;
    public Color PlayerControlledColor;

    public void ChangeColor (int Nr)
    {
        Owner = Nr;

        /// Change sprites?
        switch (Nr)
        {
            case -1:
                gameObject.GetComponent<SpriteRenderer>().color = EnemyControlledColor;
                break;
            case 0:
                gameObject.GetComponent<SpriteRenderer>().color = NeutralColor;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().color = PlayerControlledColor;
                break;
        }
    }
}
