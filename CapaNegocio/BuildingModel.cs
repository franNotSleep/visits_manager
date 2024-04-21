using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Dynamic;
using DataLayer;
using EntityLayer;

namespace BussinessLayer
{
    public class BuildingModel
    {
        private BuildingService buildingService;
        public BuildingModel()
        {
            buildingService = new BuildingService();
        }

        public Task<Building> Create(string name)
        {
            return buildingService.CreateBuilding(name);
        }

        public Task<List<Building>> GetBuildings()
        {
            return buildingService.GetBuildings();
        }
    }
}
