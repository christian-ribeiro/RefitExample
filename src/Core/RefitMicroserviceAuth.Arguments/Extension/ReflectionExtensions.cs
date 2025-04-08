using System.Reflection;

namespace RefitMicroserviceAuth.Arguments.Extension;

public static class ReflectionExtensions
{
    public static T? GetCustomAttributeFromHierarchy<T>(this Type type) where T : Attribute
    {
        return GetAllInterfacesIncludingSelf(type)
            .Select(i => i.GetCustomAttribute<T>(inherit: false))
            .FirstOrDefault(attr => attr is not null);
    }

    private static IEnumerable<Type> GetAllInterfacesIncludingSelf(Type type)
    {
        var visited = new HashSet<Type>();
        var stack = new Stack<Type>();
        stack.Push(type);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (visited.Add(current))
            {
                yield return current;

                foreach (var iface in current.GetInterfaces())
                    stack.Push(iface);
            }
        }
    }
}