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

namespace TimeTableApp
{
    public partial class ProperSubjectInsertion : PhoneApplicationPage
    {
        App a;
        bool newPage = true;
        string selectedSub = "";
        public ProperSubjectInsertion()
        {
            InitializeComponent();
            a = App.Current as App;
			daySelector.Items.Add("sunday");
			daySelector.Items.Add("monday");
			daySelector.Items.Add("tuesday");
			daySelector.Items.Add("wednesday");
			daySelector.Items.Add("thursday");
			daySelector.Items.Add("friday");
			daySelector.Items.Add("saturday");
            timePicker.Value = (DateTime)System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["LastUsedPeriodTime"];
        }
		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)  
        {
            if (newPage)
            {
                string selected = "";
                if (NavigationContext.QueryString.TryGetValue("day", out selected))
                {
                    daySelector.SelectedItem = selected;
                }
                if (NavigationContext.QueryString.TryGetValue("sub", out selectedSub))
                {
                    PageTitle.Text = selectedSub;
                }
                newPage = false;
            }
        }

        // adding subject to the list.
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            IEnumerable<Subject> s = from sub in a.subjects
                        where sub.ShortName == selectedSub
                        select sub;
            System.Diagnostics.Debug.WriteLine(timePicker.Value.Value + " <- " + (MyWeekDay)daySelector.SelectedIndex + s.ElementAt(0).ToString());
            a.timetable.Add(new TimetableElement(timePicker.Value.Value, (MyWeekDay)daySelector.SelectedIndex, s.ElementAt(0)));
            NavigationService.GoBack();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["LastUsedPeriodTime"] = timePicker.Value.Value;
        }
    }
}