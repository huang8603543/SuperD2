using UnityEngine;

namespace YunLang.SuperD.Data
{
    /// <summary>
    /// 平面结构物件实例数据///
    /// </summary>
    public class FloorPlanElementData
    {
        /// <summary>
        /// FloorPlanElementTypeData.id///
        /// </summary>
        public long type;

        /// <summary>
        /// 实例ID///
        /// </summary>
        public long instanceID;

        /// <summary>
        /// 深度（嵌入墙中）///
        /// </summary>
        public float depth;

        /// <summary>
        /// 宽度///
        /// </summary>
        public float width;

        /// <summary>
        /// 高度///
        /// </summary>
        public float height;

        /// <summary>
        /// 方向///
        /// </summary>
        public int direction;

        public string ToJsonString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
