using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using MVVM.UI.Extensions;

namespace MVVM.Behaviors.WPS8
{
    public class ControlFocusBehavior : Behavior<Control>
    {
        #region Consts
        private const int CONTROL_SWITCH_DELAY = 50;
        #endregion

        #region Properties

        public bool DelayFocus { get; set; }

        #endregion

        #region Event Handlers

        private async void OnAssociatedObjectKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DelayFocus)
                {
                    await Task.Delay(CONTROL_SWITCH_DELAY);
                }

                SwitchFocus(AssociatedObject);
            }
        }

        #endregion

        #region Behavior Members

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += OnAssociatedObjectKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= OnAssociatedObjectKeyDown;
        }

        #endregion

        #region Methods
        private void HideKeyboard(Control control)
        {
            var page = control.GetRootVisualParent<Page>();
            if (page != null)
            {
                //here is the trick. We can't actually collapse keyboard so we focus on Page.
                page.Focus();
            }
        }

        protected Control GetNextControl(Control control)
        {
            var page = control.GetRootVisualParent<Page>();
            if (page != null)
            {
                var childControls = page.GetVisualDescendents<Control>()
                    .Where(ctrl => (ctrl is PasswordBox || ctrl is TextBox) && ctrl.TabIndex < int.MaxValue)
                    .OrderBy(ctrl => ctrl.TabIndex);

                foreach (var childControl in childControls)
                {
                    if (!childControl.Equals(control) && childControl.TabIndex >= control.TabIndex)
                    {
                        return childControl;
                    }
                }
            }
            return null;
        }

        protected void SwitchFocus(Control control)
        {
            var nextControl = GetNextControl(control);

            if (nextControl != null)
            {
                nextControl.Focus();

                if (nextControl is TextBox)
                {
                    var textBoxControl = nextControl as TextBox;
                    if (!string.IsNullOrEmpty(textBoxControl.Text))
                    {
                        //we focus this on the last character in TextBox Control
                        textBoxControl.Select(textBoxControl.Text.Length, 0);
                    }
                }
            }
            else
            {
                //We cant find anything else so we can collapse keyboard
                HideKeyboard(control);
            }
        }



        #endregion
    }
}
