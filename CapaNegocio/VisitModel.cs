using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class VisitModel
    {
        VisitServices visitServices;
        public VisitModel()
        {
            this.visitServices = new VisitServices();
        }

        public Task<Visit> CreateVisit(string name, string lastName, string fieldOfStudy, string email, int buildingId, DateTime entryTime, DateTime exitTime, string reason, string photoUrl)
        {
            return this.visitServices.CreateVisit(name, lastName, fieldOfStudy, email, buildingId, entryTime, exitTime, reason, photoUrl);
        }

        public Task<List<Visit>> GetAllVisits()
        {
            return this.visitServices.GetAllVisits();
        }
    }
}
