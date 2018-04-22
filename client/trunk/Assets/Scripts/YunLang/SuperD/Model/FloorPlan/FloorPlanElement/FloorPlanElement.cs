using UnityEngine;
using NetTopologySuite.LinearReferencing;
using YunLang.SuperD.Data;
using YunLang.SuperD.TypeData;

namespace YunLang.SuperD.Model
{
    /// <summary>
    /// 平面结构物件（门窗）///
    /// </summary>
    public abstract class FloorPlanElement
    {
        public FloorPlanElementData Data
        {
            get;
            private set;
        }

        public FloorPlanElementTypeData TypeData
        {
            get;
            private set;
        }

        public GameObject GameObject
        {
            get;
            private set;
        }

        public Transform Transform
        {
            get
            {
                return GameObject.transform;
            }
        }

        public LinearLocation LinearLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否正在编辑///
        /// </summary>
        public bool Editing
        {
            get;
            set;
        }

        private bool _selectable;

        /// <summary>
        /// 是否可选中///
        /// </summary>
        public bool Selectable
        {
            get { return _selectable; }
            set
            {
                _selectable = value;
                if (!_selectable)
                {
                    ///PanelDoorWindowEdit.mPanelDoorWindowEdit.CloseScene();

                }
            }
        }

        public FloorPlanElement(FloorPlanElementData data)
        {
            Data = data;
            Selectable = true;
        }

        /// <summary>
        /// 刷新结构物件位置///
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="Width"></param>
        public virtual void UpdateElementTransform(Vector3 position, Quaternion rotation)
        {
            Transform.position = position;
            Transform.rotation = rotation;
        }

        public virtual void SelectElement(bool select)
        {
            //if (PanelDoorWindowEdit.currentElement != null)
            //{
            //    PanelDoorWindowEdit.currentElement.SetSelected(false);
            //}
            //if (select)
            //{
            //    (FloorPlanInterfaceImplement.GetInstance() as FloorPlanInterfaceImplement).EditRoom(this.LinearLocation.ComponentIndex);
            //}
        }

        //public abstract ElementModel CreateModel(IGeometry geom);
        public abstract void RemoveElement();
        public abstract float GetUIHeight();
        public abstract float GetUIWidth();
        public abstract Vector3 GetUIWorldPosition();

    }
}
