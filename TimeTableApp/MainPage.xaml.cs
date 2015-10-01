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
using Microsoft.Phone.Scheduler;

namespace TimeTableApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        App a;
        bool firstStart = true;
        System.IO.IsolatedStorage.IsolatedStorageSettings iss;
        public MainPage()
        {
            InitializeComponent();
            a = App.Current as App;
            iss = System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            // hocking up all the events for the page
            sun.goToThatPage += new EventHandler(ControlGoToThatPage);
            mon.goToThatPage += new EventHandler(ControlGoToThatPage);
            tue.goToThatPage += new EventHandler(ControlGoToThatPage);
            wed.goToThatPage += new EventHandler(ControlGoToThatPage);
            thu.goToThatPage += new EventHandler(ControlGoToThatPage);
            fri.goToThatPage += new EventHandler(ControlGoToThatPage);
            sat.goToThatPage += new EventHandler(ControlGoToThatPage);
            sun.deleteTimetableElement += new EventHandler(DeleteElementHandler);
            mon.deleteTimetableElement += new EventHandler(DeleteElementHandler);
            tue.deleteTimetableElement += new EventHandler(DeleteElementHandler);
            wed.deleteTimetableElement += new EventHandler(DeleteElementHandler);
            thu.deleteTimetableElement += new EventHandler(DeleteElementHandler);
            fri.deleteTimetableElement += new EventHandler(DeleteElementHandler);
            sat.deleteTimetableElement += new EventHandler(DeleteElementHandler);
            //MessageBox.Show("subject count : "+a.subjects.Count+"\n\n Timetablelist count : "+a.timetable.Count);
            SetupLiveTiles();
        }

        private void SetupLiveTiles()
        {
            PeriodicTask periodicTask = new PeriodicTask("PeriodicTaskDemo");
            periodicTask.Description = "Are presenting a periodic task";
            try
            {
                ScheduledActionService.Add(periodicTask);
                //ScheduledActionService.LaunchForTest("PeriodicTaskDemo", TimeSpan.FromSeconds(50));
                //MessageBox.Show("Open the background agent success");
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("exists already"))
                {
                    //MessageBox.Show("Since then the background agent success is already running");
                }
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    //MessageBox.Show("Background processes for this application has been prohibited");
                }
                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type has already been added."))
                {
                    //MessageBox.Show("You open the daemon has exceeded the hardware limitations");
                }
            }
            catch (SchedulerServiceException)
            {

            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
			PivotItem p = pivot.SelectedItem as PivotItem;
			//MessageBox.Show(p.Header.ToString());
            NavigationService.Navigate(new Uri("/InsertPeriod.xaml?day="+p.Header.ToString(), UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (firstStart)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                    pivot.SelectedIndex = 1;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    pivot.SelectedIndex = 0;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                    pivot.SelectedIndex = 2;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                    pivot.SelectedIndex = 3;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                    pivot.SelectedIndex = 4;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                    pivot.SelectedIndex = 5;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                    pivot.SelectedIndex = 6;
                firstStart = false;
            }
            pivot_LoadingPivotItem("fake obj", new PivotItemEventArgs((PivotItem)pivot.SelectedItem));
            // used to refresh page when a new subject is added to the list. To load new stuff added dynamically, instead or restarting the app to let the effects take place

            //MessagPrompt to help the users when the start the app for the first time only.
            /*
            if (!iss.Contains("MainPageFirstTime"))
            {
                var messagePrompt = new Coding4Fun.Phone.Controls.MessagePrompt
                {
                    Title = "Welcome!",
                    //Message = "Thank you for downloading The Time Table App\n This is your main page. It shows the time table, starting with the day today. Right now it will be completely empty, as no activities have been added.\n To add activities, you will first have to create Subjects. To do so, tap on the '+' sign below"
                };
                messagePrompt.Show();
                //iss.Add("MainPageFirstTime", a.subjects);
                
                MessageBox.Show("Thank you for downloading The Time Table App\n This is your main page. It shows the time table, starting with the day today. Right now it will be completely empty, as no activities have been added.\n To add activities, you will first have to create Subjects. To do so, tap on the '+' sign below", "Welcome!", MessageBoxButton.OK);
            }
            */
        }
        void ControlGoToThatPage(object sender, EventArgs e)
        {
            Grid s = sender as Grid;
            NavigationService.Navigate(new Uri("/SubjectDetails.xaml?sub="+s.Tag.ToString(), UriKind.Relative));
        }

        void DeleteElementHandler(object sender, EventArgs e)
        {
            //MessageBox.Show((sender as MenuItem).Tag.ToString());
            var messagePrompt = new MessagePrompt
            {
                Title = "Are you sure?",
                Message = "Are you sure you want to delete this entry?",
                IsCancelVisible = true,
                Tag = (sender as MenuItem).Tag
            };
            messagePrompt.Completed += messagePrompt_Completed;
            messagePrompt.Show();
        }

         void messagePrompt_Completed(object sender, Coding4Fun.Phone.Controls.PopUpEventArgs<string, Coding4Fun.Phone.Controls.PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
            {
                //MessageBox.Show((sender as MessagePrompt).Tag.ToString());
                a.timetable.Remove((TimetableElement)(sender as MessagePrompt).Tag);
                pivot_LoadingPivotItem("fake obj", new PivotItemEventArgs((PivotItem)pivot.SelectedItem));
            }
        }
            
        private void pivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item == null)
            {
                System.Diagnostics.Debug.WriteLine("pivot_LoadingPivotItem's EventArgs returns null");
                return;
            }
            System.Diagnostics.Debug.WriteLine("pivot loaded, we are good!");

            switch (e.Item.Header.ToString())
            {
                case "sunday":
                    sun.SubjectList.ItemsSource = null;
                    sun.SubjectList.ItemsSource = from x in a.timetable
                                                  where x.day == MyWeekDay.Sunday
                                                  orderby x.time.TimeOfDay
                                                  select new
                                                  {
                                                      time = x.time.Hour.ToString().PadLeft(2, '0') + ":" + x.time.Minute.ToString().PadLeft(2, '0'),
                                                      shortName = x.subject.ShortName,
                                                      to = (x.time.AddTicks(x.subject.Duration)).Hour.ToString().PadLeft(2, '0') + ":" + (x.time.AddTicks(x.subject.Duration)).Minute.ToString().PadLeft(2, '0'),
                                                      //longName = x.subject.LongName,
                                                      tte = x
                                                  };
                    break;
                case "monday":
                    mon.SubjectList.ItemsSource = null;
                    mon.SubjectList.ItemsSource = from x in a.timetable
                                                  where x.day == MyWeekDay.Monday
                                                  orderby x.time.TimeOfDay
                                                  select new
                                                  {
                                                      time = x.time.Hour.ToString().PadLeft(2, '0') + ":" + x.time.Minute.ToString().PadLeft(2, '0'),
                                                      shortName = x.subject.ShortName,
                                                      to = (x.time.AddTicks(x.subject.Duration)).Hour.ToString().PadLeft(2, '0') + ":" + (x.time.AddTicks(x.subject.Duration)).Minute.ToString().PadLeft(2, '0'),
                                                      //longName = x.subject.LongName,
                                                      tte = x
                                                  };
                    break;
                case "tuesday":
                    tue.SubjectList.ItemsSource = null;
                    tue.SubjectList.ItemsSource = from x in a.timetable
                                                  where x.day == MyWeekDay.Tuesday
                                                  orderby x.time.TimeOfDay
                                                  select new
                                                  {
                                                      time = x.time.Hour.ToString().PadLeft(2, '0') + ":" + x.time.Minute.ToString().PadLeft(2, '0'),
                                                      shortName = x.subject.ShortName,
                                                      to = (x.time.AddTicks(x.subject.Duration)).Hour.ToString().PadLeft(2, '0') + ":" + (x.time.AddTicks(x.subject.Duration)).Minute.ToString().PadLeft(2, '0'),
                                                      //longName = x.subject.LongName,
                                                      tte = x
                                                  };
                    break;
                case "wednesday":
                    wed.SubjectList.ItemsSource = null;
                    wed.SubjectList.ItemsSource = from x in a.timetable
                                                  where x.day == MyWeekDay.Wednesday
                                                  orderby x.time.TimeOfDay
                                                  select new
                                                  {
                                                      time = x.time.Hour.ToString().PadLeft(2, '0') + ":" + x.time.Minute.ToString().PadLeft(2, '0'),
                                                      shortName = x.subject.ShortName,
                                                      to = (x.time.AddTicks(x.subject.Duration)).Hour.ToString().PadLeft(2, '0') + ":" + (x.time.AddTicks(x.subject.Duration)).Minute.ToString().PadLeft(2, '0'),
                                                      //longName = x.subject.LongName,
                                                      tte = x
                                                  };
                    break;
                case "thursday":
                    thu.SubjectList.ItemsSource = null;
                    thu.SubjectList.ItemsSource = from x in a.timetable
                                                  where x.day == MyWeekDay.Thursday
                                                  orderby x.time.TimeOfDay
                                                  select new
                                                  {
                                                      time = x.time.Hour.ToString().PadLeft(2, '0') + ":" + x.time.Minute.ToString().PadLeft(2, '0'),
                                                      shortName = x.subject.ShortName,
                                                      to = (x.time.AddTicks(x.subject.Duration)).Hour.ToString().PadLeft(2, '0') + ":" + (x.time.AddTicks(x.subject.Duration)).Minute.ToString().PadLeft(2, '0'),
                                                      //longName = x.subject.LongName,
                                                      tte = x
                                                  };
                    break;
                case "friday":
                    fri.SubjectList.ItemsSource = null;
                    fri.SubjectList.ItemsSource = from x in a.timetable
                                                  where x.day == MyWeekDay.Friday
                                                  orderby x.time.TimeOfDay
                                                  select new
                                                  {
                                                      time = x.time.Hour.ToString().PadLeft(2, '0') + ":" + x.time.Minute.ToString().PadLeft(2, '0'),
                                                      shortName = x.subject.ShortName,
                                                      to = (x.time.AddTicks(x.subject.Duration)).Hour.ToString().PadLeft(2, '0') + ":" + (x.time.AddTicks(x.subject.Duration)).Minute.ToString().PadLeft(2, '0'),
                                                      //longName = x.subject.LongName,
                                                      tte = x
                                                  };
                    break;
                case "saturday":
                    sat.SubjectList.ItemsSource = null;
                    sat.SubjectList.ItemsSource = from x in a.timetable
                                                  where x.day == MyWeekDay.Saturday
                                                  orderby x.time.TimeOfDay
                                                  select new
                                                  {
                                                      time = x.time.Hour.ToString().PadLeft(2, '0') + ":" + x.time.Minute.ToString().PadLeft(2, '0'),
                                                      shortName = x.subject.ShortName,
                                                      to = (x.time.AddTicks(x.subject.Duration)).Hour.ToString().PadLeft(2, '0') + ":" + (x.time.AddTicks(x.subject.Duration)).Minute.ToString().PadLeft(2, '0'),
                                                      //longName = x.subject.LongName,
                                                      tte = x
                                                  };
                    break;
            }

        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/HowTo.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SeeAllSubjects.xaml", UriKind.Relative));
        }
    }
}
