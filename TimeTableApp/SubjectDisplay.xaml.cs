using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Coding4Fun.Phone.Controls;

namespace TimeTableApp
{
	public partial class SubjectDisplay : UserControl
	{
        public event EventHandler goToThatPage;
        public event EventHandler deleteTimetableElement;
		public SubjectDisplay()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void Grid_Tap(object sender, GestureEventArgs e)
        {
            if(goToThatPage!=null)
            {
                goToThatPage(sender, e);
                //goToThatPage(sender, e);
                //NavigationService.Navigate(new Uri("/ProperSubjectInsertion.xaml?", UriKind.Relative));
            }
            //MessageBox.Show(p.Tag.ToString());
        }

        private void MenuItem_Tap(object sender, GestureEventArgs e)
        {
            if(sender!=null && deleteTimetableElement!=null)
                deleteTimetableElement(sender, e);
        }
	}
}