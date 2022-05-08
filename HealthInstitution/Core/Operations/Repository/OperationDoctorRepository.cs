using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Operations.Repository
{
    internal class OperationDoctorRepository
    {
        private String _fileName;
        private OperationDoctorRepository(String fileName)
        {
            this._fileName = fileName;
            this.LoadFromFile();
        }

        private static OperationDoctorRepository s_instance = null;

        public static OperationDoctorRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new OperationDoctorRepository(@"..\..\..\Data\JSON\operationDoctor.json");
                }
                return s_instance;
            }
        }

    public void LoadFromFile()
    {
        var doctorsByUsername = DoctorRepository.GetInstance().DoctorsByUsername;
        var operationsById = OperationRepository.GetInstance().OperationsById;
        var operationIdsDoctorUsernames = JArray.Parse(File.ReadAllText(this._fileName));
        foreach (var pair in operationIdsDoctorUsernames)
        {
            int id = (int)pair["id"];
            String username = (String)pair["username"];
            Doctor doctor = doctorsByUsername[username];
            Operation operation = operationsById[id];
            doctor.Operations.Add(operation);
            operation.Doctor = doctor;
        }
    }

    public void Save()
    {
        List<dynamic> operationIdsDoctorUsernames = new List<dynamic>();
        var operations = OperationRepository.GetInstance().Operations;
        foreach (var operation in operations)
        {
            Doctor doctor = operation.Doctor;
            operationIdsDoctorUsernames.Add(new { id = operation.Id, username = doctor.Username });
        }
            var allPairs = JsonSerializer.Serialize(operationIdsDoctorUsernames);
            File.WriteAllText(this._fileName, allPairs);
        }
}
}

