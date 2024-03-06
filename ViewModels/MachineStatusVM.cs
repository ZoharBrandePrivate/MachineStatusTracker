using MachineStatusTracker.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MachineStatusTracker.ViewModels
{
    /// <summary>
    /// View Model class for managing machine statuses.
    /// </summary>
    public class MachineStatusVM : INotifyPropertyChanged
    {
        private ObservableCollection<MachineStatus> _machineStatuses;

        /// <summary>
        /// Gets or sets the collection of machine statuses.
        /// </summary>
        public ObservableCollection<MachineStatus> MachineStatuses
        {
            get { return _machineStatuses; }
            private set { _machineStatuses = value; OnPropertyChanged("MachineStatuses"); }
        }

        /// <summary>
        /// Initializes a new instance of the MachineStatusVM class.
        /// </summary>
        public MachineStatusVM()
        {
            _machineStatuses = new ObservableCollection<MachineStatus>();
            InitTestData(5);
        }

        /// <summary>
        /// Initializes test data for machine statuses.
        /// </summary>
        /// <param name="dataSize">The number of test data to generate.</param>
        public void InitTestData(int dataSize)
        {
            for (int i = 0; i < dataSize; i++)
            {
                MachineStatus machineStatus = new MachineStatus(this)
                {
                    Name = "machine" + i,
                    Description = "description for machine" + i,
                    Status = MachineOperationalStatus.Running,
                    Notes = "my notes for machine" + i
                };
                _machineStatuses.Add(machineStatus);
            }
            MachineStatuses = _machineStatuses;
        }

        /// <summary>
        /// Deletes a machine status from the collection.
        /// </summary>
        /// <param name="deletedMachine">The machine status to delete.</param>
        /// <returns>True if the machine status is successfully deleted; otherwise, false.</returns>
        public bool DeleteMachineStatus(MachineStatus deletedMachine)
        {
            bool deleted = _machineStatuses.Remove(deletedMachine);
            OnPropertyChanged("MachineStatuses");
            return deleted;
        }

        /// <summary>
        /// Adds a new machine status to the collection.
        /// </summary>
        /// <returns>True if the machine status is successfully added; otherwise, false.</returns>
        public bool AddNewMachineStatus()
        {
            MachineStatus newMachine = new MachineStatus(this);
            EditMachineStatusWindow editMachineStatusWindow = new EditMachineStatusWindow(newMachine);
            bool? result = editMachineStatusWindow.ShowDialog();
            if (!result.HasValue) { return false; }
            if (_machineStatuses == null)
            {
                _machineStatuses = new ObservableCollection<MachineStatus>();
            }
            if (_machineStatuses.Contains(newMachine))
            {
                return false;
            }
            _machineStatuses.Add(newMachine);
            OnPropertyChanged("MachineStatuses");
            return true;
        }

        /// <summary>
        /// Edits a machine status in the collection.
        /// </summary>
        /// <param name="editMachine">The machine status to edit.</param>
        /// <returns>True if the machine status is successfully edited; otherwise, false.</returns>
        public bool EditMachineStatus(MachineStatus editMachine)
        {
            EditMachineStatusWindow editMachineStatusWindow = new EditMachineStatusWindow(editMachine);
            bool? result = editMachineStatusWindow.ShowDialog();
            if (!result.HasValue) { return false; }
            return result.Value;
        }

        #region INotifyCollectionChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = propertyExpression.Body as MemberExpression;
                Debug.Assert(memberExpr != null, "memberExpr != null");
                string propertyName = memberExpr.Member.Name;
                OnPropertyChanged(propertyName);
            }
        }
        #endregion
    }
}
