using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager와 ParticlePool에 접근이 가능하다.
/// </summary>
public class Singleton : MonoBehaviour
{
    private static Singleton _instance;
    public static Singleton Instance
    {
        get
        {
            if(_instance==null) //없으면
            {
                //찾음
                _instance = FindObjectOfType<Singleton>();
            }
            return _instance;
        }

    }

    [SerializeField] private GameManager gameManager;
    [SerializeField] private BallManager ballManager;
    [SerializeField] private BrickManager brickManager;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private ObjectPool ballParticle;
    [SerializeField] private ObjectPool brickParticle;
    [SerializeField] private ObjectPool bonusParticle;

    public static GameManager GameManager => Instance.gameManager;
    public static BallManager BallManager => Instance.ballManager;
    public static BrickManager BrickManager => Instance.brickManager;
    public static AudioManager AudioManager => Instance.audioManager;
    
    public static ObjectPool BallParticle => Instance.ballParticle;
    public static ObjectPool BrickParticle => Instance.brickParticle;
    public static ObjectPool BonusParticle => Instance.bonusParticle;


}
