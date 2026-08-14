using System.Windows.Forms;

namespace Pflegehaushaltsbuch.FormControls
{
    public static class ControlBindingExtensions
    {
        public static void ClearBinding(this Control control, string propertyName)
        {
            for (int i = control.DataBindings.Count - 1; i >= 0; i--)
            {
                if (control.DataBindings[i].PropertyName == propertyName)
                    control.DataBindings.RemoveAt(i);
            }
        }
    }
}
