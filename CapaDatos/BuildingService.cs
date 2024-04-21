using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using EntityLayer;
using Npgsql;

namespace DataLayer
{
    public class BuildingService
    {
        private NpgsqlDataSource dataSource;

        public BuildingService()
        {
            InitializeDatabaseConnection();
        }
        private void InitializeDatabaseConnection()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=droide03;Database=visitsdb";
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
            this.dataSource = dataSource;
        }
        public async Task<Building> CreateBuilding(string name)
        {
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT * FROM create_building('{name}')");
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(1);
                string newName = reader.GetString(0);
                Building building = new Building(id, newName, new List<Classroom>(), new List<Visit>());
                reader.Close();
                return building;
            }

            reader.Close();
            return null;
        }

        public async Task<List<Building>> GetBuildings()
        {
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT * FROM get_buildings()");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            List<Building> buildings = new List<Building>();

            while (await reader.ReadAsync())
            {
                int buildingId = reader.GetInt32(2);
                string buildingName = reader.GetString(3);

                Building building = buildings.FirstOrDefault(b => b.Id == buildingId);
                if (building == null)
                {
                    building = new Building(buildingId, buildingName, new List<Classroom>(), new List<Visit>());
                    buildings.Add(building);
                }

                string classroomName = reader.IsDBNull(0) ? null : reader.GetString(0);
                int? classroomId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);



                if (classroomId != null)
                {
                    building.Classrooms.Add(new Classroom(classroomId ?? 0, classroomName, building));
                }
                
                if (!reader.IsDBNull(4))
                {

                    string firstName = reader.GetString(4);
                    string lastName = reader.GetString(5);
                    string fieldOfStudy = reader.GetString(6);
                    string email = reader.GetString(7);
                    DateTime entryTime = reader.GetDateTime(8);
                    DateTime exitTime = reader.GetDateTime(9);
                    string reason = reader.GetString(10);
                    
                    string photos = reader.IsDBNull(11) ? null :reader.GetString(11);
                    int visitId = reader.GetInt32(12);
                    building.Visits.Add(new Visit(visitId, firstName, lastName, fieldOfStudy, email,entryTime, exitTime, reason, photos, building));
                }
            }
            reader.Close();
            return buildings;
        }
        public async Task<Building> GetBuildingById(int id)
        {
            NpgsqlCommand cmd = this.dataSource.CreateCommand($"SELECT * FROM get_building_by_id({id})");
            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                int buildingId = reader.GetInt32(0);
                string buildingName = reader.GetString(1);

                Building building = new Building(buildingId, buildingName, null, null);
                return building;
            }

            reader.Close();
            return null;
        }
    }
}
