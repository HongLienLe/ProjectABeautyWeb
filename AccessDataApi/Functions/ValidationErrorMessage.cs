using System;
namespace AccessDataApi.Functions
{
    public class ValidationErrorMessage : IValidationErrorMessage
    {
        public string TreatmentNotFoundMessage(int id)
        {
            return $"Treatment {id} does not exist";
        }

        public string EmployeeNotFoundMessage(int id)
        {
            return $"Employee {id} does not exist";
        }
    }
}
