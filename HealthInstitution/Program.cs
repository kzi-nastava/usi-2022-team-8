// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.GUI.LoginView;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.GUI.DoctorView;

namespace HealthInstitution
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var services = new DIServiceCollection();

            //services.RegisterSingleton<Random>();
            //services.RegisterTransient<Random>();

            services.RegisterTransient<IDrugService, DrugService>();
            services.RegisterTransient<IDrugVerificationService, DrugVerificationService >();
            services.RegisterTransient<IDrugRepository, DrugRepository>();

            services.RegisterTransient<DrugsVerificationTable>();
            services.RegisterSingleton<LoginWindow>();


            var container = services.BuildContainer();

            

            var loginWindow = container.GetService<LoginWindow>();

            //Console.WriteLine(service1.Method());
            //Console.WriteLine(service2.Method());

            await loginWindow.StartAsync();

        }
    }
}




















