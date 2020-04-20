﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMongoCollection<Employee> _employees;

        public OperatingHoursService(ISalonDatabaseSettings settings, IClientConfiguration clientConfiguration)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(clientConfiguration.MerchantId);

            _operatinghrs = database.GetCollection<OperatingHours>("OperatingHours");
            _employees = database.GetCollection<Employee>("Employees");

        }

        public List<OperatingHours> Get() =>
            _operatinghrs.Find(operatingHours => true).ToList();

        public OperatingHours Get(string date) =>
                    _operatinghrs.Find<OperatingHours>(op => op.About.Day == DateTime.Parse(date).DayOfWeek.ToString()).FirstOrDefault();

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