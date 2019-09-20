using DHI.Generic.MikeZero.DFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHI.DFS.Utilities
{
    public static class DfsHelper
    {
        public static bool IsDfsu3d(IList<IDfsDynamicItemInfo> items)
        {
            var isDfsu3d = false;
            if (items[0].Name == "Z coordinate")
            {
                isDfsu3d = true;
            }
            return isDfsu3d;
        }

        private static int _ItemOffset(bool isDfsu3d)
        {
            return isDfsu3d ? 1 : 0;
        }

        public static int ItemOffset(IList<IDfsDynamicItemInfo> items)
        {
            return IsDfsu3d(items) ? 1 : 0;
        }

        public static int NumberItems(IList<IDfsDynamicItemInfo> items)
        {
            var isDfsu3d = IsDfsu3d(items);            
            return (items.Count - _ItemOffset(isDfsu3d));
        }

        public static IDfsDynamicItemInfo CloneDynamicItem(DfsBuilder dfsBuilder, IDfsDynamicItemInfo dynamicItem)
        {
            DfsDynamicItemBuilder itemBuilder = dfsBuilder.CreateDynamicItemBuilder();
            itemBuilder.Set(dynamicItem.Name, dynamicItem.Quantity, dynamicItem.DataType);
            itemBuilder.SetValueType(dynamicItem.ValueType);
            itemBuilder.SetAxis(dynamicItem.SpatialAxis);
            itemBuilder.SetReferenceCoordinates(dynamicItem.ReferenceCoordinateX, dynamicItem.ReferenceCoordinateY, dynamicItem.ReferenceCoordinateZ);
            return itemBuilder.GetDynamicItemInfo();
        }
    }
}
