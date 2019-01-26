using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHandController : MonoBehaviour
{
    public PlayerDogController m_dog;
    private Rigidbody2D m_dogRigidbody;

    void Start()
    {
        if (m_dog == null) {
            m_dog = GameObject.Find("Player_Dog").GetComponent<PlayerDogController>();
        }
        Debug.Assert(m_dog != null);
        m_dogRigidbody = m_dog.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 dogPos = m_dogRigidbody.position;
        Vector2 handPos = transform.position;

        Vector3 angles = transform.eulerAngles;
        angles.z = Vector2.SignedAngle(Vector2.up, handPos - dogPos);
        transform.eulerAngles = angles;
    }
}
