using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [Header("[Brick]")]
    [SerializeField] private GameObject brickPrefab; //브릭 프리펩
    [SerializeField] private ObjectPool brickPool; //브릭 풀
    [SerializeField] private Transform brickPos; //브릭 위치 기준
    private int maxBrickCnt=6; //최대 브릭 수
    private int brickCnt=1; //브릭 개수

    [Header("[Bonus]")]
    [SerializeField] private ObjectPool bonusPool; //보너스볼 풀
    private int bonusCnt=1; //보너스 볼 개수

    private List<float> poslist; //브릭or보너스볼 위치를 넣은 리스트
    public int unitScore=20; //브릭 수가 늘어나는 점수 단위

    private void Awake() 
    {
        poslist = new List<float>();
    }

    public void MakeBricks()
    {

        //점수에 따른 브릭 수 조정하기
        BrickCount();
        
        //효과음
        Singleton.AudioManager.stageSound.Play();

        //기존 오브젝트 아래로 이동
        MoveObject();
    
        //위치 리스트 초기화
        InitializePos();

        //보너스 먼저 뽑고 생성
        RandomPick(bonusCnt,bonusPool);
        //브릭 생성
        RandomPick(brickCnt, brickPool);

        //모든 오브젝트 아래로 이동
        MoveObject();

        //브릭을 다 생성했으므로 다시 공을 쏘고 돌아올때까지 fasle
        Singleton.GameManager.isGenerate = false;
    } 

    /// <summary>
    /// 브릭이 들어갈 자리를 초기화하는 함수
    /// </summary>
    private void InitializePos()
    {
        //위치 초기화
        if(poslist.Count!=0)
        //위치를 저장한 리스트에 뭐가 있다면 다 없애기
            poslist.Clear(); 

        //최대로 들어갈 수 있는 브릭의 수만큼 위치 생성하기
        for(int i=0;i<maxBrickCnt;i++)
        {
            //기준 브릭 위치에서 브릭 프리펩의 x축 사이즈를 더해서 위치를 미리 저장한다.
            poslist.Add(brickPos.position.x + i * brickPrefab.transform.localScale.x);
        }
    }

    /// <summary>
    /// 오브젝트 이동시키는 함수
    /// </summary>
    private void MoveObject()
    {
        //기존에 생성한 브릭들 밑으로 내리기
        foreach(var v in brickPool.PoolList)
        {
            StartCoroutine(MoveObjectRoutine(v));
        }
        //기존에 생성한 보너스볼 밑으로 내리기
        foreach(var v in bonusPool.PoolList)
        {
            StartCoroutine(MoveObjectRoutine(v));
        }
    }

    /// <summary>
    /// 기존의 오브젝트 한칸씩 밑으로 내리는 코루틴
    /// </summary>
    /// <param name="pool"></param>
    public IEnumerator MoveObjectRoutine(GameObject v)
    {
        //활성화되있다면
        if(v.activeSelf && v!=null)
        {
            //yield return new WaitForSeconds(0.1f);
            int frame=0;
            Vector3 targetPos = v.transform.position + Vector3.down * brickPrefab.transform.localScale.y;
            //현재 위치에서 밑으로 한칸 내리기
                //v.transform.position += Vector3.down * brickPrefab.transform.localScale.y;

                while(frame<20)
                {
                    v.transform.position = Vector3.Lerp(v.transform.position, targetPos,0.1f);
                    frame++;
                    yield return null;
                }
        } 
    }

    /// <summary>
    /// 랜덤으로 자리 지정하여 prefab오브젝트를 cnt개수 만큼 생성하는 함수
    /// </summary>
    /// <param name="cnt">생성할 개수를 넣는다.</param>
    /// <param name="prefab">생성할 오브젝트의 프리펩을 넣는다.</param>
    private void RandomPick(int cnt, ObjectPool pool)
    {
        for(int i=0;i<cnt;i++)
        {
            //위치 뽑기
            int randomIndex = Random.Range(0,poslist.Count);
            //비활성화 오브젝트 하나 얻어오기
            GameObject objectInstant = pool.GetObject();
            objectInstant.SetActive(true); //오브젝트 활성화
            
            //활성화한 오브젝트의 위치를 뽑은 위치로 설정한다.
            objectInstant.transform.position = new Vector3(poslist[randomIndex], brickPos.position.y, 0);

            poslist.RemoveAt(randomIndex); //뽑은 위치 없애기 -> 중복 방지를 위함
        }
        
    }

    /// <summary>
    /// 점수에 따라 브릭 수를 지정하는 함수
    /// </summary>
    private void BrickCount()
    {
        //unitScore 점수 마다 브릭 수가 증가한다.
        switch (Singleton.GameManager.Score / unitScore) {
            case 0:
                brickCnt=1;
                break;
            case 1:
                brickCnt=2;
                break;
            case 2:
                brickCnt=3;
                break;
            default :
                brickCnt=4;
                break;
        }
    }


    

}
