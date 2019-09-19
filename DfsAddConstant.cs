namespace DHI.DFS.Utilities
{
    public class DfsAddConstant
    {
        public void Run(string inputfile1, double constant, string outputfile)
        {
            var tool = new DfsCalcSingleFile();
            tool.Run(inputfile1, 1.0, constant, outputfile);
        }
    }
}
