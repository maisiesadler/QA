using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA
{
    public class UserModule : NancyModule
    {
        private char[] notallowed = { ' ', '|' };

        public UserModule()
        {
            var userToRedis = new UserToRedis();

            Options["/{catchAll*}"] = parameters =>
            {
                this.EnableCors();
                return new Response { StatusCode = HttpStatusCode.Accepted };
            };

            Post["/register"] = p =>
            {
                this.EnableCors();
                Logger.Debug("Post[register]");
                var user = this.Bind<User>();

                if (string.IsNullOrEmpty(user.name))
                    return "false|username cannot be empty or null";

                if (string.IsNullOrEmpty(user.password))
                    return "false|password cannot be empty or null";

                if (user.password.Length < 4)
                {
                    return "false|really?";
                }

                foreach (var c in notallowed)
                {
                    if (user.name.Contains(c))
                        return "false|username not allowed chars";

                    if (user.password.Contains(c))
                        return "false|password not allowed chars";
                }

                var newUser = userToRedis.AddUser(user);
                if(newUser == null)
                {
                    return "false|username taken";
                }

                Logger.Info("New user registered " + newUser.name);

                return "true|" + newUser.userkey;
            };

            Post["/signin"] = p =>
            {
                this.EnableCors();
                Logger.Debug("Post[signin]");
                var user = this.Bind<User>();
                
                if (!string.IsNullOrEmpty(user.password))
                {
                    try
                    {
                        var us = userToRedis.UpdateUserGuid(user);
                        if (us == null)
                        {
                            return "False|incorrect password";
                        }
                        return "True|" + us.userkey;
                    }
                    catch(UserDoesNotExistException ex)
                    {
                        return $"False|user {ex.Username} does not exist";
                    }
                }

                if (!string.IsNullOrEmpty(user.userkey))
                {
                    var fromDb = userToRedis.GetUser(user.name);
                    if (fromDb.userkey == user.userkey)
                    {
                        return "True";
                    }
                    Logger.Warn("User tried logging in with incorrect user key");
                    return "False|please try logging in with password";
                }

                return "false";
            };
        }
    }   
}
