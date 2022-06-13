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

public static class DoctorService
{
    private static DoctorRepository s_doctorRepository = DoctorRepository.GetInstance();

    public static void LoadAppointments()
    {
        OperationDoctorRepository.GetInstance();
        ExaminationDoctorRepository.GetInstance();
    }
    public static void LoadNotifications()
    {
        AppointmentNotificationDoctorRepository.GetInstance();
        AppointmentNotificationPatientRepository.GetInstance();
        RestRequestNotificationDoctorRepository.GetInstance();
    }
    public static List<Doctor> GetAll()
    {
        return s_doctorRepository.GetAll();
    }

    public static Doctor GetById(String username)
    {
        return s_doctorRepository.GetById(username);
    }
    public static void DeleteNotifications(Doctor doctor)
    {
        s_doctorRepository.DeleteNotifications(doctor);
    }

    public static void DeleteExamination(Examination examination)
    {
        s_doctorRepository.DeleteExamination(examination);
    }

    public static void DeleteOperation(Operation operation)
    {
        s_doctorRepository.DeleteOperation(operation);
    }
    public static void DeleteRestRequest(RestRequest restRequest)
    {
        s_doctorRepository.DeleteRestRequest(restRequest);
    }

    public static List<Doctor> SearchBySpeciality(string keyword)
    {
        return s_doctorRepository.GetSearchSpeciality(keyword);
    }

    public static List<Doctor> SearchByName(string keyword)
    {
        return s_doctorRepository.GetSearchName(keyword);
    }

    public static List<Doctor> SearchBySurname(string keyword)
    {
        return s_doctorRepository.GetSearchSurname(keyword);
    }

    public static List<Doctor> OrderByDoctorSpeciality(List<Doctor> examinations)
    {
        return examinations.OrderBy(o => o.Specialty).ToList();
    }

    public static List<Doctor> OrderByDoctorName(List<Doctor> examinations)
    {
        return examinations.OrderBy(o => o.Name).ToList();
    }

    public static List<Doctor> OrderByDoctorSurname(List<Doctor> examinations)
    {
        return examinations.OrderBy(o => o.Specialty).ToList();
    }

    public static List<Doctor> OrderByDoctorRating(List<Doctor> examinations)
    {
        return examinations.OrderBy(o => o.AvgRating).ToList();
    }

    public static void AssignScorebyId(string username, double avgRating)
    {
        GetById(username).AvgRating = avgRating;
        s_doctorRepository.Save();
    }
    public static List<AppointmentNotification> GetActiveAppointmentNotification(Doctor doctor)
    {
        List<AppointmentNotification> appointmentNotifications = new List<AppointmentNotification>();
        foreach (AppointmentNotification appointmentNotification in doctor.AppointmentNotifications)
            if (appointmentNotification.ActiveForDoctor)
                appointmentNotifications.Add(appointmentNotification);
        return appointmentNotifications;
    }
    public static List<RestRequestNotification> GetActiveRestRequestNotification(Doctor doctor)
    {
        List<RestRequestNotification> restRequestNotifications = new List<RestRequestNotification>();
        foreach (RestRequestNotification restRequestNotification in doctor.RestRequestNotifications)
            if (restRequestNotification.Active)
                restRequestNotifications.Add(restRequestNotification);
        return restRequestNotifications;
    }
}