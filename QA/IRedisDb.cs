using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public abstract class IRedisDb
    {
        protected string _config = "localhost";
        public ConnectionMultiplexer _cm;
        public ConnectionMultiplexer Cm
        {
            get
            {
                if (_cm == null)
                {
                    _cm = ConnectionMultiplexer.Connect(_config);
                }
                return _cm;
            }
        }

        public IDatabase GetDatabase()
        {
            return Cm.GetDatabase();
        }
    }

    public class LocalRedis : IRedisDb
    {
        private static LocalRedis _instance;
        public static LocalRedis Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LocalRedis();
                return _instance;
            }
        }
    }
}
