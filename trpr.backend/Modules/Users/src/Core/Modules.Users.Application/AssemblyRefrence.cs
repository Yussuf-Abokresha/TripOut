using System.Reflection;

namespace Modules.Users.Application;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
