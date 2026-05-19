using System.Reflection;
namespace Modules.Users.Infrastructure;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
