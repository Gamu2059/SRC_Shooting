using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ある1つのボスのクラス。
[System.Serializable]
public class Smasher1Boss1 : System.Object
{

    [SerializeField, Tooltip("弾幕の種類")]
    private int m_AttackNum;

    [SerializeField, Tooltip("第何形態目か")]
    private int m_AttackFormNum;

    [SerializeField, Tooltip("その弾幕の開始からの経過時間")]
    protected float m_Time;

    [SerializeField, Tooltip("それぞれの形態の開始からの経過時間")]
    protected float[] m_Times;

    [SerializeField, Tooltip("攻撃を変える")]
    private bool m_AttackChange;

    [SerializeField, Tooltip("形態変化する")]
    private bool m_AttackIncrease;


    [SerializeField, Tooltip("単位弾幕のインデックスの配列")]
    private int[] m_FormChangeArray;


    [SerializeField, Tooltip("単位弾幕のインデックスの配列")]
    private int[][] m_FormChangeArrayArray;


    [SerializeField, Tooltip("弾幕の抽象クラスの配列の配列")]
    private DanmakuCountAbstract[][] m_DanmakuCountAbstractArrayArray;

    [SerializeField, Tooltip("単位弾幕のパラメータの配列の配列")]
    private UDParamsArrayArray m_UDParamsArrayArray;


    public void Awakes()
    {
        m_FormChangeArrayArray = new int[m_FormChangeArray.Length][];

        for (int i = 0; i < m_FormChangeArrayArray.Length; i++)
        {
            m_FormChangeArrayArray[i] = new int[m_FormChangeArray[i]];
        }

        int count = 0;

        for (int i = 0; i < m_FormChangeArrayArray.Length; i++)
        {
            for (int j = 0; j < m_FormChangeArrayArray[i].Length; j++)
            {
                m_FormChangeArrayArray[i][j] = count;
                count++;
            }
        }

        m_DanmakuCountAbstractArrayArray = new DanmakuCountAbstract[m_UDParamsArrayArray.m_UDParametersArrayArray.Length][];

        for (int i = 0;i < m_UDParamsArrayArray.m_UDParametersArrayArray.Length;i++)
        {
            m_DanmakuCountAbstractArrayArray[i] = new DanmakuCountAbstract[m_UDParamsArrayArray.m_UDParametersArrayArray[i].m_UDParametersArray.Length];

            for (int j = 0;j < m_UDParamsArrayArray.m_UDParametersArrayArray[i].m_UDParametersArray.Length;j++)
            {
                m_DanmakuCountAbstractArrayArray[i][j] = EUDToUDObject(m_UDParamsArrayArray.m_UDParametersArrayArray[i].m_UDParametersArray[j].m_EUD);

                m_DanmakuCountAbstractArrayArray[i][j].Awakes(m_UDParamsArrayArray.m_UDParametersArrayArray[i].m_UDParametersArray[j]);
            }
        }
    }


    // 単位弾幕の種類の列挙型を、単位弾幕のオブジェクトそのものに変換する
    public DanmakuCountAbstract EUDToUDObject(E_U_D eUD)
    {
        switch (eUD)
        {
            case E_U_D.ASW:
                return new UDAsw();

            case E_U_D.JIK:
                return new UDJik();

            case E_U_D.KIN:
                return new UDKin();

            case E_U_D.LIS:
                return new UDLis();

            case E_U_D.OMN:
                return new UDOmn();

            case E_U_D.SIN:
                return new UDSin();

            case E_U_D.CRS:
                return new UDOmn2();

            case E_U_D.WAY:
                return new UDWay();

            // 追加しやすいようにしている（この部分は変えない）
            default:
                return new UDAsw();
        }
    }


    public void Updates(EnemyController enemyController)
    {
        if (m_AttackChange)
        {
            m_AttackChange = false;

            m_Time = 0;
            m_AttackFormNum = 0;
            m_AttackNum++;
        }

        if (m_AttackIncrease)
        {
            m_AttackIncrease = false;

            m_AttackFormNum++;
        }

        // もし現在撃っている弾幕が形態変化するなら
        if (m_UDParamsArrayArray.m_UDParametersArrayArray[m_AttackNum].m_IsFormChangable)
        {
            for (int i = 0; i <= m_AttackFormNum; i++)
            {
                m_Times[i] += Time.deltaTime;
            }

            for (int i = 0; i <= m_AttackFormNum; i++)
            {
                // 2次元配列内のi番目の配列
                int[] indexArray = m_FormChangeArrayArray[i];

                for (int j = 0; j < indexArray.Length; j++)
                {
                    // 2次元配列内のi番目の配列内のj番目の整数
                    int index = indexArray[j];

                    m_DanmakuCountAbstractArrayArray[m_AttackNum][index].Updates(enemyController, m_Times[i]);
                }
            }
        }
        else
        {
            //時間を進める
            m_Time += Time.deltaTime;

            foreach (DanmakuCountAbstract danmakuCountAbstract in m_DanmakuCountAbstractArrayArray[m_AttackNum])
            {
                danmakuCountAbstract.Updates(enemyController, m_Time);
            }
        }
    }
}



//[SerializeField, Tooltip("文字列出力の実験")]
//private string m_ExperimentString;



//[SerializeField, Tooltip("直線軌道の交差弾の弾幕")]
//private UDAsw[] uDAsw;
//[SerializeField, Tooltip("直線軌道の交差弾の弾幕")]
//private UDJik[] uDJik;
//[SerializeField, Tooltip("直線軌道の交差弾の弾幕")]
//private UDKin[] uDKin;
//[SerializeField, Tooltip("直線軌道の交差弾の弾幕")]
//private UDLis[] uDLis;
//[SerializeField, Tooltip("直線軌道の交差弾の弾幕")]
//private UDOmn[] uDOmn;
//[SerializeField, Tooltip("直線軌道の交差弾の弾幕")]
//private UDSin[] uDSin;
//[SerializeField, Tooltip("直線軌道の交差弾の弾幕")]
//private UDCrs[] uDCrs;

//[SerializeField, Tooltip("金閣寺と全方位弾の弾幕")]
//private DKinOmn dKinOmn;

//[SerializeField, Tooltip("リサージュと自機狙いの弾幕")]
//private DLisJik dLisJik;

//[SerializeField, Tooltip("弾源渦巻きと全方位弾の弾幕")]
//private DAswAsw dAswAsw;

//[SerializeField, Tooltip("サインカーブと全方位弾の弾幕")]
//private DSinOmn dSinOmn;

//[SerializeField, Tooltip("サインカーブ2つと全方位弾の弾幕")]
//private DSinSinOmn dSinSinOmn;

//[SerializeField, Tooltip("直線軌道の交差弾2つの弾幕")]
//private DCrsCrs dCrsCrs;


//[SerializeField, Tooltip("")]
//private E_U_D[] eUD;

//[SerializeField, Tooltip("")]
//private int[] index;

//[SerializeField, Tooltip("単位弾幕のインスタンスへの参照の配列の配列")]
//private UDReferences[] uDReferenceses;


//m_DanmakuCountAbstractArrayArray[0][0] = new UDCrs();
//m_DanmakuCountAbstractArrayArray[0][1] = new UDJik();

//m_DanmakuCountAbstractArrayArray[0][0].Awakes(m_UDParamsArrayArray[0].m_UDParametersArray[0]);
//m_DanmakuCountAbstractArrayArray[0][1].Awakes(m_UDParamsArrayArray[0].m_UDParametersArray[1]);


//// それぞれの単位弾幕のインスタンスの数のDictionary
//Dictionary<E_U_D, int> numUDs = new Dictionary<E_U_D, int>();

//        foreach (E_U_D eUD in System.Enum.GetValues(typeof(E_U_D)))
//        {
//            numUDs.Add(eUD, 0);
//        }


//public void A(EnemyController enemyController, float time, UDReference uDReference)
//{
//    switch (uDReference.m_EUD)
//    {
//        case E_U_D.ASW:
//            uDAsw[uDReference.m_Index].Updates(enemyController, time);
//            break;

//        case E_U_D.JIK:
//            uDJik[uDReference.m_Index].Updates(enemyController, time);
//            break;

//        case E_U_D.KIN:
//            uDKin[uDReference.m_Index].Updates(enemyController, time);
//            break;

//        case E_U_D.LIS:
//            uDLis[uDReference.m_Index].Updates(enemyController, time);
//            break;

//        case E_U_D.OMN:
//            uDOmn[uDReference.m_Index].Updates(enemyController, time);
//            break;

//        case E_U_D.SIN:
//            uDSin[uDReference.m_Index].Updates(enemyController, time);
//            break;

//        case E_U_D.CRS:
//            uDCrs[uDReference.m_Index].Updates(enemyController, time);
//            break;
//    }
//}


//switch (m_AttackNum)
//{
//    case 0:
//        dKinOmn.Updates(enemyController, m_Time);
//        break;

//    case 1:
//        dLisJik.Updates(enemyController, m_Time);
//        break;

//    case 2:
//        dAswAsw.Updates(enemyController, m_Time);
//        break;

//    case 3:
//        dSinOmn.Updates(enemyController, m_Time);
//        break;

//    case 4:
//        dSinSinOmn.Updates(enemyController, m_Time);
//        break;

//    case 5:
//        //dCrsCrs.Updates(enemyController, m_Time);

//        for (int i = 0; i < uDReferenceses[0].m_UDReference.Length; i++)
//        {
//            A(enemyController, m_Time, uDReferenceses[0].m_UDReference[i]);
//        }

//        break;
//}


//for (int i = 0; i < uDReferenceses[m_AttackNum].m_UDReference.Length; i++)
//{
//    A(enemyController, m_Time, uDReferenceses[m_AttackNum].m_UDReference[i]);
//}


//[SerializeField, Tooltip("単位弾幕のパラメータの配列の配列")]
//private UDParamsArray[] m_UDParamsArrayArray;


// これだと、すべての弾幕を一気に出している。attackNumで分ける。
//foreach (DanmakuCountAbstract[] danmakuCountAbstractArray in m_DanmakuCountAbstractArrayArray)
//{
//}


//for (int i = 0;i<m_DanmakuCountAbstractArrayArray[m_AttackNum].Length;i++)
//        {
//            //この情報を、次の、インデックスが同じ単位弾幕に与える。いや、こうせずにプログラム構造を変えた方が良さそう。
//            //m_Time / m_DanmakuCountAbstractArrayArray[m_AttackNum][i].m_Float[0];
//        }


//int[] indexArray = m_UDParamsArrayArray.m_UDParametersArrayArray[m_AttackNum].m_FormChange[i];