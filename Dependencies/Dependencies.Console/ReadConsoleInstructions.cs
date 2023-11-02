namespace Dependencies.Console
{
    internal static class ReadConsoleInstructions
    {
        public static bool Process(List<string> instructions)
        {
            var packagesResult = ProcessItems(instructions, "How many packages do you want to provide? ");
            var dependenciesResult = ProcessItems(instructions, "How many dependencies do you want to provide? ");
            return packagesResult && dependenciesResult;
        }

        private static bool ProcessItems(List<string> instructions, string question)
        {
            System.Console.Write(question);
            var itemsCount = System.Console.ReadLine();
            if (!int.TryParse(itemsCount, out var items))
            {
                System.Console.WriteLine("Incorrect value provided");
                return false;
            }

            instructions.Add(itemsCount);
            for (int i = 0; i < items; i++)
            {
                System.Console.Write($"Item {i + 1}:");
                instructions.Add(System.Console.ReadLine());
            }
            return true;
        }
    }
}
