using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Library
{
    public class IssueReturnHistoryViewModel : BaseViewModel
    {
        private readonly IBookTransactionService _service;

        public ObservableCollection<BookTransactionModel> Transactions { get; set; }

        #region Fine Summary

        private decimal _totalCollectedFine;
        public decimal TotalCollectedFine
        {
            get => _totalCollectedFine;
            set { _totalCollectedFine = value; OnPropertyChanged(); }
        }

        private decimal _pendingFine;
        public decimal PendingFine
        {
            get => _pendingFine;
            set { _pendingFine = value; OnPropertyChanged(); }
        }

        #endregion

        #region Filters

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                CurrentPage = 1;
                LoadData();
            }
        }

        public List<string> StatusList { get; set; }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
                CurrentPage = 1;
                LoadData();
            }
        }

        #endregion

        #region Pagination

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }

        private int _pageSize = 10;
        public int PageSize => _pageSize;

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set { _totalPages = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand ReturnBookCommand { get; }
        public ICommand IssueBookCommand { get; }

        #endregion

        public IssueReturnHistoryViewModel(IBookTransactionService service)
        {
            _service = service;

            Transactions = new ObservableCollection<BookTransactionModel>();

            StatusList = new List<string>
            {
                null,      // All
                "Issued",
                "Returned"
            };

            NextPageCommand = new RelayCommand(OnNextPage, () => CurrentPage < TotalPages);
            PreviousPageCommand = new RelayCommand(OnPreviousPage, () => CurrentPage > 1);
            IssueBookCommand = new RelayCommand(OpenIssueBookDialog);
            ReturnBookCommand = new RelayCommand<BookTransactionModel>(
                OnReturnBook,
                CanReturnBook);

            LoadData();
        }

        #region Return Logic
        private void OpenIssueBookDialog()
        {
            var dialog = new IssueBookDialog();
            var vm = new IssueBookDialogViewModel(_service);

            dialog.DataContext = vm;

            vm.CloseAction = () =>
            {
                dialog.DialogResult = true;  // 🔥 VERY IMPORTANT
                dialog.Close();
            };

            dialog.Owner = Application.Current.MainWindow;

            if (dialog.ShowDialog() == true)
            {
                SelectedStatus = null;
                CurrentPage = 1;
                LoadData();
            }
        }




        private bool CanReturnBook(BookTransactionModel model)
        {
            return model != null && model.Status == "Issued";
        }
        private void OnIssueBook()
        {
            try
            {
                // Example dummy data (replace with popup later)
                int bookId = 1;
                int studentName = 123;
                DateTime issueDate = DateTime.Now;
                DateTime dueDate = DateTime.Now.AddDays(7);

                _service.IssueBook(bookId, studentName, issueDate, dueDate);

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnReturnBook(BookTransactionModel model)
        {
            if (model == null) return;

            var result = MessageBox.Show(
                $"Return book for {model.StudentName}?",
                "Confirm Return",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                _service.ReturnBook(model.TransactionId);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Load Data

        private void LoadData()
        {
            Transactions.Clear();

            // 🔹 Fine Summary
            var summary = _service.GetFineSummary();
            TotalCollectedFine = summary.totalCollectedFine;
            PendingFine = summary.pendingFine;

            // 🔹 Pagination Calculation
            var totalRecords = _service.GetCount(SearchText, SelectedStatus);
            TotalPages = totalRecords == 0
                ? 1
                : (int)Math.Ceiling((double)totalRecords / PageSize);

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            // 🔹 Load Data
            var list = _service.GetPaged(SearchText, SelectedStatus, CurrentPage, PageSize);

            foreach (var item in list)
                Transactions.Add(item);

            // 🔹 Refresh Commands
            (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (ReturnBookCommand as RelayCommand<BookTransactionModel>)?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Pagination

        private void OnNextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadData();
            }
        }

        private void OnPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadData();
            }
        }

        #endregion
    }
}
