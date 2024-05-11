using System.ComponentModel.DataAnnotations;

namespace Service.Base.Settings;

public enum OpenTelemetryExportProtocol
{
    HTTP, GRPC
}

public class OpenTelemetrySettings
{
    [Required]
    public required string GlobalExportEndpoint { get; set; }

    [Required]
    [EnumDataType(typeof(OpenTelemetryExportProtocol))]
    public OpenTelemetryExportProtocol GlobalExportProtocol { get; set; }

    public bool UseConsoleExporter { get; set; } = false;
}