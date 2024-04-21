using DataLayer;
using EntityLayer;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ClassroomModel
    {

        private ClassroomService classroomService;
        public ClassroomModel(NpgsqlDataSource dataSource)
        {
            this.classroomService = new ClassroomService(dataSource);
        }

        public Task<Classroom> CreateClassroom(string name, int buildingId)
        {
            return this.classroomService.CreateClassroom(name, buildingId);
        }
    }
}
