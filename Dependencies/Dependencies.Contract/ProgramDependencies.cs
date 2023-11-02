namespace Dependencies.Contract
{
    public class ProgramDependencies
    {
        public List<Package> Packages { get; set; } = new List<Package>();
        public List<PackageDependency> Dependencies { get; set; } = new List<PackageDependency> { };
    }
}
