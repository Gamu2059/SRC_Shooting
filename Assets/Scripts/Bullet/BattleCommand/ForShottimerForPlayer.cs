#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機のための、発射タイミングを表すfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/shottimerForPlayer", fileName = "ForShottimerForPlayer", order = 0)]
[System.Serializable]
public class ForShottimerForPlayer : ForShottimer
{

    [SerializeField, Tooltip("ショットボタンが押されているかどうか")]
    private OperationBoolBase m_IsShotButtonPressed;

    /// <summary>
    /// ショットボタンが押されているかどうか（値の一時保存用）
    /// </summary>
    private bool m_IsShot;


    public override void Setup()
    {
        base.Setup();

        m_NowTime = 0;
    }


    public override void Init()
    {
        // ショットボタンが押されていれば、このオブジェクト内の時刻を進める。
        if (m_IsShotButtonPressed.GetResultBool())
        {
            m_IsShot = true;
            m_NowTime += Time.deltaTime;
        }
        // ショットボタンが押されていなければ、その記録だけしてfor文を終わる
        else
        {
            m_IsShot = false;
        }
    }


    public override bool IsTrue()
    {
        // ショットボタンが押されていたら、発射するかどうかの処理を行う
        if (m_IsShot)
        {
            return base.IsTrue();
        }
        // ショットボタンが押されていなければ、何もせずにfor文を終わる
        else
        {
            return false;
        }
    }
}