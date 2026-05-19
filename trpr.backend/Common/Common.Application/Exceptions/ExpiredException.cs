using Common.Domain;
using Microsoft.AspNetCore.Http;

namespace Common.Application.Exceptions;

public class ExpiredException(string code, params object[] args) : LocalizedHttpException(code, StatusCodes.Status410Gone, args)
{
    public string Code { get; } = code;
}
