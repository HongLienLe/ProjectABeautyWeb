using System;
using AutoMapper;
using DataMongoApi.Models;

namespace DataMongoApi.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Treatment, TreatmentForm>();
        }
    }
}
