using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public delegate void EventTreeMarkedByPlayer(Vector2 treePos, int index);

    public static event EventTreeMarkedByPlayer OnTreeMarkedByPlayer;

    private static TreeManager m_instance;

    public Transform[] Trees;
    private TreeController[] TreeControllers;

    public static TreeManager GetInstance()
    {
        Debug.Assert(m_instance != null);
        return m_instance;
    }

    void Awake()
    {
        if (m_instance == null) {
            m_instance = this;
        } else if (m_instance != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GameObject[] TreesGO = GameObject.FindGameObjectsWithTag("Tree");
        Trees = new Transform[TreesGO.Length];
        TreeControllers = new TreeController[TreesGO.Length];
        for (int i = 0; i < TreesGO.Length; i++)
        {
            Trees[i] = TreesGO[i].GetComponent<Transform>();
            TreeControllers[i] = TreesGO[i].GetComponent<TreeController>();
            TreeControllers[i].Index = i;
        }
    }

    void Update()
    {
        if (!IsTreeUncotrolledByEnemy())
        {
            Debug.Log("ALL TREES ARE CONTROLLED BY ENEMIES - GAME OVER!");
        }
    }

    void EmitTreeMarkedByPlayer(Vector2 treePos, int treeIndex)
    {
        if (OnTreeMarkedByPlayer != null) {
            OnTreeMarkedByPlayer(treePos, treeIndex);
        }
    }

    public void MarkTreeByEnemy(int index)
    {
        TreeControllers[index].Owner = TreeOwnerType.Enemy;
    }

    public void MarkByPlayer(int index)
    {
        TreeControllers[index].Owner = TreeOwnerType.Player;
        Vector2 treePos = Trees[index].position;
        EmitTreeMarkedByPlayer(treePos, index);
    }

    public void LockTree(int index)
    {
        TreeControllers[index].IsLocked = true;
    }

    public void UnlockTree(int index)
    {
        TreeControllers[index].IsLocked = false;
    }

    public bool IsTreeUncotrolledByEnemy()
    {
        for (int i = 0; i < TreeControllers.Length; ++i) {
            if (TreeControllers[i].Owner != TreeOwnerType.Enemy) {
                return true;
            }
        }
        return false;
    }

    public Transform GetRandomTreeTransformUncontrolledByEnemy()
    {
        for (int i = 0; i < 100; ++i) {  // Prevent infinitive loop
            int index = Random.Range(0, Trees.Length);
            TreeController controller = TreeControllers[index];
            if (controller.Owner == TreeOwnerType.Enemy) {
                return Trees[index];
            }
        }

        // Should not happen
        return Trees[Random.Range(0, Trees.Length)];
    }

    public int GetNumberOfEnemyTrees()
    {
        int result = 0;
        for (int i = 0; i < TreeControllers.Length; ++i) {
            if (TreeControllers[i].Owner == TreeOwnerType.Enemy) {
                result++;
            }
        }
        return result;
    }

    public int GetTotalNumberOfTrees()
    {
        return Trees.Length;
    }
}
