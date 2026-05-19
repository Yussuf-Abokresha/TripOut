using Microsoft.Extensions.DependencyInjection;
using Modules.Users.Application.Interfaces;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Factories;

public class OtpHandlerFactory(IServiceProvider serviceProvider)
{
    public ITokenHandler CreateOtpHandler(TokenType tokenType)
    {
        return serviceProvider.GetRequiredKeyedService<ITokenHandler>(tokenType);
    }
}
