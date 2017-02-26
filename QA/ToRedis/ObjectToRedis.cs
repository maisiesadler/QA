using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public class ObjectToJsonRedis
    {
        protected const string __objType = "__objType";
        protected const string value = "value";
        
        public T Get<T>(int id)
        {
            return Get<T>(id.ToString());
        }

        public T Get<T>(string id)
        {
            var key = Helper.GetKeyFromType<T>();
            var c = RedisStorage.Instance.GetAllHash(key + ":" + id);
            var hashMe = JsonConvert.DeserializeObject<T>(Helper.PropertyFromHash(c, value));
            return hashMe;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var key = Helper.GetKeyFromType<T>();
            var ids = RedisStorage.Instance.GetSetMembers(key);
            foreach (var id in ids)
            {
                yield return Get<T>(id);
            }
        }

        protected int GetAvailableId(string key)
        {
            var set = RedisStorage.Instance.GetSetMembers(key);
            var count = set.Count();
            while (set.Contains(count.ToString()))
            {
                count++;
            }
            return count;
        }
    }

    //public static class ObjectToJsonRedis2
    //{
    //    private const string __objType = "__objType";
    //    private const string value = "value";

    //    public static int AddOrUpdateQp(QuestionPage obj)
    //    {
    //        var key = Helper.GetKeyFromType<QuestionPage>();

    //        if (obj.id == null)
    //            obj.id = GetAvailableId(key);

    //        var hashes = new List<HashEntry>();
    //        hashes.Add(new HashEntry(__objType, obj.GetType().FullName));
    //        hashes.Add(new HashEntry(value, JsonConvert.SerializeObject(obj)));
    //        var k = key + ":" + obj.id;
    //        RedisStorage.Instance.SetAdd(key, obj.id.ToString());
    //        RedisStorage.Instance.AddHash(k, hashes);
    //        return (int)obj.id;
    //    }

    //    public static User AddUser(User obj)
    //    {
    //        obj.userkey = Guid.NewGuid().ToString();

    //        var hashes = new List<HashEntry>();
    //        hashes.Add(new HashEntry(__objType, obj.GetType().FullName));
    //        hashes.Add(new HashEntry(value, JsonConvert.SerializeObject(obj)));

    //        if (RedisStorage.Instance.GetSetMembers("users").Contains(obj.name))
    //        {
    //            return null;
    //        }

    //        RedisStorage.Instance.SetAdd("users", obj.name);
    //        RedisStorage.Instance.AddHash(obj.name, hashes);
    //        return obj;
    //    }

    //    public static User UpdateUserGuid(User obj)
    //    {
    //        obj.userkey = Guid.NewGuid().ToString();

    //        var u = GetUser(obj.name);

    //        if (u.password == obj.password)
    //        {
    //            var hashes = new List<HashEntry>();
    //            hashes.Add(new HashEntry(__objType, obj.GetType().FullName));
    //            hashes.Add(new HashEntry(value, JsonConvert.SerializeObject(obj)));

    //            RedisStorage.Instance.AddHash(obj.name, hashes);
    //            return obj;
    //        }

    //        return null;
    //    }

    //    public static User GetUser(string username)
    //    {
    //        var c = RedisStorage.Instance.GetAllHash(username);
    //        var hashMe = JsonConvert.DeserializeObject<User>(Helper.PropertyFromHash(c, value));
    //        return hashMe;
    //    }

    //    public static T Get<T>(int id)
    //    {
    //        return Get<T>(id.ToString());
    //    }

    //    public static T Get<T>(string id)
    //    {
    //        var key = Helper.GetKeyFromType<T>();
    //        var c = RedisStorage.Instance.GetAllHash(key + ":" + id);
    //        var hashMe = JsonConvert.DeserializeObject<T>(Helper.PropertyFromHash(c, value));
    //        return hashMe;
    //    }

    //    public static void DeleteQp(int id)
    //    {
    //        var key = Helper.GetKeyFromType<QuestionPage>();
    //        RedisStorage.Instance.Delete(key + ":" + id);
    //        RedisStorage.Instance.DeleteSetMemberAt(key, id.ToString());
    //    }

    //    public static void Delete(string key)
    //    {
    //        var parts = key.Split(':');
    //        RedisStorage.Instance.Delete(key);
    //        RedisStorage.Instance.DeleteSetMemberAt(parts[0], parts[1]);
    //    }

    //    public static IEnumerable<T> GetAll<T>()
    //    {
    //        var key = Helper.GetKeyFromType<T>();
    //        var ids = RedisStorage.Instance.GetSetMembers(key);
    //        foreach (var id in ids)
    //        {
    //            yield return Get<T>(id);
    //        }
    //    }

    //    private static int GetAvailableId(string key)
    //    {
    //        var set = RedisStorage.Instance.GetSetMembers(key);
    //        var count = set.Count();
    //        while (set.Contains(count.ToString()))
    //        {
    //            count++;
    //        }
    //        return count;
    //    }
    //}
}
