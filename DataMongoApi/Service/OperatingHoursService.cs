using System;
using System.Collections.Generic;
using System.Linq;
using DataMongoApi.DbContext;
using DataMongoApi.Middleware;
using DataMongoApi.Models;
using DataMongoApi.Service.InterfaceService;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataMongoApi.Service
{
    public class OperatingHoursService : IOperatingHoursService
    {
        private readonly IMongoCollection<OperatingHours> _operatinghrs;
        private readonly IMongoDbContext _context;


        public OperatingHoursService(IMongoDbContext context)
        {
            _context = context;

            _operatinghrs = _context.GetCollection<OperatingHours>("OperatingHours");

        }

        public List<OperatingHours> Get() =>
            _operatinghrs.Find(operatingHours => true).ToList();

        public OperatingHours Get(string day) =>
                    _operatinghrs.Find<OperatingHours>(op => op.About.Day == day).FirstOrDefault();

        public OperatingHours Create(OperatingHours ophrs)
        {
            _operatinghrs.InsertOne(ophrs);
            return ophrs;
        }

        public void Update(string id, OperatingHoursDetails ophrsIn)
        {
            var filter = Builders<OperatingHours>.Filter.Eq(d => d.ID, id);
            var update = Builders<OperatingHours>.Update
                .Set(d => d.About, ophrsIn)
                .CurrentDate(d => d.ModifiedOn);

            _operatinghrs.UpdateOne(filter, update);
        }

        public void Remove(OperatingHours ophrsIn) =>
            _operatinghrs.DeleteOne(op => op.ID == ophrsIn.ID);

        public void Remove(string id) =>
            _operatinghrs.DeleteOne(op => op.ID == id);


        //public void UpdateEmployee(string id, List<string> employeeIds)
        //{
        //    var workDay = Get(id);

        //    var employees = employeeIds.Select(x => ObjectId.Parse(x)).ToList();

        //    workDay.Employees.AddRange(employees);
        //}
    }
}
