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
    public class OperationDoctorRepository : IOperationDoctorRepository
    {
        private String _fileName = @"..\..\..\Data\JSON\operationDoctor.json";
        private IDoctorRepository _doctorRepository;
        private IOperationRepository _operationRepository;

        public OperationDoctorRepository(IDoctorRepository doctorRepository, IOperationRepository operationRepository)
        {
            _doctorRepository = doctorRepository;
            _operationRepository = operationRepository;
            this.LoadFromFile();
        }

        
        public void LoadFromFile()
        {
            var doctorsByUsername = _doctorRepository.GetAllByUsername();
            var operationsById = _operationRepository.GetAllById();
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
            var operations = _operationRepository.GetAll();
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

