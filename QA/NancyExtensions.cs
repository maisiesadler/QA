using Nancy;

namespace QA
{
    public static class NancyExtensions
    {
        public static void EnableCors(this NancyModule module)
        {
            module.After.AddItemToEndOfPipeline(x =>
            {
                x.Response.WithHeader("Access-Control-Allow-Origin", "*");
                x.Response.WithHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, PATCH, OPTIONS");
                x.Response.WithHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization");
            });
        }
    }
}
