using Dependencies.Console.Processor;
using Dependencies.Contract;
using Dependencies.Contract.Services;
using Moq;
using Xunit;

namespace Dependencies.Console.Tests
{
    public class DependencyProcessorTests
    {
        private readonly Mock<IDependencyService> _dependencyService;
        private readonly DependencyProcessor _dependencyProcessor;

        public DependencyProcessorTests()
        {
            _dependencyService = new Mock<IDependencyService>();
            _dependencyProcessor = new DependencyProcessor(_dependencyService.Object);
        }

        [Fact]
        public void Proccess_ShouldFailWhenNoInstructions()
        {
            // arrange
            var instructions = new string[0];

            // act
            var result = _dependencyProcessor.Process(instructions);

            // assert
            Assert.False(result);
        }


        [Fact]
        public void Proccess_ShouldFailWhenChecksAreBad()
        {
            // arrange
            var instructions = new string[1];
            _dependencyService.Setup(x => x.CheckDependencies(It.IsAny<ProgramDependencies>()))
                .Returns(false);

            // act
            var result = _dependencyProcessor.Process(instructions);

            // assert
            Assert.False(result);
        }


        [Fact]
        public void Proccess_ShouldPassWhenChecksAreGood()
        {
            // arrange
            var instructions = new string[1];
            _dependencyService.Setup(x => x.CheckDependencies(It.IsAny<ProgramDependencies>()))
                .Returns(true);

            // act
            var result = _dependencyProcessor.Process(instructions);

            // assert
            Assert.True(result);
        }
    }
}
