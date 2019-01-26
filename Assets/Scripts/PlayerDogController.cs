using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDogController : MonoBehaviour
{
    public GirlController m_girl = null;

    void OnEnable()
    {
        GirlController.OnGirlInstance += OnGirlInstance;
    }

    void OnDisable()
    {
        GirlController.OnGirlInstance -= OnGirlInstance;
    }

    void Start()
    {
        Debug.Assert(m_girl != null);
    }

    void FixedUpdate()
    {
    }

    void OnGirlInstance(GirlController girl)
    {
        m_girl = girl;
    }
}
