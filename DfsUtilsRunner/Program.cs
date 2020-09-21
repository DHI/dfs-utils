using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHI.DFS.Utilities;

namespace DHI.DFS.Utilities.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!_VerifyArgs(args))
            { 
                _PrintUsage();
                return;
            }

            var tool = _GetTool(args[0]);
            
            try
            {
                switch (tool)
                {
                    case DfsTool.Scale:
                        _RunScaleTool(args);
                        break;
                    case DfsTool.AddConstant:
                        _RunAddConstantTool(args);
                        break;
                    case DfsTool.Sum:
                        _RunSumTool(args);
                        break;
                    case DfsTool.Diff:
                        _RunDiffTool(args);
                        break;
                    case DfsTool.ExtractTimeSteps:
                        _RunExtractStepsTool(args);
                        break;
                    case DfsTool.ExtractItems:
                        _RunExtractItemsTool(args);
                        break;
                    case DfsTool.TimeAverage:
                        _RunTimeAverageTool(args);
                        break;
                    case DfsTool.Flatten:
                        _RunFlattenTool(args);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
            }            
        }

        private static void _RunScaleTool(string[] args)
        {            
            if (args.Count() != 4)
            {   
                Console.WriteLine(">DfsUtils Scale infile.dfsu 0.9 outfile.dfsu");
                throw new ArgumentException("Scale needs 3 arguments");
            }
            var infile = args[1];
            var scale = Convert.ToDouble(args[2]);
            var outfile = args[3];

            var scaler = new DfsScale();
            scaler.Run(infile, scale, outfile);            
        }

        private static void _RunAddConstantTool(string[] args)
        {
            if (args.Count() != 4)
            {
                Console.WriteLine(">DfsUtils AddConstant infile.dfsu 7 outfile.dfsu");
                throw new ArgumentException("AddConstant needs 3 arguments");
            }
            var infile = args[1];
            var cnst = Convert.ToDouble(args[2]);
            var outfile = args[3];

            var constantAdder = new DfsAddConstant();
            constantAdder.Run(infile, cnst, outfile);
        }

        private static void _RunSumTool(string[] args)
        {
            if (args.Count() != 4)
            {
                Console.WriteLine(">DfsUtils Sum file1.dfsu file2.dfsu outfile.dfsu");
                throw new ArgumentException("Sum needs 3 arguments");
            }
            var file1 = args[1];
            var file2 = args[2];
            var outfile = args[3];

            var adder = new DfsAdd();
            adder.Run(file1, file2, outfile);
        }

        private static void _RunDiffTool(string[] args)
        {
            if (args.Count() != 4)
            {
                Console.WriteLine(">DfsUtils Diff infile.dfsu 0.9 outfile.dfsu");
                throw new ArgumentException("Diff needs 3 arguments");
            }
            var file1 = args[1];
            var file2 = args[2];
            var outfile = args[3];

            var differ = new DfsDiff();
            differ.Run(file1, file2, outfile);
        }

        private static void _RunExtractStepsTool(string[] args)
        {
            if (args.Count() < 5 || args.Count() > 6)
            {
                Console.WriteLine(">DfsUtils ExtractSteps infile.dfsu outfile.dfsu start end [stride]");
                Console.WriteLine(">DfsUtils ExtractSteps infile.dfsu outfile.dfsu 10 -1 2");
                throw new ArgumentException("ExtractSteps needs 4 or 5 arguments");
            }
            var infile = args[1];            
            var outfile = args[2];
            var startstep = Convert.ToInt32(args[3]);
            var endstep = Convert.ToInt32(args[4]);
            if (args.Count() == 6)
            {
                var stride = Convert.ToInt32(args[5]);
                DfsTimeStepsExtractor.Extract(infile, outfile, startstep, endstep, stride);
            }
            else
            {
                DfsTimeStepsExtractor.Extract(infile, outfile, startstep, endstep);
            }
        }

        private static void _RunExtractItemsTool(string[] args)
        {
            if (args.Count() < 4)
            {
                Console.WriteLine(">DfsUtils ExtractItems infile.dfs0 outfile.dfs0 1 3 5 8");
                throw new ArgumentException("ExtractItems 3 or more arguments");
            }
            var infile = args[1];
            var outfile = args[2];

            int nitems = args.Count() - 3;
            var items = new int[nitems];

            for (int i = 0; i < nitems; i++)
            {
                items[i] = Convert.ToInt32(args[i + 3]) - 1;  // make 0-based index
                //Console.WriteLine("item " +i + " is " + items[i]);
            }
            
            DfsItemsExtractor.Extract(infile, outfile, items);            
        }

        private static void _RunTimeAverageTool(string[] args)
        {
            if (args.Count() != 3)
            {
                Console.WriteLine(">DfsUtils TimeAverage infile.dfsu outfile.dfsu");
                throw new ArgumentException("TimeAverage needs 2 arguments");
            }
            var infile = args[1];
            var outfile = args[2];

            var ta = new DfsTimeAverage();
            ta.Run(infile, outfile);
        }

        private static void _RunFlattenTool(string[] args)
        {
            if (args.Count() != 3)
            {
                Console.WriteLine(">DfsUtils Flatten infile.dfsu outfile.dfsu");
                throw new ArgumentException("Flatten needs 2 arguments");
            }
            var infile = args[1];            
            var outfile = args[2];

            var flattener = new DfsFlatten();
            flattener.Run(infile, outfile);
        }

        private static bool _VerifyArgs(string[] args)
        {
            DfsTool tool;
            try
            { 
                tool = _GetTool(args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        private static void _PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine(">DfsUtils [toolname] [args]");
            Console.WriteLine(">DfsUtils Scale infile.dfsu 0.9 outfile.dfsu");
            Console.WriteLine(">DfsUtils AddConstant infile.dfsu 10.0 outfile.dfsu");
            Console.WriteLine(">DfsUtils Sum file1.dfs2 file2.dfs2 outfile.dfs2");
            Console.WriteLine(">DfsUtils Diff file1.dfs2 file2.dfs2 outfile.dfs2");
            Console.WriteLine(">DfsUtils ExtractSteps infile.dfs0 outfile.dfs0 20 -1 2");
            Console.WriteLine(">DfsUtils ExtractItems infile.dfs0 outfile.dfs0 1,3,5,8");
            Console.WriteLine(">DfsUtils TimeAverage infile.dfs1 outfile.dfs1");
            // Console.WriteLine(">DfsUtils Flatten infile.dfs1 outfile.dfs1");
        }

        private static DfsTool _GetTool(string arg0)
        {
            var toolName = arg0;
            switch (toolName.ToLower())
            {
                case "scale":
                    return DfsTool.Scale;
                case "addconstant":
                    return DfsTool.AddConstant;                    
                case "add": case "sum":
                    return DfsTool.Sum;
                case "diff":
                    return DfsTool.Diff;
                case "extracttimesteps": case "extractsteps":
                    return DfsTool.ExtractTimeSteps;
                case "extractitems":
                    return DfsTool.ExtractItems;
                case "flatten":
                    return DfsTool.Flatten;
                case "timeaverage":                     
                    return DfsTool.TimeAverage;
                default:
                    throw new Exception("No such tool: " + arg0);
            }
        }
    }

    enum DfsTool { Scale, AddConstant, Sum, Diff, ExtractTimeSteps, ExtractItems, Flatten, TimeAverage}

}
