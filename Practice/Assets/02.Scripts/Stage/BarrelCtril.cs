using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtril : MonoBehaviour
{
    public GameObject expEffect;
    private int hitCount = 0;
    private Rigidbody rb;

    public Texture[] textures;

    public Mesh[] meshes;
    private MeshFilter meshFilter;

    private MeshRenderer _renderer;

    public float expRadius = 10.0f;

    private AudioSource _audio;
    public AudioClip expSfx;

    // Shake 클래스를 저장할 변수
    public Shake shake;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        meshFilter = GetComponent<MeshFilter>();

        _renderer = GetComponent<MeshRenderer>();

        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];

        _audio = GetComponent<AudioSource>();

        // Shake 스크립트를 추출
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            if(++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2.0f);
        rb.mass = 1.0f;
        rb.AddForce(Vector3.up * 1000.0f);

        IndirectDamage(transform.position);

        int idx = Random.Range(0, meshes.Length);           // 인자타입에 따른 결과값
        meshFilter.sharedMesh = meshes[idx];
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];

        _audio.PlayOneShot(expSfx, 1.0f);

        // 셰이크 효과 호출
        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f));
    }

    void IndirectDamage(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 12);
        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;
            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f);
        }
    }
}
