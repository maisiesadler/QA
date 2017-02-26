using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public class QuestionPageToRedis : ObjectToJsonRedis
    {
        public int AddOrUpdate(QuestionPage obj)
        {
            var key = Helper.GetKeyFromType<QuestionPage>();

            if (obj.id == null)
                obj.id = GetAvailableId(key);

            var hashes = new List<HashEntry>();
            hashes.Add(new HashEntry(__objType, obj.GetType().FullName));
            hashes.Add(new HashEntry(value, JsonConvert.SerializeObject(obj)));
            var k = key + ":" + obj.id;
            RedisStorage.Instance.SetAdd(key, obj.id.ToString());
            RedisStorage.Instance.AddHash(k, hashes);
            return (int)obj.id;
        }

        public void Delete(int id)
        {
            var key = Helper.GetKeyFromType<QuestionPage>();
            RedisStorage.Instance.Delete(key + ":" + id);
            RedisStorage.Instance.DeleteSetMemberAt(key, id.ToString());
        }

        public void Delete(string key)
        {
            var parts = key.Split(':');
            RedisStorage.Instance.Delete(key);
            RedisStorage.Instance.DeleteSetMemberAt(parts[0], parts[1]);
        }

        public IEnumerable<QuestionPage> GetLatest()
        {
            var all = GetAll().ToList();
            var format = "ddd MMM dd yyyy HH:mm:ss 'GMT'K '(GMT Standard Time)'";
            return all.OrderByDescending(q => DateTime.ParseExact(q.question.date, format, CultureInfo.InvariantCulture)).Take(10);
        }

        public IEnumerable<QuestionPage> GetAll()
        {
            return GetAll<QuestionPage>();
        }

        public QuestionPage Get(int id)
        {
            return Get<QuestionPage>(id.ToString());
        }

        public QuestionPage Get(string id)
        {
            return Get<QuestionPage>(id);
        }
    }
}
