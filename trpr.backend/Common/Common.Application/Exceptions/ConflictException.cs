using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;
using Microsoft.AspNetCore.Http;

namespace Common.Application.Exceptions
{
    public class ConflictException(string code, params object[] args) : LocalizedHttpException(code, StatusCodes.Status409Conflict, args)
    {
        public string Code { get; } = code;
    }
}
