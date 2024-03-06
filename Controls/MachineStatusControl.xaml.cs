using MachineStatusTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MachineStatusTracker.Controls
{
    /// <summary>
    /// Interaction logic for MachineStatusControl.xaml
    /// </summary>
    public partial class MachineStatusControl : UserControl, INotifyPropertyChanged
    {
        public List<string> StatusList { get; set; }
        public MachineStatusControl()
        {
            InitializeComponent();
        }



        private void DeleteStatusButton_Click(object sender, RoutedEventArgs e)
        {
            MachineStatus machine = (MachineStatus)((Button)sender).DataContext;
            if(machine == null) { return; }
            machine.Delete();
        }

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
