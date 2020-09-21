using DHI.Generic.MikeZero.DFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DHI.DFS.Utilities
{
    public class DfsKitchenSink
    {
        private IDfsFile _inputDfs;
        private IDfsFile _outputDfs;
        public string InputFile { get; set; }
        public string OutputFile { get; set; }

        public DfsKitchenSink() { }

        public DfsKitchenSink(string inputFile, string outputFile)
        {
            this.InputFile = inputFile;
            this.OutputFile = outputFile;

            if (!File.Exists(InputFile))
                throw new Exception(String.Format("Input file {0} does not exist!", InputFile));

            var ext1 = Path.GetExtension(inputFile).ToLower();
            var ext2 = Path.GetExtension(outputFile).ToLower();
            if (ext1 != ext2)
                throw new Exception("Input and output files must have same extension!");

        }

        public void ExtractItems(List<int> items)
        {
            try
            {
                _OpenFiles();
                var starttimestep = 0;
                var endtimestep = _inputDfs.FileInfo.TimeAxis.NumberOfTimeSteps-1;
                var stride = 1;
                var timesteps = _GetTimeSteps(starttimestep, endtimestep);
                _outputDfs = _CreateFromTemplate(_inputDfs, OutputFile, timesteps, stride, items);                
                _ProcessAllTimeSteps(timesteps, stride, items);
            }
            finally
            {
                _inputDfs.Close();
                _outputDfs.Close();
            }
        }

        public void ExtractTimeSteps(int starttimestep, int endtimestep)
        {
            ExtractTimeSteps(starttimestep, endtimestep, 1);
        }

        public void ExtractTimeSteps(int starttimestep, int endtimestep, int stride)
        {
            try
            {
                _OpenFiles();
                var timesteps = _GetTimeSteps(starttimestep, endtimestep);
                _outputDfs = _CreateFromTemplate(_inputDfs, OutputFile, timesteps, stride);
                _ProcessAllTimeSteps(timesteps, stride);
            }
            finally
            {
                _inputDfs.Close();
                _outputDfs.Close();
            }            
        }

        public void ExtractTimeSteps(List<int> timesteps, int stride)
        {
            try
            {
                _OpenFiles();
                _VerifyStride(timesteps.Last(), stride);
                _outputDfs = _CreateFromTemplate(_inputDfs, OutputFile, timesteps, stride);
                _ProcessAllTimeSteps(timesteps, stride);
            }
            finally
            {
                _inputDfs.Close();
                _outputDfs.Close();
            }                        
        }

        public void ExtractDateTimes(DateTime dateTime1, DateTime dateTime2)
        {
            try
            {
                _OpenFiles();
                var timesteps = _GetTimeSteps(dateTime1, dateTime2);
                _outputDfs = _CreateFromTemplate(_inputDfs, OutputFile, timesteps, 1);
                _ProcessAllTimeSteps(timesteps);
            }
            finally
            {
                _inputDfs.Close();
                _outputDfs.Close();
            }                        
        }

        private void _OpenFiles()
        {
            if (!File.Exists(InputFile))
                throw new Exception(String.Format("Input file {0} does not exist!", InputFile));
            
            _inputDfs = DfsFileFactory.DfsGenericOpen(InputFile);            
        }

        private DfsFile _CreateFromTemplate(IDfsFile dfsTemplate, string outputfile, IEnumerable<int> timesteps, int stride)
        {
            var items = Enumerable.Range(0, DfsOutput._NumberItems(dfsTemplate.ItemInfo)).ToList();            
            return _CreateFromTemplate(dfsTemplate, outputfile, timesteps, stride, items);
        }        

        private DfsFile _CreateFromTemplate(IDfsFile dfsTemplate, string outputfile, IEnumerable<int> timesteps, int stride, List<int> items)
        {
            IDfsFileInfo fi = dfsTemplate.FileInfo;
            //this._AnalyzeDfsInputItems(dfsTemplate.ItemInfo);
            var builder = DfsBuilder.Create(fi.FileTitle, fi.ApplicationTitle, fi.ApplicationVersion);

            IDfsTemporalAxis timeAxis = _CorrectTimeAxis(fi.TimeAxis, timesteps.First(), stride);
            DfsOutput.CreateHeader(fi, builder, timeAxis);
            DfsOutput.CreateDynamicItems(builder, dfsTemplate.ItemInfo, items);

            builder.CreateFile(outputfile);

            IDfsStaticItem staticItem;
            while ((staticItem = dfsTemplate.ReadStaticItemNext()) != null)
                builder.AddStaticItem(staticItem);

            return builder.GetFile();
        }

        private void _VerifyStride(int endtimestep, int stride)
        {
            if (stride > endtimestep)
            {
                throw new Exception(String.Format("Stride {0} can not be larger than endtimestep {1}", stride, endtimestep));
            }
        }

        private List<int> _GetTimeSteps()
        {
            var allTimesteps = Enumerable.Range(0, _inputDfs.FileInfo.TimeAxis.NumberOfTimeSteps).ToList();
            return allTimesteps;
        }

        private List<int> _GetTimeSteps(DateTime dateTime1, DateTime dateTime2)
        {
            var datetimes = _inputDfs.FileInfo.TimeAxis.GetDateTimes();

            var starttimestep = Array.FindIndex(datetimes, d => d == dateTime1);
            var endtimestep = Array.FindIndex(datetimes, d => d == dateTime2);

            var ntimesteps = endtimestep - starttimestep + 1;
            var timesteps = Enumerable.Range(starttimestep, ntimesteps).ToList();
            return timesteps;
        }

        private List<int> _GetTimeSteps(int starttimestep, int endtimestep)
        {
            if (endtimestep == -1)
            {
                endtimestep = _inputDfs.FileInfo.TimeAxis.NumberOfTimeSteps - 1;
            }
            var ntimesteps = endtimestep - starttimestep + 1;
            var timesteps = Enumerable.Range(starttimestep, ntimesteps).ToList();
            return timesteps;
        }

        private void _ProcessAllTimeSteps(List<int> timesteps)
        {
            _ProcessAllTimeSteps(timesteps, 1);
        }

        private void _ProcessAllTimeSteps(List<int> timesteps, int stride)
        {            
            var numItems = _inputDfs.ItemInfo.Count;
            int j = -1;

            foreach (var timestep in timesteps)
            {
                j++;
                if (j % stride != 0)
                    continue;                
                
                for (int item = 1; item <= numItems; ++item)
                {
                    var itemdata = _inputDfs.ReadItemTimeStep(item, timestep);
                    var data = (float[])itemdata.Data;
                    var time = itemdata.Time;

                    _outputDfs.WriteItemTimeStepNext(time, data);
                }                
            }
        }

        private void _ProcessAllTimeSteps(List<int> timesteps, int stride, List<int> items)
        {
            int j = -1;
            var is_dfsu = _inputDfs.ItemInfo[0].Name == "Z coordinate";
            var item_offset = 0;
            if (is_dfsu)
                item_offset = 1;

            foreach (var timestep in timesteps)
            {
                j++;
                if (j % stride != 0)
                    continue;

                if (is_dfsu)
                {
                    var itemdata = _inputDfs.ReadItemTimeStep(1, timestep); // zn
                    var data = (float[])itemdata.Data;
                    var time = itemdata.Time;
                    _outputDfs.WriteItemTimeStepNext(time, data);
                }
                    //dfsBuilder.AddDynamicItem(dynamicItems[0]);  //  z item (node values) 


                foreach (int item in items)
                {
                    var itemdata = _inputDfs.ReadItemTimeStep(item + 1 + item_offset, timestep); // from 0 to 1-based
                    var data = (float[])itemdata.Data;
                    var time = itemdata.Time;

                    _outputDfs.WriteItemTimeStepNext(time, data);
                }
            }
        }

        private IDfsTemporalAxis _CorrectTimeAxis(IDfsTemporalAxis timeAxis, int firsttimestep, int stride)
        {
            IDfsEqCalendarAxis newTimeAxis = timeAxis as IDfsEqCalendarAxis;
            if (newTimeAxis != null)
            {
                var dateTimes = newTimeAxis.GetDateTimes();
                newTimeAxis.StartDateTime = dateTimes[firsttimestep];
                newTimeAxis.TimeStep = newTimeAxis.TimeStep * stride;
                return newTimeAxis;
            }
            return timeAxis;
        }

    }
}
