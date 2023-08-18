using DeviceRepository.Models.Interfaces;

namespace DeviceRepository.Models;

public abstract class ModelBase : IModelBase
{
    public long Identifier { get; set; }
}