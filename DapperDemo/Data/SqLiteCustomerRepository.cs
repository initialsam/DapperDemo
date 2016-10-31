using System.IO;
using System.Linq;
using Dapper;
using SQLiteDemo.Model;
using System;
using System.Data.SQLite;

namespace SQLiteDemo.Data
{
    public class SqLiteCustomerRepository
    {
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + "\\SimpleDb.sqlite"; }
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }

        public SqLiteCustomerRepository()
        {
            if (!File.Exists(DbFile))
            {
                CreateDatabase();
            }
        }

        public void DapperDemo()
        {
            using (var cnn = SimpleDbConnection())
            {
                var sql = @"
                    INSERT INTO Customer 
                    ( FirstName, LastName, DateOfBirth ) 
                    VALUES 
                    ( @FirstName, @LastName, @DateOfBirth );
                    select last_insert_rowid()
                ";
                var customer = new Customer
                {
                    FirstName = "Sergey",
                    LastName = "Maskalik",
                    DateOfBirth = DateTime.Now
                };
                cnn.Open();
                var Id1 = cnn.Query<long>(sql, customer).First();
                var Id2 = cnn.QueryFirst<long>(sql, customer);
                var Id3 = cnn.QueryFirstOrDefault<long>(sql, customer);
                var Id4 = cnn.QuerySingle<long>(sql, customer);
                var Id5 = cnn.QuerySingleOrDefault<long>(sql, customer);

                sql = @"
                    UPDATE  Customer 
                    SET     FirstName = @Data
                ";

                var updateRow = cnn.Execute(sql,new {Data="ABC" });

                sql = @"
                    SELECT  COUNT(*) 
                    FROM    Customer;

                    SELECT  Id, FirstName, LastName, DateOfBirth
                    FROM    Customer
                    WHERE   Id = @Id
                ";
                var p = new DynamicParameters();
                p.Add("@Id", 1);

                var gridReader = cnn.QueryMultiple(sql,p);
                var totalCount1 = gridReader.ReadSingle<int>();
                var result1 = gridReader.ReadSingleOrDefault<Customer>();
                gridReader.Dispose();

                sql = @"
                    DELETE FROM Customer 
                    WHERE   Id > @Id
                ";

                var deleteRow = cnn.Execute(sql, new { Id = 0});

                sql = @"
                    SELECT  Id, FirstName, LastName, DateOfBirth
                    FROM    Customer
                ";
                var list = cnn.Query<Customer>(sql);
                var notNull = list.Count() == 0;
            }
        }

        private static void CreateDatabase()
        {
            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"create table Customer
                      (
                         ID                                  integer primary key AUTOINCREMENT,
                         FirstName                           varchar(100) not null,
                         LastName                            varchar(100) not null,
                         DateOfBirth                         datetime not null
                      )");
            }
        }
    }
}
