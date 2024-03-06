using MachineStatusTracker.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MachineStatusTracker.ViewModels
{
    public class MachineStatusVM :INotifyPropertyChanged
    {
        private ObservableCollection<MachineStatus> _machineStatuses;
        public ObservableCollection<MachineStatus> MachineStatuses { get { return _machineStatuses; } private set { _machineStatuses = value; OnPropertyChanged("MachineStatuses"); } }

        public MachineStatusVM()
        {
            _machineStatuses = new ObservableCollection<MachineStatus>();
            InitTestData(5);
        }
       

        public void InitTestData(int dataSize)
        {
            for (int i = 0; i < dataSize; i++)
            {
                MachineStatus machineStatus = new MachineStatus(this)
                {
                    Name = "machine" + i,
                    Description = "description for machine" + i,
                    Status = MachineOperationalStatus.Running,
                    Notes =  "my notes for machine" + i
                };
                _machineStatuses.Add(machineStatus);
                
            }
            MachineStatuses = _machineStatuses;
        }

        public bool DeleteMachineStatus(MachineStatus deletedMachine)
        {
            bool deleted = _machineStatuses.Remove(deletedMachine);
            OnPropertyChanged("MachineStatuses");
            return deleted;
        }

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
            if(_machineStatuses.Contains(newMachine))
            {
                return false;
            }
            _machineStatuses.Add(newMachine);
            OnPropertyChanged("MachineStatuses");
            return true;
        }
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
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
       
        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = propertyExpression.Body as MemberExpression;
                Debug.Assert(memberExpr != null, "memberExpr != null");
                string propertyName = memberExpr.Member.Name;
                this.OnPropertyChanged(propertyName);
            }
        }
        #endregion
    }
}
