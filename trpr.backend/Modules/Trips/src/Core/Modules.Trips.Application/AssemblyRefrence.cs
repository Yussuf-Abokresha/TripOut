using System.Reflection;

namespace Modules.Trips.Application;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
