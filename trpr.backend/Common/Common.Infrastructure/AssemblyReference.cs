using System.Reflection;

namespace Common.Infrastructure;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
