using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Mappers
{
    public static class UserMapper
    {
        public static UserResponseDto ToResponseDto(this User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
    }
}
