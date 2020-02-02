//using System;
//using System.Collections.Generic;
//using Appointment.Models;

//namespace Appointment
//{
//    public class EmployeeData {

//        public Dictionary<int,Employee> ListOfEmployees;

//        public EmployeeData()
//        {
//            ListOfEmployees = CreateEmployeeList();
//        }

//        private Dictionary<int,Employee> CreateEmployeeList()
//        {
//            Dictionary<int, Employee> employees = new Dictionary<int, Employee>() {


//                { 1, new Employee()
//                    {
//                        Id = 1,
//                        EmployeName = "Employee1",
//                   //     Treatments = employeeTreatment1(),
//                        OffDays = new List<string> {"Monday","Tuesday","Friday"}
//                    }
//                },
//                { 2, new Employee()
//                    {
//                        Id = 2,
//                        EmployeName = "Employee2",
//                   //     Treatments = employeeTreatment2(),
//                        OffDays = new List<string>{"Tuesday","Wednsday","Thursday","Saturday"}
//                    }
//                },
//                { 3, new Employee()
//                    {
//                        Id = 3,
//                        EmployeName = "Employee3",
//                  //      Treatments = employeeTreatment3(),
//                        OffDays = new List<string>{"Friday","Saturday","Wednsday"}
//                    }
//                }
//            };

//            return employees;
            
//        }

//        //TreatmentList

//        //public List<Treatment> employeeTreatment1()
//        //{
//        //    List<Treatment> allTreatments = new List<Treatment>()
//        //    {
//        //        { new Treatment() { Id = 1, TreatmentName = "Fullset", TreatmentType = TreatmentType.Acrylic, Duration = 45, Price = 22 } },
//        //       { new Treatment() { Id = 2, TreatmentName = "Infill", TreatmentType = TreatmentType.Acrylic, Duration = 45, Price = 16 } },
//        //      {  new Treatment() { Id = 3, TreatmentName = "Fullset", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 28 } },
//        //      {  new Treatment() { Id = 4, TreatmentName = "Infill", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 25 } },
//        //     {  new Treatment() { Id = 5, TreatmentName = "Gel Polish Express", TreatmentType = TreatmentType.GelPolish, Duration = 20, Price = 20 } },
//        //    {   new Treatment() { Id = 6, TreatmentName = "Gel Polish with Manicure", TreatmentType = TreatmentType.GelPolish, Duration = 45, Price = 25 } },
//        //   {   new Treatment() { Id = 7, TreatmentName = "Pedicure", TreatmentType = TreatmentType.NaturalNail, Duration = 45, Price = 22 } }
//        //    };

//        //    return allTreatments;
//        //}

//        //public Dictionary<int, Treatment> employeeTreatment2()
//        //{
//        //    Dictionary<int, Treatment> allTreatments = new Dictionary<int, Treatment>()
//        //    {
//        //       {3,  new Treatment() { Id = 3, TreatmentName = "Fullset", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 28 } },
//        //      {4,  new Treatment() { Id = 4, TreatmentName = "Infill", TreatmentType = TreatmentType.Sns, Duration = 45, Price = 25 } },
//        //     {5,   new Treatment() { Id = 5, TreatmentName = "Gel Polish Express", TreatmentType = TreatmentType.GelPolish, Duration = 20, Price = 20 } },
//        //    {6,    new Treatment() { Id = 6, TreatmentName = "Gel Polish with Manicure", TreatmentType = TreatmentType.GelPolish, Duration = 45, Price = 25 } },
//        //   {7,     new Treatment() { Id = 7, TreatmentName = "Pedicure", TreatmentType = TreatmentType.NaturalNail, Duration = 45, Price = 22 } }
//        //    };

//        //    return allTreatments;
//        //}
//        //public Dictionary<int,Treatment> employeeTreatment3()
//        //{
//        //    Dictionary<int, Treatment> allTreatments = new Dictionary<int, Treatment>()
//        //    {
//        //        {5,   new Treatment() { Id = 5, TreatmentName = "Gel Polish Express", TreatmentType = TreatmentType.GelPolish, Duration = 20, Price = 20 } },
//        //        {6,    new Treatment() { Id = 6, TreatmentName = "Gel Polish with Manicure", TreatmentType = TreatmentType.GelPolish, Duration = 45, Price = 25 } },
//        //        {7,     new Treatment() { Id = 7, TreatmentName = "Pedicure", TreatmentType = TreatmentType.NaturalNail, Duration = 45, Price = 22 } }
//        //    };

//        //    return allTreatments;
//        //}
//    }
//}
