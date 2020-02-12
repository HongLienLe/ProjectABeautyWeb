using System;
using System.Collections.Generic;

namespace AccessDataApi.HTTPModels
{
    public class OneIdToManyIdModel
    {
        public int Id { get; set; }
        public List<int> Ids { get; set; }

        public OneIdToManyIdModel()
        {
            Ids = new List<int>();
        }
    }
}
