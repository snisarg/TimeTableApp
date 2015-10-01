using System;
using System.Runtime.Serialization;

namespace TimeTableApp
{
    [DataContract]
    public class TimetableElement
    {
        [DataMember]
        public DateTime time { get; set; }
        [DataMember]
        public MyWeekDay day { get; set; }
        [DataMember]
        public Subject subject { get; set; }

        public TimetableElement(DateTime t, MyWeekDay d, Subject s)
        {
            time = t;
            day = d;
            subject = s;
        }
    }
}
