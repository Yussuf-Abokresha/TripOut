using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;
using Microsoft.AspNetCore.Http;

namespace Common.Application.Exceptions;

public class NotAuthorizedException(string code, params object[] args) : LocalizedHttpException(code, StatusCodes.Status401Unauthorized, args)
{
    public string Code { get; private init; } = code;
}
