using MachineStatusTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MachineStatusTracker.Controls
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl, INotifyPropertyChanged
    {
        public int SelectedMachineStatusId { get; set; }
        public MachineStatusVM MainControlVM { get; set; }
        public ObservableCollection<MachineStatus> MachineStatuses { get; set; }

        private MachineOperationalStatus _filterStatus;
        public List<string> OperationalStatusFilter { get; set; }
        public MainControl()
        {
            MainControlVM = new MachineStatusVM();
            MachineStatuses = MainControlVM.MachineStatuses;
            InitOperationalStatusFilter();
            InitializeComponent();
            this.DataContext = this;
        }

        private void InitOperationalStatusFilter()
        {
            OperationalStatusFilter = new List<string>();
            OperationalStatusFilter.Add("All");
            OperationalStatusFilter.AddRange(Enum.GetNames(typeof(MachineOperationalStatus)));
        }
        
        private void FilterMachinesStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
           
            string selectedItem = comboBox.SelectedItem as string;
            ICollectionView _resultsView = CollectionViewSource.GetDefaultView(MachineStatuses);
            _resultsView.Filter = null;
            if (comboBox.SelectedIndex != 0) // ALL
            {
                _filterStatus = (MachineOperationalStatus) Enum.Parse(typeof(MachineOperationalStatus),selectedItem);
                _resultsView.Filter += new Predicate<object>(StatusEquals);
            }
            OnPropertyChanged("MachineStatuses");
        }

        public bool StatusEquals(object ms)
        {
            MachineStatus machine = ms as MachineStatus;
            return (machine.Status == _filterStatus);
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MachineStatuses = MainControlVM.MachineStatuses;
            OnPropertyChanged("MachineStatuses");
        }

        private void AddNewStatusButton_Click(object sender, RoutedEventArgs e)
        {
            MainControlVM.AddNewMachineStatus();
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
