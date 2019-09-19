using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHI.DFS.Utilities
{
    public static class DfsTimeStepsExtractor
    {
        public static void Extract(string inputFile, string outputFile, int starttimestep, int endtimestep, int stride)
        {
            var ks = new DfsKitchenSink(inputFile, outputFile);
            ks.ExtractTimeSteps(starttimestep, endtimestep, stride);
        }
        
        public static void Extract(string inputFile, string outputFile, int starttimestep, int endtimestep)
        {
            var ks = new DfsKitchenSink(inputFile, outputFile);
            ks.ExtractTimeSteps(starttimestep, endtimestep, 1);
        }

        public static void Extract(string inputFile, string outputFile, int[] timesteps)
        {
            var ks = new DfsKitchenSink(inputFile, outputFile);
            ks.ExtractTimeSteps(timesteps.ToList(), 1);
        }

        public static void Extract(string inputFile, string outputFile, DateTime dateTime1, DateTime dateTime2)
        {
            var ks = new DfsKitchenSink(inputFile, outputFile);
            ks.ExtractDateTimes(dateTime1, dateTime2);
        }
    }
}
