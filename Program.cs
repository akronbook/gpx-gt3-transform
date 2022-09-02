namespace GPX_GT3_Transform
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputGpxFile = args[0];
            var outputGpxFile = args[1];
            Console.WriteLine($"Input File={inputGpxFile}, Output File={outputGpxFile}");
        }
    }
}