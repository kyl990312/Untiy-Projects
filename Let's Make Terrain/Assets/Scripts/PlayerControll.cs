using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    Transform tr;
    Animation anim;
    Rigidbody rigid;

    public float speed = 1.0f;
    public float rotAngle = 10.0f;

    public float jumpForce = 2.0f;
    bool isJumping = false;

    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        tr.Translate(new Vector3(0.0f, 0.0f, v) * speed * Time.deltaTime);
        tr.Rotate(new Vector3(0.0f, h, 0.0f), rotAngle * Time.deltaTime);

        Jump();


    }

    void Jump()
    {
        if (isJumping) {
            rigid.velocity += Vector3.up * jumpForce * Time.deltaTime;
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain") && isJumping)
        {
            isJumping = false;
        }
    }

}
