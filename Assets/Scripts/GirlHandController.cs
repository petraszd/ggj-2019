using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlHandController : MonoBehaviour
{
    private static float DOG_PULL_MIN_SCALE = 0.3f;
    private static float PULL_SPEED = 3.0f;

    public PlayerDogController m_dog;
    private Rigidbody2D m_dogRigidbody;

    void OnEnable()
    {
        PlayerDogController.OnPlayerDogPulled += OnPlayerDogPulled;
    }

    void OnDisable()
    {
        PlayerDogController.OnPlayerDogPulled -= OnPlayerDogPulled;
    }

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

    void OnPlayerDogPulled()
    {
        StartCoroutine(PullDog());
    }

    IEnumerator PullDog()
    {
        Vector3 maxScale = transform.localScale;
        Vector3 minScale = transform.localScale;
        minScale.y = DOG_PULL_MIN_SCALE;

        float t = 0.0f;
        Vector3 currentScale = maxScale;

        while (true) {
            yield return null;
            t += Time.deltaTime * PULL_SPEED;
            if (t > 1.0f) {
                break;
            }
            float val = Mathf.Sin(t * Mathf.PI);
            currentScale.y = Mathf.SmoothStep(maxScale.y, minScale.y, val);
            transform.localScale = currentScale;
        }

        transform.localScale = maxScale;
    }
}
