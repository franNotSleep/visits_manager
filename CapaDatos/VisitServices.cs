using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using Microsoft.Win32;
using Npgsql;

namespace DataLayer
{
    public class VisitServices
    {

        private NpgsqlDataSource dataSource;

        public VisitServices()
        {
            InitializeDatabaseConnection();
        }
        private void InitializeDatabaseConnection()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=droide03;Database=visitsdb";
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
            this.dataSource = dataSource;
        }
        public async Task<Visit> CreateVisit(string name, string lastName, string fieldOfStudy, string email, int buildingId, DateTime entryTime, DateTime exitTime, string reason, string photoUrl)
        {
            Console.WriteLine(name);
            NpgsqlCommand cmd = dataSource.CreateCommand($"" +
                $"SELECT * " +
                $"FROM create_visit(" +
                $"'{name ?? "gabo"}'," +
                $"'{lastName}'," +
                $"'{fieldOfStudy}'," +
                $"'{email}'," +
                $"{buildingId}," +
                $"'{entryTime}'," +
                $"'{exitTime}'," +
                $"'{reason}'," +
                $"'{photoUrl}'" +
                $");");
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                int oid = reader.GetInt32(0);
                string ofirstName = reader.GetString(1);
                string olastName = reader.GetString(2);
                string ofieldOfStudy = reader.GetString(3);
                string oemail = reader.GetString(4);
                DateTime oentryTime = reader.GetDateTime(6);
                DateTime oexitTime =reader.GetDateTime(7);
                string oreason = reader.GetString(8);
                string ophotoURL = reader.GetString(9);

                Visit visit = new Visit(oid, ofirstName, olastName, ofieldOfStudy, oemail, oentryTime, oexitTime, oreason, ophotoURL, null);
                reader.Close();
                return visit;
            }

            reader.Close();
            return null;
        }
        public async Task<List<Visit>> GetAllVisits()
        {
            NpgsqlCommand cmd = dataSource.CreateCommand("SELECT * FROM get_all_visit()");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            List<Visit> visits = new List<Visit>();

            while (await reader.ReadAsync())
            {
                int visitId = reader.GetInt32(0);
                string name = reader.GetString(1);
                string lastName = reader.GetString(2);
                string fieldOfStudy = reader.GetString(3);
                string email = reader.GetString(4);
                DateTime entryTime = reader.GetDateTime(5);
                DateTime exitTime = reader.GetDateTime(6);
                string reason = reader.GetString(7);

                Visit visit = new Visit(visitId, name, lastName, fieldOfStudy, email, entryTime, exitTime, reason, null, null);
                visits.Add(visit);
            }

            reader.Close();
            return visits;
        }

    }
}
