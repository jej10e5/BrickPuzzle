using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 설정한 poolSize 만큼 오브젝트를 생성하여 _poolList에 담는다. 
/// _poolList는 PoolList를 통해 조회 가능하다.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /*
    오브젝트 풀링
    : 메모리 할당, 해제를 반복하면 GC 호출이 많아져서 CPU 성능 저하와
    파편화된 메모리로 인한 메모리 성능 저하로 인해 게임의 전체적인 성능이 저하된다.
    이를 막기 위한 디자인 패턴으로 객체의 할당과 해제를 하지 않고 
    활성화, 비활성화를 이용해 할당과 해제의 역할을 대신한다.
    */
    [SerializeField] private GameObject prefabObject; //생성할 오브젝트 프리펩
    [SerializeField] private Transform poolGroup; //pool을 생성할 곳

    public int poolSize; //pool크기
    private List<GameObject> _poolList; //오브젝트를 저장할 공간

    //_poolList 인스턴스 접근을 위한 프로퍼티 read only
    public List<GameObject> PoolList => _poolList; 

    private void Awake() 
    {
        _poolList = new List<GameObject>();
        Initialize(); //최초 1회 pool size에 맞게 풀 생성
    }

    /// <summary>
    /// poolSize 만큼 풀 생성
    /// </summary>
    private void Initialize() 
    {
        for(int i=0;i<poolSize;i++)
        {
            CreateObject(); 
        }
    }

    /// <summary>
    /// 오브젝트 생성하는 함수
    /// </summary>
    /// <returns>새로 생성한 객체 반환</returns>
    private GameObject CreateObject()
    {
        GameObject newObj = Instantiate(prefabObject,poolGroup);
        _poolList.Add(newObj); //객체 생성하여 리스트에 추가해주기
        newObj.SetActive(false); //생성시 비활성화 상태로 생성해준다.

        return newObj;
    }

    /// <summary>
    /// pool에 비활성화 오브젝트가 없다면 생성하고
    /// 남는 오브젝트가 있다면 그것을 반환한다.
    /// </summary>
    /// <returns>찾은 객체 반환</returns>
    public GameObject GetObject()
    {
        foreach(var v in _poolList) //pool에
        {
            if(!v.activeSelf) //비활성화된 객체가 있다면
            {
                return v; //그 객체를 반환
            }
        }

        //비활성화된 객체가 없다면 새로운 객체를 생성해서 반환
        return CreateObject(); 
    }
}
