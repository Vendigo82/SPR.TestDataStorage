using AutoFixture.Xunit2;
using FluentAssertions;
using SPR.TestDataStorage.App.Models;
using SPR.TestDataStorage.App.Repositories;
using SPR.TestDataStorage.App.UseCases.SaveData;

namespace SPR.TestDataStorage.App.UseCases;

public class SaveDataUseCaseTests
{
    readonly Mock<IDataRepository> repositoryMock = new();
    readonly SaveDataUseCase target;

    public SaveDataUseCaseTests()
    {
        repositoryMock.SetReturnsDefault<Task<bool>>(Task.FromResult(true));
        target = new SaveDataUseCase(repositoryMock.Object);
    }

    [Theory, AutoData]
    public async Task WhenAllDataCorrect_ShouldBeSuccess(DataContent data)
    {
        // setup

        // action
        var result = await target.SaveDataAsync(data);

        // asserts
        result.Should().Be(SaveDataResult.Success);
        repositoryMock.Verify(f => f.SaveDataAsync(data), Times.Once);
    }

    [Theory, AutoData]
    public async Task WhenProjectNotFound_ShouldBeFailure(DataContent data)
    {
        // setup
        repositoryMock.Setup(f => f.IsProjectExistsAsync(data.Project)).ReturnsAsync(false);

        // action
        var result = await target.SaveDataAsync(data);

        // asserts
        result.Should().Be(SaveDataResult.ProjectNotFound);
        repositoryMock.Verify(f => f.SaveDataAsync(data), Times.Never);
    }

    [Theory, AutoData]
    public async Task WhenObjectTypeNotFound_ShouldBeFailure(DataContent data)
    {
        // setup
        repositoryMock.Setup(f => f.IsObjectTypeExistsAsync(data.ObjectType)).ReturnsAsync(false);

        // action
        var result = await target.SaveDataAsync(data);

        // asserts
        result.Should().Be(SaveDataResult.ObjectTypeNotFound);
        repositoryMock.Verify(f => f.SaveDataAsync(data), Times.Never);
    }
}
