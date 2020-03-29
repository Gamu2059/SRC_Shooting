using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyParamChanger
{
    /// <summary>
    /// 敵の公開パラメータを変更する。
    /// </summary>
    public static void ChangeEnemyParam(BattleRealEnemyBase enemy, IEnemyParamChange param)
    {
        if (enemy == null)
        {
            Debug.LogWarningFormat("指定された敵はnullです。");
            return;
        }

        if (param == null)
        {
            Debug.LogWarningFormat("指定されたパラメータはnullです。");
            return;
        }

        if (param.UseLookMoveDirChange)
        {
            enemy.IsLookMoveDir = param.ApplyLookMoveDir;
        }

        if (param.UseWillDestroyOnOutOfEnemyFieldChange)
        {
            enemy.WillDestroyOnOutOfEnemyField = param.ApplyWillDestroyOnOutOfEnemyField;
        }

        if (param.UseCriticalColliderEnableChange)
        {
            var critical = enemy.GetCollider().GetColliderTransform(E_COLLIDER_TYPE.CRITICAL);
            enemy.GetCollider().SetEnableCollider(critical.Transform, param.ApplyCriticalColliderEnable);
        }
    }
}
