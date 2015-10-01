using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Linq;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using TimeTableApp;
using System;

namespace ScheduledTaskAgent1
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        public List<Subject> subjects { get; set; }
        public List<TimetableElement> timetable;
        IsolatedStorageSettings iss = IsolatedStorageSettings.ApplicationSettings;

        private static volatile bool _classInitialized;

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;
                // Subscribe to the managed exception handler
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        /// Code to execute on Unhandled Exceptions
        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            ShellTile TileToFind = ShellTile.ActiveTiles.First();
            if (TileToFind != null)
            {
                if (!iss.Contains("sub") && !iss.Contains("tt"))
                {
                    NotifyComplete();
                    return;
                }

                if (iss.Contains("sub"))
                    subjects = (List<Subject>)iss["sub"];
                if (iss.Contains("tt"))
                    timetable = (List<TimetableElement>)iss["tt"];

                MyWeekDay mwd = MyWeekDay.Monday;
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                    mwd = MyWeekDay.Monday;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    mwd = MyWeekDay.Sunday;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                    mwd = MyWeekDay.Tuesday;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
                    mwd = MyWeekDay.Wednesday;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
                    mwd = MyWeekDay.Thursday;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                    mwd = MyWeekDay.Friday;
                else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                    mwd = MyWeekDay.Saturday;

                var CurrentSubject = from atte in timetable
                                     where atte.day == mwd
                                     && atte.time.TimeOfDay >= DateTime.Now.TimeOfDay
                                     orderby atte.time.TimeOfDay
                                     select atte;

                string BackContentString = "-";
                StandardTileData NewTileData;
                if (CurrentSubject.Count() > 0)
                {
                    BackContentString = CurrentSubject.First().subject.ShortName + "\r" + CurrentSubject.First().time.Hour.ToString().PadLeft(2, '0') + ":" + CurrentSubject.First().time.Minute.ToString().PadLeft(2, '0') + " - " + (CurrentSubject.First().time.AddTicks(CurrentSubject.First().subject.Duration)).Hour.ToString().PadLeft(2, '0') + ":" + (CurrentSubject.First().time.AddTicks(CurrentSubject.First().subject.Duration)).Minute.ToString().PadLeft(2, '0');
                    NewTileData = new StandardTileData
                    {
                        Title = "Time Table App",
                        BackTitle = "Time Table App",
                        BackContent = BackContentString
                    };
                }
                else
                {
                    NewTileData = new StandardTileData
                    {
                        //BackBackgroundImage = new Uri("IDontExist", UriKind.Relative),
                        BackContent = string.Empty,
                        BackTitle = string.Empty
                    };
                }
                
                TileToFind.Update(NewTileData);
            }
            NotifyComplete();
        }
    }
}