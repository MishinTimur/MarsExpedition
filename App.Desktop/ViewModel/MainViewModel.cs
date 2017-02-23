﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            mImportHelper = new ImportHelper();
            Init();
        }

        private const int cItemsPerPage = 20;
        private readonly IUnitOfWorkFactory mUnitOfWorkFactory;
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

        private ObservableCollection<Questionnaire> mItems;
        public ObservableCollection<Questionnaire> Items
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
                    Items = new ObservableCollection<Questionnaire>(res.Items);
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
    }
}
