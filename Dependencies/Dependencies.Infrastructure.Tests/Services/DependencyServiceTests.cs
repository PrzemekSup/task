using Dependencies.Contract;
using Dependencies.Infrastructure.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace Dependencies.Infrastructure.Tests.Services
{
    public class DependencyServiceTests
    {
        private readonly DependencyService _dependencyService;

        public DependencyServiceTests()
        {
            _dependencyService = new DependencyService();
        }

        [Fact]
        public void Translate_ShouldFail_WrongNumberOfPackages()
        {
            // arrange
            var instructions = new[] { "", "a1,1" };

            // act & assert
            Assert.Throws<ArgumentException>(() => _dependencyService.Translate(instructions));
        }

        [Fact]
        public void Translate_ShouldFail_WrongNumberOfDependencies()
        {
            // arrange
            var instructions = new[] { "1", "a1,1", "b1,1" };

            // act & assert
            Assert.Throws<ArgumentException>(() => _dependencyService.Translate(instructions));
        }

        [Fact]
        public void Translate_ShouldReadPackages()
        {
            // arrange
            var instructions = new[] { "2", "A,1", "B,2" };

            // act
            var result = _dependencyService.Translate(instructions);

            // assert
            Assert.Equal(2, result.Packages.Count);
            Assert.Equal("A", result.Packages[0].Name);
            Assert.Equal("2", result.Packages[1].Version);
        }


        [Fact]
        public void Translate_ShouldReadDependencies()
        {
            // arrange
            var instructions = new[] { "1", "A,1", "2", "A,1,B,1,C,2", "B,1,C,2"};

            // act
            var result = _dependencyService.Translate(instructions);

            // assert
            Assert.Equal(2, result.Dependencies.Count);

            Assert.Equal("A", result.Dependencies[0].Package.Name);
            Assert.Equal("1", result.Dependencies[0].Package.Version);
            Assert.Equal(2, result.Dependencies[0].RequiredPackages.Count);
            Assert.Equal("B", result.Dependencies[0].RequiredPackages[0].Name);
            Assert.Equal("1", result.Dependencies[0].RequiredPackages[0].Version);
            Assert.Equal("C", result.Dependencies[0].RequiredPackages[1].Name);
            Assert.Equal("2", result.Dependencies[0].RequiredPackages[1].Version);

            Assert.Equal("B", result.Dependencies[1].Package.Name);
            Assert.Equal("1", result.Dependencies[1].Package.Version);
            Assert.Single(result.Dependencies[1].RequiredPackages);
            Assert.Equal("C", result.Dependencies[1].RequiredPackages[0].Name);
            Assert.Equal("2", result.Dependencies[1].RequiredPackages[0].Version);
        }

        [Fact]
        public void CheckDependencies_ShouldFail()
        {
            // arrange
            var program = new ProgramDependencies()
            {
                Packages = new List<Package>()
                {
                    new Package()
                    {
                        Name = "A",
                        Version = "1"
                    }
                },
                Dependencies = new List<PackageDependency>()
                {
                    new PackageDependency()
                    {
                        Package = new Package()
                        {
                            Name = "A",
                            Version= "1"
                        },
                        RequiredPackages = new List<Package>()
                        {
                            new Package()
                            {
                                Name = "B",
                                Version = "1"
                            },
                            new Package()
                            {
                                Name = "B",
                                Version = "2"
                            }
                        }
                    }
                }
            };

            // act
            var result = _dependencyService.CheckDependencies(program);

            // assert
            Assert.False(result);
        }

        [Fact]
        public void CheckDependencies_ShouldPass()
        {
            // arrange
            var program = new ProgramDependencies()
            {
                Packages = new List<Package>()
                {
                    new Package()
                    {
                        Name = "B",
                        Version = "2"
                    }
                },
                Dependencies = new List<PackageDependency>()
                {
                    new PackageDependency()
                    {
                        Package = new Package()
                        {
                            Name = "A",
                            Version= "1"
                        },
                        RequiredPackages = new List<Package>()
                        {
                            new Package()
                            {
                                Name = "B",
                                Version = "2"
                            }
                        }
                    },
                    new PackageDependency()
                    {
                        Package = new Package()
                        {
                            Name = "B",
                            Version= "2"
                        },
                        RequiredPackages = new List<Package>()
                        {
                            new Package()
                            {
                                Name = "A",
                                Version = "1"
                            }
                        }
                    }
                }
            };

            // act
            var result = _dependencyService.CheckDependencies(program);

            // assert
            Assert.True(result);
        }
    }
}
