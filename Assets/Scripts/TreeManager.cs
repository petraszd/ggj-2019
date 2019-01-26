using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public GameObject[] TreesGO;
    public Transform[] Trees;
    public List<Transform> TreesForEnemies;

    void Start()
    {
        TreesGO = GameObject.FindGameObjectsWithTag("Tree");
        Trees = new Transform[TreesGO.Length];
        for (int i = 0; i < TreesGO.Length; i++)
        {
            Trees[i] = TreesGO[i].GetComponent<Transform>();
        }

        TreesForEnemies = new List<Transform>();
    }

    void Update()
    {
        TreesForEnemies.Clear();
        foreach (Transform t in Trees)
        {
            if (t.GetComponent<TreeController>().Owner != -1)
            {
                TreesForEnemies.Add(t);
            }
        }

        if (TreesForEnemies.Count == 0)
        {
            Debug.Log("ALL TREES ARE CONTROLLED BY ENEMIES - GAME OVER!");
        }
    }
}
