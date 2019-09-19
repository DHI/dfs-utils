namespace DHI.DFS.Utilities
{
    public class DfsScale
    {
        public void Run(string inputfile1, double factor, string outputfile)
        {
            var tool = new DfsCalcSingleFile();
            tool.Run(inputfile1, factor, 0.0, outputfile);
        }
    }
}
