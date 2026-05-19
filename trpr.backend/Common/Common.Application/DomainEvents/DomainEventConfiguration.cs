using System.Reflection;

namespace Common.Application.DomainEvents
{
    public class DecoratorConfiguration
    {
        public List<Type> Decorators { get; } = new();

        public DecoratorConfiguration AddDecorator(Type openGenericDecorator)
        {
            if (!openGenericDecorator.IsGenericTypeDefinition)
                throw new ArgumentException("Must be an open generic type");

            Decorators.Add(openGenericDecorator);
            return this;
        }
    }

    public class AssemblyConfiguration
    {
        public List<Assembly> Assemblies { get; } = new();

        public AssemblyConfiguration AddAssembly(Assembly assembly)
        {
            Assemblies.Add(assembly);
            return this;
        }
        public AssemblyConfiguration AddAssemblies(params Assembly[] assemblies)
        {
            Assemblies.AddRange(assemblies);
            return this;
        }
    }

    public class DomainEventConfiguration
    {
        public DecoratorConfiguration Decorators { get; set; } = new();
        public AssemblyConfiguration Assemblies { get; set; } = new();

        public DomainEventConfiguration AddPipeline(Type openGenericDecorator)
        {
            Decorators = Decorators.AddDecorator(openGenericDecorator);
            return this;
        }

        public DomainEventConfiguration AddAssembly(Assembly assembly)
        {
            Assemblies = Assemblies.AddAssembly(assembly);
            return this;
        }

        public DomainEventConfiguration AddAssemblies(params Assembly[] assemblies)
        {
            Assemblies = Assemblies.AddAssemblies(assemblies);
            return this;
        }
    }

}