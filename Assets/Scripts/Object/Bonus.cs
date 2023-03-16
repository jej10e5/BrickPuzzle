using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.transform.tag == "Ball")
        {
            //공 개수 증가
            Singleton.BallManager.IncreaseBallCnt();

            //이펙트
            GameObject particleObj = Singleton.BonusParticle.GetObject();
            particleObj.SetActive(true);
            particleObj.transform.position=transform.position;

            //사운드
            Singleton.AudioManager.bonusSound.Play();

            //보너스 공 오브젝트 반납
            gameObject.SetActive(false);

        }

        if(other.transform.tag == "Floor")
        {
            //바닥에 보너스볼이 닿으면 그냥 없어지게 만들기
            gameObject.SetActive(false);
        }

    }
}
