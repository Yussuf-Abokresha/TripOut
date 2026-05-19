using System.Reflection;
namespace Modules.Trips.Infrastructure;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
