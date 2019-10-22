using DHI.Generic.MikeZero.DFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DHI.DFS.Utilities
{
    public class DfsFlatten
    {
        private IDfsFile _dfsInput;
        private IDfsFile _dfsOutput;

        public void Run(string inputfile, string outputfile)
        {
            if (!File.Exists(inputfile))
                throw new Exception(String.Format("Input file {0} does not exist!", inputfile));

            try
            {
                _dfsInput = DfsFileFactory.DfsGenericOpen(inputfile);
                _dfsOutput = DfsOutput.CreateFromTemplate(_dfsInput, outputfile);

                ProcessAllTimeSteps(_dfsOutput);
            }
            finally
            {
                _dfsInput.Close();
                _dfsOutput.Close();
            }
        }

        private void ProcessAllTimeSteps(IDfsFile outputDfs)
        {
            var nTimes = _dfsInput.FileInfo.TimeAxis.NumberOfTimeSteps;
            var nItems = _dfsInput.ItemInfo.Count;

            List<float[]> outdatalist = new List<float[]>();

            int timestep0 = 0;
            for (int item = 1; item <= nItems; ++item)
            {
                var indatatime = _dfsInput.ReadItemTimeStep(item, timestep0);
                var indata = (float[])indatatime.Data;                
                outdatalist.Add(indata);
            }

            // from step 1 and onwards
            for (int timestep = 1; timestep < nTimes; timestep++)
            {
                for (int item = 1; item <= nItems; ++item)
                {
                    var indatatime = _dfsInput.ReadItemTimeStep(item, timestep);
                    var indata = (float[])indatatime.Data;
                    //var time = indatatime.Time;

                    outdatalist[item-1] = outdatalist[item-1].Zip(indata, (x, y) => x + y).ToArray();                    
                }
            }

            for (int item = 1; item <= nItems; ++item)
            {
                outdatalist[item-1] = outdatalist[item-1].Select(
                    x => x / Convert.ToSingle(nTimes)).ToArray();
                outputDfs.WriteItemTimeStepNext(timestep0, outdatalist[item-1]);
            }
        }

    }
}
