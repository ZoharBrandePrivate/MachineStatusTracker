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
    /// <summary>
    /// Enumeration representing different operational statuses of a machine.
    /// </summary>
    public enum MachineOperationalStatus { Running, Idle, Offline, Unavailable }

    /// <summary>
    /// Represents the status of a machine.
    /// </summary>
    public class MachineStatus : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the ID of the machine.
        /// </summary>
        public int MachineId { get; set; }

        /// <summary>
        /// Gets or sets the name of the machine.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the machine.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the operational status of the machine.
        /// </summary>
        public MachineOperationalStatus Status { get; set; }

        /// <summary>
        /// Gets or sets additional notes about the machine.
        /// </summary>
        public string Notes { get; set; }

        private MachineStatusVM _parent;

        /// <summary>
        /// Initializes a new instance of the MachineStatus class.
        /// </summary>
        /// <param name="parent">The parent view model.</param>
        public MachineStatus(MachineStatusVM parent)
        {
            _parent = parent;
            MachineId = GetNextMachineId();
        }

        /// <summary>
        /// Updates the machine status with new information.
        /// </summary>
        /// <param name="newName">The new name of the machine.</param>
        /// <param name="newDescription">The new description of the machine.</param>
        /// <param name="newStatus">The new operational status of the machine.</param>
        /// <param name="newNotes">The new notes about the machine.</param>
        public void Update(string newName, string newDescription, MachineOperationalStatus newStatus, string newNotes)
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

        /// <summary>
        /// Retrieves the next available machine ID.
        /// </summary>
        /// <returns>The next available machine ID.</returns>
        public int GetNextMachineId()
        {
            _curMachineId++;
            return _curMachineId;
        }

        /// <summary>
        /// Opens the edit window for this machine status.
        /// </summary>
        internal void Edit()
        {
            _parent.EditMachineStatus(this);
        }

        /// <summary>
        /// Deletes this machine status.
        /// </summary>
        public void Delete()
        {
            _parent.DeleteMachineStatus(this);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current machine status.
        /// </summary>
        /// <param name="o">The object to compare with the current machine status.</param>
        /// <returns>True if the specified object is equal to the current machine status; otherwise, false.</returns>
        public override bool Equals(object o)
        {
            MachineStatus machineStatus = o as MachineStatus;
            if (machineStatus == null) return false;
            return (machineStatus.MachineId == MachineId);
        }

        /// <summary>
        /// Returns the hash code for this machine status.
        /// </summary>
        /// <returns>The hash code for this machine status.</returns>
        public override int GetHashCode()
        {
            return MachineId.GetHashCode();
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
                this.OnPropertyChanged(propertyName);
            }
        }
        #endregion
    }
}
