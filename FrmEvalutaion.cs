using Evaluation_Manager.Models;
using Evaluation_Manager.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evaluation_Manager
{
	public partial class FrmEvalutaion : Form
	{
		private Student student;
		public FrmEvalutaion(Student selectedStudent)
		{
			InitializeComponent();
			student = selectedStudent;
		}

		private void FrmEvalutaion_Load(object sender, EventArgs e)
		{
			Text = student.FirstName + " " + student.LastName;
			List<Activity> activities = ActivityRepository.GetActivities();
			cboActivities.DataSource = activities;
		}

		private void cboActivities_SelectedIndexChanged(object sender, EventArgs e)
		{
			Activity currentActivity = cboActivities.SelectedItem as Activity;
			txtActivityDesription.Text = currentActivity.Description;
			txtMinForGrade.Text = currentActivity.MinPointsForGrade + "/" + currentActivity.MaxPoints;
			txtMin.Text = currentActivity.MinPoints + "/" + currentActivity.MaxPoints;
			numPoints.Minimum = 0;
			numPoints.Maximum = currentActivity.MaxPoints;

			var evaluation = EvaluationRepository.GetEvaluation(student, currentActivity);
			if(evaluation != null)
            {
				txtTeacher.Text = evaluation.Evaluator.ToString();
				txtEvaluationDate.Text = evaluation.EvaluationDate.ToString();
				numPoints.Value = evaluation.Points;
            }
            else
            {
                txtTeacher.Text = FrmLogin.LoggedTeacher.ToString();
				txtEvaluationDate = "-";
				numPoints.Value = 0;
            }
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

        private void btnSave_Click(object sender, EventArgs e)
        {
			Activity currentActivity = cboActivities.SelectedItem as Activity;
			var teacher = FrmLogin.LoggedTeacher;
			int points = (int)numPoints.Value;
			teacher.PerformEvaluation(student, currentActivity, points);
			Close();
        }
    }
}
