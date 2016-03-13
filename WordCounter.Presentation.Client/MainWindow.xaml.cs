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
using WordCounter.BusinessLogic;

namespace WordCounter.Presentation.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.txt_content.TextChanged += Txt_content_TextChanged;
        }

        private void Txt_content_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txt_result.Text = string.Empty;
        }

        private void btn_calculate_Click(object sender, RoutedEventArgs e)
        {
            string content = this.txt_content.Text;
            WordCounterUtility wc = new WordCounterUtility(ConfigurationManager.GetWordSeparatorsCharacters(), ConfigurationManager.GetWordTrimChars(), new Dictionary<string,int>());
            var result = wc.CountWordsInStringSequence(content);
            StringBuilder strBuilder = new StringBuilder();
            foreach(var element in result)
            {
                strBuilder.AppendLine(string.Format("{0}-{1}", element.Key, element.Value));
            }

            this.txt_result.Text = strBuilder.ToString();
        }
    }
}
