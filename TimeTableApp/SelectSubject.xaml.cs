using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Coding4Fun.Phone.Controls;
namespace TimeTableApp
{
    public partial class SelectSubject : PhoneApplicationPage
    {
        App a;
        public SelectSubject()
        {
            InitializeComponent();
            a = App.Current as App;
            tsp.Value = new TimeSpan(1, 0, 0);
        }
		
        private void ApplicationBarIconButton_Click(object sender, System.EventArgs e)
        {
            if (SmallName.Text == "")
            {
                //MessageBox.Show("The 'Name' of the subject identifies the subject in the Time Table.\nThis field cannot be left empty");
                var messagePrompt = new MessagePrompt
                {
                    Title = "Error!",
                    Message = "The 'Name' of the subject identifies the subject in the Time Table.\nThis field cannot be left empty"
                };
                messagePrompt.Show();
                return;
            }
            IEnumerable<Subject> s = from sub in a.subjects
                                     where sub.ShortName == SmallName.Text
                                     select sub;
            if (s.Count()!=0)
            {
                //MessageBox.Show("A subject with this name already exists, please select that from the list directly to enter into the timetable");
				var messagePrompt = new MessagePrompt
                {
                    Title = "Error!",
                    Message = "A subject with this name already exists, please select that from the list directly to enter into the timetable"
                };
                messagePrompt.Show();
                return;
            }
        	a.subjects.Add(new Subject(SmallName.Text, LongName.Text, tsp.Value.Value, DescriptionTB.Text));
			NavigationService.GoBack();
        }
		
		private void ApplicationBarIconCancel_Click(object sender, System.EventArgs e)
        {
			NavigationService.GoBack();
        }
    }
}