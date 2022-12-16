using PSL.MicroserviceTemplate.Domain.Primitives;
using PSL.MicroserviceTemplate.Domain.Templates.GetTemplate;

namespace PSL.MicroserviceTemplate.Api.Controllers.Templates.v1;

[ApiController]
[ApiVersion(Routes.V1.VersionNumber)]
public class TemplatesController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public TemplatesController(IMediator mediator,
                               ILogger<TemplatesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet(Routes.V1.Templates.Get)]
    [ProducesResponseType(typeof(GetTemplateContract), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromRoute] TemplateId templateId)
    {
        try
        { 
            var client = _mediator.CreateRequestClient<GetTemplateRequest>();
            var response = await client.GetResponse<GetTemplateResponse>(new GetTemplateRequest(templateId));
            return Ok(new GetTemplateContract(response.Message.Template));
        }
        catch (RequestException ex)
        {
            _logger.LogError(ex.InnerException, $"{nameof(Get)} threw an exception");
            throw ex.InnerException;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(Get)} threw an exception");
            throw;
        }
    }
}
