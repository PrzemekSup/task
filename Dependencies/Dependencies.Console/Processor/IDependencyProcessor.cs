namespace Dependencies.Console.Processor
{
    internal interface IDependencyProcessor
    {
        bool Process(string[] instructions);
    }
}
