using YunLang.SuperD.Enum;
using UnityEngine;

namespace YunLang.SuperD.TypeData
{
    /// <summary>
    /// 3D模型类型数据///
    /// </summary>
    public class ModelTypeData
    {
        /// <summary>
        /// 类型ID///
        /// </summary>
        public long id;

        /// <summary>
        /// 是否可编辑///
        /// </summary>
        public bool editable;

        /// <summary>
        /// 模型类型///
        /// </summary>
        public EModelType modelType;

        /// <summary>
        /// 旋转模式///
        /// </summary>
        public ERotateType rotateType;

        /// <summary>
        /// 缩放模式///
        /// </summary>
        public EScaleType scaleType;
    }
}
