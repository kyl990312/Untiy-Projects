using System.Collections.Generic;
using UnityEngine;

public class WalkMotion : MonoBehaviour
{
    Animator m_animator;
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool iswalkingPressed = Input.GetKey("space");
        m_animator.SetBool("IsWalking", iswalkingPressed);
    }
}
