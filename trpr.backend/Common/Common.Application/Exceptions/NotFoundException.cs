using Common.Domain;
using Microsoft.AspNetCore.Http;

namespace Common.Application.Exceptions
{
    public class NotFoundException(string code, params object[] args) : LocalizedHttpException(code, StatusCodes.Status404NotFound, args)
    {
        public string Code { get; init; } = code;
    }
}
