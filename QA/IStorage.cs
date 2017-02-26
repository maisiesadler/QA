using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public abstract class IKeyValueStorage
    {
        public abstract void SetValue(string key, string value);
        public abstract string GetValue(string key);
    }

    public class RedisStorage : IKeyValueStorage
    {
        private static RedisStorage _instance;
        public static RedisStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RedisStorage();
                return _instance;
            }
        }

        private LocalRedis _redisInstance;
        public RedisStorage()
        {
            _redisInstance = LocalRedis.Instance;
        }

        public IDatabase _database => _redisInstance.GetDatabase();

        public override string GetValue(string key)
        {
            return _database.StringGet(key);
        }

        public override void SetValue(string key, string value)
        {
            _database.StringSet(key, value);
        }

        public void SetAdd(string key, params string[] values)
        {
            foreach (var value in values)
            {
                _database.SetAdd(key, value);
            }
        }

        public IEnumerable<string> GetSetMembers(string key)
        {
            return _database.SetMembers(key).Select(m => m.ToString());
        }

        public void DeleteSetMemberAt(string key, string location)
        {
            _database.SetRemove(key, location);
        }

        public void AddHash(string key, params HashEntry[] values)
        {
            _database.HashSet(key, values);
        }

        public void AddHash(string key, List<HashEntry> values)
        {
            var hashes = new HashEntry[values.Count];
            for(int i = 0; i < values.Count; i++)
            {
                hashes[i] = values[i];
            }
            AddHash(key, hashes);
        }

        public void Delete(string key)
        {
            _database.KeyDelete(key);
        }

        public string GetHash(string key, string hashField)
        {
            return _database.HashGet(key, hashField);
        }

        public HashEntry[] GetAllHash(string key)
        {
            return _database.HashGetAll(key);
        }
    }
}
