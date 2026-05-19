using System.Reflection;

namespace Modules.Conversations.Application;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}
