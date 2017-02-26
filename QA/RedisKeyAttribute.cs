using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public class RedisKeyAttribute : Attribute
    {
        public readonly string Key;
        public RedisKeyAttribute(string key)
        {
            Key = key;
        }
    }

    [RedisKey("hashme")]
    public class HashMe
    {
        [RedisKey("myproperty")]
        public string MyProperty { get; set; }
        [RedisKey("anotherproperty")]
        public string AnotherProperty { get; set; }
    }
}
