using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Coding4Fun.Phone.Controls;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace TimeTableApp
{
    public partial class PanoramaPage1 : PhoneApplicationPage
    {
        App a;
        Subject current;

        public PanoramaPage1()
        {
            InitializeComponent();
            a = App.Current as App;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string selected = "";
            if (NavigationContext.QueryString.TryGetValue("sub", out selected))
            {
                IEnumerable<Subject> s = from sub in a.subjects
                                            where sub.ShortName == selected
                                            select sub;
                current = s.ElementAt(0);
            }
            if (current != null)
            {
                panoramaControl.DataContext = current;
                LoadNotes();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("subject selection failed");
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            Coding4Fun.Phone.Controls.InputPrompt ip = new Coding4Fun.Phone.Controls.InputPrompt();
            ip.Completed += input_Completed;
            ip.Title = "New Note";
            ip.Message = "Enter the text content of the new note :";
            ip.Show();
        }

        // Ok sign clicked for Prompt, add notes!
        void input_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            //MessageBox.Show((sender as InputPrompt).Value);
            if ((sender as InputPrompt).Value.Length != 0)
            {
                current.notes.Add((sender as InputPrompt).Value);
                LoadNotes();
            }
        }

        // Context Menu's delete option tapped, deleting note.
        private void MenuItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //MessageBox.Show(sender.ToString());
            current.notes.Remove((sender as MenuItem).Tag.ToString());
            LoadNotes();
        }

        // Manage loading notes for the subject, along with message when there are no notes selected.
        private void LoadNotes()
        {
            notesListbox.ItemsSource = null;
            if (current.notes.Count != 0)
            {
                notesListbox.ItemsSource = current.notes;
                emptyNotesTB.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
                emptyNotesTB.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
