using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// int型をカウンター変数とする多重forループを表すクラス。（forループをメタ的に見ている）bool型のフィールドをつけて、Init()をなくすか？
/// </summary>
[System.Serializable]
public class IntMultiLoop : object
{
    [SerializeField]
    private OperationIntProcCondLinear[] m_OperationArray;


    /// <summary>
    /// 初期化する。
    /// </summary>
    public bool Init()
    {
        return Process(0, false);
    }


    /// <summary>
    /// 進める（全てのループを抜け終わった場合はfalseを返す。）
    /// </summary>
    public bool Process()
    {
        return Process(m_OperationArray.Length - 1, true);
    }


    public bool Process(int index, bool isLooping)
    {
        // どのforループよりも内側にいるなら
        if (index > m_OperationArray.Length - 1)
        {
            return true;
        }
        // どのforループよりも外側に行ったなら
        else if (index < 0)
        {
            return false;
        }

        // 増分の方から来たなら
        if (isLooping)
        {
            // 増分の処理をする
            m_OperationArray[index].Process();
        }
        // 初期化の方から来たなら
        else
        {
            // 初期化の処理をする
            m_OperationArray[index].Init();
        }

        if (m_OperationArray[index].IsTrue())
        {
            // 条件判定がtrueなら、1つ内側へ行ってそこで初期化をする
            return Process(index + 1, false);
        }
        else
        {
            // 条件判定がfalseなら、このループは終わって、1つ外側へ行ってループする
            return Process(index - 1, true);
        }
    }
}





//if (index < 0)
//{
//    return false;
//}

//m_OperationArray[index].Process();

//if (m_OperationArray[index].IsTrue())
//{
//    m_OperationArray[index + 1].Init();

//    if (m_OperationArray[index + 1].IsTrue())
//    {

//    }
//}
//else
//{
//    return Process(index - 1);
//}


//for (int i = 0;i < m_OperationArray.Length;i++)
//{
//    m_OperationArray[i].Init();

//    if (m_OperationArray[i].IsTrue())
//    {
//        continue;
//    }
//    else
//    {
//        break;
//    }
//}
