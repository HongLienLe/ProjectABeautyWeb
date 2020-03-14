using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class TreatmentRepo : ITreatmentRepo
    {
        private ApplicationContext _context;

        public TreatmentRepo(ApplicationContext context)
        {
            _context = context;
        }

        public List<Treatment> GetTreatments()
        {
            return _context.Treatments.ToList();
        }

        public List<Treatment> GetTreatmentByType(TreatmentType treatmentType)
        {
            var treatmentList = _context.Treatments.Where(x => x.TreatmentType == treatmentType).ToList();

            if(treatmentList != null)
                return treatmentList;
            return null;
        }

        public Treatment GetTreatment(int id)
        {
            if (!DoesTreatmentExist(id))
                return null;

            var treatment = _context.Treatments.First(x => x.TreatmentId == id);
                return treatment;
        }

        public void AddTreatment(Treatment treatment)
        {
            _context.Treatments.Add(treatment);
            _context.SaveChanges();
        }

        public Treatment UpdateTreatment(int id, Treatment treatment)
        {
            if (!DoesTreatmentExist(id))
                return null;

            var oldTreatment = _context.Treatments.First(x => x.TreatmentId == id);

            oldTreatment.TreatmentName = treatment.TreatmentName;
            oldTreatment.Price = treatment.Price;
            oldTreatment.Duration = treatment.Duration;

            _context.SaveChanges();

            return GetTreatment(id);

        }

        public string DeleteTreatment(int id)
        {
            if (!DoesTreatmentExist(id))
                return null;

            _context.Remove(_context.Treatments.Single(x => x.TreatmentId == id));

            _context.SaveChanges();

            return "Successfully deleted treatment";
        }

        private bool DoesTreatmentExist(int id)
        {
            return _context.Treatments.Any(x => x.TreatmentId == id);
        }
    }
}
