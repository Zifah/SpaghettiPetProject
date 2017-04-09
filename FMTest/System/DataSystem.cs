using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using static FMTest.Business.OrmSystem;

namespace FMTest.Business
{
    public class DataSystem
    {
        private string _connectionString = @"Data Source=C:\.NET Projects\FMDQTests.sqlite;Version=3;FailIfMissing=True;Foreign Keys=True;datetimeformat=CurrentCulture";
        private string _yorubaConnectionString = ConfigurationManager.ConnectionStrings["YorubaToneMarker"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

        public IList<T> GetAll<T>(string tableName, string connectionString = null) where T : class
        {
            string query = string.Format("SELECT * from {0}", tableName);

            connectionString = connectionString ?? _connectionString;

            var result = new OrmSystem().PopulateDataset<T>(query, connectionString);

            return result;
        }

        public IList<T> RunSelect<T>(string selectQuery, string connectionString = null) where T : class
        {
            connectionString = connectionString ?? _connectionString;
            var result = new OrmSystem().PopulateDataset<T>(selectQuery, connectionString);

            return result;
        }

        internal int SaveAppointment(AppointmentDto appointment)
        {
            string queryString = @"insert into Appointments(DoctorName,Patient,Comment,CreationTime) values (@doctor,@patient,@comment,CURRENT_TIMESTAMP);";

            return new OrmSystem().RunChangeQuery(queryString, _connectionString, new List<RdbmsParameter> {
                 new Business.RdbmsParameter { Name = "@doctor", Value = appointment.Doctor },
                 new Business.RdbmsParameter { Name = "@patient", Value = Convert.ToString(appointment.PatientId) },
                 new Business.RdbmsParameter { Name = "@comment", Value = appointment.Comment }
            });
        }

        internal List<ToneMarkingLog> GetProcessedYorubaWords()
        {
            string queryString = @"select Input, Output from ToneMarkingLogs where output is not null order by id desc limit 50;";
            return RunSelect<ToneMarkingLog>(queryString, _yorubaConnectionString).ToList();
        }

        public int SaveToneMarkingLog(string words)
        {
            string queryString = @"insert into ToneMarkingLogs(Input,ClientIp) values (@input,@clientIp);";

            return new OrmSystem().RunChangeQuery(queryString, _yorubaConnectionString, new List<RdbmsParameter> {
                 new Business.RdbmsParameter { Name = "@input", Value = words },
                 new Business.RdbmsParameter { Name = "@clientIp", Value = HttpContext.Current.Request.Headers["X-Forwarded-For"] ?? HttpContext.Current.Request.UserHostAddress
                 }
            });
        }

        internal int UpdateToneMarkingLog(ToneMarkingLog log)
        {
            string queryString = @"update ToneMarkingLogs set Output = @output, LastModificationTime = CURRENT_TIMESTAMP where Id = @id and ClientIp = @clientIp;";

            return new OrmSystem().RunChangeQuery(queryString, _yorubaConnectionString, new List<RdbmsParameter> {
                 new Business.RdbmsParameter { Name = "@output", Value = log.Output },
                 new Business.RdbmsParameter { Name = "@id", Value = Convert.ToString(log.Id) },
                 new Business.RdbmsParameter { Name = "@clientIp", Value = log.ClientIp }
            });
        }

        public IList<T> GetById<T>(string tableName, int id, string connectionString = null) where T : class
        {
            string query = string.Format("SELECT * from {0} where Id = @id", tableName);

            connectionString = connectionString ?? _connectionString;

            var result = new OrmSystem().PopulateDataset<T>(query, connectionString);

            return result;
        }

        public IList<T> GetBy<T>(string tableName, Dictionary<string, object> parameters) where T : class
        {
            string whereClause = "";

            for(int i = 0; i < parameters.Count; i++)
            {
                whereClause = string.Format("{0}{1}", whereClause, i == 0 ? " where ": " and ");
                whereClause = string.Format("{0}{1}=@{1}", whereClause, parameters.ElementAt(i).Key);
            }

            string query = string.Format("SELECT * from {0} {1}", tableName, whereClause);

            var result = new OrmSystem().PopulateDataset<T>(query, _connectionString, parameters.Select(x => new RdbmsParameter
            {
                Name = string.Format("@{0}", x.Key),
                Value = Convert.ToString(x.Value)
            }).ToList());

            return result;
        }

        public IList<Customer> GetCustomers(string name)
        {
            string query = "SELECT * from Customers where Name like @name";

            var result = new OrmSystem().PopulateDataset<Customer>(query, _connectionString, new List<RdbmsParameter>() { new RdbmsParameter {
                 Name = "@name",
                 Value = string.Format("%{0}%", name)
            } });

            return result;
        }

        public IList<Portfolio> GetPortfolio(string accountNumber)
        {
            string query = string.Format("SELECT PurchaseDate, PurchasePrice, CurrentPrice , Name CompanyName, Industry CompanyIndustry, Number AccountNumber from Portfolio P inner join Companies C on C.Id = P.CompanyId inner join Accounts A on A.Id = P.AccountId where Number = @number");

            var result = new OrmSystem().PopulateDataset<Portfolio>(query, _connectionString, new List<RdbmsParameter> { new RdbmsParameter{
                 Name = "@number",
                 Value = accountNumber
            } });

            return result;
        }

        public int SavePatient(Patient patient)
        {
            string queryString = @"insert into Patients(Name,Hospital,Age,Address,CreationTime) values (@name,@hospital,@age,@address,CURRENT_TIMESTAMP);";

            return new OrmSystem().RunChangeQuery(queryString, _connectionString, new List<RdbmsParameter> {
                 new Business.RdbmsParameter { Name = "@name", Value = patient.Name },
                 new Business.RdbmsParameter { Name = "@hospital", Value = patient.Hospital },
                 new Business.RdbmsParameter { Name = "@age", Value = Convert.ToString(patient.Age) },
                 new Business.RdbmsParameter { Name = "@address", Value = patient.Address }
            });
        }
    }
}