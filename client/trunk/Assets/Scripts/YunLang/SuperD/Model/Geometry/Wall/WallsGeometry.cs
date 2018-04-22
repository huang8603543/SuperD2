using System.Collections.Generic;
using YunLang.SuperD.DataStruct;

namespace YunLang.SuperD.Model
{
    /// <summary>
    /// 墙结构///
    /// </summary>
    public class WallsGeometry
    {
        private List<WallSegment> walls;
        private List<WallNode> headModeList;
        private List<WallNode> nodeList;
        private List<MeshVertices> wallsMeshVs;

        public WallsGeometry()
        {
            Init();
        }

        public void Init()
        {
            walls = new List<WallSegment>();
            headModeList = new List<WallNode>();
            nodeList = new List<WallNode>();
            wallsMeshVs = new List<MeshVertices>();
        }
    }
}
