using GeoAPI.Geometries;
using System.Collections.Generic;

namespace YunLang.SuperD.DataStruct
{
    /// <summary>
    /// 墙节点///
    /// </summary>
    public struct WallNode
    {
        /// <summary>
        /// 坐标///
        /// </summary>
        public Coordinate coord;

        /// <summary>
        /// 索引///
        /// </summary>
        public List<int> crossIndex;
    }
}
