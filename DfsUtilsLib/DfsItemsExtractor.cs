using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHI.DFS.Utilities
{
    public static class DfsItemsExtractor
    {
        public static void Extract(string inputFile, string outputFile, int[] items)
        {
            var ks = new DfsKitchenSink(inputFile, outputFile);
            ks.ExtractItems(items.ToList());
        }
    }
}
