//////////////////////////////////////////////////////////////////
//                                                              //
//  Copyright (c) 2016 Berk C. Celebisoy. All Rights Reserved.  //
//                                                              //
//////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Kalculator
{
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();

            richTextBox.Text = "";

            UpdateText();
        }


        private void buttonCopy_Click(object sender, EventArgs e)
        {
            string buffer = "";

            for (int i = 0; i < richTextBox.Text.Length; i++)
            {
                char curr = richTextBox.Text[i];

                //line breaks are 2 bytes: \r\n 
                if (curr == '\n')
                {
                    buffer += '\r';
                }

                buffer += curr;
            }

            Clipboard.SetText(buffer.ToString());
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(saveDialog.ShowDialog(this) == DialogResult.OK)
            {
                Stream     file   = saveDialog.OpenFile();
                List<byte> buffer = new List<byte>();

                for(int i=0; i<richTextBox.Text.Length; i++)
                {
                    byte curr = Convert.ToByte(richTextBox.Text[i]);

                    //line breaks are 2 bytes: \r\n 
                    if (curr == '\n')
                    {
                        buffer.Add(Convert.ToByte('\r'));
                    }

                    buffer.Add(curr);
                }

                file.Write(buffer.ToArray(), 0, buffer.Count);
                file.Close();
            }
        }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            Buffer.GetInstance().Clear();
            UpdateText();
        }


        public void UpdateText()
        {
            richTextBox.Text           = Buffer.GetInstance().GetBuffer();
            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.ScrollToCaret();

            if (richTextBox.Text.Trim() == "")
            {
                buttonSave.Enabled  = false;
                buttonClear.Enabled = false;
                buttonCopy.Enabled  = false;
            }
            else
            {
                buttonSave.Enabled  = true;
                buttonClear.Enabled = true;
                buttonCopy.Enabled  = true;
            }
        }
    }
}
