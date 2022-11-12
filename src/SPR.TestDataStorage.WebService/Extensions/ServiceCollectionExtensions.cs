using SPR.TestDataStorage.App.Repositories;
using SPR.TestDataStorage.App.UseCases.SaveData;
using SPR.TestDataStorage.Infra.Repositories;

namespace SPR.TestDataStorage.WebService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services) => services
        .AddTransient<IDataRepository, DataRepository>()
        .AddTransient<ISaveDataUseCase, SaveDataUseCase>();
}
