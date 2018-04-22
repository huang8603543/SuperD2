using System;
using UnityEngine;
using YunLang.SuperD.Data;

namespace YunLang.SuperD.Model
{
    /// <summary>
    /// 基本窗///
    /// </summary>
    public class FloorPlanWindowElement : FloorPlanElement
    {
        public FloorPlanWindowElement(FloorPlanElementData data) : base(data)
        {
        }

        public override float GetUIHeight()
        {
            throw new NotImplementedException();
        }

        public override float GetUIWidth()
        {
            throw new NotImplementedException();
        }

        public override Vector3 GetUIWorldPosition()
        {
            throw new NotImplementedException();
        }

        public override void RemoveElement()
        {
            throw new NotImplementedException();
        }
    }
}
