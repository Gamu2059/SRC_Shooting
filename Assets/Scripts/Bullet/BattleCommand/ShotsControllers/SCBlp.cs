using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射パラメータを操作するクラス。発射位置を円内でブレさせる。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotParamListControllerBase/Blp", fileName = "SCBlp", order = 0)]
[System.Serializable]
public class SCBlp : ShotsControllerBij
{

    [SerializeField, Tooltip("ブレの半径")]
    public float m_BlurRadius;


    public override void GetshotsParamFor(ShotParam shotParam, ShotTimer shotTimer, HackingBossPhaseState state)
    {
        //shotParam.Position.Value += Random.insideUnitCircle * m_BlurRadius;

        shotParam.Position = new OperationVector2Plus(new OperationVector2Base[] { shotParam.Position, new OperationVector2Init(Random.insideUnitCircle * m_BlurRadius) });
    }
}





//array[i] = new ShotParam(array[i].BulletIndex, new Boxing1<Vector2>(array[i].Position.m_Value + Random.insideUnitCircle * m_BlurRadius), array[i].Angle, array[i].Speed);

//array[i].Position.SetValue(array[i].Position.GetValue() + Random.insideUnitCircle * m_BlurRadius);


//int arraySize = array.Count;

//for (int i = 0; i < arraySize; i++)
//{
//    array[i].Position.Value += Random.insideUnitCircle * m_BlurRadius;
//}
