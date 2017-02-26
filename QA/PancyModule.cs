using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public class PancyModule : NancyModule
    {
        public PancyModule()
        {
            var qpToRedis = new QuestionPageToRedis();

            Options["/{catchAll*}"] = parameters =>
            {
                this.EnableCors();
                return new Response { StatusCode = HttpStatusCode.Accepted };
            };

            Get["/questionPages"] = p =>
            {
                this.EnableCors();
                Logger.Debug("Get[questionPages]");
                var otr = qpToRedis.GetAll().ToList();
                Logger.Debug("Got: " + otr.Count);
                
                return otr;
            };

            Get["/latest"] = p =>
            {
                this.EnableCors();
                Logger.Debug("Get[latest]");
                var latest = qpToRedis.GetLatest().ToList();
                return latest;
            };

            Put["/save"] = p =>
            {
                this.EnableCors();
                Logger.Debug("Post[save]");
                var questionPage = this.Bind<QuestionPage>();
                var msg = questionPage.id == null ? "Adding" : "Updating " + questionPage.id;
                Logger.Debug(msg + ": " + questionPage.question.title + " from user " + questionPage.question.user);
                var id = qpToRedis.AddOrUpdate(questionPage);
                return id.ToString();
            };

            Put["/saveAll"] = p =>
            {
                this.EnableCors();
                Logger.Debug("Put[saveAll]");
                var questionPages = this.Bind<List<QuestionPage>>();
                Logger.Debug($"Saving {questionPages.Count} pages");
                foreach (var qp in questionPages)
                {
                    qpToRedis.AddOrUpdate(qp);
                }
                return "";
            };

            Put["/qp"] = p =>
            {
                this.EnableCors();
                Logger.Debug("Delete[qp]");
                var questionPage = this.Bind<QuestionPage>();

                if (questionPage.id != null)
                {
                    Logger.Debug("Deleting " + questionPage.id);
                    qpToRedis.Delete((int)questionPage.id);
                }
                return "";
            };
        }
    }
}
