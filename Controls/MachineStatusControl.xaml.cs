using MachineStatusTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace MachineStatusTracker.Controls
{
    /// <summary>
    /// Represents a control for displaying machine statuses.
    /// </summary>
    public partial class MachineStatusControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the list of available status options.
        /// </summary>
        public List<string> StatusList { get; set; }

        /// <summary>
        /// Initializes a new instance of the MachineStatusControl class.
        /// </summary>
        public MachineStatusControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delete button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteStatusButton_Click(object sender, RoutedEventArgs e)
        {
            MachineStatus machine = (MachineStatus)((Button)sender).DataContext;
            if (machine == null) { return; }
            machine.Delete();
        }
        /// <summary>
        /// Edit button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditStatusButton_Click(object sender, RoutedEventArgs e)
        {
            MachineStatus machine = (MachineStatus)((Button)sender).DataContext;
            if (machine == null) { return; }
            machine.Edit();
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
