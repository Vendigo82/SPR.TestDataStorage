using Microsoft.AspNetCore.Mvc;
using SPR.TestDataStorage.App.UseCases.SaveData;
using System.ComponentModel.DataAnnotations;

namespace SPR.TestDataStorage.WebService.Controllers.DataController;

[Route("api/[controller]/{project}/{objectType}/{objectId}")]
[ApiController]
public class DataController : ControllerBase
{
    [FromRoute(Name = "project"), MinLength(1)]
    public string Project { get; set; } = null!;

    [FromRoute(Name = "objectType"), MinLength(1)]
    public string ObjectType { get; set; } = null!;

    [FromRoute(Name = "objectId"), MinLength(1)]
    public string ObjectIdentity { get; set; } = null!;


    [HttpPut("{name}")]
    public async Task<IActionResult> PutDataAsync(
        [FromRoute, MinLength(1)] string name,
        [FromServices] ISaveDataUseCase useCase)
    {
        var result = await useCase.SaveDataAsync(new App.Models.DataContent
        {
            DataName = name,
            Project = Project,
            ObjectType = ObjectType,
            ObjectIdentity = ObjectIdentity
        });
        if (result == SaveDataResult.Success)
            return Ok();

        
        // TODO: analize result errors
        return BadRequest();
    }
}
