using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using App.Business.BusinessLayer;
using App.Business.DataAccessLayer;
using App.Business.Infrastructure;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            mUnitOfWorkFactory = new UnitOfWorkFactory();
            mParser = new DirtyModelParser();
            mImportHelper = new ImportHelper();
            Init();
        }

        private const int cItemsPerPage = 20;
        private static IUnitOfWorkFactory mUnitOfWorkFactory;
        private static IDirtyModelParser mParser;
        private readonly ImportHelper mImportHelper;

        private bool mIsBusy;
        public bool IsBusy
        {
            get { return mIsBusy; }
            set { Set(() => IsBusy, ref mIsBusy, value); }
        }


        private int mCurrentPage;
        public int CurrentPage
        {
            get { return mCurrentPage; }
            set { Set(() => CurrentPage, ref mCurrentPage, value); }
        }

        private int mTotalCount;
        public int TotalCount
        {
            get { return mTotalCount; }
            set
            {
                Set(() => TotalCount, ref mTotalCount, value);
            }
        }

        private ObservableCollection<QuestionnaireWrapper> mItems;
        public ObservableCollection<QuestionnaireWrapper> Items
        {
            get { return mItems; }
            set { Set(() => Items, ref mItems, value); }
        }

        private async void Init()
        {
            IsBusy = true;
            await LoadItems();
            IsBusy = false;
        }

        private async Task LoadItems()
        {
            try
            {
                using (var uof = mUnitOfWorkFactory.CreateUnitOfWork())
                {
                    var res = await uof.GetItems(CurrentPage, cItemsPerPage);
                    TotalCount = res.TotalCount;
                    Items = new ObservableCollection<QuestionnaireWrapper>(res.Items.Select(a => new QuestionnaireWrapper(a)));
                }
            }
            catch (Exception ex)
            {
            }
        }

        private RelayCommand mNextPageCommand;
        public RelayCommand NextPageCommand
            => mNextPageCommand ?? (mNextPageCommand = new RelayCommand(OnNextPage, CanNextPage));

        private bool CanNextPage()
        {
            return !IsBusy && TotalCount != 0 && mCurrentPage < TotalCount/ cItemsPerPage;
        }

        private void OnNextPage()
        {
            CurrentPage++;
            Init();
        }

        private RelayCommand mPreviousPageCommand;
        public RelayCommand PreviousPageCommand => mPreviousPageCommand ?? (mPreviousPageCommand = new RelayCommand(OnPreviousPage, CanPreviousPage));

        private bool CanPreviousPage()
        {
            return !IsBusy && mCurrentPage != 0;
        }

        private void OnPreviousPage()
        {
            CurrentPage--;
            Init();
        }

        private RelayCommand mUploadFileCommand;
        public RelayCommand UploadFileCommand => mUploadFileCommand ?? (mUploadFileCommand = new RelayCommand(OnUploadFile));

        private async void OnUploadFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.DefaultExt = ".tsv";
            dialog.Filter = "TSV files (.tsv)|*.tsv";

            if (dialog.ShowDialog() == true)
            {
                IsBusy = true;
                await mImportHelper.Import(dialog.OpenFile());
                await LoadItems();
                IsBusy = false;
            }
        }

        public class QuestionnaireWrapper : ObservableObject, IEditableObject
        {
            private readonly Questionnaire mQuestionnaire;
            private string mName;
            private DateTime mDateOfBirth;
            private string mEmail;
            private string mPhone;

            public QuestionnaireWrapper(Questionnaire questionnaire)
            {
                mQuestionnaire = questionnaire;
                Init();
            }

            private void Init()
            {
                mName = mQuestionnaire.Name;
                mDateOfBirth = mQuestionnaire.DateOfBirth;
                mEmail = mQuestionnaire.Email;
                mPhone = mQuestionnaire.Phone;
            }

            public Guid ID
            {
                get { return mQuestionnaire.ID; }
            }

            public string Name
            {
                get { return mName; }
                set { Set(() => Name, ref mName, value); }
            }

            public DateTime DateOfBirth
            {
                get { return mDateOfBirth; }
                set { Set(() => DateOfBirth, ref mDateOfBirth, value); }
            }

            public string Email
            {
                get { return mEmail; }
                set { Set(() => Email, ref mEmail, value); }
            }

            public string Phone
            {
                get { return mPhone; }
                set { Set(() => Phone, ref mPhone, value); }
            }

            public void BeginEdit()
            {
            }

            public async void EndEdit()
            {
                try
                {
                    bool hasChanged = false;
                    Name = mParser.ParseName(mName);
                    if (mName != mQuestionnaire.Name)
                    {
                        hasChanged = true;
                        mQuestionnaire.Name = mName;
                    }
                    if (mDateOfBirth != mQuestionnaire.DateOfBirth)
                    {
                        hasChanged = true;
                        mQuestionnaire.DateOfBirth = mDateOfBirth;
                    }
                    Email = mParser.ParseEmail(mEmail);
                    if (mEmail != mQuestionnaire.Email)
                    {
                        hasChanged = true;
                        mQuestionnaire.Email = mEmail;
                    }
                    Phone = mParser.ParsePhone(mPhone);
                    if (mQuestionnaire.Phone != mPhone)
                    {
                        hasChanged = true;
                        mQuestionnaire.Phone = mPhone;
                    }
                    if (hasChanged)
                    {
                        using (var uof = mUnitOfWorkFactory.CreateUnitOfWork())
                        {
                            uof.AddOrUpdate(mQuestionnaire);
                            await uof.SaveAsync();
                        }
                    }
                }
                catch
                {
                    CancelEdit();   
                }
            }

            public void CancelEdit()
            {
                Init();
            }
        }
    }
}
