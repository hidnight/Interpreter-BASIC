using System;
using System.Collections.Generic;
using System.IO;
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
using BASIC_Interpreter_Library;

namespace Интерпретатор_языка_BASIC {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Code.Text = "dim a as long\n";
        }

        private void Interpreter_Click(object sender, RoutedEventArgs e) {
            Output.Text = "";
            using (MemoryStream inputStream = new MemoryStream(Encoding.ASCII.GetBytes(Code.Text)),
                outputStream = new MemoryStream(),
                parseStream = new MemoryStream(),
                errorStream = new MemoryStream()
                ) {
                StreamWriter parseStreamWriter = new StreamWriter(parseStream),
                       outputStreamWriter = new StreamWriter(outputStream),
                       errorStreamWriter = new StreamWriter(errorStream);
                try {
                    Lexan lex = new Lexan(new StreamReader(inputStream, Encoding.ASCII), ref errorStreamWriter);
                    Syntaxan syntaxan = new Syntaxan(ref lex, ref parseStreamWriter, ref outputStreamWriter);
                    syntaxan.parse();
                    parseStreamWriter.Flush();
                    outputStreamWriter.Flush();
                    errorStreamWriter.Flush();
                    errorStream.Seek(0, SeekOrigin.Begin);
                    parseStream.Seek(0, SeekOrigin.Begin);
                    outputStream.Seek(0, SeekOrigin.Begin);
                    if (DebugInfo.IsChecked) {
                        Output.Text += "------------- Отладочная информация -------------\n";
                        Output.Text += "************* Лексический анализатор ************\n";
                        Output.Text += new StreamReader(errorStream).ReadToEnd() + "\n";
                        Output.Text += "\n************* Синтаксический анализатор *********\n";
                        Output.Text += new StreamReader(outputStream).ReadToEnd();
                        Output.Text += "\n-------------------------------------------------\n\n\n";
                    }
                    Output.Text += new StreamReader(parseStream).ReadToEnd();
                } catch (Exception ex) {
                    Output.Text += "\n\nОШИБКА\n" + ex.Message;
                }
                parseStreamWriter.Dispose();
                outputStreamWriter.Dispose();
                errorStreamWriter.Dispose();
            }
        }
    }
}
