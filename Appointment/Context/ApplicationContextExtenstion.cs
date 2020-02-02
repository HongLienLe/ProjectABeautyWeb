using System;
using System.Collections.Generic;
using System.Linq;
using Appointment.Context;
using Appointment.Models;

namespace Appointment.Context
{
    public static class ApplicationContextExtenstion
    {
        public static void TreatmentSeedData(this ApplicationContext context)
        {
            if (context.Treatments.Any())
                return;

            var treatments = new List<Treatment>()
            {
                { new Treatment() { TreatmentName = "Fullset", TreatmentType = TreatmentType.Acrylic, Duration = 45, Price = 22 } },
                { new Treatment() { TreatmentName = "Infill", TreatmentType = TreatmentType.Acrylic, Duration = 45, Price = 16 } },
                { new Treatment() {TreatmentName = "Fullset", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 28 } },
                { new Treatment() {TreatmentName = "Infill", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 25 } },
                { new Treatment() {TreatmentName = "Gel Polish Express", TreatmentType = TreatmentType.GelPolish, Duration = 20, Price = 20 } },
                { new Treatment() {TreatmentName = "Gel Polish with Manicure", TreatmentType = TreatmentType.GelPolish, Duration = 45, Price = 25 } },
                { new Treatment() {TreatmentName = "Pedicure", TreatmentType = TreatmentType.NaturalNail, Duration = 45, Price = 22 } }
            };

            context.Treatments.AddRange(treatments);
            context.SaveChanges();

        }
    }
}
