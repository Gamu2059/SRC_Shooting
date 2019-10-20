using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stage1
{
    public class MotherColonyViewController : EventControllableScript
    {
        private const string UNDER_MOTHER_COLONY = "UnderMotherColony";
        private const string MOTHER_COLONY = "MotherColony";

        public override void OnStart()
        {
            base.OnStart();

            var stageHolder = BattleRealStageManager.Instance.GetHolder(BattleRealStageManager.E_HOLDER_TYPE.STAGE_OBJECT);
            var underMotherColony = stageHolder.Find(UNDER_MOTHER_COLONY);
            var motherColony = stageHolder.Find(MOTHER_COLONY);

            underMotherColony.gameObject.SetActive(false);
            motherColony = motherColony.GetChild(0);
            motherColony.gameObject.SetActive(true);
        }
    }
}
