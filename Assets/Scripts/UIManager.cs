using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text TreeAmountText;
    public TreeManager TreeManagerCode;

    void Start()
    {
        TreeManagerCode = TreeManager.GetInstance();
    }

    void Update()
    {
        TreeAmountText.text = TreeManagerCode.GetNumberOfEnemyTrees().ToString() + "/" + TreeManagerCode.GetTotalNumberOfTrees().ToString();
    }
}
