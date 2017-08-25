using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using ARMD.BLToolKitForSQLExpress;
using ARMD.DataContracts.ToStations.ReferenceData.TicketExemptions;
using BLToolkit.Mapping;
using BLToolkit.Mapping.Fluent;
using FastMember;
using Newtonsoft.Json;
using NLog;

namespace TestApp.DAL
{
    public class Class1
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private const string ConnectionString = "Data Source=localhost; Initial Catalog=ReferenceDatabase; user=sa;password=sa;";

        public void TestBulkInsert()
        {
            using (SqlConnection destinationConnection = new SqlConnection(ConnectionString))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
            {
                destinationConnection.Open();
                bulkCopy.DestinationTableName = "[dbo].[Exemptions]";
                Helper.SetCorrectMapping(bulkCopy);
                var table = GetDataTable();
                bulkCopy.WriteToServer(table);
            }
        }

        public void TestBulkInsertWithBLToolkit()
        {
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            using (Database db = new Database(ConnectionString))
            {
                //var exemptions = Helper.GetExemptionsFromFile(@"C:\Temp\DataTable.json");
                var table = SerializerHelper.Deserialize(@"C:\Temp\DataTable2.json");
                var exemptions = table.ToExemptions();
                db.BulkInsert(exemptions);
            }
        }

        public void TestSerializing(string fileName)
        {
            var dataTable = GetDataTable();
            SerializerHelper.Serialize(dataTable, fileName);

            var table = SerializerHelper.Deserialize(fileName);
            Console.WriteLine("Count: " + table.Rows.Count);
        }

        private DataTable GetDataTable()
        {
            var dataTable = new DataTable();
            Helper.SetColumns(dataTable);

            using (var reader = ObjectReader.Create(Helper.GetExemptions()))
            {
                dataTable.Load(reader);
            }
            return dataTable;
        }


    }

    public class Database : DbManagerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Database(string connectionString) : base(connectionString)
        {
            /*new FluentMap<Exemption>().TableName("Exemptions")
                .PrimaryKey(e => e.Code)
                .PrimaryKey(e => e.ActiveFromDate)
                .PrimaryKey(e => e.VersionId)
                .MapTo(Map.DefaultSchema);*/
        }

        public void BulkInsert<T>(IEnumerable<T> collection)
        {
            var bulkCopyOptions = SqlBulkCopyOptions.Default | SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.TableLock;
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)Connection, bulkCopyOptions, (SqlTransaction)Transaction))
            {
                bulkCopy.DestinationTableName = "Exemptions";
                var table = MappingSchema.MapListToDataTable(collection.ToList());
                foreach (DataColumn column in table.Columns)
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                //Helper.SetCorrectMapping(bulkCopy);
                bulkCopy.WriteToServer(table);
            }
        }
    }

    static class Helper
    {
        public static void SetColumns(DataTable dataTable)
        {
            dataTable.Columns.Add(new DataColumn("Code", typeof(int)));
            dataTable.Columns.Add(new DataColumn("ActiveFromDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("VersionId", typeof(int)));
            dataTable.Columns.Add(new DataColumn("ExemptionExpressCode", typeof(int)));
            dataTable.Columns.Add(new DataColumn("RegionOkatoCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ExemptionOrganizationCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ExemptionGroupCode", typeof(int)));
            dataTable.Columns.Add(new DataColumn("GVC", typeof(int)));
            dataTable.Columns.Add(new DataColumn("Percentage", typeof(int)));
            dataTable.Columns.Add(new DataColumn("ActiveTillDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("ChildTicketAvailable", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("MassRegistryAvailable", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("CppkRegistryBan", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("Leavy", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("RequireSnilsNumber", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("RequireSocialCard", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("IsRegionOnly", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("ChangedDateTime", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("DataChecksum", typeof(byte[])));
            dataTable.Columns.Add(new DataColumn("DeleteInVersionId", typeof(int)));
            dataTable.Columns.Add(new DataColumn("NewExemptionExpressCode", typeof(int)));
            dataTable.Columns.Add(new DataColumn("CanUpgradeCar", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("ExpressActiveFromDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("ExpressActiveTillDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("NotRequireDocumentNumber", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("NotRequireFIO", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("Presale7000WithPlace", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("Presale6000Once", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("Presale6000Abonement", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("CanReturnAbonement", typeof(bool)));
        }

        internal static void SetCorrectMapping(SqlBulkCopy bulkCopy)
        {
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Code", "Code"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ActiveFromDate", "ActiveFromDate"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("VersionId", "VersionId"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ExemptionExpressCode", "ExemptionExpressCode"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("RegionOkatoCode", "RegionOkatoCode"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Name", "Name"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ExemptionOrganizationCode", "ExemptionOrganizationCode"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ExemptionGroupCode", "ExemptionGroupCode"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("GVC", "GVC"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Percentage", "Percentage"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ActiveTillDate", "ActiveTillDate"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ChildTicketAvailable", "ChildTicketAvailable"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("MassRegistryAvailable", "MassRegistryAvailable"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CppkRegistryBan", "CppkRegistryBan"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Leavy", "Leavy"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("RequireSnilsNumber", "RequireSnilsNumber"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("RequireSocialCard", "RequireSocialCard"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IsRegionOnly", "IsRegionOnly"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ChangedDateTime", "ChangedDateTime"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DataChecksum", "DataChecksum"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DeleteInVersionId", "DeleteInVersionId"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NewExemptionExpressCode", "NewExemptionExpressCode"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CanUpgradeCar", "CanUpgradeCar"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ExpressActiveFromDate", "ExpressActiveFromDate"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ExpressActiveTillDate", "ExpressActiveTillDate"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NotRequireDocumentNumber", "NotRequireDocumentNumber"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("NotRequireFIO", "NotRequireFIO"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Presale7000WithPlace", "Presale7000WithPlace"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Presale6000Once", "Presale6000Once"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Presale6000Abonement", "Presale6000Abonement"));
            bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CanReturnAbonement", "CanReturnAbonement"));
        }

        public static IEnumerable<Exemption> GetExemptions()
        {
            return new[]
            {
                new Exemption
                {
                    Name = "Name",
                    Code = 481,
                    ChangedDateTime = DateTime.Now,
                    DataChecksum = Encoding.GetEncoding(1251).GetBytes("Some string"),
                    ActiveFromDate = DateTime.Today,
                    ActiveTillDate = new DateTime(2018, 12, 25),
                    CanReturnAbonement = true,
                    CanUpgradeCar = true,
                    ChildTicketAvailable = false,
                    CppkRegistryBan = false,
                    DeleteInVersionId = null,
                    ExemptionExpressCode = 12,
                    ExemptionGroupCode = 2,
                    ExemptionOrganizationCode = "DF",
                    Percentage = 25,
                    RegionOkatoCode = "3",
                    VersionId = 10
                }
            };
        }

        public static IEnumerable<Exemption> ToExemptions(this DataTable table)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                yield return new Exemption
                {
                    Code = row.Get<int>("Code"),
                    ActiveFromDate = row.Get<DateTime>("ActiveFromDate"),
                    VersionId = row.Get<int>("VersionId"),
                    ExemptionExpressCode = row.Get<int>("ExemptionExpressCode"),
                    RegionOkatoCode = row.Get<string>("RegionOkatoCode"),
                    Name = row.Get<string>("Name"),
                    ExemptionOrganizationCode = row.Get<string>("ExemptionOrganizationCode"),
                    ExemptionGroupCode = (int)row.Get<int>("ExemptionGroupCode"),
                    GVC = (int)row.Get<int>("GVC"),
                    Percentage = (int)row.Get<int>("Percentage"),
                    ActiveTillDate = row.Get<DateTime?>("ActiveTillDate"),
                    ChildTicketAvailable = (bool)row.Get<bool>("ChildTicketAvailable"),
                    MassRegistryAvailable = (bool)row.Get<bool>("MassRegistryAvailable"),
                    CppkRegistryBan = (bool)row.Get<bool>("CppkRegistryBan"),
                    Leavy = (bool)row.Get<bool>("Leavy"),
                    RequireSnilsNumber = (bool)row.Get<bool>("RequireSnilsNumber"),
                    RequireSocialCard = row.Get<bool>("RequireSocialCard"),
                    IsRegionOnly = row.Get<bool>("IsRegionOnly"),
                    ChangedDateTime = row.Get<DateTime>("ChangedDateTime"),
                    DataChecksum = row.Get<byte[]>("DataChecksum"),
                    DeleteInVersionId = row.Get<int?>("DeleteInVersionId"),
                    NewExemptionExpressCode = row.Get<int?>("NewExemptionExpressCode"),
                    CanUpgradeCar = row.Get<bool>("CanUpgradeCar"),
                    ExpressActiveFromDate = row.Get<DateTime?>("ExpressActiveFromDate"),
                    ExpressActiveTillDate = row.Get<DateTime?>("ExpressActiveTillDate"),
                    NotRequireDocumentNumber = row.Get<bool>("NotRequireDocumentNumber"),
                    NotRequireFIO = row.Get<bool>("NotRequireFIO"),
                    Presale7000WithPlace = row.Get<bool>("Presale7000WithPlace"),
                    Presale6000Once = row.Get<bool>("Presale6000Once"),
                    Presale6000Abonement = row.Get<bool>("Presale6000Abonement"),
                    CanReturnAbonement = row.Get<bool>("CanReturnAbonement")
                };
            }
        }

        public static T Get<T>(this DataRow row, string fieldName)
        {
            if (row[fieldName] is DBNull)
                return default(T);
            return (T)row[fieldName];
        }

        public static IEnumerable<Exemption> GetExemptionsFromFile(string fileName)
        {
            using (var fileStreamReader = File.OpenText(fileName))
            {
                return DeserializeJsonFromReader<Exemption[]>(fileStreamReader);
            }
        }

        public static T DeserializeJsonFromReader<T>(TextReader textReader)
        {
            using (JsonReader jsonReader = new JsonTextReader(textReader))
            {
                var serializer = new JsonSerializer { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified };

                return serializer.Deserialize<T>(jsonReader);
            }
        }
    }

    class SerializerHelper
    {
        public static byte[] StrToByteArray(string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string ByteArrayToStr(byte[] barr)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetString(barr, 0, barr.Length);
        }

        public static void Serialize(DataTable dt, string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(fileStream, dt);
            }
        }

        public static DataTable Deserialize(string fileName)
        {
            var fileStream = File.OpenRead(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            return (DataTable)serializer.Deserialize(fileStream);
        }
    }
}