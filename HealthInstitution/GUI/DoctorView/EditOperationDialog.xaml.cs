﻿using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for EditOperationDialog.xaml
    /// </summary>
    public partial class EditOperationDialog : Window
    {
        private Operation _selectedOperation;

        public EditOperationDialog(Operation operation)
        {
            this._selectedOperation = operation;
            InitializeComponent();
            datePicker.SelectedDate = this._selectedOperation.Appointment;
            durationTextBox.Text = operation.Duration.ToString();
        }

        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var patientComboBox = sender as System.Windows.Controls.ComboBox;
            List<Patient> patients = PatientRepository.GetInstance().Patients;
            foreach (Patient patient in patients)
            {
                patientComboBox.Items.Add(patient);
            }
            patientComboBox.SelectedItem = this._selectedOperation.MedicalRecord.Patient;
        }

        private void HourComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> hours = new List<String>();
            for (int i = 9; i < 22; i++)
            {
                hours.Add(i.ToString());
            }
            hourComboBox.ItemsSource = hours;
            hourComboBox.SelectedItem = this._selectedOperation.Appointment.Hour.ToString();
        }

        private void MinuteComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> minutes = new List<String>();
            minutes.Add("00");
            minutes.Add("15");
            minutes.Add("30");
            minutes.Add("45");
            minuteComboBox.ItemsSource = minutes;
            String operationMinutes = this._selectedOperation.Appointment.Minute.ToString();
            if (operationMinutes.Length == 1)
            {
                operationMinutes = operationMinutes + "0";
            }
            minuteComboBox.SelectedItem = operationMinutes;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime appointment = (DateTime)datePicker.SelectedDate;
                int minutes = Int32.Parse(minuteComboBox.Text);
                int hours = Int32.Parse(hourComboBox.Text);
                appointment = appointment.AddHours(hours);
                appointment = appointment.AddMinutes(minutes);
                int duration = Int32.Parse(durationTextBox.Text);

                Patient patient = (Patient)patientComboBox.SelectedItem;
                MedicalRecord medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
                if (appointment <= DateTime.Now)
                {
                    System.Windows.MessageBox.Show("You have to change dates for upcoming ones!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    OperationRepository.GetInstance().Update(this._selectedOperation.Id, appointment, medicalRecord, duration);
                    //ExaminationDoctorRepository.GetInstance().Save();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

       
    }
}
