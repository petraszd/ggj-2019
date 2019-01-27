using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public delegate void EventTreeMarkedByDog(Vector2 treePos, int index);
    public delegate void EventTreeNumbersChanged(int nTotal, int nEnemies, int nPlayer);

    public static event EventTreeMarkedByDog OnTreeMarkedByPlayer;
    public static event EventTreeMarkedByDog OnTreeMarkedByEnemy;
    public static event EventTreeNumbersChanged OnTreeNumbersChanged;

    private static TreeManager m_instance;

    public int m_numToWin;
    public Transform[] Trees;
    private TreeController[] TreeControllers;

    public int NumToWin
    {
        get { return m_numToWin; }
    }

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

    void Start()
    {
        EmitTreeNumbersChanged();
    }

    void Update()
    {
        if (!IsTreeUncotrolledByEnemy())
        {
            Debug.Log("ALL TREES ARE CONTROLLED BY ENEMIES - GAME OVER!");
            AudioManager.PlayLose();
        }
    }

    void EmitTreeMarkedByPlayer(Vector2 treePos, int treeIndex)
    {
        if (OnTreeMarkedByPlayer != null) {
            OnTreeMarkedByPlayer(treePos, treeIndex);
        }
    }

    void EmitTreeMarkedByEnemy(Vector2 treePos, int treeIndex)
    {
        if (OnTreeMarkedByEnemy != null) {
            OnTreeMarkedByEnemy(treePos, treeIndex);
        }
    }

    void EmitTreeNumbersChanged()
    {
        if (OnTreeNumbersChanged != null) {
            OnTreeNumbersChanged(
                    GetTotalNumberOfTrees(),
                    GetNumberOfEnemyTrees(),
                    GetNumberOfPlayerTrees());
        }
    }

    public void MarkTreeByEnemy(int index)
    {
        TreeControllers[index].Owner = TreeOwnerType.Enemy;
        Vector2 treePos = Trees[index].position;
        EmitTreeMarkedByEnemy(treePos, index);
        EmitTreeNumbersChanged();
    }

    public void MarkByPlayer(int index)
    {
        TreeControllers[index].Owner = TreeOwnerType.Player;
        Vector2 treePos = Trees[index].position;
        EmitTreeMarkedByPlayer(treePos, index);
        EmitTreeNumbersChanged();
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
        return GetNumberOfTreesByType(TreeOwnerType.Enemy);
    }

    public int GetNumberOfPlayerTrees()
    {
        return GetNumberOfTreesByType(TreeOwnerType.Player);
    }

    private int GetNumberOfTreesByType(TreeOwnerType type)
    {
        int result = 0;
        for (int i = 0; i < TreeControllers.Length; ++i) {
            if (TreeControllers[i].Owner == type) {
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
