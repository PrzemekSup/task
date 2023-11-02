using Dependencies.Contract;

namespace Dependencies.Infrastructure.Extensions
{
    public static class ProgramDependenciesExtension
    {
        public static Package? AddPackage(this ProgramDependencies program, string name, string version)
        {
            var programPackage = program.Packages.FirstOrDefault(x => x.Name == name);
            if (programPackage == null)
            {
                var newPackage = new Package()
                {
                    Name = name,
                    Version = version
                };

                program.Packages.Add(newPackage);
                return newPackage;
            }

            if (programPackage.Version != version)
            {
                Console.WriteLine($"Package {programPackage.Name} requires different versions {programPackage.Version} and {version}");
                return null;
            }
            return programPackage;
        }

        public static bool WasPackageValidated(this ProgramDependencies program, string name, string version)
        {
            var programPackage = program.Packages.FirstOrDefault(x => x.Name == name && x.Version == version);
            return programPackage != null && programPackage.IsValidated;
        }
    }
}
