﻿using System.Windows;
using System.Windows.Controls;
using HealthInstitution.Core.Prescriptions.Model;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for RecepieNotificationSettings.xaml
/// </summary>
public partial class RecepieNotificationSettings : Window
{
    private int _hours;
    private int _minutes;

    public RecepieNotificationSettings()
    {
        InitializeComponent();
        FormDataGrid();
    }

    private void HourComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var hourComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> hours = new List<String>();
        for (int i = 0; i < 23; i++)
        {
            hours.Add(i.ToString());
        }
        hourComboBox.ItemsSource = hours;
        hourComboBox.SelectedIndex = 0;
    }

    private void MinuteComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var minuteComboBox = sender as System.Windows.Controls.ComboBox;
        List<String> minutes = new List<String>();
        for (int i = 0; i < 59; i++)
        {
            minutes.Add(i.ToString());
        }
        minuteComboBox.ItemsSource = minutes;
        minuteComboBox.SelectedIndex = 0;
    }

    private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var hourComboBox = sender as System.Windows.Controls.ComboBox;
        int h = hourComboBox.SelectedIndex;
        _hours = h;
    }

    private void MinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var minuteComboBox = sender as System.Windows.Controls.ComboBox;
        int m = minuteComboBox.SelectedIndex;
        this._minutes = m;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
    }

    private void FormDataGrid()
    {
    }
}