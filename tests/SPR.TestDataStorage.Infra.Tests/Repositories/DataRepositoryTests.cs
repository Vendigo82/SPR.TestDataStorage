using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SPR.TestDataStorage.Infra.Data;
using SPR.TestDataStorage.Infra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPR.TestDataStorage.Infra.Repositories;

public partial class DataRepositoryTests
{
    readonly SPRTestDataStorageContext context;
    readonly DataRepository target;

    public DataRepositoryTests()
    {
        var optBuilder = new DbContextOptionsBuilder<SPRTestDataStorageContext>();
        optBuilder.
            UseInMemoryDatabase(Guid.NewGuid().ToString(), b => b.EnableNullChecks(false))
            .ConfigureWarnings(configurationBuilder => configurationBuilder.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        context = new SPRTestDataStorageContext(optBuilder.Options);

        target = new(context);
        InitDb();
    }

    private void InitDb()
    {
        context.Projects.Add(new ProjectModel { Id = Guid.NewGuid(), SystemName = "prj_test1" });
        context.ObjectTypes.Add(new ObjectTypeModel { Id = Guid.NewGuid(), SystemName = "obj_test1" });
        context.SaveChanges();
    }
}
