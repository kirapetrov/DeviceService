using DeviceRepository.Common;

namespace DeviceService.QueryParameters;

public class SearchParameters
{
    public string? Name { get; set; }
    public object? Value { get; set; }
    public OperandType Operand { get; set; }

    internal DeviceRepository.Common.SearchParameters? GetRepositorySearchParameters()
    {
        return !string.IsNullOrWhiteSpace(Name)
            ? new DeviceRepository.Common.SearchParameters(Name, Operand, Value)
            : null;
    }
}