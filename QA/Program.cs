
using Nancy;
using Nancy.Hosting.Self;
using Nancy.ModelBinding;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

namespace QA
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var nancy = new LocalNancyHost())
            {
                nancy.Start();
                Console.ReadLine();
            }
            Logger.Log("Stopped");
        }
    }
    
    public static class Logger
    {
        public static void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Warn(string message)
        {
            Console.WriteLine(message);
        }

        public static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public static void Write(string message)
        {
            var length = 100 - message.Length;
            Console.Write("\r" + message + new string(' ', length));
        }
    }

    [RedisKey("questionpage")]
    public class QuestionPage
    {
        public int? id { get; set; }
        public Question question { get; set; }
        public List<Comment> comments { get; set; }
    }

    public class Question
    {
        public string title { get; set; }
        public string description { get; set; }
        public User user { get; set; }
        public string date { get; set; }
    }

    public class Comment
    {
        public string description { get; set; }
        public List<Comment> comments { get; set; }
        public User user { get; set; }
    }

    [RedisKey("user")]
    public class User
    {
        public string name { get; set; }
        public string userkey { get; set; }
        public string password { get; set; }
    }
}
