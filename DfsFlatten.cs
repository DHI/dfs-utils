using DHI.Generic.MikeZero.DFS;
using System;
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


            throw new NotImplementedException();

            try
            {
                _dfsInput = DfsFileFactory.DfsGenericOpen(inputfile);
                _dfsOutput = DfsOutput.CreateFromTemplate(_dfsInput, outputfile);

                //ProcessAllTimeSteps(_dfsOutput);
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

            for (int timestep = 0; timestep < nTimes; timestep++)
            {
                for (int item = 1; item <= nItems; ++item)
                {
                    var indatatime = _dfsInput.ReadItemTimeStep(item, timestep);
                    var indata = (float[])indatatime.Data;
                    var time = indatatime.Time;

                    //var outdata = indata.Select(r => r * fac + constant).ToArray();

                    //outputDfs.WriteItemTimeStepNext(time, outdata);
                }
            }
        }

    }
}
