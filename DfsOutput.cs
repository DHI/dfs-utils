using DHI.Generic.MikeZero.DFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHI.DFS.Utilities
{
    public static class DfsOutput
    {
        public static DfsFile CreateFromTemplate(string dfsTemplate, string outputfile)
        {
            var iDfsTemplate = DfsFileFactory.DfsGenericOpen(dfsTemplate);
            var outputDfs = CreateFromTemplate(iDfsTemplate, outputfile);
            iDfsTemplate.Close();
            return outputDfs;
        }

        public static DfsFile CreateFromTemplate(IDfsFile dfsTemplate, string outputfile)
        {
            return CreateFromTemplate(dfsTemplate, outputfile, 1);
        }

        public static DfsFile CreateFromTemplate(IDfsFile dfsTemplate, string outputfile, int nRepeats)
        {
            IDfsFileInfo fi = dfsTemplate.FileInfo;
            //this._AnalyzeDfsInputItems(dfsTemplate.ItemInfo);
            var builder = DfsBuilder.Create(fi.FileTitle, fi.ApplicationTitle, fi.ApplicationVersion);

            CreateHeader(fi, builder);

            var isDfsu3d = _IsDfsu3d(dfsTemplate.ItemInfo);
            if (nRepeats>1)
            { 
                if (isDfsu3d)
                    builder.AddDynamicItem(dfsTemplate.ItemInfo[0]);

                _CreateRepeatedDynamicItems(builder, dfsTemplate.ItemInfo, nRepeats);
            }
            else
            {
                CreateDynamicItems(builder, dfsTemplate.ItemInfo);
            }
            builder.CreateFile(outputfile);

            IDfsStaticItem staticItem;
            while ((staticItem = dfsTemplate.ReadStaticItemNext()) != null)
                builder.AddStaticItem(staticItem);

            return builder.GetFile();
        }

        private static bool _IsDfsu3d(IList<IDfsDynamicItemInfo> items)
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

        private static int _NumberItems(IList<IDfsDynamicItemInfo> items)
        {
            var isDfsu3d = _IsDfsu3d(items);            
            return (items.Count - _ItemOffset(isDfsu3d));
        }

        public static void CreateHeader(IDfsFileInfo fi, DfsBuilder builder)
        {
            CreateHeader(fi, builder, fi.TimeAxis);
        }

        public static void CreateHeader(IDfsFileInfo fi, DfsBuilder builder, IDfsTemporalAxis timeAxis)
        {
            builder.SetDataType(fi.DataType);
            builder.SetGeographicalProjection(fi.Projection);
            builder.SetTemporalAxis(timeAxis);            
            builder.SetItemStatisticsType(fi.StatsType);
            builder.DeleteValueByte = fi.DeleteValueByte;
            builder.DeleteValueDouble = fi.DeleteValueDouble;
            builder.DeleteValueFloat = fi.DeleteValueFloat;
            builder.DeleteValueInt = fi.DeleteValueInt;
            builder.DeleteValueUnsignedInt = fi.DeleteValueUnsignedInt;
            if (fi.IsFileCompressed)
            {
                int[] xKey;
                int[] yKey;
                int[] zKey;
                fi.GetEncodeKey(out xKey, out yKey, out zKey);
                builder.SetEncodingKey(xKey, yKey, zKey);
            }
            foreach (var cb in fi.CustomBlocks)
                builder.AddCustomBlock(cb);
        }

        public static void CreateDynamicItems(DfsBuilder dfsBuilder, IList<IDfsDynamicItemInfo> dynamicItems)
        {
            var nItems = dynamicItems.Count;
            //var nItems = _NumberItems(dynamicItems);
            //var itemoffset = _ItemOffset(_IsDfsu3d(dynamicItems));

            for (int index = 0; index < nItems; ++index)
            {
                //var dynamicItem = dynamicItems[index + itemoffset];
                dfsBuilder.AddDynamicItem(dynamicItems[index]);
            }
        }

        private static void _CreateRepeatedDynamicItems(DfsBuilder dfsBuilder, IList<IDfsDynamicItemInfo> dynamicItems, int nRepeats)
        {
            var nItems = _NumberItems(dynamicItems);
            var itemoffset = _ItemOffset(_IsDfsu3d(dynamicItems));
            for (int i = 0; i < nRepeats; i++)
            {                
                for (int index = 0; index < nItems; ++index)
                {                    
                    var dynamicItem = DfsHelper.CloneDynamicItem(dfsBuilder, dynamicItems[index + itemoffset]);
                    dynamicItem.Name = dynamicItem.Name + ", #" + (i + 1).ToString();
                    dfsBuilder.AddDynamicItem(dynamicItem);     
                }
            }
        }
    }
}
