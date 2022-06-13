using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using System.Windows;

namespace HealthInstitution.Core.Scheduling
{
    public class SchedulingService : ISchedulingService
    {
        private static Random rnd = new Random();
        IExaminationRepository _examinationRepository;
        IOperationRepository _operationRepository;
        IDoctorOperationAvailabilityService _doctorOperationAvailabilityService;
        IPatientOperationAvailabilityService _patientOperationAvailabilityService;
        IDoctorExaminationAvailabilityService _doctorExaminationAvailabilityService;
        IPatientExaminationAvailabilityService _patientExaminationAvailabilityService;
        IOperationService _operationService;
        IExaminationService _examinationService;
        IRoomService _roomService;

        public SchedulingService(IExaminationRepository examinationRepository, IOperationRepository operationRepository, 
            IDoctorOperationAvailabilityService doctorOperationAvailabilityService, IPatientOperationAvailabilityService patientOperationAvailabilityService, 
            IDoctorExaminationAvailabilityService doctorExaminationAvailabilityService, IPatientExaminationAvailabilityService patientExaminationAvailabilityService,
            IOperationService operationService, IExaminationService examinationService, IRoomService roomService)
        {
            _examinationRepository = examinationRepository;
            _operationRepository = operationRepository;
            _doctorOperationAvailabilityService = doctorOperationAvailabilityService;
            _patientOperationAvailabilityService = patientOperationAvailabilityService;
            _doctorExaminationAvailabilityService = doctorExaminationAvailabilityService;
            _patientExaminationAvailabilityService = patientExaminationAvailabilityService;
            _operationService = operationService;
            _examinationService = examinationService;
            _roomService = roomService;
        }

        public bool CheckOccurrenceOfRoom(Room room)
        {
            if (_examinationRepository.GetAll().Find(examination => examination.Room == room) == null)
                return false;
            if (_operationRepository.GetAll().Find(operation => operation.Room == room) == null)
                return false;
            return true;
        }

        public void RedirectByType(Referral referral, DateTime appointment, MedicalRecord medicalRecord)
        {
            if (referral.Type == ReferralType.SpecificDoctor)
                ScheduleWithSpecificDoctor(appointment, referral, medicalRecord);
            else
                ScheduleWithOrderedSpecialty(appointment, referral, medicalRecord);
        }

        private void ScheduleWithSpecificDoctor(DateTime appointment, Referral referral, MedicalRecord medicalRecord)
        {
            ExaminationDTO examination = new ExaminationDTO(appointment, null, referral.ReferredDoctor, medicalRecord);
            ReserveExamination(examination);
            System.Windows.MessageBox.Show("You have scheduled the examination!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            referral.Active = false;
            ReferralRepository.GetInstance().Save(); //TODO
        }

        private void ScheduleWithOrderedSpecialist(ExaminationDTO examination, Referral referral)
        {
            try
            {
                ReserveExamination(examination);
                System.Windows.MessageBox.Show("You have scheduled the examination! Doctor: " + examination.Doctor.Name + " " + examination.Doctor.Surname, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                referral.Active = false;
                ReferralRepository.GetInstance().Save(); //TODO
            }
            catch (Exception ex)
            {
                if (ex.Message == "That doctor is not available")
                    throw new Exception("That doctor is not available");
                else
                    System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ScheduleWithOrderedSpecialty(DateTime appointment, Referral referral, MedicalRecord medicalRecord)
        {
            SpecialtyType specialtyType = (SpecialtyType)referral.Type;
            List<Doctor> doctors = DoctorRepository.GetInstance().Doctors;
            bool successfulScheduling = false;
            foreach (Doctor doctor in doctors)
            {
                if (doctor.Specialty == specialtyType)
                {
                    try
                    {
                        ExaminationDTO examinationDTO = new ExaminationDTO(appointment, null, doctor, medicalRecord);
                        ScheduleWithOrderedSpecialist(examinationDTO, referral);
                        successfulScheduling = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "That doctor is not available")
                            continue;
                        throw;
                    }
                }
            }
            if (!successfulScheduling) throw new Exception("It is not possible to find a doctor with this specialty");
        }

        public void ReserveOperation(OperationDTO operationDTO, int id = 0)
        {
            operationDTO.Validate();
            operationDTO.Room = FindAvailableOperationRoom(operationDTO);
            _doctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO);
            _patientOperationAvailabilityService.CheckIfPatientIsAvailable(operationDTO);
            _operationService.Add(operationDTO);
        }

        public void ReserveExamination(ExaminationDTO examinationDTO)
        {
            examinationDTO.Validate();
            examinationDTO = CheckExaminationAvailable(examinationDTO);
            _examinationService.Add(examinationDTO);
        }

        public Room FindAvailableOperationRoom(OperationDTO operationDTO, int id = 0)
        {
            bool isAvailable;
            List<Room> availableRooms = new List<Room>();
            var rooms = _roomService.GetNotRenovating();
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;

            foreach (var room in rooms)
            {
                if (room.Type != RoomType.OperatingRoom) continue;
                isAvailable = true;
                foreach (var operation in _operationService.GetAll())
                {
                    if (operation.Room.Id == room.Id && operation.Id != id)
                    {
                        if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(duration) > operation.Appointment))
                        {
                            isAvailable = false;
                            break;
                        }
                    }
                }
                if (isAvailable)
                    availableRooms.Add(room);
            }

            if (availableRooms.Count == 0) throw new Exception("There are no available rooms!");
            int index = rnd.Next(0, availableRooms.Count);
            return availableRooms[index];
        }

        public Room FindAvailableExaminationRoom(DateTime appointment)
        {
            List<Room> availableRooms = FindAllAvailableRooms(RoomType.ExaminationRoom, appointment);

            if (availableRooms.Count == 0) throw new Exception("There are no available rooms!");

            int index = rnd.Next(0, availableRooms.Count);
            return availableRooms[index];
        }

        public List<Room> FindAllAvailableRooms(RoomType roomType, DateTime appointment)
        {
            bool isAvailable;
            List<Room> availableRooms = new List<Room>();
            foreach (var room in _roomService.GetNotRenovating())
            {
                if (room.Type != roomType) continue;
                isAvailable = true;
                foreach (var examination in _examinationService.GetAll())
                {
                    if (examination.Appointment == appointment && examination.Room.Id == room.Id)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                if (isAvailable)
                    availableRooms.Add(room);
            }
            return availableRooms;
        }

        public ExaminationDTO CheckExaminationAvailable(ExaminationDTO examinationDTO)
        {
            examinationDTO.Room = FindAvailableExaminationRoom(examinationDTO.Appointment);
            _doctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
            _patientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
            return examinationDTO;
        }

        /*public void UpdateExamination(int id, ExaminationDTO examinationDTO)
        {
            examinationDTO.Validate();
            Examination examination = new Examination(examinationDTO);
            DoctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
            PatientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
            _examinationRepository.Update(id, examination);
        }*/
    }
}