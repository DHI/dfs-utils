using DHI.Generic.MikeZero.DFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHI.DFS.Utilities
{
    public class DfsAdd
    {
        private IDfsFile _dfsInput1;
        private IDfsFile _dfsInput2;
        private IDfsFile _dfsOutput;
        private int _nTimes;

        public void Run(string inputfile1, string inputfile2, string outputfile)
        {
            Run(inputfile1, inputfile2, 1.0, 1.0, outputfile);
        }

        public void Run(string inputfile1, string inputfile2, double fac1, double fac2, string outputfile)
        {
            if (!File.Exists(inputfile1))
                throw new Exception(String.Format("First input file {0} does not exist!", inputfile1));
            if (!File.Exists(inputfile2))
                throw new Exception(String.Format("Second input file {0} does not exist!", inputfile1));

            try
            {
                _dfsInput1 = DfsFileFactory.DfsGenericOpen(inputfile1);
                _dfsInput2 = DfsFileFactory.DfsGenericOpen(inputfile2);
                _VerifyInputSimilarity(_dfsInput1, _dfsInput2);
                _dfsOutput = DfsOutput.CreateFromTemplate(_dfsInput1, outputfile);

                ProcessAllTimeSteps(_dfsOutput, (float)fac1, (float)fac2);
            }
            finally
            {
                _dfsInput1.Close();
                _dfsInput2.Close();
                _dfsOutput.Close();
            }            
        }

        private void _VerifyInputSimilarity(IDfsFile _dfsInput1, IDfsFile _dfsInput2)
        {
            // In case number of time steps does not match, take the smallest.
            _nTimes = _dfsInput1.FileInfo.TimeAxis.NumberOfTimeSteps;
            if (_nTimes > _dfsInput2.FileInfo.TimeAxis.NumberOfTimeSteps)
            {
                _nTimes = _dfsInput2.FileInfo.TimeAxis.NumberOfTimeSteps;
                Console.Out.WriteLine("Number of time steps does not match, using the smallest number");
            }

            var numItems1 = _dfsInput1.ItemInfo.Count;
            var numItems2 = _dfsInput2.ItemInfo.Count;
            if (_dfsInput1.ItemInfo.Count != _dfsInput2.ItemInfo.Count)
                throw new Exception("Two input files must have same number of dynamic items!");

            for (int i = 0; i < _dfsInput1.ItemInfo.Count; i++)
            {
                var itemInfo = _dfsInput1.ItemInfo[i];

                // Validate item sizes
                var itemInfo2 = _dfsInput2.ItemInfo[i];
                if (itemInfo.ElementCount != itemInfo2.ElementCount)
                    throw new Exception("Dynamic items must have same size, item number " + (i + 1) +
                                        " has different sizes in the two files");
                // Validate the data type, only supporting floats.                
                else if (itemInfo.DataType != DfsSimpleType.Float)
                    throw new Exception("Dynamic item must be float, item number " + (i + 1) + " is of type " +
                                        (itemInfo.DataType));
            }
        }

        private void ProcessAllTimeSteps(IDfsFile outputDfs, float fac1, float fac2)
        {             
            //int numTimes = _dfsInput1.FileInfo.TimeAxis.NumberOfTimeSteps;
            var numItems = _dfsInput1.ItemInfo.Count;

            for (int timestep = 0; timestep < _nTimes; timestep++)
            {
                for (int item = 1; item <= numItems; ++item)
                {                    
                    var datatime1 = _dfsInput1.ReadItemTimeStep(item, timestep);
                    var data1 = (float[])datatime1.Data;
                    var time1 = datatime1.Time;
                    var data2 = (float[])_dfsInput2.ReadItemTimeStep(item, timestep).Data;

                    data1 = data1.Select(r => r * fac1).ToArray();
                    data2 = data2.Select(r => r * fac2).ToArray();
                    var outdata = data1.Zip(data2, (x, y) => x + y).ToArray();

                    outputDfs.WriteItemTimeStepNext(time1, outdata);
                }
            }
        
        }

    }
}
