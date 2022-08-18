using Amazon;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Bank.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Bank.Infra.Data.NoSql
{
    public class AwsDynamoDbClient : INoSqlClient
    {
        private readonly Amazon.DynamoDBv2.AmazonDynamoDBClient _singletonInstance;
        private readonly IOutputLogger _logger;
        private readonly string _awsAccessKey = "";
        private readonly string _awsAccessSecret = "";
        private readonly string _awsAccessRegion = "";
        private List<string> _tablesNames;
        private const int _waitTableCreationRetries = 3;

        public AwsDynamoDbClient(IConfiguration config,
                                 IOutputLogger logger)
        {
            _awsAccessKey = config["AppDynamoDbSettings:AccessKey"];
            _awsAccessSecret = config["AppDynamoDbSettings:SecretKey"];
            _awsAccessRegion = config["AppDynamoDbSettings:Region"];
            _logger = logger;

            if (_singletonInstance == null)
            {
                try
                {
                    _singletonInstance = new Amazon.DynamoDBv2.AmazonDynamoDBClient(_awsAccessKey, _awsAccessSecret, RegionEndpoint.GetBySystemName(_awsAccessRegion));

                    var tables = new ListTablesRequest();
                    var tablesResponse = _singletonInstance.ListTablesAsync(tables).Result;
                    _tablesNames = tablesResponse.TableNames;
                }
                catch
                {
                }
            }
        }

        public string GetAllSerialized(string tableName, string fieldName, string fieldValue)
        {
            Dictionary<string, KeysAndAttributes> data = new Dictionary<string, KeysAndAttributes>();

            KeysAndAttributes attr = new KeysAndAttributes();

            var dicKeys = new Dictionary<string, AttributeValue>();

            dicKeys.Add(fieldName, new AttributeValue(fieldValue));
            attr.Keys.Add(dicKeys);
            data.Add(tableName, attr);

            BatchGetItemRequest request = new BatchGetItemRequest(data);
            var response = _singletonInstance.BatchGetItemAsync(request);

            if (!response.IsCompletedSuccessfully) return null;

            var results = response.Result.Responses[tableName];

            if (results == null) return null;

            return JsonConvert.SerializeObject(results);

        }

        public async void CreateTable(string tableName)
        {

            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "Timestamp",
                        AttributeType = "N"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH" // Partition Key
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Timestamp",
                        KeyType = "Range" // Sort Key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                }
            };

            await _singletonInstance.CreateTableAsync(request);

            WaitUntilTableReady(tableName);

            _tablesNames.Add(tableName);
        }

        public void WaitUntilTableReady(string tableName)
        {
            string status = null;
            int loops = 0;

            do
            {
                Thread.Sleep(2000);
                try
                {
                    var res = _singletonInstance.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {

                }

                loops++;

            } while (status != "ACTIVE" && loops < _waitTableCreationRetries + 1);
            {
            }
        }

        public void InsertNoWait<T>(string tableName, T value) where T : class
        {
            new Task(() =>
            {
                if (!_tablesNames.Contains(tableName))
                    CreateTable(tableName);

                if (!_tablesNames.Contains(tableName))
                {
                    return;
                }

                PutItemRequest putRequest = new PutItemRequest(tableName, ToAttributeMap(value));
                _singletonInstance.PutItemAsync(putRequest);
            }).Start();
        }

        public async void Insert<T>(string tableName, T value) where T : class
        {
            if (!_tablesNames.Contains(tableName))
                CreateTable(tableName);

            if (!_tablesNames.Contains(tableName)) return;

            PutItemRequest putRequest = new PutItemRequest(tableName, ToAttributeMap(value));

            await _singletonInstance.PutItemAsync(putRequest);
        }

        private Dictionary<string, AttributeValue> ToAttributeMap<T>(T obj) where T : class
        {
            Dictionary<string, AttributeValue> convertedData = new Dictionary<string, AttributeValue>();

            object propValue;

            if (obj != null)
            {
                foreach (var propertyInfo in obj.GetType().GetProperties().OrderBy(prop => prop.Name))
                {

                    propValue = propertyInfo.GetValue(obj);

                    if (propValue == null)
                    {
                        propValue = "";
                    }

                    convertedData.Add(propertyInfo.Name, new AttributeValue(propValue.ToString()));
                }
            }

            return convertedData;
        }
    }
}
