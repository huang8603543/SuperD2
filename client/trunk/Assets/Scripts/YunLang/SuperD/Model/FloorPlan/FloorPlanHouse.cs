using UnityEngine;
using System.Collections.Generic;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace YunLang.SuperD.Model
{
    /// <summary>
    /// 平面房屋///
    /// </summary>
    public class FloorPlanHouse
    {
        /// <summary>
        /// 房间的集合///
        /// </summary>
        public List<FloorPlanRoom> floorPlanRooms = new List<FloorPlanRoom>();

        /// <summary>
        /// 添加一个房间///
        /// </summary>
        /// <param name="roomData">现有房间数据</param>
        public void AddRoomData(FloorPlanRoom roomData)
        {
            if (roomData == null)
            {
                Debug.LogError("roomData is null");
                return;
            }
            floorPlanRooms.Add(roomData);
        }

        /// <summary>
        /// 创建一个新房间///
        /// </summary>
        /// <param name="newRoom"></param>
        /// <returns></returns>
        public FloorPlanRoom CreateNewRoom(IGeometry newRoom)
        {
            if (newRoom != null && !newRoom.IsEmpty)
            {
                Debug.LogError("newRoom is null or newRoom is empty!");
                return null;
            }

            FloorPlanRoom room = new FloorPlanRoom();
            floorPlanRooms.Add(room);
            //this.SortByRoomHeightGetIndex(-1);

            Polygon polygon = newRoom as Polygon;
            if (polygon != null)
            {
                //ILineString wallString = FloorPlanHelper.GetDefaultGeometryFactory().CreateLineString(polygon.Coordinates);
                //float[] wallsThickness = new float[] { 0.1f };
                //item.AddWalls(wallString, wallsThickness);
                //item.UpdateWalls();
                //item.UpdateSelfGeometry();
                //item.UpdateBufferedGeometry();
                //this.UpdateGeometry(item, newRoom);
            }

            return room;
        }

        /// <summary>
        /// 获取所有编辑元素///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<FloorPlanElement> GetAllElement<T>()
        {
            List<FloorPlanElement> list = new List<FloorPlanElement>();
            //for (int i = 0; i < this.floorPlanRoomsData.Count; i++)
            //{
            //    if (this.floorPlanRoomsData[i].FloorPlanElements != null)
            //    {
            //        list.AddRange(this.floorPlanRoomsData[i].FloorPlanElements.FindAll(new Predicate<FloorPlanElement>(FloorPlanHouse.< GetAllElement`1 > m__0<T>)));
            //    }
            //}
            return list;
        }
    }
}
