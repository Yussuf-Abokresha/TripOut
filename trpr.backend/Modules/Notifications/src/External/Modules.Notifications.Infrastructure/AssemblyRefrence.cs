using System.Reflection;

namespace Modules.Notifications.Infrastructure;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
