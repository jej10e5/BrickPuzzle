using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private int _score; //점수
    public int Score => _score; //점수 접근을 위한 프로퍼티 read only

    private Vector3 ballDirec; //공 발사 방향
    private bool isEscape; //터치 패드에서 나갔는지 확인하는 용
    private bool isGuide; //가이드 활성화 여부

    [Header("[Game State]")]
    public bool isReady; //공을 쏠 준비
    public bool isGenerate; //브릭을 만들 준비
    public bool isOver; //게임 오버 여부
    public bool isSpeedUp; //배속 여부

    [Header("[Score UI]")]
    [SerializeField]private Text bestScoreText;
    [SerializeField]private Text scoreText;

    [Header("[Panel UI]")]
    [SerializeField]private GameObject gameOverPanel;
    [SerializeField]private Text resultText;
    [SerializeField]private GameObject optionPanel;


    [Header("Guide System")]
    [SerializeField] private GuideSystem guide;

    [Header("Touch Pad")]
    [SerializeField] private GameObject touchPad;

    [Header("AdMob")]
    [SerializeField] private GameObject ad;
    
    private void Awake() 
    {
        //프레임고정
        Application.targetFrameRate=60; 

        //BestScore->최고점수 저장
        if(!PlayerPrefs.HasKey("BestScore"))
        {
            //BestScore가 없다면 생성한다.
            PlayerPrefs.SetInt("BestSocre",1);
        }

        //광고 보상 저장
        //이전 점수를 저장해놨다가 그 점수부터 시작 가능
        if(!PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetInt("Score",0);
        }
        _score = PlayerPrefs.GetInt("Score");

        //게임을 처음 시작했을 때 isReady를 true로 만들어서
        //공을 쏠 준비를 한다.
        isReady=true;

        //브릭을 만들 준비를 한다.
        isGenerate=true;
    }


    private void Update() 
    {

        if(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.A)) //뒤로가기 혹은 esc누르면
        {
            PlayerPrefs.SetInt("Score",0);
            Application.Quit(); // 종료

        }

        guide.GuideRender(isGuide,ballDirec); //가이드라인 그리기
        GenerateBricks(); //브릭 생성
        ScoreUpdate(); //ui 점수 수정
        GameOver(); //게임 오버 여부

         
    }

    /// <summary>
    /// 브릭을 생성하는 함수
    /// </summary>
    private void GenerateBricks()
    {
        //브릭을 생성해도 되는지
            //isGenerate는 공이 전부 돌아오면 true로 바뀐다
            // BallManger-CheckBall에서 확인 가능
        if(isGenerate) 
        {
            //점수 증가
            _score++;
            //브릭 생성
            Singleton.BrickManager.MakeBricks();
        }
    }

    /// <summary>
    /// ui 점수 업데이트
    /// 기존에 저장된 최고점수보다
    /// 현재 점수가 높다면 수정하고
    /// ui에도 수정한 내용을 반영한다.
    /// </summary>
    private void ScoreUpdate()
    {
        if(_score>PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore",_score);
            
        }
        scoreText.text = "Score : "+_score;
        bestScoreText.text ="Best Score : "+ PlayerPrefs.GetInt("BestScore");
    }


    /// <summary>
    /// 게임 오버시 실행되는 부분
    /// 게임오버패널 활성화, 사운드 활성화
    /// </summary>
    private void GameOver()
    {
        if(isOver)
        {
            touchPad.SetActive(false);
            //효과음
            Singleton.AudioManager.gameOverSound.Play();

            //게임오버 panel을 활성화한다.
            gameOverPanel.SetActive(true);
            //현재 점수를 알려준다.
            resultText.text = _score.ToString();

            isOver = false;
        }
    }

    

    /// <summary>
    /// 게임을 재실행하도록 Load한다.
    /// </summary>
    public void Restart()
    {
        Time.timeScale=1f; //배속 초기화
        SceneManager.LoadScene(0);
    }


    /// <summary>
    /// 다시하기 버튼
    /// </summary>
    public void OnRetry()
    {
        PlayerPrefs.SetInt("Score",0);
        Restart();
    }

    /// <summary>
    /// 보상획득 버튼
    /// </summary>
    public void OnReward()
    {
        Singleton.AudioManager.bgm.Pause();
        ad.GetComponent<RewardAdmob>().UserChoseToWatchAd();
    }

    /// <summary>
    /// 현재 최종 점수를 저장한다.
    /// </summary>
    public void RewardScore()
    {
        PlayerPrefs.SetInt("Score",_score);
    }

    

    /// <summary>
    /// 터치 패드 클릭 시
    /// </summary>
    public void TouchBegan()
    {
        if(isReady)
        {
            isGuide=true;
            isEscape=false;
            ballDirec = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0)) - Singleton.BallManager.BallPos;
        }
        
    }

    /// <summary>
    /// 터치 패드에서 드래그
    /// </summary>
    public void TouchDrag()
    {
        ballDirec = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,0)) - Singleton.BallManager.BallPos;

    }

    /// <summary>
    /// 눌렀다 떼면 발사
    /// 터치 패드 이외에서 떼면 발사 안함
    /// </summary>
    public void TouchEnded()
    {
        //터치 패드에서 나갔는가?
        //쏠 준비가 됐는가?
        if(!isEscape && isReady)
        {
            isGuide=false;
            isReady=false;
            StartCoroutine(Singleton.BallManager.LanchBall(ballDirec)); //발사
        }  
    }

    /// <summary>
    /// 터치 패드에서 나갔는지 확인용
    /// 바닥보다 아래로 쏘는 경우는 있을 수 없음
    /// 그래서 막아놓음
    /// </summary>
    public void TouchOut()
    {
        isGuide=false;
        isEscape=true;
    }

    

    /// <summary>
    /// 옵션 버튼을 누르면 OptionPanel을 활성화하여
    /// 사운드를 조절하는 Panel을 열어준다.
    /// </summary>
    public void OnOption()
    {
        optionPanel.SetActive(true);
        touchPad.SetActive(false);
    }

    /// <summary>
    /// OptionPanel을 비활성화한다.
    /// </summary>
    public void ExitOption()
    {
        optionPanel.SetActive(false);
        touchPad.SetActive(true);
    }


    /// <summary>
    /// 플레이 속도를 조정한다.
    /// </summary>
    public void SpeedControl()
    {
        isSpeedUp = !isSpeedUp;

        if(isSpeedUp)
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1f;

        }

    }

    public void EditScore(int score)
    {
        _score += score;
    }
    
    public void EditGameOver()
    {
        isOver=true;
    }

    
}
