using UnityEngine;
using YunLang.SuperD.Util;

namespace YunLang.SuperD.Data
{
    /// <summary>
    /// 3D模型实例数据///
    /// </summary>
    public class ModelData
    {
        /// <summary>
        /// ModelTypeData.id///
        /// </summary>
        public long type;

        /// <summary>
        /// 实例id///
        /// </summary>
        public long instanceId;

        public HSBColor color;

        public string ToJsonString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
