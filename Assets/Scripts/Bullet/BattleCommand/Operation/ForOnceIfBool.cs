#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// bool型の値によって条件分岐する、一度しかループしないfor文を表すクラス。（未使用）
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/forOnce/if/bool", fileName = "ForOnceIfBool", order = 0)]
[System.Serializable]
public class ForOnceIfBool : ForOnceBase
{

    /// <summary>
    /// この値によって条件分岐する
    /// </summary>
    [SerializeField]
    private OperationBoolBase[] m_If;

    /// <summary>
    /// trueだった場合の値
    /// </summary>
    [SerializeField]
    private ForOnceBase[] m_True;

    /// <summary>
    /// falseだった場合の値
    /// </summary>
    [SerializeField]
    private ForOnceBase m_False;


    public override void Do()
    {
        for (int i = 0; i < m_If.Length; i++)
        {
            if (m_If[i].GetResultBool())
            {
                if (m_True[i] != null)
                {
                    m_True[i].Do();
                }
                return;
            }
        }

        if (m_False != null)
        {
            m_False.Do();
        }
    }
}




//public override void Setup()
//{
//    //for (int i = 0; i < m_If.Length; i++)
//    //{
//    //    if (m_If[i].GetResultBool())
//    //    {
//    //        if (m_True[i] != null)
//    //        {
//    //            m_True[i].Setup();
//    //        }
//    //        return;
//    //    }
//    //}

//    //if (m_False != null)
//    //{
//    //    m_False.Setup();
//    //}
//}