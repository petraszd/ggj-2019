using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public Image m_percentBackground;
    public TextMeshProUGUI m_percentText;
    public TextMeshProUGUI m_numberOfDogsText;

    [HeaderAttribute("Colors")]
    public Color m_percentBackGoodColor;
    public Color m_percentBackBadColor;

    public Color m_percentTextGoodColor;
    public Color m_percentTextBadColor;

    private float m_goodBadRatio = 0.0f;
    private int m_goodBadDelta = 0;
    private TreeManager m_treeManager;

    void OnEnable()
    {
        TreeManager.OnTreeNumbersChanged += OnTreeNumbersChanged;
        EnemiesSpawner.OnTimeRemaining += OnTimeRemaining;
    }

    void OnDisable()
    {
        TreeManager.OnTreeNumbersChanged -= OnTreeNumbersChanged;
        EnemiesSpawner.OnTimeRemaining -= OnTimeRemaining;
    }

    void Start()
    {
        m_treeManager = TreeManager.GetInstance();
        UpdatePercentColors();
    }

    void Update()
    {
        if (m_goodBadDelta != 0) {
            m_goodBadRatio += 2.0f * Time.deltaTime * m_goodBadDelta;
            if (m_goodBadRatio > 1.0f) {
                m_goodBadRatio = 1.0f;
                m_goodBadDelta = 0;
            } else if (m_goodBadRatio < 0.0f) {
                m_goodBadRatio = 0.0f;
                m_goodBadDelta = 0;
            }
            UpdatePercentColors();
        }
    }

    void UpdatePercentColors()
    {
        m_percentBackground.color = Color.Lerp(m_percentBackBadColor, m_percentBackGoodColor, m_goodBadRatio);
        m_percentText.color = Color.Lerp(m_percentTextBadColor, m_percentTextGoodColor, m_goodBadRatio);
    }

    void OnTreeNumbersChanged(int nTotal, int nEnemies, int nPlayer)
    {
        if (m_treeManager == null) {
            m_treeManager = TreeManager.GetInstance();
        }
        float playerPercent = nPlayer / (float)(nTotal) * 100.0f;
        float enemyPercent = nEnemies / (float)(nTotal) * 100.0f;

        if (nPlayer < m_treeManager.NumToWin) {
            m_goodBadDelta = -1;
        } else {
            m_goodBadDelta = 1;
        }

        m_percentText.text = string.Format("{0:F1}%", playerPercent);
    }

    void OnTimeRemaining(float timeRemaining)
    {
        m_numberOfDogsText.text = string.Format("Time {0:F1} s.", timeRemaining);
    }
}
