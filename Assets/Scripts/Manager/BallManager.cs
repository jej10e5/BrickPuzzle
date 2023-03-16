using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour
{
    [Header("[Ball]")]
    [SerializeField] private ObjectPool ballPool; //공 오브젝트 pool
    public float intervalTime; //공 쏘는 시간 간격
    public float ballPower=10f;

    [Header("[Ball UI]")]
    [SerializeField] private GameObject preBall; //보여지는 ball
    [SerializeField] private Text ballCntText; //공 개수 텍스트
    

    private int ballCnt=1; //현재 ball의 개수

    private Vector3 _ballPos; //현재 공 시작점
    public Vector3 BallPos => _ballPos;
    private Vector3 nextPos;//다음 위치
    
    private int targetBallCnt; //발사 당시 ballCnt 개수, 돌아와야하는 공의 수
    //-> 보너스 공을 먹어서 ballCnt가 바뀔 경우가 있을 수 있어서 분리
    private int returnBallCnt; //돌아온 ball개수

    private void Awake() 
    {
        //최초의 공 위치는 preBall 위치로 한다.
        _ballPos = preBall.transform.position;
        InitBallCnt(PlayerPrefs.GetInt("Score"));
    }

    private void Update() {
        if(ballCntText.gameObject.activeSelf)
        {
            ballCntText.text = "x "+ballCnt;
        }
    }

    /// <summary>
    /// 공 개수 초기화
    /// </summary>
    public void InitBallCnt(int cnt)
    {
        ballCnt += cnt;
    }


    /// <summary>
    /// 공 개수 증가
    /// </summary>
    public void IncreaseBallCnt()
    {
        ballCnt++;
    }


    /// <summary>
    /// 공 발사 전 초기화 내용
    /// </summary>
    public void LanchInit()
    {
        ballCntText.gameObject.SetActive(false);//공 개수 텍스트 비활성화

        preBall.gameObject.SetActive(false);
        returnBallCnt=0; //돌아온 공의 수를 초기화해줌
        targetBallCnt=ballCnt; //돌아와야 하는 공의 수를 현재 공의 개수로 맞춤
    }

    /// <summary>
    /// 공을 발사하는 코루틴
    /// </summary>
    /// <param name="dir">발사 방향</param>
    /// <returns></returns>
    public IEnumerator LanchBall(Vector2 dir)
    {
        
        LanchInit(); //발사 전 초기화 내용

        //공을 발사하는 도중에도 ballCnt는 바뀔수있음
        //발사하는 도중에 보너스 볼을 먹으면 ballCnt가 늘어나므로
        //더 많이 발사하게됨
        //그래서 targetBallCnt에 발사 당시 ballCnt를 넣어놓고
        //그만큼만 발사하도록 설정
        for(int i=0;i<targetBallCnt;i++)
        {
            var v = ballPool.GetObject();
            //바닥에 도착한 공들은 모두 비활성화 되므로 
            //공을 발사할 때 다시 활성화해준다.
            //이때 공의 시작점도 바꿔준다.
            v.transform.position = _ballPos;
            v.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(intervalTime);
            v.GetComponent<Ball>().Lanch(dir); //발사하는 부분
            yield return new WaitForSeconds(intervalTime);

        }
        
    }

    /// <summary>
    /// 공이 돌아 오는 것을 확인하는 함수
    /// Ball에서 Floor과 부딪히면 호출하도록 한다.
    /// </summary>
    /// <param name="pos"></param>
    public void CheckBall(Vector3 pos)
    {
        returnBallCnt++;

        if(returnBallCnt==1) //첫번째로 돌아왔다면
        {
            nextPos=pos; //다음 시작 위치를 저장한다.
            nextPos.y=_ballPos.y; //충돌 후 공이 조금씩 위로 튀길래 y값을 고정시킴

            //첫번째 공이 돌아오면 미리보기 공을 활성화한다.
            //바닥에 도착한 공 오브젝트들을 비활성화하므로
            preBall.SetActive(true);
            //미리보기 공 위치 변경
            preBall.transform.position=nextPos;
        }
        if(returnBallCnt==targetBallCnt) //모두 돌아왔다면
        {
            //공 개수 UI
            ballCntText.gameObject.SetActive(true);
            ballCntText.text = "x "+ballCnt;

            _ballPos=nextPos; //공 위치 바꿔주기
            Singleton.GameManager.isReady=true; //다음 발사 준비를 한다.

            //공이 다 돌아왔다면 다음 블럭 생성
            Singleton.GameManager.isGenerate=true;
            
        }

    }    


    public void EditBall(int ball)
    {
        ballCnt+=ball;
    }
}
