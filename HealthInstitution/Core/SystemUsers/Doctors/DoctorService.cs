using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.RestRequestNotifications.Repository;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.RestRequests.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution.Core.SystemUsers.Doctors;

public class DoctorService : IDoctorService
{ 
    IDoctorRepository _doctorRepository;
    IOperationDoctorRepository _operationDoctorRepository;
    IExaminationDoctorRepository _examinationDoctorRepository;
    IAppointmentNotificationDoctorRepository _appointmentNotificationDoctorRepository;
    IAppointmentNotificationPatientRepository _appointmentNotificationPatientRepository;
    IRestRequestNotificationDoctorRepository _restRequestNotificationDoctorRepository;

    public DoctorService(IDoctorRepository doctorRepository, IOperationDoctorRepository operationDoctorRepository, IExaminationDoctorRepository examinationDoctorRepository, IAppointmentNotificationDoctorRepository appointmentNotificationDoctorRepository, IAppointmentNotificationPatientRepository appointmentNotificationPatientRepository, IRestRequestNotificationDoctorRepository restRequestNotificationDoctorRepository)
    {
        _doctorRepository = doctorRepository;
        _operationDoctorRepository = operationDoctorRepository;
        _examinationDoctorRepository = examinationDoctorRepository;
        _appointmentNotificationDoctorRepository = appointmentNotificationDoctorRepository;
        _appointmentNotificationPatientRepository = appointmentNotificationPatientRepository;
        _restRequestNotificationDoctorRepository = restRequestNotificationDoctorRepository;
    }

    public void LoadAppointments()
    {
        //_operationDoctorRepository.LoadFromFile();
        //_examinationDoctorRepository.LoadFromFile();
    }
    public void LoadNotifications()
    {
        //_appointmentNotificationDoctorRepository.LoadFromFile();
        //_appointmentNotificationPatientRepository.LoadFromFile();
        //_restRequestNotificationDoctorRepository.LoadFromFile();
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
    public List<AppointmentNotification> GetActiveAppointmentNotification(Doctor doctor)
    {
        List<AppointmentNotification> appointmentNotifications = new List<AppointmentNotification>();
        foreach (AppointmentNotification appointmentNotification in doctor.AppointmentNotifications)
            if (appointmentNotification.ActiveForDoctor)
                appointmentNotifications.Add(appointmentNotification);
        return appointmentNotifications;
    }
    public List<RestRequestNotification> GetActiveRestRequestNotification(Doctor doctor)
    {
        List<RestRequestNotification> restRequestNotifications = new List<RestRequestNotification>();
        foreach (RestRequestNotification restRequestNotification in doctor.RestRequestNotifications)
            if (restRequestNotification.Active)
                restRequestNotifications.Add(restRequestNotification);
        return restRequestNotifications;
    }

    public List<Doctor> GetDoctorsOrderByRating()
    {
        return _doctorRepository.GetAll().OrderBy(d => d.AvgRating).ToList();
    }
}