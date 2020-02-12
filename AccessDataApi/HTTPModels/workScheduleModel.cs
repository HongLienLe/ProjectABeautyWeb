using System;
namespace AccessDataApi.HTTPModels
{
    public class WorkScheduleModel : OneIdToManyIdModel
    {
        public bool isEmployee { get; set; }
    }
}
