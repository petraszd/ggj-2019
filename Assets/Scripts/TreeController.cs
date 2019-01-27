using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public TreeOwnerType Owner = TreeOwnerType.Neutral;
    public int Index;
    public bool IsLocked;

    public Sprite PlayerControlledSprite;
    public Sprite EnemyControlledSprite;
    private SpriteRenderer GlowSpriteRenderer;

    private TreeManager m_treeManager;
    public float PissAnimationTime;

    void Start()
    {
        m_treeManager = TreeManager.GetInstance();
        GlowSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();

        // Hack to prevent 2d z axis fighting
        GlowSpriteRenderer.GetComponent<Transform>().parent = null;
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
                    StartCoroutine(ChangeSprite(EnemyControlledSprite));
                }
                break;
            case "Player":
                if (Owner != TreeOwnerType.Player) {
                    Owner = TreeOwnerType.Player;
                    m_treeManager.MarkByPlayer(Index);
                    StartCoroutine(ChangeSprite(PlayerControlledSprite));
                }
                break;
        }
    }

    IEnumerator ChangeSprite (Sprite _Sprite)
    {
        yield return new WaitForSeconds(PissAnimationTime);
        GlowSpriteRenderer.sprite = _Sprite;
    }
}
