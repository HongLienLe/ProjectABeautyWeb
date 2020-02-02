using System;
using System.Collections.Generic;
using System.Linq;
using AccessDataApi.Models;
using Itenso.TimePeriod;

namespace AccessDataApi.Data
{
    public static class ApplicationContextExtension
    {
        public static void CreateSeedData(this ApplicationContext context)
        {

            context.SaveChanges();

            if (context.Employees.Any())
                return;

            List<Employee> employees = new List<Employee>() {
                   new Employee() { EmployeName = "Staff2" },
            new Employee() { EmployeName = "Staff3" },
             new Employee() { EmployeName = "Staff4" }
             };

            List<Treatment> treatments = new List<Treatment>()
            {
              new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Acrylic, Duration = 45, Price = 16 },
              new Treatment() { TreatmentName = "Fullset", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 28 },
              new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 25 },
              new Treatment() { TreatmentName = "Gel Polish Express", TreatmentType = TreatmentType.GelPolish, Duration = 20, Price = 20 },
              new Treatment() { TreatmentName = "Gel Polish with Manicure", TreatmentType = TreatmentType.GelPolish, Duration = 45, Price = 25 },
              new Treatment() { TreatmentName = "Pedicure", TreatmentType = TreatmentType.NaturalNail, Duration = 45, Price = 22 }
            };


            context.Employees.AddRange(employees);
            context.Treatments.AddRange(treatments);
            context.SaveChanges();
        }
        
    }
}

