using DeviceRepository.Common.Search;

namespace DeviceService.Common.QueryParameters;

public class SearchParameters
{
    public string? Name { get; set; }
    public object? Value { get; set; }
    public OperandType Operand { get; set; }

    internal DeviceRepository.Common.Search.SearchParameters? GetRepositorySearchParameters()
    {
        return !string.IsNullOrWhiteSpace(Name)
            ? new DeviceRepository.Common.Search.SearchParameters(Name, Operand, Value)
            : null;
    }
}