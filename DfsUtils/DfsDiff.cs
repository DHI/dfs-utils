namespace DHI.DFS.Utilities
{
    public class DfsDiff
    {
        public void Run(string inputfile1, string inputfile2, string outputfile)
        {
            var tool = new DfsAdd();
            tool.Run(inputfile1, inputfile2, 1.0, -1.0, outputfile);
        }
    }
}
