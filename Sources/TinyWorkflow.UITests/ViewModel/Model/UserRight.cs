using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.UITests.ViewModel.Model
{
    public class UserRight : ViewModelBase
    {
        #region User
        private string _User = string.Empty;

        /// <summary>
        /// Sets and gets the User property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string User
        {
            get
            {
                return _User;
            }

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
                RaisePropertyChanged("User");
            }
        }
        #endregion

        #region Rights
        private ObservableCollection<string> _Rights = new ObservableCollection<string>();

        /// <summary>
        /// Sets and gets the Rights property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<string> Rights
        {
            get
            {
                return _Rights;
            }

            set
            {
                if (_Rights == value)
                {
                    return;
                }

                _Rights = value;
                RaisePropertyChanged("Rights");
            }
        }
        #endregion
    }
}
