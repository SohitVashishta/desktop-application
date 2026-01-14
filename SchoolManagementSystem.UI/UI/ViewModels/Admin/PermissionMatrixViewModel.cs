using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.ViewModels;
using System.Collections.ObjectModel;
namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    public class PermissionMatrixViewModel : BaseViewModel
    {
        private readonly PermissionService _service;

        public ObservableCollection<RoleDto> Roles { get; set; }
        public ObservableCollection<PermissionRowViewModel> Permissions { get; set; }

        private RoleDto _selectedRole;
        public RoleDto SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
                _ = LoadMatrixAsync();
            }
        }

        public RelayCommand SaveCommand { get; }

        public PermissionMatrixViewModel()
        {
            _service = new PermissionService();
            SaveCommand = new RelayCommand(SaveAsync);
            _ = InitAsync();
        }

        private async Task InitAsync()
        {
            Roles = new ObservableCollection<RoleDto>(await _service.GetRolesAsync());
            OnPropertyChanged(nameof(Roles));
            SelectedRole = Roles.FirstOrDefault();
        }

        private async Task LoadMatrixAsync()
        {
            if (SelectedRole == null) return;

            var permissions = await _service.GetPermissionsAsync();
            var rolePerms = await _service.GetRolePermissionsAsync(SelectedRole.RoleId);

            Permissions = new ObservableCollection<PermissionRowViewModel>(
                permissions.Select(p =>
                {
                    var match = rolePerms.FirstOrDefault(r => r.PermissionId == p.PermissionId);
                    return new PermissionRowViewModel
                    {
                        PermissionId = p.PermissionId,
                        Code = p.Code,
                        IsAllowed = match?.IsAllowed ?? false
                    };
                }));

            OnPropertyChanged(nameof(Permissions));
        }

        private async Task SaveAsync()
        {
            foreach (var row in Permissions)
            {
                await _service.SaveAsync(new RolePermissionDto
                {
                    RoleId = SelectedRole.RoleId,
                    PermissionId = row.PermissionId,
                    IsAllowed = row.IsAllowed
                });
            }
        }
    }
}