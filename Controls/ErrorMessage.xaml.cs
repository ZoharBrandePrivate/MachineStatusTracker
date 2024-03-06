using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows;

namespace MachineStatusTracker.Controls
{
    /// <summary>
    /// Interaction logic for ErrorMessage.xaml
    /// </summary>
    public partial class ErrorMessage : Window, INotifyPropertyChanged
    {
        public string ErrorMessageStr { get; set; } 
        public ErrorMessage(string errorMessage)
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;
            ErrorMessageStr = errorMessage;
            this.DataContext = this;
            InitializeComponent();
            OnPropertyChanged("ErrorMessageStr");
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        #region INotifyPropertyChanged
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
