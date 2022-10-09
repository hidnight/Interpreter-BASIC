﻿using System;
using System.IO;
using System.Text;
using System.Windows;
using BASIC_Interpreter_Library;
using System.Windows.Forms;

namespace Интерпретатор_языка_BASIC {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        string FilePath = "";
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;
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
            //openFileDialog.Options
            //Code.Text = "Dim A As Long\nA = 1\nIf A <> 0 Then\nPrint (3 + A * 5)\nElse\nPrint(0)\nEnd_If\n";
            Code.Text += "Dim A As Long\nA=10\nwhile A > 0\nprint A\n A = A - 1\nend_while\n";
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
                parseStreamWriter.Dispose();
                outputStreamWriter.Dispose();
                errorStreamWriter.Dispose();
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e) {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                FilePath = openFileDialog.FileName;
                try {
                    Code.Text = File.ReadAllText(FilePath/*, Encoding.ASCII*/);
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            if (FilePath == "") {
                SaveAs_Click(sender, e);
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

        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                FilePath = saveFileDialog.FileName;
                try {
                    File.WriteAllText(FilePath, Code.Text, Encoding.ASCII);
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
