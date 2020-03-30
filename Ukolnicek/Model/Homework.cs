using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ukolnicek.Model
{
    public class Homework : INotifyPropertyChanged
    {
        private string _name;
        private bool _isDone;
        private Subject _subject;
        private DateTimeOffset _date;
        private string _description;
        public string Name { get { return _name; } set { _name = value; NotifyPropertyChanged(); } }
        public Subject Subject { get { return _subject; } set { _subject = value; NotifyPropertyChanged(); } }
        public DateTimeOffset Date { get { return _date; } set { _date = value; NotifyPropertyChanged(); } }
        public bool IsDone { get { return _isDone; } set { _isDone = value; NotifyPropertyChanged(); } }
        public string Description { get { return _description; } set { _description = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name + " " + Subject.ToString(); 
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
