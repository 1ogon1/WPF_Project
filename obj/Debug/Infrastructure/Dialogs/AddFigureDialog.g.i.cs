﻿#pragma checksum "..\..\..\..\Infrastructure\Dialogs\AddFigureDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "EB551EB249BF4E4CA349DA30878EE10F386C70AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Project;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Project {
    
    
    /// <summary>
    /// AddFigureDialog
    /// </summary>
    public partial class AddFigureDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\..\Infrastructure\Dialogs\AddFigureDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Name;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Infrastructure\Dialogs\AddFigureDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Project;component/infrastructure/dialogs/addfiguredialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Infrastructure\Dialogs\AddFigureDialog.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\..\Infrastructure\Dialogs\AddFigureDialog.xaml"
            ((Project.AddFigureDialog)(target)).Loaded += new System.Windows.RoutedEventHandler(this.OnLoad);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Name = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\..\..\Infrastructure\Dialogs\AddFigureDialog.xaml"
            this.Name.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ValidateField);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SaveButton = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\..\Infrastructure\Dialogs\AddFigureDialog.xaml"
            this.SaveButton.Click += new System.Windows.RoutedEventHandler(this.Save);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

