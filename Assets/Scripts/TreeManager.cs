using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
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
    }
}
