using Microsoft.AspNetCore.Mvc;
using DeviceService.Models;
using DeviceService.Page;
using DeviceRepository.Repositories.Interfaces;
using DeviceRepository.Models.Interfaces;
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
        QueryParameters.QueryParameters? queryParameters)
    {
        var orderProperty = queryParameters?.OrderInfo?.Name ?? nameof(IDeviceModel.Name);
        var orderType = queryParameters?.OrderInfo?.OrderType ?? OrderType.Ascending;
        var pageNumber = queryParameters?.PageInfo?.Number ?? 1;
        var pageSize = queryParameters?.PageInfo?.Size ?? 20;
        var searchParameters = queryParameters?
            .SearchParameters?
            .Select(x => x.GetRepositorySearchParameters())
            .OfType<SearchParameters>()
            .ToArray();

        var result = await _deviceRepository
            .GetAsync(
                orderProperty,
                orderType,
                pageNumber,
                pageSize,
                searchParameters)
            .ConfigureAwait(false);

        return Ok(new PagedResult<Device>()
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalCount = result.TotalCount,
            PageCount = (ushort)Math.Ceiling((double)result.TotalCount / pageSize),
            Collection = result.Collection
                .Select(x => x.ToDevice(true))
                .ToArray()
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
        var result = await _deviceRepository
            .AddAsync(
                1, //TODO user identifier
                device.ToModel())
            .ConfigureAwait(false);

        return result is not null
            ? CreatedAtAction(
                nameof(GetDevice),
                new { identifier = result.Identifier },
                result.ToDevice())
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