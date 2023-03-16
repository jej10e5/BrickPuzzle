using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BtnImageChange : MonoBehaviour
{
    public Sprite onceImage;
    public Sprite doubleImage;

    Image thisImage;

    private void Awake() 
    {
        thisImage = GetComponent<Image>();
    }

    public void ChangeImage()
    {
        Singleton.GameManager.SpeedControl();

        //누르면 적용될 것을 이미지로 보여줘야한다.
        //현재 배속 이라면 -> 배속이 아닌 이미지를 보여줘야함
        if(Singleton.GameManager.isSpeedUp)
        {
            //현재 배속 적용이 되었으므로
            //보여지는 이미지는 배속이 아닌 이미지이다.
            thisImage.sprite = onceImage;
        }
        else
            thisImage.sprite = doubleImage;
        

    }
}
