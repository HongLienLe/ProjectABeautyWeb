using System;
namespace AccessDataApi.Functions
{
    public interface IValidationErrorMessage
    {
        public string TreatmentNotFoundMessage(int id);
        public string EmployeeNotFoundMessage(int id);


    }
}
