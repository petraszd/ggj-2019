using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TreeAmountText;
    public GameObject TreeManagerGO;

    void Start()
    {
        TreeManagerGO = GameObject.FindGameObjectWithTag("Tree Manager");
    }
    
    void Update()
    {
        TreeAmountText.text = TreeManagerGO.GetComponent<TreeManager>().TreesForEnemies.Count.ToString() + "/" + TreeManagerGO.GetComponent<TreeManager>().Trees.Length.ToString();
    }
}
