using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class StudentAttendanceViewModel : INotifyPropertyChanged
    {
        private bool _isPresent;

        public int StudentId { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Display helper
        public string FullName => $"{FirstName} {LastName}";

        public bool IsPresent
        {
            get => _isPresent;
            set
            {
                if (_isPresent != value)
                {
                    _isPresent = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
