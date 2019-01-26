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
                m_treeManager.MarkTreeByEnemy(Index);
                Owner = TreeOwnerType.Enemy;
                break;
            case "Player":
                m_treeManager.MarkByPlayer(Index);
                Owner = TreeOwnerType.Player;
                break;
        }
    }
}
