using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Operations.Repository
{
    internal class OperationDoctorRepository
    {
        public String fileName { get; set; }
        private OperationDoctorRepository(String fileName)
        {
            this.fileName = fileName;
            this.LoadOperationDoctor();
        }

        private static OperationDoctorRepository instance = null;

        public static OperationDoctorRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new OperationDoctorRepository(@"..\..\..\Data\JSON\operationDoctor.json");
                }
                return instance;
            }
        }

    public void LoadOperationDoctor()
    {
        var doctorsByUsername = DoctorRepository.GetInstance().doctorsByUsername;
        var operationsById = OperationRepository.GetInstance().operationsById;
        var pairs = JArray.Parse(File.ReadAllText(this.fileName));
        foreach (var pair in pairs)
        {
            int id = (int)pair["id"];
            String username = (String)pair["username"];
            Doctor doctor = doctorsByUsername[username];
            Operation operation = operationsById[id];
            doctor.operations.Add(operation);
            operation.doctor = doctor;
        }
    }

    public void SaveOperationDoctor()
    {
        List<dynamic> operationsIdDoctorsUsername = new List<dynamic>();
        var operations = OperationRepository.GetInstance().operations;
        foreach (var operation in operations)
        {
            Doctor doctor = operation.doctor;
            operationsIdDoctorsUsername.Add(new { id = operation.id, username = doctor.username });
        }
    }
}
}

