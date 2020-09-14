using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runR;
    public AnimationClip runL;
    public AnimationClip runB;
    public AnimationClip runF;

}

public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    private Transform tr;

    public float moveSpeed = 0.0f;
    public float rotSpeed = 0.0f;

    public  PlayerAnim playerAnim;
    public  Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
        anim.clip = playerAnim.idle;
        anim.Play();

        // 불러온 데이터 값을 moveSpeed에 적용
        moveSpeed = GameManager.instance.gameData.speed;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        if (v >= 0.01f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.05f);     // 전진
        }else if(v <= -0.01f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.05f);
        }else if (h >= 0.01f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.05f);
        }else if(h <= -0.01f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.05f);
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.05f);
        }
    }
}
