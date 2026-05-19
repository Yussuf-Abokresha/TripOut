using System.Reflection;
namespace Modules.Conversations.Infrastructure;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
