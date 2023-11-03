namespace DeviceRepository.Common.Search;

public class SearchParameter
{
    public string? Name { get; set; }
    public object? Value { get; set; }
    public OperandType Operand { get; set; }    
}