using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using TinyWorkflow.UITests.ServiceMock;
using TinyWorkflow.UITests.ServiceMock.Enums;
using TinyWorkflow.UITests.ViewModel.Model;
using TinyWorkflow.UITests.ViewModel.States;

namespace TinyWorkflow.UITests.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Services

        private RightManagementService rmService = new RightManagementService();
        private SideDataService sdService = new SideDataService();

        #endregion

        public Dispatcher Dispatcher { get; set; }

        #region Private Variables

        private Workflow<MainState> loadingWorkflow = new Workflow<MainState>();

        #endregion

        #region MainState
        private MainState _MainState = new MainState();

        /// <summary>
        /// Sets and gets the MainState property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MainState MainState
        {
            get
            {
                return _MainState;
            }

            set
            {
                if (_MainState == value)
                {
                    return;
                }

                _MainState = value;
                RaisePropertyChanged("MainState");
            }
        }
        #endregion


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////Gather all rights.
            //loadingWorkflow
            //    .Do(SetupLoading)
            //    .DoAsynch(IsReadOnlyUser, IsReadWriteUser, IsReadSuperUser)
            //    .Block(3)
            //    //Get user list
            //    .DoAsynch(GetUsers)
            //    .Block()
            //    .Foreach<string>(EnumerateUsers, new Workflow<Tuple<string, MainState>>()
            //                                        .Do(RegisterAccess)
            //                                        .Block())
            //    .Do(RemoveLoading);

            //Gather all rights.
            loadingWorkflow
                .Do(SetupLoading)
                .DoAsynch(IsReadOnlyUser, IsReadWriteUser, IsReadSuperUser)
                .Block(3)
                //Get user list
                .DoAsynch(GetUsers)
                .Block()
                .Foreach<string>(EnumerateUsers, new Workflow<Tuple<string, MainState>>()
                                                    .DoAsynch(RegisterAccess))
                .Block((s) => { return s.UserRights.Count(); })
                .Do(RemoveLoading);

            InitializedCommand = new RelayCommand(Initialized);
            loadingWorkflow.Start(MainState);
        }

        #region WriteManagement
        private void IsReadOnlyUser(MainState s)
        {
            rmService.HasPrivilege(Rights.ReadOnly, (result) =>
            {
                s.IsReadOnlyUser = result;
                loadingWorkflow.Unblock();
            }
            );
        }

        private void IsReadWriteUser(MainState s)
        {
            rmService.HasPrivilege(Rights.ReadWrite, (result) =>
            {
                s.IsReadWriteUser = result;
                loadingWorkflow.Unblock();
            }
            );
        }

        private void IsReadSuperUser(MainState s)
        {
            rmService.HasPrivilege(Rights.SuperUser, (result) =>
            {
                s.IsSuperUser = result;
                loadingWorkflow.Unblock();
            }
            );
        }
        #endregion

        #region UserManagement

        private void GetUsers(MainState s)
        {
            s.LoadingString = "Gathering users ...";
            sdService.GetUsers((result) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (var item in result)
                        {
                            s.UserRights.Add(new UserRight() { User = item });
                        }
                        s.MaxLoadingLevel = result.Count();
                        loadingWorkflow.Unblock();
                    }));
            }
            );
        }

        private IEnumerable<string> EnumerateUsers(MainState s)
        {
            return s.UserRights.Select(c => c.User).ToList();
        }

        private void RegisterAccess(Tuple<string, MainState> t)
        {
            t.Item2.LoadingString = "Get data for user " + t.Item1 + " ...";
            sdService.GetUserSections(t.Item1, (result) =>
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                   {
                       UserRight found = t.Item2.UserRights.FirstOrDefault(c => c.User == t.Item1);
                       if (found != null)
                       {
                           lock (t.Item2)
                           {
                               found.Rights.Clear();
                               foreach (var item in result)
                               {
                                   found.Rights.Add(item);
                               }
                           }
                       }
                       t.Item2.ActualLoadingLevel++;
                       loadingWorkflow.Unblock();
                   }));
                });
        }

        #endregion

        #region Loading

        public void SetupLoading(MainState s)
        {
            s.IsLoading = true;
            s.LoadingString = "Checking user rights...";

        }

        public void RemoveLoading(MainState s)
        {
            s.IsLoading = false;
        }

        #endregion

        #region Commands

        public RelayCommand InitializedCommand { get; set; }

        public void Initialized()
        {

        }

        #endregion
    }
}