using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private string CurentFilePass = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void CreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Документ был изменен. \nСохранить изменения?", "Сохранение документа", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            switch (result)
            {
                case DialogResult.Yes:
                    {
                        SaveAsToolStripMenuItem_Click(sender, e);
                        CodeWindow.Text = "";
                        CurentFilePass = "";
                        break;
                    }

                case DialogResult.Cancel:
                    {
                        return;
                    }

                case DialogResult.No:
                    {
                        CodeWindow.Text = "";
                        CurentFilePass = "";
                        break;
                    }
            }

        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Все файлы (*.*)|*.*|Текстовые документы (*.txt)|*.txt|Документы Python (*.py)|*.py|Документы CPP (*.cpp)|*.cpp|Документы CS (*.cs)|*.cs";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();
                    CurentFilePass = filePath;

                    using (StreamReader reader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            CodeWindow.Text = fileContent;
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(CurentFilePass))
            {
                string content = CodeWindow.Text;
                File.WriteAllText(CurentFilePass, content, System.Text.Encoding.UTF8);
            }

            else
            {
                SaveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = " ";
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Все файлы (*.*)|*.*|Текстовые документы (*.txt)|*.txt|Документы Python (*.py)|*.py|Документы CPP (*.cpp)|*.cpp|Документы CS (*.cs)|*.cs";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                }
            }


            string text = CodeWindow.Text;
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(text);
                }

            }
            catch
            {

            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Документ был изменен. \nСохранить изменения?", "Сохранение документа", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            switch (result)
            {
                case DialogResult.Yes: // Да - сохранить и выйти
                    {
                        SaveToolStripMenuItem_Click(sender, e);
                        Close();
                        break;
                    }

                case DialogResult.Cancel: // Отмена - вернуться к документу
                    {
                        return;
                    }

                case DialogResult.No: // Нет - выйти без сохранения изменений
                    {
                        Close();
                        break;
                    }
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeWindow.Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeWindow.Redo();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeWindow.Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeWindow.Copy();
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeWindow.Paste();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeWindow.SelectedText = "";
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeWindow.SelectAll();
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"Help\Вызов справки.html");
        }

        private void AboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"Help\О программе.html");

        }

        private void CreateToolStripButton_Click(object sender, EventArgs e)
        {
            CreateToolStripMenuItem_Click(sender, e);
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            OpenToolStripMenuItem_Click(sender, e);
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveToolStripMenuItem_Click(sender, e);
        }

        private void UndoToolStripButton_Click(object sender, EventArgs e)
        {
            UndoToolStripMenuItem_Click(sender, e);
        }

        private void RedoToolStripButton_Click(object sender, EventArgs e)
        {
            RedoToolStripMenuItem_Click(sender, e);
        }

        private void CutToolStripButton_Click(object sender, EventArgs e)
        {
            CutToolStripMenuItem_Click(sender, e);
        }

        private void CopyToolStripButton_Click(object sender, EventArgs e)
        {
            CopyToolStripMenuItem_Click(sender, e);
        }

        private void PasteToolStripButton_Click(object sender, EventArgs e)
        {
            PasteToolStripMenuItem_Click(sender, e);
        }

        private void StartToolStripButton_Click(object sender, EventArgs e)
        {
            ResultsWindow.Text = "";
            string[] str = CodeWindow.Text.Split(new char[] { '\n' });
            string[] TypeData = { "int", "boolean", "long", "short", "String", "true", "false", "double" };
            string[] MathSym = { "/", "(", ")", "-", "+", "*", "=" };
            string[] NoName = { ":", "?", "{", "}", "[", "]" };
            string[] Digits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            bool Error;
            string sub;
            string repl;
            int way, space_way, step;
            for (int i = 0; i < str.Length; i++)
            {
                way = 0;
                space_way = 0;
                step = 0;
                sub = str[i];
                str[i] = str[i].Replace(";\r", ";");
                if (str[i].Contains(";") && str[i].Contains(" ;") == false)
                    str[i] = str[i].Replace(";", " ;");

                for (int m = 0; m < NoName.Length; m++)
                {
                    repl = " ";
                    str[i] = str[i].Replace(NoName[m], repl);
                }

                ResultsWindow.Text += " Строка " + i + "\r\n";
                string[] words = str[i].Split(new char[] { ' ' });

                for (int j = 0; j < words.Length; j++)
                {
                    for (int c = 0; c < NoName.Length; c++)
                    {

                    }

                    if (sub.Contains(" "))
                    {
                        ResultsWindow.Text += " 10 - разделитель - c " + ((sub.IndexOf(" ", 0)) + step) + " по " + ((sub.IndexOf(" ", 0)) + step) + " символ " + "\r\n";
                        step = step + (sub.IndexOf(" ", 0) + 1);
                        sub = sub.Substring(sub.IndexOf(" ", space_way) + 1);
                    }

                    if (TypeData.Contains(words[j]))
                    {
                        ResultsWindow.Text += " " + (Array.IndexOf(TypeData, words[j]) + 1) + " - ключевое слово - " + words[j] + " - с " + str[i].IndexOf((words[j]), way) + " по " + (str[i].IndexOf(words[j], way) + (words[j].Length - 1)) + " символ " + "\r\n";
                    }

                    else if (words[j].Contains('=') == true)
                    {
                        ResultsWindow.Text += " 11 - оператор присваивания - " + words[j] + " - с " + (str[i].IndexOf((words[j]), way)) + " по " + (str[i].IndexOf(words[j], way)) + " символ " + "\r\n";
                    }

                    else if (words[j][0] == '"' && words[j][words[j].Length - 1] == '"')
                    {
                        ResultsWindow.Text += " 12 - строка - " + words[j] + " - с " + ((str[i].IndexOf(words[j], way)) - 2) + " по " + ((str[i].IndexOf(words[j], way) + (words[j].Length - 1)) - 2) + " символ " + "\r\n";
                    }

                    else if (words[j].All(char.IsDigit))
                    {
                        ResultsWindow.Text += " 13 - целое число - " + words[j] + " - с " + ((str[i].IndexOf((words[j]), way)) - 2) + " по " + ((str[i].IndexOf(words[j], way) + (words[j].Length - 1)) - 2) + " символ " + "\r\n";
                    }

                    else if (words[j].Contains('.') && Char.IsDigit(words[j][0]) == true)
                    {
                        string test = words[j].Replace(".", "");
                        if (test.All(char.IsDigit))
                        {
                            ResultsWindow.Text += " 14 - вещественное число - " + words[j] + " - с " + (str[i].IndexOf((words[j]), way) - 2) + " по " + ((str[i].IndexOf(words[j], way) + (words[j].Length - 1)) - 2) + " символ " + "\r\n";
                        }
                    }

                    else if (words[j].Contains(';') == true && words[j].Length == 1)
                    {
                        ResultsWindow.Text += " 15 - конец оператора - ; " + " - с " + ((str[i].IndexOf((words[j]), way) - 1) - 2) + " по " + ((str[i].IndexOf(words[j], way) - 1) - 2) + " символ " + "\r\n";
                    }

                    else if (words[j].Contains("\\n") == true)
                    {
                        ResultsWindow.Text += " 16 - конец строки - \\n " + " - с " + ((str[i].IndexOf((words[j]), way) - 1) - 2) + " по " + ((str[i].IndexOf(words[j], way) - 1) - 2) + " символ " + "\r\n";
                    }

                    else
                    {
                        Error = false;
                        for (int c = 0; c < NoName.Length; c++)
                        {
                            if (words[j].Contains(NoName[c]))
                            {
                                Error = true;
                            }
                        }

                        if (Char.IsDigit(words[j][0]))
                        {
                            Error = true;
                        }

                        if (Error == false)
                        {
                            ResultsWindow.Text += " 7 - идентификатор - " + words[j] + " - с " + (str[i].IndexOf((words[j]), way)) + " по " + ((str[i].IndexOf(words[j], way) + (words[j].Length - 1))) + " символ " + "\r\n";
                        }
                        else
                        {
                            ResultsWindow.Text += " Error недопустимый символ в названии переменной " + "\r\n";
                        }
                    }
                    way = way + System.Convert.ToInt32(words[j].Length) + 1;
                }
            }
        }
        private void HelpToolStripButton_Click(object sender, EventArgs e)
        {
            HelpToolStripMenuItem_Click(sender, e);
        }

        private void Code_Window(object sender, EventArgs e)
        {

        }

        private void Results_Window(object sender, EventArgs e)
        {
            ResultsWindow.ReadOnly = true;
        }

    }
}





