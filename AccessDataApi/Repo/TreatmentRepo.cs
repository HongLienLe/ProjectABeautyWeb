using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Data;
using AccessDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessDataApi.Repo
{
    public class TreatmentRepo
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
            {
                return treatmentList;
            }

            return null;
        }

        public Treatment GetTreatment(int treatmentId)
        {
            var treatment = _context.Treatments.First(x => x.TreatmentId == treatmentId);

            if(treatment != null)
            {
                return treatment;
            }

            return null;
        }

        public void AddTreatment(Treatment treatment)
        {
            using(var context = _context)
            {
                _context.Treatments.Add(treatment);
                _context.SaveChanges();

            }
        }

        public void UpdateTreatment(int id, Treatment treatment)
        {
            using (var context = _context)
            {

                var oldTreatment = context.Treatments.First(x => x.TreatmentId == id);

                oldTreatment.TreatmentName = treatment.TreatmentName;
                oldTreatment.Price = treatment.Price;
                oldTreatment.Duration = treatment.Duration;

                context.SaveChanges();
            }
        }

        //Delete
        public void DeleteTreatment(int id)
        {
            using(var context = _context)
            {
                var treatment = new Treatment() { TreatmentId = id };

                context.Entry(treatment).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }
    }
}
