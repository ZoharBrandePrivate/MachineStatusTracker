using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MachineStatusTracker.ViewModels
{
    public enum MachineOperationalStatus { Running, Idle, Offline, Unavailable}
    

    public class MachineStatus : INotifyPropertyChanged
    {
        public int MachineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MachineOperationalStatus Status { get; set; }  
        public string Notes { get; set; }
        private MachineStatusVM _parent;
        public MachineStatus(MachineStatusVM parent)
        { 
            _parent = parent; 
            MachineId = GetNextMachineId(); 
        }

        public void Update(string newName,string newDescription , MachineOperationalStatus newStatus, string newNotes)
        {
            Name = newName;
            OnPropertyChanged("Name");
            Description = newDescription;
            OnPropertyChanged("Description");
            Status = newStatus;
            OnPropertyChanged("Status");
            Notes = newNotes;
            OnPropertyChanged("Notes");
        }
        private static int _curMachineId = 0;
        public int GetNextMachineId()
        {
            _curMachineId++;
            return _curMachineId;
        }

        internal void Edit()
        {
            _parent.EditMachineStatus(this);
        }
        public void Delete()
        {
            _parent.DeleteMachineStatus(this);
        }
        public override bool Equals(object o)
        { 
            MachineStatus machineStatus = o as MachineStatus;
            if(machineStatus == null) return false;
            return(machineStatus.MachineId == MachineId);            
        }

        public override int GetHashCode()
        {
            return MachineId.GetHashCode();
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
