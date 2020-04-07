using Fotografolio.Data.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<ApplicationUser> WithoutPasswords(this IEnumerable<ApplicationUser> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static ApplicationUser WithoutPassword(this ApplicationUser user)
        {
            //typeof(ApplicationUser).GetProperty
            if (user.GetType().GetProperty("Password") != null)
                user.GetType().GetProperty("Password").SetValue(user, null);
            if (user.GetType().GetProperty("PasswordHash") != null)
                //user.PasswordHash = null;
                user.GetType().GetProperty("PasswordHash").SetValue(user, null);
            return user;
        }

        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Any());
            }
            return null;
        }

        public static string GetHerokuPostgreSQLConnectionString(this IServiceCollection services)
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            //databaseUrl = "";
            if (String.IsNullOrEmpty(databaseUrl)) return null;

            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = Npgsql.SslMode.Prefer,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }

    }
}
