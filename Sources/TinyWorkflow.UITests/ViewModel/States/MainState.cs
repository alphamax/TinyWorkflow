using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyWorkflow.UITests.ViewModel.Model;

namespace TinyWorkflow.UITests.ViewModel.States
{
    public class MainState : ViewModelBase
    {
        public MainState()
        {
        }

        #region IsReadOnlyUser
        private bool _IsReadOnlyUser = false;

        public bool IsReadOnlyUser
        {
            get
            {
                return _IsReadOnlyUser;
            }

            set
            {
                if (_IsReadOnlyUser == value)
                {
                    return;
                }

                _IsReadOnlyUser = value;
                RaisePropertyChanged("IsReadOnlyUser");
            }
        }
        #endregion

        #region IsReadWriteUser
        private bool _IsReadWriteUser = false;

        public bool IsReadWriteUser
        {
            get
            {
                return _IsReadWriteUser;
            }

            set
            {
                if (_IsReadWriteUser == value)
                {
                    return;
                }

                _IsReadWriteUser = value;
                RaisePropertyChanged("IsReadWriteUser");
            }
        }
        #endregion

        #region IsSuperUser
        private bool _IsSuperUser = false;

        /// <summary>
        /// Sets and gets the IsSuperUser property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSuperUser
        {
            get
            {
                return _IsSuperUser;
            }

            set
            {
                if (_IsSuperUser == value)
                {
                    return;
                }

                _IsSuperUser = value;
                RaisePropertyChanged("IsSuperUser");
            }
        }
        #endregion

        #region UserRights
        private ObservableCollection<UserRight> _UserRights = new ObservableCollection<UserRight>();

        /// <summary>
        /// Sets and gets the UserRights property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<UserRight> UserRights
        {
            get
            {
                return _UserRights;
            }

            set
            {
                if (_UserRights == value)
                {
                    return;
                }

                _UserRights = value;
                RaisePropertyChanged("UserRights");
            }
        }
        #endregion

        // Loading Stuff

        #region MaxLoadingLevel
        private int _MaxLoadingLevel = 0;

        /// <summary>
        /// Sets and gets the MaxLoadingLevel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int MaxLoadingLevel
        {
            get
            {
                return _MaxLoadingLevel;
            }

            set
            {
                if (_MaxLoadingLevel == value)
                {
                    return;
                }

                _MaxLoadingLevel = value;
                RaisePropertyChanged("MaxLoadingLevel");
            }
        }
        #endregion

        #region ActualLoadingLevel
        private int _ActualLoadingLevel = 0;

        /// <summary>
        /// Sets and gets the ActualLoadingLevel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ActualLoadingLevel
        {
            get
            {
                return _ActualLoadingLevel;
            }

            set
            {
                if (_ActualLoadingLevel == value)
                {
                    return;
                }

                _ActualLoadingLevel = value;
                RaisePropertyChanged("ActualLoadingLevel");
            }
        }
        #endregion

        #region IsLoading
        private bool _IsLoading = false;

        /// <summary>
        /// Sets and gets the IsLoading property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return _IsLoading;
            }

            set
            {
                if (_IsLoading == value)
                {
                    return;
                }

                _IsLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }
        #endregion

        #region LoadingString
        private string _LoadingString = string.Empty;

        /// <summary>
        /// Sets and gets the LoadingString property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string LoadingString
        {
            get
            {
                return _LoadingString;
            }

            set
            {
                if (_LoadingString == value)
                {
                    return;
                }

                _LoadingString = value;
                RaisePropertyChanged("LoadingString");
            }
        }
        #endregion
    }
}
