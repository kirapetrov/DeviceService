using DeviceRepository.Common.Models.Interfaces;

namespace DeviceRepository.Common.Models;

public abstract class ModelBase : IModelBase
{
    public long Identifier { get; set; }
}