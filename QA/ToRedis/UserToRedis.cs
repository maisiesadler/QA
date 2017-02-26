using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public class UserToRedis : ObjectToJsonRedis
    {
        public User AddUser(User obj)
        {
            obj.userkey = Guid.NewGuid().ToString();

            var hashes = new List<HashEntry>();
            hashes.Add(new HashEntry(__objType, obj.GetType().FullName));
            hashes.Add(new HashEntry(value, JsonConvert.SerializeObject(obj)));

            if (RedisStorage.Instance.GetSetMembers("users").Contains(obj.name))
            {
                return null;
            }

            RedisStorage.Instance.SetAdd("users", obj.name);
            RedisStorage.Instance.AddHash(obj.name, hashes);
            return obj;
        }

        public User UpdateUserGuid(User obj)
        {
            obj.userkey = Guid.NewGuid().ToString();

            var u = GetUser(obj.name);

            if (u.password == obj.password)
            {
                var hashes = new List<HashEntry>();
                hashes.Add(new HashEntry(__objType, obj.GetType().FullName));
                hashes.Add(new HashEntry(value, JsonConvert.SerializeObject(obj)));

                RedisStorage.Instance.AddHash(obj.name, hashes);
                return obj;
            }

            return null;
        }

        public User GetUser(string username)
        {
            var c = RedisStorage.Instance.GetAllHash(username);
            if (c.Length == 0)
            {
                throw new UserDoesNotExistException(username);
            }
            var hashMe = JsonConvert.DeserializeObject<User>(Helper.PropertyFromHash(c, value));
            return hashMe;
        }
    }
}
