using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectABeautyWeb.Models;

namespace ProjectABeautyWeb.Data
{
    public static class ApplicationContextExtenstion
    {
        public static void SeedData(IServiceProvider serviceProvider)
        {

            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationContext>>()))
            {
                if (context.Enquiries.Any())
                    return;

                Enquiry enquiry = new Enquiry("Name", "Name@app.com", "012345678901", "Added Notes");

                context.Enquiries.Add(enquiry);
                context.SaveChanges();
            }
        }
    }
}
