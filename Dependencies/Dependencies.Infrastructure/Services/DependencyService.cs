using Dependencies.Contract;
using Dependencies.Contract.Services;
using Dependencies.Infrastructure.Extensions;

namespace Dependencies.Infrastructure.Services
{
    public class DependencyService : IDependencyService
    {
        public ProgramDependencies Translate(string[] instructions)
        {
            var program = new ProgramDependencies();
            if (!int.TryParse(instructions[0], out int numberOfPackages))
            {
                throw new ArgumentException($"Wrong number of packages: {instructions[0]}");
            }

            for (int i = 1; i < numberOfPackages + 1; i++)
            {
                var (name, version) = GetPackage(instructions[i]);
                program.AddPackage(name, version);
            }

            if (numberOfPackages + 1 == instructions.Length)
            {
                return program;
            }

            if (!int.TryParse(instructions[numberOfPackages + 1], out int numberOfDependencies))
            {
                throw new ArgumentException($"Wrong number of dependencies: {instructions[numberOfPackages + 1]}");
            }

            for (int i  = numberOfPackages + 2; i < numberOfPackages + numberOfDependencies + 2; i++)
            {
                var packageDependency = GetPackageDependency(instructions[i]);
                program.Dependencies.Add(packageDependency);
            }
            return program;
        }

        public bool CheckDependencies(ProgramDependencies program)
        {
            var packagesToCheck = new List<Package>(program.Packages);
            foreach (var item in packagesToCheck)
            {
                if(!CheckPackage(item, program))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckPackage(Package item, ProgramDependencies program)
        {
            item.IsValidated = true;
            var dependencies = program.Dependencies
                .Where(x => x.Package.Name == item.Name && x.Package.Version == item.Version)
                .SelectMany(x => x.RequiredPackages);

            foreach (var dependency in dependencies)
            {
                if (program.WasPackageValidated(dependency.Name, dependency.Version))
                    continue;

                var newPackage = program.AddPackage(dependency.Name, dependency.Version);
                if (newPackage == null)
                    return false;

                var newPackageCheck = CheckPackage(newPackage, program);
                if (!newPackageCheck)
                    return false;
            }

            return true;
        }

        private (string name, string version) GetPackage(string line)
        {
            var data = line.Split(",");
            return (data[0], data[1]);
        }

        private PackageDependency GetPackageDependency(string line)
        {
            var data = line.Split(",");
            var packageDependency = new PackageDependency()
            {
                Package = new Package()
                {
                    Name = data[0],
                    Version = data[1]
                }
            };

            var i = 2;
            while (i < data.Length)
            {
                var requiredPackage = new Package()
                {
                    Name = data[i++],
                    Version = data[i++],
                };
                packageDependency.RequiredPackages.Add(requiredPackage);
            }

            return packageDependency;
        }
    }
}
