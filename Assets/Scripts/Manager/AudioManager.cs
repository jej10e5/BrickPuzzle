using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    [Header("[Master]")]
    public AudioMixer audioMixer;
    public Slider masterSlider;

    [Header("[BGM]")]
    public AudioSource bgm;
    public Slider bgmSlider;

    [Header("[SFX]")]
    public AudioSource ballSound;
    public AudioSource brickSound;
    public AudioSource bonusSound;
    public AudioSource gameOverSound;
    public AudioSource stageSound;

    public Slider sfxSlider;
    public Slider ballSfxSlider;

    private void Awake() 
    {
        //오디오 설정값 가져오기
        if(!PlayerPrefs.HasKey("Master"))
        {
            //Master 없다면 생성한다.
            PlayerPrefs.SetFloat("Master",0.5f); 
        }

        if(!PlayerPrefs.HasKey("BGM"))
        {
            //Bgm 없다면 생성한다.
            PlayerPrefs.SetFloat("BGM",0.5f); 
        }

        if(!PlayerPrefs.HasKey("SFX"))
        {
            //EffectSound 없다면 생성한다.
            PlayerPrefs.SetFloat("SFX",0.5f);
        }

        if(!PlayerPrefs.HasKey("BallSFX"))
        {
            //BallEffect 없다면 생성한다.
            PlayerPrefs.SetFloat("BallSFX",0.5f);
        }

        //저장해둔 정보를 가져와서 슬라이더에 반영해준다.
        masterSlider.value = PlayerPrefs.GetFloat("Master");
        bgmSlider.value = PlayerPrefs.GetFloat("BGM");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX");
        ballSfxSlider.value = PlayerPrefs.GetFloat("BallSFX");

        
    }

    //audioMixer.SetFloat()을 Awake()에서 실행하면 안되는 오류가 있음. 그래서 Start()에 작성
    private void Start() {
        //slider 는 0~1의 범위를 가지고
        //audioMixer는 -80~0dB 을 범위로 가진다.
        //slider의 값을 audioMixer로 변환하기 위해 slider의 범위를 0.001 ~ 1로 설정하고
        //slider의 값을 log함수를 사용해서 -60~0dB값으로 변환한다.
            // -60dB 정도면 mute랑 거의 비슷
        audioMixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat("Master"))*20);
        audioMixer.SetFloat("BGM", Mathf.Log10(PlayerPrefs.GetFloat("BGM"))*20);
        audioMixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX"))*20);
        audioMixer.SetFloat("BallSFX", Mathf.Log10(PlayerPrefs.GetFloat("BallSFX"))*20);

    }

    

    /// <summary>
    /// Master Volme을 조절한다.
    /// </summary>
    public void SetMasterVolme()
    {
        PlayerPrefs.SetFloat("Master",masterSlider.value);
        audioMixer.SetFloat("Master", Mathf.Log10(masterSlider.value)*20);
    }

    /// <summary>
    /// Bgm Volme을 조절한다.
    /// </summary>
    public void SetBgmVolme()
    {
        PlayerPrefs.SetFloat("BGM",bgmSlider.value);
        audioMixer.SetFloat("BGM", Mathf.Log10(bgmSlider.value)*20);
    }

    /// <summary>
    /// Sfx Volme을 조절한다.
    /// </summary>
    public void SetSfxVolme()
    {
        PlayerPrefs.SetFloat("SFX",sfxSlider.value);
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value)*20);
    }

    /// <summary>
    /// Sfx 중에서도 ball의 Volume을 조절한다.
    /// </summary>
    public void SetBallSfxVolme()
    {
        
        PlayerPrefs.SetFloat("BallSFX",ballSfxSlider.value);
        audioMixer.SetFloat("BallSFX", Mathf.Log10(ballSfxSlider.value)*20);
       
    }

    

}
