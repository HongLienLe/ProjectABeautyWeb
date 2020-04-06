using System;
using System.Collections.Generic;

namespace AccessDataApi.HTTPModels
{
    public class OneIdToManyIdForm
    {
        public int Id { get; set; }
        public List<int> Ids { get; set; }

        public OneIdToManyIdForm()
        {
            Ids = new List<int>();
        }
    }
}
