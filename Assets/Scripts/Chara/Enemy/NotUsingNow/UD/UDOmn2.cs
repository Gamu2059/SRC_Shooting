#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDOmn2 : UDOmn1
{

    [SerializeField, Tooltip("発射位置")]
    private Vector3 m_LaunchPosition;

    [SerializeField, Tooltip("発射地点の円の半径")]
    private float circleRadius;


    // 発射位置を計算する
    public override Vector3 CalcLaunchPosition(BattleRealEnemyController enemyController, float time)
    {
        Vector2 randomPos = Random.insideUnitCircle * circleRadius;

        return enemyController.transform.position + new Vector3(randomPos.x, 0, randomPos.y);
    }
}
