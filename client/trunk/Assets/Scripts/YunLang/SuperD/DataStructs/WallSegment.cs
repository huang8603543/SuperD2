using NetTopologySuite.Noding;
using GeoAPI.Geometries;

namespace YunLang.SuperD.DataStruct
{
    /// <summary>
    /// 墙段///
    /// </summary>
    public struct WallSegment
    {
        public Coordinate point1;
        public Coordinate point2;

        /// <summary>
        /// 厚度///
        /// </summary>
        public float thickness;

        /// <summary>
        /// 节点化片段结构///
        /// </summary>
        public NodedSegmentString segmentString;
    }
}
