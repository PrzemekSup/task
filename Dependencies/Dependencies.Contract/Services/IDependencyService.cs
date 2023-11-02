namespace Dependencies.Contract.Services
{
    public interface IDependencyService
    {
        ProgramDependencies Translate(string[] instructions);
        bool CheckDependencies(ProgramDependencies dependencies);
    }
}
