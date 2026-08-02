using System;
using System.Collections.Generic;
using System.Linq;
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
namespace Pflegehaushaltsbuch.WPFControls
{
    /// <summary>
    /// Represents a custom user Text Box control used by the application user interface.
    /// </summary>
    public partial class UserTextBox : TextBox// System.Windows.Controls.UserControl
    {
        string focusText = string.Empty;
        /// <summary>
        /// Runs the validated Delegate operation and updates the related application state.
        /// </summary>
        public delegate void ValidatedDelegate();
        public event ValidatedDelegate Validated;
        /// <summary>
        /// Creates a new User Text Box instance and initializes the required state.
        /// </summary>
        public UserTextBox()
        {
            ConfigureTextBox();
            GotFocus += UserTextBox_GotFocus;
            LostFocus += UserTextBox_LostFocus;
        }
        /// <summary>
        /// Runs the configure Text Box operation and updates the related application state.
        /// </summary>
        private void ConfigureTextBox()
        {
            SpellCheck.SetIsEnabled(this, true);
            BorderThickness = new Thickness(1);
            BorderBrush = Brushes.Black;
            SelectionBrush = (Brush)new BrushConverter().ConvertFromString("#FF008EFF");
            Background = Brushes.White;
            AcceptsReturn = true;
            AcceptsTab = true;
            TextWrapping = TextWrapping.Wrap;
            AutoWordSelection = true;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            var style = new Style(typeof(TextBox), Style);
            style.Triggers.Add(CreateSetterTrigger(UIElement.IsEnabledProperty, false, Control.OpacityProperty, 0.56));
            style.Triggers.Add(CreateSetterTrigger(UIElement.IsMouseOverProperty, true, Control.BorderBrushProperty, Brushes.Red));
            style.Triggers.Add(CreateSetterTrigger(UIElement.IsFocusedProperty, true, Control.BorderBrushProperty, Brushes.Red));
            Style = style;
        }
        /// <summary>
        /// Creates the setter Trigger data or user interface element for the current workflow.
        /// </summary>
        private static Trigger CreateSetterTrigger(DependencyProperty property, object value, DependencyProperty targetProperty, object targetValue)
        {
            var trigger = new Trigger { Property = property, Value = value };
            trigger.Setters.Add(new Setter(targetProperty, targetValue));
            return trigger;
        }
        /// <summary>
        /// Handles the lost Focus event for user Text Box and updates the related state.
        /// </summary>
        private void UserTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Text.Equals(focusText))
            {
                if (Validated != null)
                    Validated();
            }
        }
        /// <summary>
        /// Handles the got Focus event for user Text Box and updates the related state.
        /// </summary>
        private void UserTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            focusText = Text;
        }
        /// <summary>
        /// Runs the bind operation and updates the related application state.
        /// </summary>
        public void Bind(
            DependencyProperty property,
            object source,
            string member,
            UpdateSourceTrigger trigger = UpdateSourceTrigger.PropertyChanged,
            BindingMode bindingMode = BindingMode.TwoWay)
        {
            BindingOperations.ClearBinding(this, property);
            //this.DataContext = null;
            Binding myBinding = new Binding(member);
            myBinding.Path = new PropertyPath(member);
            myBinding.Source = source;
            myBinding.Mode = bindingMode;
            myBinding.UpdateSourceTrigger = trigger;
            //this.DataContext = source;
            var bin = BindingOperations.SetBinding(this, property, myBinding);
           // bin.UpdateSource();
           // bin.UpdateTarget();
        }
    }
}
