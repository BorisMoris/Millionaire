﻿using System;
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
using System.Windows.Shapes;

namespace Millionaire.Views
{
    /// <summary>
    /// Interakční logika pro UniversalInputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public string Answer { get { return answerTextBox.Text; } }
        
        public InputDialog(string question, string defaultAnswer)
        {
            InitializeComponent();

            questionTextBlock.Text = question;
            answerTextBox.Text = defaultAnswer;           
            answerTextBox.Focus();
            answerTextBox.SelectAll();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
