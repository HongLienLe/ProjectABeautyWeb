using System;
using System.Collections.Generic;
using AccessDataApi.Models;

namespace AccessDataApi.Repo
{
    public interface ITreatmentRepo
    {
        public List<Treatment> GetTreatments();

        public List<Treatment> GetTreatmentByType(TreatmentType treatmentType);

        public Treatment GetTreatment(int id);

        public string AddTreatment(Treatment treatment);

        public Treatment UpdateTreatment(int id, Treatment treatment);

        public string DeleteTreatment(int id);




    }
}
