using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MVVM.Behaviors
{

    public class InvokeMethodBehavior : Behavior<DependencyObject>
    {

        public string MethodName
        {
            get { return (string)GetValue(MethodNameProperty); }
            set { SetValue(MethodNameProperty, value); }
        }
        public static readonly DependencyProperty MethodNameProperty =
            DependencyProperty.Register("MethodName", typeof(string), typeof(InvokeMethodBehavior), new PropertyMetadata(string.Empty));


        public object MethodParameter
        {
            get { return (object)GetValue(MethodParameterProperty); }
            set { SetValue(MethodParameterProperty, value); }
        }
        public static readonly DependencyProperty MethodParameterProperty =
            DependencyProperty.Register("MethodParameter", typeof(object), typeof(InvokeMethodBehavior), new PropertyMetadata(null, MethodParameterPropertyChangedCallback));

        static void MethodParameterPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InvokeMethodBehavior sender = (InvokeMethodBehavior)d;
            sender.InvokeMethod();
        }

        private void InvokeMethod()
        {
            MethodInfo method = AssociatedObject.GetType().GetMethod(MethodName);
            if (method != null)
            {
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length > 0)
                {
                    method.Invoke(AssociatedObject, new object[] { MethodParameter });
                }
                else
                {
                    method.Invoke(AssociatedObject, null);
                }
            }
        }
    }
}
