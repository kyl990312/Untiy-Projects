using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE = 0,
        SHOTGUN
    };

    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bullet;
    public Transform firePos;

    public ParticleSystem cartridge;
    private ParticleSystem muzzleFlash;

    private AudioSource _audio;
    public PlayerSfx playerSfx;

    // Shake 클래스를 저장할 변수
    private Shake shake;

    // 탄창 이미지 Image UI
    public Image magazineImg;
    // 남은 총알 수 Text UI
    public Text magazineText;

    // 최대 총알 수
    public int maxBullet = 10;
    // 남은 총알 수
    public int remainingBullet = 10;

    // 재장전 시간
    public float reloadTime = 2.0f;
    // 재장전 여부를 판단할 변수
    private bool isReloading = false;
    // 장애물의 레이어 값을 저장할 변수
    private int obstacleLayer;
    // 레이어 마스크의 비트 연산을 위한 변수
    private int layerMask;

    // 변경할 무기 이미지
    public Sprite[] weaponIcons;
    // 교체할 무기 이미지 UI
    public Image weaponImage;
    // 적 캐릭터의 레이어 값을 저장할 변수
    private int enemyLayer;

    // 자동 발사 여부를 판단할 변수
    private bool isFire = false;
    //다음 발사 시간을 저장할 변수
    private float nextFire;
    // 총알의 발사 간격
    public float fireRate = 0.1f;
    

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
        // Shake 스크립트를 추출
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();

        // 적 캐릭터의 레이어 값을 추출
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        // 장애물의 레이어 값을 추출
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        // 레이어 마스크의 비트 연산(OR 연산)
        layerMask = 1 << obstacleLayer | 1 << enemyLayer;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20.0f, Color.green);

        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 레이캐스트에 검출된 객체의 정보를 저장할 변수
        RaycastHit hit;

        // 레이캐스트를 생성해 저 캐릭터를 검출
        // 광선의 발사 원점 좌표, 광선의 발사 방향 벡터, 검출된 객체의 정보를 반환받을 변수, 광선의 도달 거리, 검출할 레이어
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 20.0f, layerMask))
        {
            isFire = (hit.collider.CompareTag("ENEMY"));
        }
        else
        {
            isFire = false;
        }

        // 레이캐스트에 적 캐릭터가 닿았을 때 자동 발사
        if(isReloading && isFire)
        {
            if (Time.time > nextFire)
            {
                // 총알 수를 하나 감소
                --remainingBullet;
                Fire();

                // 남은 총알이 없을 경우 재장전 코루틴 호출
                if(remainingBullet == 0)
                {
                    StartCoroutine(Reloading());
                }
                // 다음 총알 발사 시간을 계산
                nextFire = Time.time + fireRate;
            }
        }

        // 마우스 왼쪽 버튼을 클릭했을 때  Fire 험수 호출
        if (!isReloading && Input.GetMouseButtonDown(0))        // 0: left  1: right  2: middle
        {
            // 총알 수를 하나 감소
            --remainingBullet;
            Fire();

            // 남은 총알이 없을 경우 재장선 코루틴 호출
            if(remainingBullet == 0)
            {
                StartCoroutine(Reloading());
            }
        }
    }

    void Fire()
    {
        // 셰이크 효과 호출
        StartCoroutine(shake.ShakeCamera());
        // 프리팹 동적 생성
        //Instantiate(bullet, firePos.position, firePos.rotation);
        var _bullet = GameManager.instance.GetBullet();
        if(_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }

        cartridge.Play();
        muzzleFlash.Play();

        FireSfx();

        // 재장전 이미지의 fillAmount 속성값 지정
        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
        // 남은 총알 수 갱신
        UpdateBulletText();
    }

    void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon];
        _audio.PlayOneShot(_sfx, 1.0f);
    }

    IEnumerator Reloading()
    {
        isReloading = true;
        _audio.PlayOneShot(playerSfx.reload[(int)currWeapon], 1.0f);

        // 재장전 오디오의 길이 + 0.3초 동안 대기
        yield return new WaitForSeconds(playerSfx.reload[(int)currWeapon].length + 0.3f);

        // 각종 변수값의 초기화
        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        // 남은 총알 수 갱신
        UpdateBulletText();
    }

    void UpdateBulletText()
    {
        // (남은 총알 수 / 최대 총알 수) 표시
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}", remainingBullet, maxBullet);
    }

    public void OnChangeWeapon()
    {
        currWeapon = (WeaponType)((int)++currWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currWeapon];
    }
}
