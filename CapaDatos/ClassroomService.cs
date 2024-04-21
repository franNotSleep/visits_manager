using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using Npgsql;

namespace DataLayer
{
    public class ClassroomService
    {

        private NpgsqlDataSource dataSource;
        private BuildingService buildingService;

        public ClassroomService(NpgsqlDataSource dataSource)
        {
            this.dataSource = dataSource;
            this.buildingService = new BuildingService();
        }
        public async Task<Classroom> CreateClassroom(string name, int buildingId)
        {
            NpgsqlCommand cmd = dataSource.CreateCommand($"SELECT * FROM create_classroom('{name}', {buildingId})");

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                int classroomId = reader.GetInt32(0);
                string classroomName = reader.GetString(1);

                Building building = await this.buildingService.GetBuildingById(buildingId);
                Classroom classroom = new Classroom(classroomId, classroomName, building);
                return classroom;
            }

            return null;
        }
    }
}
