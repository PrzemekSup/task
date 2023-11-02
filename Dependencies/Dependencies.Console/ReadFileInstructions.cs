namespace Dependencies.Console
{
    internal static class ReadFileInstructions
    {
        public static bool Process(List<string> instructions)
        {
            System.Console.WriteLine("eg. '../../../../testdata/input000.txt' or '{localpath}/Dependencies/testdata/'");
            System.Console.Write("Path to file: ");
            var path = System.Console.ReadLine();
            if (!File.Exists(path))
            {
                System.Console.WriteLine("File not found");
                return false;
            }

            using (var reader = new StreamReader(path))
            {
                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    instructions.Add(line);
                }
            }

            return true;
        }
    }
}
