namespace PSL.MicroserviceTemplate.Api.Configuration;

public class OpenTelemetrySettings
{
    /// <summary>
    /// OpenTelemetry Backend address
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Defines whether OpenTelementy instrumentation is enabled
    /// </summary>
    public bool Enabled { get; set; }
}
