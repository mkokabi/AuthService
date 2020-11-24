using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using UserServiceBase;
using Amazon.DynamoDBv2;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UserServiceDynamoRepository
{
    public class DynamoUserRepository : IUserRepository
    {
        // private static readonly string EndpointUrl = "http://localhost:8000";
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

        public async Task<int> UpsertUser(User user)
        {
            var userAttrs = new Dictionary<string, AttributeValue>
            {
                { "UserName", new AttributeValue(user.Username) },
                { "UserID", new AttributeValue { N = user.UserId.ToString() } },
                { "Password", new AttributeValue(user.Password.ToString()) },
                // { "CreatedDate", new AttributeValue { N = DateTimeToUnix(DateTime.Now).ToString()} }
                { "CreatedDate", new AttributeValue(DateTime.Now.ToString("u")) }
            };
            var response = await Client.PutItemAsync("Users", userAttrs);
            var statusCode = response.HttpStatusCode;
            if (statusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Failed with status code {statusCode}");
            }
            return user.UserId;
        }
    }
}
