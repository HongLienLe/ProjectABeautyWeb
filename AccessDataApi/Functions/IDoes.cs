using System;
namespace AccessDataApi.Functions
{
    public interface IDoes
    {
        public bool EmployeeExist(int id);
        public bool TreatmentExist(int id);
        public bool ClientExist(int id);
        public bool AppIdExist(int id);
        public bool DateTimeKeyExist(DateTime date);
        public bool OpeningIdExist(int id);
   //     public bool PaymentIdExist(int id);
    }
}
