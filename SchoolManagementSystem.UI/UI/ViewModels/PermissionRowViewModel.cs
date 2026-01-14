using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.ViewModels;

public class PermissionRowViewModel : BaseViewModel
{
    public int PermissionId { get; set; }
    public string Code { get; set; }

    private bool _isAllowed;
    public bool IsAllowed
    {
        get => _isAllowed;
        set
        {
            _isAllowed = value;
            OnPropertyChanged(nameof(IsAllowed));
        }
    }
}
