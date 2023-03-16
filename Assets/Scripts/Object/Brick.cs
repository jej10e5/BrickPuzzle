using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Brick : MonoBehaviour
{

    [SerializeField]private Text hpText;//벽돌 체력 text
    [SerializeField]private ColorPalette brickPalette; //벽돌 색

    private int hp;
    BoxCollider2D boxCollider;
    Rigidbody2D rigid;
    Animator anim;


    private void Awake() 
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() 
    {
        //오브젝트 활성화 시 hp 초기화하고 hp에 맞게 색을 바꿈
        hp = Singleton.GameManager.Score;
        BrickRander();
    }

    private void Update() 
    {
        //벽돌 체력에 맞게 색과 텍스트 렌더
        BrickRander();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.transform.tag=="Ball")
        { 
            hp--;
            if(hp==0)
            {
                //데미지 효과 나타내는 코루틴 멈추기
                StopCoroutine(BrickDamaged());
                //해당 오브젝트 비활성화
                gameObject.SetActive(false);
                //벽돌 파괴 이펙트
                GameObject particleObj = Singleton.BrickParticle.GetObject();
                particleObj.SetActive(true);
                particleObj.transform.position=transform.position;

                //벽돌 파괴 사운드 
                Singleton.AudioManager.brickSound.Play();

            }
            else
            {
                BrickRander();
                //브릭 데미지 효과 코루틴
                StartCoroutine(BrickDamaged());
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.transform.tag == "Floor")
        {
            StopCoroutine(BrickDamaged());
            Singleton.GameManager.isOver=true;

        }
    }

    /// <summary>
    /// 벽돌 체력에 맞게 텍스트와 색 업데이트
    /// </summary>
    public void BrickRander()
    {   
        SpriteRenderer renderer = transform.Find("BrickColor").gameObject.GetComponent<SpriteRenderer>();
        //체력에 따라 색 변화를 준다
        float interval = (float)Singleton.GameManager.Score / brickPalette.brickColor.Length;
        int colorIndex = (int) (hp / interval);
        if(colorIndex<0) 
            colorIndex=0;
        if(colorIndex>brickPalette.brickColor.Length-1) 
            colorIndex = brickPalette.brickColor.Length-1;

        renderer.color = brickPalette.brickColor[colorIndex];

        //체력 갱신
        hpText.text = hp.ToString();

    }

    /// <summary>
    /// 깜박거리는 효과
    /// </summary>
    /// <returns></returns>
    public IEnumerator BrickDamaged()
    {
        SpriteRenderer renderer = transform.Find("BrickColor")
                    .gameObject.GetComponent<SpriteRenderer>();
        Color temp = renderer.color;
        renderer.color=brickPalette.damageColor;
        yield return new WaitForSeconds(0.01f);
        renderer.color=temp;
        yield return new WaitForSeconds(0.01f);

    }


}
