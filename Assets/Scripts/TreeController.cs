using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public TreeOwnerType Owner = TreeOwnerType.Neutral;
    public int Index;
    public bool IsLocked;

    public Color EnemyControlledColor;
    public Color NeutralColor;
    public Color PlayerControlledColor;

    private TreeManager m_treeManager;

    void Start()
    {
        m_treeManager = TreeManager.GetInstance();
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (IsLocked) {
            return;
        }

        string Tag = col.tag;

        /// Change sprites?
        switch (Tag)
        {
            case "Enemy":
                if (Owner != TreeOwnerType.Enemy) {
                    Owner = TreeOwnerType.Enemy;
                    m_treeManager.MarkTreeByEnemy(Index);
                }
                break;
            case "Player":
                if (Owner != TreeOwnerType.Player) {
                    Owner = TreeOwnerType.Player;
                    m_treeManager.MarkByPlayer(Index);
                }
                break;
        }
    }
}
