using UnityEngine;

namespace YunLang.SuperD.DataStruct
{
    /// <summary>
    /// 网格(线段)顶点集合///
    /// </summary>
    public struct MeshVertices
    {
        /// <summary>
        /// 起始点集合///
        /// </summary>
        public Vector2[] startSide;

        /// <summary>
        /// 结束点集合///
        /// </summary>
        public Vector2[] endSide;

        /// <summary>
        /// 中心点///
        /// </summary>
        public Vector2 center;
    }
}
