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
    public partial class SeeAllSubjects : PhoneApplicationPage
    {
        App a;
        public SeeAllSubjects()
        {
            a = App.Current as App;
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            existingSubjects.ItemsSource = null;
            if (a.subjects.Count != 0)
            {
                //existingSubjects.ItemsSource = a.subjects;
                existingSubjects.ItemsSource = from x in a.subjects
                                               orderby x.ShortName
                                               select x;
                emptyMsgTB.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
                emptyMsgTB.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var messagePrompt = new MessagePrompt
            {
                Title = "Are you sure?",
                Message = "Are you sure you want to delete the subject " + (sender as Button).Tag.ToString(),
                IsCancelVisible = true,
                Tag = (sender as Button).Tag.ToString()
            };
            messagePrompt.Completed += messagePrompt_Completed;
            messagePrompt.Show();
        }

        void messagePrompt_Completed(object sender, Coding4Fun.Phone.Controls.PopUpEventArgs<string, Coding4Fun.Phone.Controls.PopUpResult> e)
        {
            // OK to delete :
            if (e.PopUpResult == PopUpResult.Ok)
            {
                string searchStr = (sender as MessagePrompt).Tag.ToString();
                int i = 0;
                foreach (Subject x in a.subjects)
                {
                    if (x.ShortName.Equals(searchStr))
                        break;
                    i++;
                }

                a.subjects.RemoveAt(i);

                // remove from a.timetable
                for (i = (a.timetable.Count - 1); i >= 0; i--)
                {
                    if (a.timetable[i].subject.ShortName.Equals(searchStr))
                        a.timetable.RemoveAt(i);
                }
                existingSubjects.ItemsSource = null;
                existingSubjects.ItemsSource = a.subjects;
            }
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel p = sender as StackPanel;
            string day;
            NavigationContext.QueryString.TryGetValue("day", out day);
            NavigationService.Navigate(new Uri("/SubjectDetails.xaml?sub=" + p.Tag.ToString(), UriKind.Relative));
            //NavigationService.Navigate(new Uri("/ProperSubjectInsertion.xaml?sub=" + p.Tag.ToString() + "&day=" + day, UriKind.Relative));
            //MessageBox.Show(p.Tag.ToString());
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SelectSubject.xaml", UriKind.Relative));
        }
    }
}
