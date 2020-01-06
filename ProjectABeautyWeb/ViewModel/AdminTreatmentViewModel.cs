using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectABeautyWeb.Models;

namespace ProjectABeautyWeb.ViewModel
{
    public class AdminTreatmentViewModel
    {
        public Treatment Treatment { get; set; }
        public IEnumerable<SelectListItem> TreatmentTypesSelectedListItems { get; set; }

        public AdminTreatmentViewModel()
        {
            Treatment = new Treatment();
        }


       
    }
}
