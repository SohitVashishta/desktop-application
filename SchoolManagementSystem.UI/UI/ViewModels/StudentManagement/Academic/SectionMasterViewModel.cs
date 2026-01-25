using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Academic
{
    public class SectionMasterViewModel : BaseViewModel
    {
        private readonly ISectionService _sectionService;

        /* ================= DATA ================= */

        public ObservableCollection<SectionModel> Sections { get; } = new();
        public ObservableCollection<SectionModel> FilteredSections { get; } = new();

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }
            = new SnackbarMessageQueue();

        /* ================= FILTER ================= */

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        /* ================= DIALOG ================= */

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set { _isDialogOpen = value; OnPropertyChanged(); }
        }

        private SectionModel _editSection;
        public SectionModel EditSection
        {
            get => _editSection;
            set
            {
                _editSection = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DialogTitle));
            }
        }

        public string DialogTitle =>
            EditSection?.SectionId == 0 ? "Add Section" : "Edit Section";

        /* ================= COMMANDS ================= */

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }

        /* ================= CONSTRUCTOR ================= */

        public SectionMasterViewModel(ISectionService sectionService)
        {
            _sectionService = sectionService;

            AddCommand = new RelayCommand(OpenAddDialog);
            EditCommand = new RelayCommand<SectionModel>(OpenEditDialog);
            SaveCommand = new RelayCommand(async () => await SaveAsync());

            _ = LoadAsync();
        }

        /* ================= LOAD ================= */

        private async Task LoadAsync()
        {
            Sections.Clear();

            foreach (var s in await _sectionService.GetAllAsync())
                Sections.Add(s);

            ApplyFilter();
        }

        /* ================= FILTER ================= */

        private void ApplyFilter()
        {
            FilteredSections.Clear();

            var query = Sections.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(x =>
                    x.SectionName?.Contains(SearchText,
                        System.StringComparison.OrdinalIgnoreCase) == true);
            }

            foreach (var s in query)
                FilteredSections.Add(s);
        }

        /* ================= DIALOG ================= */

        private void OpenAddDialog()
        {
            EditSection = new SectionModel
            {
                IsActive = true
            };

            IsDialogOpen = true;
        }

        private void OpenEditDialog(SectionModel section)
        {
            if (section == null) return;

            EditSection = new SectionModel
            {
                SectionId = section.SectionId,
                SectionName = section.SectionName,
                IsActive = section.IsActive
            };

            IsDialogOpen = true;
        }

        private async Task SaveAsync()
        {
            if (EditSection == null ||
                string.IsNullOrWhiteSpace(EditSection.SectionName))
                return;

            if (EditSection.SectionId == 0)
            {
                await _sectionService.AddAsync(EditSection);
                SnackbarMessageQueue.Enqueue("Section added successfully");
            }
            else
            {
                await _sectionService.UpdateAsync(EditSection);
                SnackbarMessageQueue.Enqueue("Section updated successfully");
            }

            IsDialogOpen = false;
            await LoadAsync();
        }
    }
}
