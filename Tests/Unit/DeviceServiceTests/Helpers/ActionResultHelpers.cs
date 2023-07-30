using Microsoft.AspNetCore.Mvc;

namespace DeviceServiceTests.Helpers;

public static class ActionResultHelpers
{
    public static TValue? GetResult<TResult, TValue>(
        this ActionResult<TValue> actionResult)
        where TResult : ObjectResult
    {
        if (actionResult?.Result is TResult result &&
            result.Value is TValue value)
        {
            return value;
        }

        return default;
    }
}