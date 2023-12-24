namespace DeviceRepository.Common.Search;

public record SearchParameters(string Name, OperandType Operand, object? Value);