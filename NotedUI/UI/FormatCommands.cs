using JustMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotedUI.UI
{
    public class FormatCommands : MVVMBase
    {
        public ICommand Header1Command { get { return new RelayCommand<TextBox>(Header1Exec, CanHeader1Exec); } }
        public ICommand Header2Command { get { return new RelayCommand<TextBox>(Header2Exec, CanHeader2Exec); } }
        public ICommand Header3Command { get { return new RelayCommand<TextBox>(Header3Exec, CanHeader3Exec); } }
        public ICommand Header4Command { get { return new RelayCommand<TextBox>(Header4Exec, CanHeader4Exec); } }
        public ICommand Header5Command { get { return new RelayCommand<TextBox>(Header5Exec, CanHeader5Exec); } }
        public ICommand Header6Command { get { return new RelayCommand<TextBox>(Header6Exec, CanHeader6Exec); } }
        public ICommand BoldCommand { get { return new RelayCommand<TextBox>(BoldExec, CanBoldExec); } }
        public ICommand ItalicCommand { get { return new RelayCommand<TextBox>(ItalicExec, CanItalicExec); } }
        public ICommand StrikethroughCommand { get { return new RelayCommand<TextBox>(StrikethroughExec, CanStrikethroughExec); } }
        public ICommand QuotesCommand { get { return new RelayCommand<TextBox>(QuotesExec, CanQuotesExec); } }
        public ICommand CodeCommand { get { return new RelayCommand<TextBox>(CodeExec, CanCodeExec); } }
        public ICommand BulletPointCommand { get { return new RelayCommand<TextBox>(BulletPointExec, CanBulletPointExec); } }
        public ICommand ListCommand { get { return new RelayCommand<TextBox>(ListExec, CanListExec); } }
        public ICommand ImageCommand { get { return new RelayCommand<TextBox>(ImageExec, CanImageExec); } }
        public ICommand LinkCommand { get { return new RelayCommand<TextBox>(LinkExec, CanLinkExec); } }
        public ICommand HorizontalRuleCommand { get { return new RelayCommand<TextBox>(HorizontalRuleExec, CanHorizontalRuleExec); } }

        public bool CanHeader1Exec(TextBox contentTextBox)
        {
            return true;
        }

        public void Header1Exec(TextBox contentTextBox)
        {

        }

        public bool CanHeader2Exec(TextBox contentTextBox)
        {
            return true;
        }

        public void Header2Exec(TextBox contentTextBox)
        {

        }

        public bool CanHeader3Exec(TextBox contentTextBox)
        {
            return true;
        }

        public void Header3Exec(TextBox contentTextBox)
        {

        }

        public bool CanHeader4Exec(TextBox contentTextBox)
        {
            return true;
        }

        public void Header4Exec(TextBox contentTextBox)
        {

        }

        public bool CanHeader5Exec(TextBox contentTextBox)
        {
            return true;
        }

        public void Header5Exec(TextBox contentTextBox)
        {

        }

        public bool CanHeader6Exec(TextBox contentTextBox)
        {
            return true;
        }

        public void Header6Exec(TextBox contentTextBox)
        {

        }

        public bool CanBoldExec(TextBox contentTextBox)
        {
            return true;
        }

        public void BoldExec(TextBox contentTextBox)
        {

        }

        public bool CanItalicExec(TextBox contentTextBox)
        {
            return true;
        }

        public void ItalicExec(TextBox contentTextBox)
        {

        }

        public bool CanStrikethroughExec(TextBox contentTextBox)
        {
            return true;
        }

        public void StrikethroughExec(TextBox contentTextBox)
        {

        }

        public bool CanQuotesExec(TextBox contentTextBox)
        {
            return true;
        }

        public void QuotesExec(TextBox contentTextBox)
        {

        }

        public bool CanCodeExec(TextBox contentTextBox)
        {
            return true;
        }

        public void CodeExec(TextBox contentTextBox)
        {

        }

        public bool CanBulletPointExec(TextBox contentTextBox)
        {
            return true;
        }

        public void BulletPointExec(TextBox contentTextBox)
        {

        }

        public bool CanListExec(TextBox contentTextBox)
        {
            return true;
        }

        public void ListExec(TextBox contentTextBox)
        {

        }

        public bool CanImageExec(TextBox contentTextBox)
        {
            return true;
        }

        public void ImageExec(TextBox contentTextBox)
        {

        }

        public bool CanLinkExec(TextBox contentTextBox)
        {
            return true;
        }

        public void LinkExec(TextBox contentTextBox)
        {

        }

        public bool CanHorizontalRuleExec(TextBox contentTextBox)
        {
            return true;
        }

        public void HorizontalRuleExec(TextBox contentTextBox)
        {

        }


    }
}
