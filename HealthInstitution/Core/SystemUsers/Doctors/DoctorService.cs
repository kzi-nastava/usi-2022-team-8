using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.RestRequests.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution.Core.SystemUsers.Doctors;

public class DoctorService
{ 
    IDoctorRepository _doctorRepository;
    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public void LoadAppointments()
    {
        OperationDoctorRepository.GetInstance();
        ExaminationDoctorRepository.GetInstance();
    }
    public List<Doctor> GetAll()
    {
        return _doctorRepository.GetAll();
    }

    public Doctor GetById(String username)
    {
        return _doctorRepository.GetById(username);
    }
    public void DeleteNotifications(Doctor doctor)
    {
        _doctorRepository.DeleteNotifications(doctor);
    }

    public void DeleteExamination(Examination examination)
    {
        _doctorRepository.DeleteExamination(examination);
    }

    public void DeleteOperation(Operation operation)
    {
        _doctorRepository.DeleteOperation(operation);
    }
    public void DeleteRestRequest(RestRequest restRequest)
    {
        _doctorRepository.DeleteRestRequest(restRequest);
    }

    public List<Doctor> SearchBySpeciality(string keyword)
    {
        return _doctorRepository.GetSearchSpecialty(keyword);
    }

    public List<Doctor> SearchByName(string keyword)
    {
        return _doctorRepository.GetSearchName(keyword);
    }

    public List<Doctor> SearchBySurname(string keyword)
    {
        return _doctorRepository.GetSearchSurname(keyword);
    }

    public List<Doctor> OrderByDoctorSpeciality(List<Doctor> examinations)
    {
        return examinations.OrderBy(o => o.Specialty).ToList();
    }

    public List<Doctor> OrderByDoctorName(List<Doctor> examinations)
    {
        return examinations.OrderBy(o => o.Name).ToList();
    }

    public List<Doctor> OrderByDoctorSurname(List<Doctor> examinations)
    {
        return examinations.OrderBy(o => o.Specialty).ToList();
    }

    public List<Doctor> OrderByDoctorRating(List<Doctor> examinations)
    {
        return examinations.OrderBy(o => o.AvgRating).ToList();
    }

    public void AssignScorebyId(string username, double avgRating)
    {
        GetById(username).AvgRating = avgRating;
        _doctorRepository.Save();
    }
}