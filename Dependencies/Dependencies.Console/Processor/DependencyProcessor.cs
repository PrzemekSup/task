using Dependencies.Contract.Services;

namespace Dependencies.Console.Processor
{
    public class DependencyProcessor : IDependencyProcessor
    {
        private readonly IDependencyService _dependencyService;
        public DependencyProcessor(IDependencyService dependencyService)
        {
            _dependencyService = dependencyService;
        } 

        public bool Process(string[] instructions)
        {
            if (instructions.Length == 0)
            {
                System.Console.WriteLine("Instructions cannot be found");
                return false;
            }

            var result = false;
            try
            {
                var program = _dependencyService.Translate(instructions.ToArray());
                result = _dependencyService.CheckDependencies(program);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Exception occured during checking instructions: {ex.Message}");
                return false;
            }

            System.Console.WriteLine(result ? "PASS" : "FAIL");
            return result;
        }
    }
}
