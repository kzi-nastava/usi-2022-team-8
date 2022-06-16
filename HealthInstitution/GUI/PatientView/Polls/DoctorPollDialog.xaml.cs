﻿using System;
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
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.GUI.PatientView.Polls;

/// <summary>
/// Interaction logic for DoctorPollDialog.xaml
/// </summary>
public partial class DoctorPollDialog : Window
{
    private Doctor _doctor;
    IPollService _pollService;
    public DoctorPollDialog(IPollService pollService)
    {
        InitializeComponent();
        _pollService = pollService;
        LoadQuestionLabels();
    }
    public void SetRatedDoctor(Doctor doctor)
    {
        _doctor = doctor;
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        AddComment();
        AddQuetionResaults();
        this.Close();
    }

    private void AddComment()
    {
        var comment = CommentTextBox.Text;
        if (comment.Length > 0)
        {
            PollCommentDTO pollCommentDTO = new PollCommentDTO(comment, _doctor);
            _pollService.AddComment(pollCommentDTO);
        }
    }

    private List<Grid> GetGridsList()
    {
        List<Grid> gridList = new List<Grid>();
        gridList.Add(Q1);
        gridList.Add(Q2);
        gridList.Add(Q3);
        gridList.Add(Q4);
        gridList.Add(Q5);

        return gridList;
    }

    private void AddQuetionResaults()
    {
        var grids = GetGridsList();
        var hospitalQuestion = _pollService.GetHospitalQuestions();
        for (int i = 0; i < grids.Count; i++)
        {
            AddForOneQuestion(grids[i], hospitalQuestion[i]);
        }
    }

    private void AddForOneQuestion(Grid grid, string question)
    {
        var checkedButton = grid.Children.OfType<System.Windows.Controls.RadioButton>()
                                     .FirstOrDefault(r => r.IsChecked.Value);

        List<int> ints = new List<int>();
        ints.Add(Convert.ToInt32(checkedButton.Content));

        PollQuestionDTO pollQuestionDTO = new PollQuestionDTO(question, _doctor, ints);
        _pollService.UpdateQuestionGrades(pollQuestionDTO);
    }

    private void LoadQuestionLabels()
    {
        var questions = _pollService.GetDoctorQuestions();
        LabelQ1.Content = questions[0];
        LabelQ2.Content = questions[1];
        LabelQ3.Content = questions[2];
        LabelQ4.Content = questions[3];
        LabelQ5.Content = questions[4];
    }
}