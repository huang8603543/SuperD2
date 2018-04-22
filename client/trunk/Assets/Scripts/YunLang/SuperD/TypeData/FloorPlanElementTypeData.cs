using UnityEngine;
using YunLang.SuperD.Enum;

namespace YunLang.SuperD.TypeData
{
    /// <summary>
    /// 平面结构物件类型数据///
    /// </summary>
    public class FloorPlanElementTypeData
    {
        /// <summary>
        /// 类型ID///
        /// </summary>
        public long id;

        /// <summary>
        /// 物件名///
        /// </summary>
        public string name;

        /// <summary>
        /// 描述///
        /// </summary>
        public string desc;

        /// <summary>
        /// 默认深度///
        /// </summary>
        public float defaultDepth;

        /// <summary>
        /// 最小深度///
        /// </summary>
        public float minDepth;

        /// <summary>
        /// 默认宽度///
        /// </summary>
        public float defaultWidth;

        /// <summary>
        /// 最小宽度///
        /// </summary>
        public float minWidth;

        /// <summary>
        /// 默认高度///
        /// </summary>
        public float defaultHeight;

        /// <summary>
        /// 最小高度///
        /// </summary>
        public float minHeight;

        /// <summary>
        /// 结构物件类型///
        /// </summary>
        public EElementType elementType;

        public string UIAssetBundleString;

        public string UIAssetString;

        public string ModelAssetBundleString;

        public string ModelAssetString;

        public string IconAssetBundleString;

        public string IconAssetString;

        public string ToJsonString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
