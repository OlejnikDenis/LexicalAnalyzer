﻿using LexicalAnalyzer.LexicalAnalyzer.Source;
using LexicalAnalyzer.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LexicalAnalyzer
{
    public partial class MainForm : Form
    {
        private const bool DEV_MODE = true;
        private bool syntaxTreeIsExpanded = false;

        public MainForm()
        {
            InitializeComponent();

            if (DEV_MODE)
            {
                //textBox_FilePath.Text = "";
                if (File.Exists(textBox_FilePath.Text))
                {
                    using (StreamReader reader = new StreamReader(textBox_FilePath.Text))
                    {
                        string fileContent = reader.ReadToEnd();
                        textBox_FileViewer.Text = fileContent;
                    }

                    fillTabsContent();
                }
            }
        }

        private void button_openFile_Click(object sender, EventArgs e)
        {
            try
            {
                // Reading a file and writing to a text box
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text Files (*.txt)|*.txt";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            string fileContent = reader.ReadToEnd();
                            textBox_FilePath.Text = filePath;
                            textBox_FileViewer.Text = fileContent;
                        }
                        fillTabsContent();
                    }
                }
            }
            catch (FileNotFoundException fileNotFound)
            {
                MessageBox.Show(fileNotFound.Message, "File not found!", MessageBoxButtons.OK);
                throw;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, this.GetType().Name, MessageBoxButtons.OK);
            }
        }

        private void button_ToggleTreeViewVIsib_Click(object sender, EventArgs e)
        {
            try
            {
                if (syntaxTreeIsExpanded)
                {
                    SyntaxTreeView?.CollapseAll();
                }
                else
                {
                    SyntaxTreeView?.ExpandAll();
                }
                syntaxTreeIsExpanded = !syntaxTreeIsExpanded;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, this.GetType().Name, MessageBoxButtons.OK);
            }
        }

        private void button_ShowDeepestTreeView_Click(object sender, EventArgs e)
        {
            try
            {
                throw new NotImplementedException("This feature is not implemented yet!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, this.GetType().Name, MessageBoxButtons.OK);
            }
        }

        private void textBox_FilePath_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Int32)(Keys.Enter))
            {
                try
                {
                    if (File.Exists(textBox_FilePath.Text))
                    {
                        using (StreamReader reader = new StreamReader(textBox_FilePath.Text))
                        {
                            string fileContent = reader.ReadToEnd();
                            textBox_FileViewer.Text = fileContent;
                        }
                        fillTabsContent();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, this.GetType().Name, MessageBoxButtons.OK);
                    throw;
                }
            }
        }

        private void fillTabsContent()
        {
            Lexer lexicalAnalyzer = new Lexer();
            Parser parser = new Parser();

            // Fill in the table of lexemes
            dataGridView_table?.Rows.Clear();
            List<Lex> lexicList = lexicalAnalyzer.getLexemesList(textBox_FileViewer.Text);
            for (int i = 0; i < lexicList.Count; i++)
            {
                dataGridView_table.Rows.Add(i + 1, lexicList[i].lexemWord, lexicList[i].lexemType);
            }

            // Build a syntax tree
            parser.GenerateAbstractSyntaxTree(SyntaxTreeView, lexicList);
        }


    }
}
