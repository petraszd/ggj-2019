using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeashController : MonoBehaviour
{
    public Transform m_handTransform;
    public Transform m_dogTransform;

    [HeaderAttribute("Leash ends")]
    public Rigidbody2D m_start;
    public Rigidbody2D m_end;

    [HeaderAttribute("Rendering")]
    public LineRenderer m_line;
    public Transform[] m_items;

    private Vector3[] m_positions;
    private Transform m_lineTransform;

    void Awake()
    {
        m_positions = new Vector3[m_items.Length];
        UpdateLinePositions();

        m_lineTransform = m_line.GetComponent<Transform>();
    }

    void Start()
    {
        if (m_handTransform == null) {
            m_handTransform = GameObject.Find("Girl_Leash_Position").GetComponent<Transform>();
        }
        Debug.Assert(m_handTransform != null);

        if (m_dogTransform == null) {
            m_dogTransform = GameObject.Find("Player_Dog_Leash_Position").GetComponent<Transform>();
        }
        Debug.Assert(m_dogTransform != null);
    }

    void Update()
    {
        // For sorting layers
        m_lineTransform.position = m_handTransform.position;
        UpdateLinePositions();
    }

    void FixedUpdate()
    {
        m_start.MovePosition(m_handTransform.position);
        m_end.MovePosition(m_dogTransform.position);
    }

    void UpdateLinePositions()
    {
        for (int i = 0; i < m_items.Length; ++i) {
            Vector3 pos = m_items[i].position;
            m_positions[i].Set(pos.x, pos.y, pos.z);
        }
        m_line.SetPositions(m_positions);
    }
}
