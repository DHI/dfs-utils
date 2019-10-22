using DHI.Generic.MikeZero.DFS;
using System;
using System.IO;
using System.Linq;

namespace DHI.DFS.Utilities
{
    public class DfsCalcSingleFile
    {
        private IDfsFile _dfsInput;
        private IDfsFile _dfsOutput;
        
        public void Run(string inputfile, string outputfile)
        {
            Run(inputfile, 1.0, 0.0, outputfile);
        }

        public void Run(string inputfile, double fac, double constant, string outputfile)
        {
            if (!File.Exists(inputfile))
                throw new Exception(String.Format("Input file {0} does not exist!", inputfile));

            var ext1 = Path.GetExtension(inputfile).ToLower();
            var ext2 = Path.GetExtension(outputfile).ToLower();
            if (ext1 != ext2)
                throw new Exception("Input and output files must have same extension!");
            
            try
            {
                _dfsInput = DfsFileFactory.DfsGenericOpen(inputfile);                
                _dfsOutput = DfsOutput.CreateFromTemplate(_dfsInput, outputfile);

                ProcessAllTimeSteps(_dfsOutput, (float)fac, (float)constant);
            }
            finally
            {
                _dfsInput.Close();
                _dfsOutput.Close();
            }            
        }

        private void ProcessAllTimeSteps(IDfsFile outputDfs, float fac, float constant)
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

                    var outdata = indata.Select(r => r * fac + constant).ToArray();                    
                    
                    outputDfs.WriteItemTimeStepNext(time, outdata);
                }
            }        
        }
    }
}
