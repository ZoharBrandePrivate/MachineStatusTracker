using MachineStatusTracker.ViewModels;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MachineStatusTracker.Controls
{
    /// <summary>
    /// Interaction logic for EditMachineStatusWindow.xaml
    /// </summary>
    public partial class EditMachineStatusWindow : Window, INotifyPropertyChanged
    {
        public List<string> StatusList { get; set; }
        private MachineStatus originalStatus;
        // Data context
        public string NewName { get; set; }
        public string NewDescription { get; set; }
        public MachineOperationalStatus NewStatus { get; set; }
        public string NewNotes { get; set; }
        public int SelectedMachineStatusIndex { get; set; } 
        public EditMachineStatusWindow(MachineStatus machineStatus)
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;
            originalStatus = machineStatus;
            NewName = machineStatus.Name;
            NewDescription = machineStatus.Description;
            NewNotes = machineStatus.Notes;
            NewStatus = machineStatus.Status;
            SelectedMachineStatusIndex = (int)NewStatus;
            DataContext = this;
            InitStatusList();
            InitializeComponent();
            MachinesStatusComboBox.SelectedIndex = (int)machineStatus.Status;
        }

        private void InitStatusList()
        {
            StatusList = new List<string>(Enum.GetNames(typeof(MachineOperationalStatus)));
        }
        private void CancelStatusButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SaveStatusButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(!ValidateName())
            {
                return;
            }
            originalStatus.Update(NewName,NewDescription ,NewStatus,NewNotes);
            DialogResult = true;
            Close();
        }

        private bool ValidateName()
        {
            if(string.IsNullOrEmpty(NewName))
            {
                ErrorMessage errorMessage = new ErrorMessage("Machine name can't be empty");
                errorMessage.ShowDialog();  
                return false;
            }
            return true;
        }
        private void MachinesStatus_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedValue = (string)((ComboBox)sender).SelectedValue;
            MachineOperationalStatus status = (MachineOperationalStatus)Enum.Parse(typeof(MachineOperationalStatus), selectedValue);
            NewStatus = status;
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
