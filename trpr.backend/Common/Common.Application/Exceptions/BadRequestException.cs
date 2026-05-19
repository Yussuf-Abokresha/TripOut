using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Common.Domain;
using Microsoft.AspNetCore.Http;

namespace Common.Application.Exceptions
{
    public class BadRequestException : LocalizedHttpException
    {
        public BadRequestException(string errorCode, params object[] args) : base(errorCode, StatusCodes.Status400BadRequest, args)
        {
        }
    }
}