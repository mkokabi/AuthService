using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using UserServiceBase;
using Amazon.DynamoDBv2;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UserServiceDynamoRepository
{
    public class DynamoUserRepository : IUserRepository
    {
        private AmazonDynamoDBClient Client;

        public DynamoUserRepository(IConfiguration configuration)
        {
            AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            var endpointUrl = configuration["UserRepository:EndpointUrl"];
            ddbConfig.ServiceURL = endpointUrl;
            try
            {
                Client = new AmazonDynamoDBClient(ddbConfig);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("FAILED to create a DynamoDBLocal client; " + ex.Message);
            }
        }
        public long DateTimeToUnix(DateTime MyDateTime)
        {
            TimeSpan timeSpan = MyDateTime - new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)timeSpan.TotalSeconds;
        }

        public async Task<User> GetUser(string username)
        {
            var query = new QueryRequest("Users");
            query.KeyConditionExpression = "UserName = :un";
            var userAttrs = new Dictionary<string, AttributeValue>
            {
                { ":un", new AttributeValue(username) },
            };
            query.ExpressionAttributeValues = userAttrs;
            var queryResult = await Client.QueryAsync(query);
            if (queryResult.Count == 0)
            {
                return null;
            }
            var firstUser = queryResult.Items[0];
            return new User
            {
                Username = firstUser["UserName"].S,
                Password = firstUser["Password"].S,
                SecondayPassword = firstUser.ContainsKey("SecondayPassword") ? firstUser["SecondayPassword"].S : null,
            };
        }

        public async Task<User[]> QueryUsers()
        {
            var scanResponse = await Client.ScanAsync("Users", new List<string> {
                "UserName", "Password", "SecondayPassword"});
            return scanResponse.Items.Select(u => new User
            {
                Username = u["UserName"].S,
                Password = u["Password"].S,
                SecondayPassword = u["SecondayPassword"].S,
            }).ToArray();
        }

        public async Task<int> UpsertUser(User user)
        {
            var userAttrs = new Dictionary<string, AttributeValue>
            {
                { "UserName", new AttributeValue(user.Username) },
                { "Password", new AttributeValue(user.Password.ToString()) },
                // { "CreatedDate", new AttributeValue { N = DateTimeToUnix(DateTime.Now).ToString()} }
                { "CreatedDate", new AttributeValue(DateTime.Now.ToString("u")) }
            };
            if (user.UserId.HasValue)
            {
                userAttrs.Add("UserID", new AttributeValue { N = user.UserId.ToString() });
            }
            if (!string.IsNullOrWhiteSpace(user.SecondayPassword))
            {
                userAttrs.Add("SecondayPassword", new AttributeValue(user.SecondayPassword));
            }
            var response = await Client.PutItemAsync("Users", userAttrs);
            var statusCode = response.HttpStatusCode;
            if (statusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Failed with status code {statusCode}");
            }
            return (int)statusCode;
        }
    }
}
