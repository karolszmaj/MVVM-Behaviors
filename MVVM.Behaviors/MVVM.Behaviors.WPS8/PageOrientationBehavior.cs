using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace MVVM.Behaviors.WPS8
{
    public class PageOrientationBehavior : Behavior<PhoneApplicationPage>
    {
        public string PortraitStateName
        {
            get { return (string)GetValue(PortraitStateNameProperty); }
            set { SetValue(PortraitStateNameProperty, value); }
        }

        public static readonly DependencyProperty PortraitStateNameProperty =
            DependencyProperty.Register("PortraitStateName", typeof(string), typeof(PageOrientationBehavior), new PropertyMetadata("Portrait"));



        public string LandscapeStateName
        {
            get { return (string)GetValue(LandscapeStateNameProperty); }
            set { SetValue(LandscapeStateNameProperty, value); }
        }

        public static readonly DependencyProperty LandscapeStateNameProperty =
            DependencyProperty.Register("LandscapeStateName", typeof(string), typeof(PageOrientationBehavior), new PropertyMetadata("Landscape"));

        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.OrientationChanged += OnOrientationChanged;
            }

            base.OnAttached();
        }

        private void OnOrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if (e.Orientation == PageOrientation.Landscape ||
                e.Orientation == PageOrientation.LandscapeLeft ||
                e.Orientation == PageOrientation.LandscapeRight)
            {
                VisualStateManager.GoToState(AssociatedObject, LandscapeStateName, true);
            }
            else
            {
                VisualStateManager.GoToState(AssociatedObject, PortraitStateName, true);
            }
        }
    }
}
