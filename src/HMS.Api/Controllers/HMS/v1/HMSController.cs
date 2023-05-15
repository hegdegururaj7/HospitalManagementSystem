using HMS.Service.Domain.Abstractions.Interfaces;
using HMS.Service.Domain.Abstractions.Models;

namespace HMS.Service.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/HMS")]
public class HMSController : Controller
{
    private readonly ICostEstimatorManager _costEstimatorManager;
    private readonly IIntelligentSchedulingManager _intelligentSchedulingManager;

    private readonly ILogger _logger;

    public HMSController(ICostEstimatorManager costEstimatorManager, IIntelligentSchedulingManager intelligentSchedulingManager,
                               ILogger<HMSController> logger)
    {
        _costEstimatorManager = costEstimatorManager;
        _intelligentSchedulingManager = intelligentSchedulingManager;
        _logger = logger;
    }

    [HttpPost("ExpensePredictor")]
    [ProducesResponseType(typeof(PatientViewResult), StatusCodes.Status200OK)]
    public  IActionResult ExpensePredictor([FromBody] PatientDetailsRequest patientDetailsRequest)
    {
        try
        {
            var response =  _costEstimatorManager.GetCostEstimator(patientDetailsRequest);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.InnerException, $"{nameof(ExpensePredictor)} threw an exception");
            throw;
        }
    }

    [HttpPost("RequestAppointment")]
    [ProducesResponseType(typeof(PatientViewResult), StatusCodes.Status200OK)]
    public IActionResult RequestAppointment([FromBody] PatientAppointmentRequest patientAppointmentRequest)
    {
        try
        {
            var response = _intelligentSchedulingManager.RequestAppointment(patientAppointmentRequest);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.InnerException, $"{nameof(RequestAppointment)} threw an exception");
            throw;
        }
    }
}
