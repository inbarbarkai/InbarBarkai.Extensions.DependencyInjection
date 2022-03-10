using System;
using System.Reflection;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    public interface IConstrcutorFinder
    {
        ConstructorInfo Find(Type type);
    }
}
