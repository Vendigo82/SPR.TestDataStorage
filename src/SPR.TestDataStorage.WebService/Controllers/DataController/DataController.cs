using Microsoft.AspNetCore.Mvc;

namespace SPR.TestDataStorage.WebService.Controllers.DataController;

[Route("api/[controller]/{project}/{objectType}/{objectId}")]
[ApiController]
public class DataController : ControllerBase
{
    [FromRoute(Name = "project")]
    public string Project { get; set; } = null!;

    [FromRoute(Name = "objectType")]
    public string ObjectType { get; set; } = null!;

    [FromRoute(Name = "objectId")]
    public string ObjectIdentity { get; set; } = null!;


    [HttpPut("{name}")]
    public async Task<IActionResult> PutDataAsync([FromRoute] string name)
    {
        return Ok();
    }
}
