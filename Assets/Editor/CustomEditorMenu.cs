using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEditorMenu : MonoBehaviour
{
    /// <summary>
    /// 공을 10개 추가한다.
    /// </summary>
    [MenuItem("MyMenu/Ball/+10")]
    static void IncreaseBallMenu_10()
    {
        Singleton.BallManager.EditBall(10);
    }

    /// <summary>
    /// 공을 100개 추가한다.
    /// </summary>
    [MenuItem("MyMenu/Ball/+100")]
    static void IncreaseBallMenu_100()
    {
        Singleton.BallManager.EditBall(100);
    }

    /// <summary>
    /// 점수를 10점 추가한다.
    /// </summary>
    [MenuItem("MyMenu/Score/+10")]
    static void IncreaseScoreMenu_10()
    {
        Singleton.GameManager.EditScore(10);
    }

    /// <summary>
    /// 점수를 100점 추가한다.
    /// </summary>
    [MenuItem("MyMenu/Score/+100")]
    static void IncreaseScoreMenu_100()
    {
        Singleton.GameManager.EditScore(100);
    }

    /// <summary>
    /// 점수와 공 개수 모두 10 추가한다.
    /// </summary>
    [MenuItem("MyMenu/Stage/+10")]
    static void IncreaseStageMenu_10()
    {
        Singleton.BallManager.EditBall(10);
        Singleton.GameManager.EditScore(10);

    }

    /// <summary>
    /// 브릭을 생성한다.
    /// </summary>
    [MenuItem("MyMenu/BrickGenerate")]
    static void BrickGenerateMenu()
    {
        Singleton.BrickManager.MakeBricks();

    }

    /// <summary>
    /// 게임 오버시킨다.
    /// </summary>
    [MenuItem("MyMenu/Game Over")]
    static void MenuGameOver()
    {
        Singleton.GameManager.EditGameOver();

    }
}
