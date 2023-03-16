using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuideSystem : MonoBehaviour
{
    [SerializeField] private GameObject ballGuide;
    [SerializeField] private GameObject lineGuide;

    /// <summary>
    /// 가이드를 그려주는 함수
    /// </summary>
    /// <param name="isGuide">가이드 활성화 여부</param>
    /// <param name="dir">공 발사 방향</param>
    public void GuideRender(bool isGuide, Vector3 dir)
    {
        //가이드 활성화 여부 확인
        if(isGuide)
        {
            //가이드 활성화
            ballGuide.SetActive(true);
            lineGuide.SetActive(true);

            //각도 구하기
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //가이드 라인 위치와 각도 변경
            //쿼터니언으로 변환하기
            lineGuide.transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
            lineGuide.transform.position = Singleton.BallManager.BallPos;

            //레이캐스트 circle
            RaycastHit2D hit = Physics2D.CircleCast(Singleton.BallManager.BallPos,0.125f,dir);
            //레이에 맞은게 있고 바닥이 아니라면
            if(hit.collider!=null && hit.collider.tag!="Floor")
            {
                //가이드 공 위치 변경 -> 레이캐스트 부분
                ballGuide.transform.position = new Vector3(hit.centroid.x, hit.centroid.y,0f) ;
            }
        }
        else 
        {
            //가이드 비활성화
            ballGuide.SetActive(false);
            lineGuide.SetActive(false);

        }
    }

}
