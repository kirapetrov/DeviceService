using Microsoft.AspNetCore.Mvc;
using DeviceRepository.Repositories.Interfaces;
using DeviceService.Models;
using DeviceRepository.Common;

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
    public async Task<ActionResult<PagedResult<Device>>> GetDevices(
        QueryParameters? queryParameters)
    {        
        var result = await _deviceRepository
            .GetAsync(queryParameters)
            .ConfigureAwait(false);
        var deviceCollection = result.Results.Select(DeviceHelper.ToDevice).ToArray();
        return Ok(new PagedResult<Device>(deviceCollection)
        {
            CurrentPage = result.CurrentPage,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            PageCount = result.PageCount
        });
    }

    [HttpGet("{identifier}")]
    public async Task<ActionResult<Device>> GetDevice(long identifier)
    {
        var result = await _deviceRepository
            .GetAsync(identifier)
            .ConfigureAwait(false);

        return result != null ? Ok(result.ToDevice()) : NotFound();
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