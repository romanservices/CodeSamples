using System;
using System.ComponentModel;
using System.Windows.Input;
using PEG.Domain;
using PEG.Services;
using PEG.App.Framework;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using System.Collections.ObjectModel;

namespace App.ViewModel
{
    public class ManageOfficesViewModel : ViewModelBase
    {
        #region Fields

        ICommand deleteOfficeCommand;
        ICommand viewOfficeCommand;
        ICommand editOfficeCommand;
        ICommand newOfficeCommand;
        private ICommand searchCommand;
        private ICommand sortCommand;
        private PagedList<Office> officeList;
        private ISenatorService senatorService;
        private PagerViewModel pagerViewModel;
        private OfficeSearch search;
        private ISystemLogService _systemLogService;
        private SortDirection sortDirection;
        private IList<Senator> senators;
        private string _searchText;

        #endregion

        #region Constructor

        public ManageOfficesViewModel()
        {
            Initialize();
            App.Messenger.Register<RefreshOfficesMessage>(MessageRequest.RefreshOffices,p=> GetOffices());
            App.Messenger.Register<RefreshSenatorsMessage>(MessageRequest.RefreshSenators, p=>GetOffices());
            App.Messenger.Register<ViewSenatorMessage>(MessageRequest.ViewSenator,p=>ViewSenatorOffices(p.SenatorID));
        }

        #endregion

        #region Properties

        public Senator Senator { get; set; }

        #endregion

        #region Presentation Properties

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                SearchList();
                RaisePropertyChanged("SearchText");
            }
        }

        public PagerViewModel PagerView
        {
            get
            {
                if (pagerViewModel == null)
                {
                    pagerViewModel = new PagerViewModel();

                    pagerViewModel.FirstCommand = new RelayCommand(() =>
                    {
                        officeList.PageIndex = 1;
                        officeList.Refresh();
                        RaisePropertyChanged("OfficeList");
                    });
                    pagerViewModel.LastCommand = new RelayCommand(() =>
                    {
                        officeList.PageIndex = officeList.TotalPages;
                        officeList.Refresh();
                        RaisePropertyChanged("OfficeList");
                    });
                    pagerViewModel.PreviousCommand = new RelayCommand(() =>
                    {
                        
                            officeList.PageIndex--;
                            officeList.Refresh();
                            RaisePropertyChanged("OfficeList");
                        
                    });
                    pagerViewModel.NextCommand = new RelayCommand(() =>
                    {
                        
                            officeList.PageIndex++;
                            officeList.Refresh();
                            RaisePropertyChanged("OfficeList");
                        
                    });
                }
                return pagerViewModel;
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                if(searchCommand == null)
                {
                    searchCommand = new RelayCommand(SearchList);
                }
                return searchCommand;
            }
        }
        /// <summary>
        /// Returns a command that saves the customer.
        /// </summary>
        public ICommand DeleteOfficeCommand
        {
            get
            {
                if (deleteOfficeCommand == null)
                {
                    deleteOfficeCommand = new RelayCommand<Guid>(
                        (param) => this.DeleteOffice(param)
                    );
                }
                return deleteOfficeCommand;
            }
        }

        /// <summary>
        /// Returns a command that saves the customer.
        /// </summary>
        public ICommand ViewOfficeCommand
        {
            get
            {
                if (viewOfficeCommand == null)
                {
                    viewOfficeCommand = new RelayCommand<Guid>(
                        (param) => this.ViewOffice(param)
                    );
                }
                return viewOfficeCommand;
            }
        }

        /// <summary>
        /// Returns a command that saves the customer.
        /// </summary>
        public ICommand NewOfficeCommand
        {
            get
            {
                if (newOfficeCommand == null)
                {
                    newOfficeCommand = new RelayCommand(NewOffice);
                }
                return newOfficeCommand;
            }
        }

        /// <summary>
        /// Returns a command that saves the customer.
        /// </summary>
        public ICommand EditOfficeCommand
        {
            get
            {
                if (editOfficeCommand == null)
                {
                    editOfficeCommand = new RelayCommand<Guid>(EditOffice);
                }
                return editOfficeCommand;
            }
        }

        public ObservableCollection<Office> OfficeList
        {
            get
            {
                if (officeList == null)
                {
                    GetOffices();
                }
                return officeList.ToObservableCollection<Office>();
            }
        }
        public ICommand SortCommand
        {
            get
            {
                if (sortCommand == null)
                {
                    sortCommand = new RelayCommand<string>((param) =>
                    {
                        
                        if (sortDirection == SortDirection.Ascending) sortDirection = SortDirection.Descending;
                        else sortDirection = sortDirection = SortDirection.Ascending;

                        if (sortDirection == SortDirection.Ascending) officeList.OrderBy(param);
                        else officeList.OrderByDescending(param);
                        RaisePropertyChanged("OfficeList");
                    });
                }
                return sortCommand;
            }
        }

        #endregion

        #region Public Methods

        public override void Open()
        {
            base.Open();
            search = new OfficeSearch
            {
                SortColumn = "OfficeNumber",
                SortDirection = SortDirection.Ascending
            };
            this.senatorService = ServiceFactory.CreateSenatorService(SessionConnection);
            _systemLogService = ServiceFactory.CreateSystemLogService(SessionConnection);
            SearchText = "";
            GetOffices();

        }

        public override void Close()
        {
            base.Close();
            deleteOfficeCommand = null;
            viewOfficeCommand = null;
            editOfficeCommand = null;
            newOfficeCommand = null;
            pagerViewModel = null;
            officeList = null;
            senatorService = null;
            search = null;
            _systemLogService = null;
            Senator = null;
            senators = null;
        }

        public void NewOffice()
        {
            UIHelper.SetBusyState();
            App.Messenger.NotifyColleagues(new NavigateMessage(ApplicationFactory.Resolve<EditOfficeViewModel>()));

            if(Senator != null)
            {App.Messenger.NotifyColleagues(new CreateOfficeMessage(Senator.SenatorID));}
            else
            {
                App.Messenger.NotifyColleagues(new NewOfficeMessage());
            }
        }

        public void GetOffices()
        {
            if(senatorService == null)
                Open();
          
            if (Senator != null)
            {

                search.Senator = Senator;
            }
            if(senators != null)
            {
                search.Senators = senators;
            }
            officeList = senatorService.GetOffices(search, 1, PagerView.PageSize);
            pagerViewModel.TotalItems = officeList.TotalCount;
            RaisePropertyChanged("OfficeList");
        }
        public void ViewSenatorOffices(Guid senatorID)
        {
            if (senatorService == null)
                Open();
            Senator = senatorService.GetSenatorByID(senatorID);
            if (Senator != null)
            {

                search.Senator = Senator;
            }
            if (senators != null)
            {
                search.Senators = senators;
            }
            officeList = senatorService.GetOffices(search, 1, PagerView.PageSize);
            pagerViewModel.TotalItems = officeList.TotalCount;
            RaisePropertyChanged("OfficeList");
            
        }
        public void EditOffice(Guid officeID)
        {
            UIHelper.SetBusyState();
            App.Messenger.NotifyColleagues(new NavigateMessage(ApplicationFactory.Resolve<EditOfficeViewModel>()));
            App.Messenger.NotifyColleagues(new EditOfficeMessage(officeID));
        }

        public void DeleteOffice(Guid officeID)
        {

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this office?",
                                                        "Delete Office",
                                                        MessageBoxButton.OKCancel,
                                                        MessageBoxImage.Question);

            Office thisOffice = senatorService.GetOfficeByID(officeID);
            thisOffice.OfficeStatus = OfficeStatus.Deleted;
            senatorService.SaveOffice(thisOffice);
            GetOffices();
              _systemLogService.CreateSystemLog(officeID.ToString(), "Delete Office", App.CurrentUser.UserID);
            App.Messenger.NotifyColleagues(new RefreshOfficesMessage());
        }

        public void ViewOffice(Guid officeID)
        {
            UIHelper.SetBusyState();
            App.Messenger.NotifyColleagues(new NavigateMessage(ApplicationFactory.Resolve<OfficeDetailsViewModel>()));
            App.Messenger.NotifyColleagues(new ViewOfficeMessage(officeID));
        }
        private void SearchList()
        {

            senators = senatorService.GetSenatorsByNameLike(SearchText);
            GetOffices();
        }
        #endregion

        #region Private Helpers


        #endregion

        #region IDataErrorInfo Members
        #endregion

    }
}