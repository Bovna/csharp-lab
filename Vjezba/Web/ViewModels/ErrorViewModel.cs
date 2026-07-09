namespace Vjezba.Web.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public string? OperationId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public bool ShowOperationId => !string.IsNullOrEmpty(OperationId);
}
