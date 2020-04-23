using Amazon;
using Amazon.IonDotnet;
using Amazon.IonDotnet.Builders;
using Amazon.IonDotnet.Tree;
using Amazon.QLDB.Driver;
using Amazon.QLDBSession;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using static Amazon.Internal.RegionEndpointProviderV2;
using RegionEndpoint = Amazon.RegionEndpoint;

namespace AmazonQLDB.ServiceCore
{
    public class ConnectionService
    {
        private readonly BasicAWSCredentials awsCredentials;

        public ConnectionService()
        {
            this.awsCredentials = new BasicAWSCredentials(Global.AccessKey, Global.SecretKey);
        }
        public ConnectionService(string AccessKey, string SecretKey)
        {
            this.awsCredentials = new BasicAWSCredentials(AccessKey, SecretKey);
        }

        public List<List<KeyValuePair<string, string>>> ReadData(string statement)
        {
            try
            {
                IQldbDriver driver = this.GetDriver();
                new AmazonQLDBSessionConfig().RegionEndpoint = Global.Region;
                using (IQldbSession session = driver.GetSession())
                    return this.BuildData(session.Execute(statement, (List<IIonValue>)null, (Action<int>)null));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<List<KeyValuePair<string, string>>> WriteData(
          string tableName,
          string statement)
        {
            string documentID = string.Empty;
            try
            {
                IQldbDriver driver = this.GetDriver();
                new AmazonQLDBSessionConfig().RegionEndpoint = Global.Region;
                List<List<KeyValuePair<string, string>>> keyValuePairListList;
                using (IQldbSession session = driver.GetSession())
                {
                    keyValuePairListList = this.BuildData(session.Execute(statement, (List<IIonValue>)null, (Action<int>)null));
                    keyValuePairListList[0].ForEach((Action<KeyValuePair<string, string>>)(x =>
                    {
                        if (string.IsNullOrEmpty(x.Key) || !(x.Key.ToLower() == "documentid"))
                            return;
                        documentID = x.Value;
                    }));
                }
                if (!string.IsNullOrEmpty(documentID))
                {
                    string[] strArray = new string[9];
                    strArray[0] = "SELECT * from history(";
                    strArray[1] = tableName;
                    strArray[2] = ", `";
                    DateTime dateTime1 = DateTime.UtcNow;
                    dateTime1 = dateTime1.AddMinutes(-1.0);
                    strArray[3] = dateTime1.ToString("o");
                    strArray[4] = "`, `";
                    DateTime dateTime2 = DateTime.UtcNow;
                    dateTime2 = dateTime2.AddSeconds(0.7);
                    strArray[5] = dateTime2.ToString("o");
                    strArray[6] = "`) AS h WHERE h.metadata.id = '";
                    strArray[7] = documentID;
                    strArray[8] = "'";
                    string statement1 = string.Concat(strArray);
                    keyValuePairListList.AddRange((IEnumerable<List<KeyValuePair<string, string>>>)this.ReadData(statement1));
                }
                return keyValuePairListList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private IQldbDriver GetDriver()
        {
            AmazonQLDBSessionConfig sessionConfig = new AmazonQLDBSessionConfig();
            sessionConfig.RegionEndpoint = RegionEndpoint.USEast1;
            return (IQldbDriver)PooledQldbDriver.Builder().WithAWSCredentials((AWSCredentials)this.awsCredentials).WithQLDBSessionConfig(sessionConfig).WithLedger(Global.LedgerName).Build();
        }
        private List<List<KeyValuePair<string, string>>> BuildData(IResult result)
        {
            List<List<KeyValuePair<string, string>>> keyValuePairListList = new List<List<KeyValuePair<string, string>>>();
            IEnumerator<IIonValue> enumerator = result.GetEnumerator();
            while (enumerator.MoveNext())
            {
                List<KeyValuePair<string, string>> document = new List<KeyValuePair<string, string>>();
                using (IIonReader reader = IonReaderBuilder.Build(enumerator.Current, new ReaderOptions()))
                {
                    IonType type;
                    while ((type = reader.MoveNext()) != IonType.None)
                        ConnectionService.buildData1(document, reader, type);
                }
                keyValuePairListList.Add(document);
            }
            return keyValuePairListList;
        }

        private static IonType buildData1(
          List<KeyValuePair<string, string>> document,
          IIonReader reader,
          IonType type)
        {
            if (type == IonType.Struct)
            {
                reader.StepIn();
                while ((type = reader.MoveNext()) != IonType.None)
                {
                    switch (type)
                    {
                        case IonType.Bool:
                            document.Add(new KeyValuePair<string, string>(reader.CurrentFieldName, Convert.ToString(reader.BoolValue())));
                            continue;
                        case IonType.Int:
                            document.Add(new KeyValuePair<string, string>(reader.CurrentFieldName, Convert.ToString(reader.IntValue())));
                            continue;
                        case IonType.Decimal:
                            document.Add(new KeyValuePair<string, string>(reader.CurrentFieldName, Convert.ToString((object)reader.DecimalValue())));
                            continue;
                        case IonType.Timestamp:
                            document.Add(new KeyValuePair<string, string>(reader.CurrentFieldName, Convert.ToString((object)reader.TimestampValue())));
                            continue;
                        case IonType.String:
                            document.Add(new KeyValuePair<string, string>(reader.CurrentFieldName, reader.StringValue()));
                            continue;
                        case IonType.Struct:
                            int num = (int)ConnectionService.buildData1(document, reader, type);
                            continue;
                        default:
                            continue;
                    }
                }
                reader.StepOut();
            }
            return type;
        }
    }
}
