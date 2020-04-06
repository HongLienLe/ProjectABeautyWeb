using System;
using System.ComponentModel.DataAnnotations;

namespace AccessDataApi.HTTPModels
{
    public class EmployeeForm
    {
        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string Email { get; set; }

    }

    public class EmployeeDetails : EmployeeForm
    {
        public int Id { get; set; }
    }

}
