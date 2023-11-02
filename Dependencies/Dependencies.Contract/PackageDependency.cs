namespace Dependencies.Contract
{
    public class PackageDependency
    {
        public Package Package { get; set; }
        public List<Package> RequiredPackages { get; set; } = new List<Package>();
    }
}
