using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rigid;
    CircleCollider2D circleCollider;
    private bool isShoot=false; //공을 쏘는 중인지 확인


    private void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update() 
    {
        ControlBounce();
    }

    /// <summary>
    /// 가로로 무한히 튀지 않도록 설정
    /// </summary>
    private void ControlBounce()
    {
        //가로로 무한히 튀지 않도록 설정
        //가로로 무한히 튄다 -> rigid.velocity.y==0 이므로
        //공의 y속도가 0이 되지 않도록 조정 

        if(rigid.velocity!=Vector2.zero && Mathf.Abs(rigid.velocity.y)<1f)
        {
            if(rigid.velocity.y<0f)
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y-Random.Range(0.1f,0.5f));

            else
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y+Random.Range(0.1f,0.5f));
        }

    }

    private void OnTriggerStay2D(Collider2D other) {
        //바닥에 도착하면
        if(other.transform.tag == "Floor" && isShoot)
        {
            //이펙트
            GameObject particleObj = Singleton.BallParticle.GetObject();
            particleObj.SetActive(true);
            particleObj.transform.position=transform.position;

            //사운드
            Singleton.AudioManager.ballSound.Play();

            //ballManager에서 공이 돌아오는 것을 체크한다.
            Singleton.BallManager.CheckBall(this.transform.position); 
            //ball오브젝트를 비활성화
            gameObject.SetActive(false);
            isShoot=false; //바닥에 도착하면 쏘는 중이 아님을 알려줌
        }

    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        //벽돌이나 벽에 공이 닿으면 공 사운드 플레이
        Singleton.AudioManager.ballSound.Play();
    }

    /// <summary>
    /// 공을 발사하는 함수
    /// </summary>
    /// <param name="dir"> 발사하는 방향을 파라미터로 받는다. </param>
    public void Lanch(Vector3 dir)
    {
        if(dir!=null)
        {
            //일정한 힘을 주기위해 방향벡터를
            //normalize를 통해 단위벡터로 만들기
            Vector2 d = new Vector2(dir.x,dir.y).normalized ;

            //즉발적으로 힘을 줌
            rigid.AddForce(d*Singleton.BallManager.ballPower, ForceMode2D.Impulse);

            //공을 쏘는 중
            isShoot=true;
        }
        
    }
    
}
