using System.Reflection;
namespace Modules.Users.Presentation;

public static class AssemblyRefrence
{
    public static Assembly Assembly => typeof(AssemblyRefrence).Assembly;
}