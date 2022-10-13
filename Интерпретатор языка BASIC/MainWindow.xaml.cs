using System;
using System.IO;
using System.Text;
using System.Windows;
using BASIC_Interpreter_Library;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Xml;

namespace Интерпретатор_языка_BASIC {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private string FilePath = "";
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        private void LoadHighlighting(ref TextEditor editor,
            string file = "BASIC.xshd") {
            Stream xshd_stream = File.OpenRead(file);
            XmlTextReader xshd_reader = new XmlTextReader(xshd_stream);
            editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
            xshd_reader.Close();
            xshd_stream.Close();
        }

        public MainWindow() {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            openFileDialog.RestoreDirectory = true;
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            saveFileDialog.RestoreDirectory = false;
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "txt";
            try {
                LoadHighlighting(ref Code);
            } catch (Exception ex) {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void InterpreterClick(object sender, RoutedEventArgs e) {
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
                    Lexan lex = new Lexan(new StreamReader(inputStream, Encoding.ASCII),
                        ref errorStreamWriter);
                    Syntaxan syntaxan = new Syntaxan(ref lex, ref parseStreamWriter,
                        ref outputStreamWriter);
                    syntaxan.Parse();
                } catch (Exception ex) {
                    Output.Text += "\n\nОШИБКА\n" + ex.Message;
                }
                parseStreamWriter.Flush();
                outputStreamWriter.Flush();
                errorStreamWriter.Flush();
                errorStream.Seek(0, SeekOrigin.Begin);
                parseStream.Seek(0, SeekOrigin.Begin);
                outputStream.Seek(0, SeekOrigin.Begin);
                if (DebugInfo.IsChecked) {
                    Output.Text += "\n------------- Отладочная информация -------------\n";
                    Output.Text += "************* Лексический анализатор ************\n";
                    Output.Text += new StreamReader(errorStream).ReadToEnd() + "\n";
                    Output.Text += "\n************* Синтаксический анализатор *********\n";
                    Output.Text += new StreamReader(outputStream).ReadToEnd();
                    Output.Text += "\n-------------------------------------------------\n\n\n";
                }
                Output.Text += new StreamReader(parseStream).ReadToEnd();
                File.AppendAllText(System.Windows.Forms.Application.ExecutablePath.Replace("exe", "log"),
                    DateTime.Now.ToString("G") + "\n" + Output.Text);
                parseStreamWriter.Dispose();
                outputStreamWriter.Dispose();
                errorStreamWriter.Dispose();
            }
        }

        private void OpenClick(object sender, RoutedEventArgs e) {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                FilePath = openFileDialog.FileName;
                try {
                    Code.Text = File.ReadAllText(FilePath/*, Encoding.ASCII*/);
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e) {
            if (FilePath == "") {
                SaveAsClick(sender, e);
                return;
            }
            if (Code.IsModified) {
                try {
                    File.WriteAllText(FilePath, Code.Text, Encoding.ASCII);
                } catch(Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void SaveAsClick(object sender, RoutedEventArgs e) {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                FilePath = saveFileDialog.FileName;
                try {
                    File.WriteAllText(FilePath, Code.Text, Encoding.ASCII);
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void BigFontClick(object sender, RoutedEventArgs e) {
            Code.TextArea.FontSize = BigFont.IsChecked ? 24 : 14;
            Output.FontSize = BigFont.IsChecked ? 24 : 14;
        }
    }
}
