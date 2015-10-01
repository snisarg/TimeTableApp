using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace TimeTableApp
{
    [DataContract]
    public class Subject
    {
        private TimeSpan m_dur;
        [IgnoreDataMember]
        public TimeSpan Dur
        {
            get { return m_dur; }
            set { m_dur = value; }
        }
        [DataMember]
        public long Duration
        {
            get { return m_dur.Ticks; }
            set { m_dur = new TimeSpan(value); }
        }
        [DataMember]
        public string ShortName { get; set; }
        [DataMember]
        public string LongName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public List<string> notes{get;set;}

        public Subject(string sn, string ln, TimeSpan t, string desc)
        {
            ShortName = sn;
            LongName = ln;
			if(t!=null)
				m_dur = t;
			else
				m_dur = new TimeSpan(0,1,0,0);
            notes = new List<string>();
            Description = desc;
        }

    }
}
