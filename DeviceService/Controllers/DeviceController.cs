using Microsoft.AspNetCore.Mvc;
using DeviceRepository.Interfaces;
using DeviceService.Models;

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
    public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
    {
        var result = await _deviceRepository.GetAsync().ConfigureAwait(false);
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
            .AddAsync(device.ToEntity())
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
            .UpdateAsync(identifier, device.ToEntity())
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