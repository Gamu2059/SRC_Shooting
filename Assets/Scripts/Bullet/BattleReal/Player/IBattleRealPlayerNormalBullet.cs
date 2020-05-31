using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleRealPlayerNormalBullet
{
    float GetNowDownDamage();
    void SetNowDownDamage(float value, E_RELATIVE relative = E_RELATIVE.ABSOLUTE);
}
