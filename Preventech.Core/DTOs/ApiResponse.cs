using System;

namespace Preventech.Core.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public override string ToString()
    {
        return $"Success: {Success}, Message: {Message}, Data: {Data}";
    }
}
