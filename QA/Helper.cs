using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public static class Helper
    {
        internal static string PropertyFromHash(HashEntry[] hashEntries, string propertyName)
        {
            var hashEntry = hashEntries.FirstOrDefault(x => x.Name == propertyName);
            if (hashEntry == default(HashEntry))
                throw new Exception($"Wrong type, property {propertyName} not in hash");

            return hashEntry.Value.ToString();
        }

        internal static Type FindTypeInAssembly(string typeName)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                if (type.FullName == typeName)
                    return type;
            }

            return null;
        }

        internal static string GetKeyFromType<T>()
        {
            var a = typeof(T).GetCustomAttribute<RedisKeyAttribute>();
            if (a == null)
            {
                throw new ArgumentException($"Type {typeof(T)} does not have the RedisKeyAttribute");
            }
            return a.Key;
        }
    }

    public class UserDoesNotExistException : Exception
    {
        public string Username { get; private set; }
        public UserDoesNotExistException(string username)
        {
            Username = username;
        }
    }
}
