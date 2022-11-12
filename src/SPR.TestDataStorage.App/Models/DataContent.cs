﻿using Newtonsoft.Json.Linq;

namespace SPR.TestDataStorage.App.Models;

public class DataContent
{
    public string Project { get; init; } = string.Empty;

    public string ObjectType { get; init; } = string.Empty;

    public string ObjectIdentity { get; init; } = string.Empty;

    public string DataName { get; init; } = string.Empty;

    public IDictionary<string, JToken> DataSections = new Dictionary<string, JToken>();
}
