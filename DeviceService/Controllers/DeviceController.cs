using Microsoft.AspNetCore.Mvc;
using DeviceRepository.Repositories.Interfaces;
using DeviceService.Models;
using DeviceRepository.Common.Page;

namespace DeviceService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceController(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> GetDevices(
        [FromQuery] PageInfo pageInfo)    
    {
        var result = await _deviceRepository.GetAsync(pageInfo).ConfigureAwait(false);
        return Ok(result.Select(DeviceHelper.GetDevice).ToArray());
    }

    [HttpGet("{identifier}")]
    public async Task<ActionResult<Device>> GetDevice(long identifier)
    {
        var result = await _deviceRepository
            .GetAsync(identifier)
            .ConfigureAwait(false);

        return result != null ? Ok(result.GetDevice()) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Device>> PostDevice(Device device)
    {
        //TODO need rework method
        var result = await _deviceRepository
            .AddAsync(device.ToModel())
            .ConfigureAwait(false);

        var newDevice = new Device(
            result,
            device.Name,
            device.IpAddress);

        return result > 0
            ? CreatedAtAction(nameof(GetDevice), new { identifier = result }, newDevice)
            : BadRequest();
    }

    [HttpPatch("{identifier}")]
    public async Task<IActionResult> PatchDevice(long identifier, Device device)
    {
        var result = await _deviceRepository
            .UpdateAsync(identifier, device.ToModel())
            .ConfigureAwait(false);

        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{identifier}")]
    public async Task<IActionResult> DeleteDevice(long identifier)
    {
        var result = await _deviceRepository
            .DeleteAsync(identifier)
            .ConfigureAwait(false);

        return result ? NoContent() : BadRequest();
    }
}