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
using System.Diagnostics;


namespace Kalculator
{
    public partial class MainForm : Form
    {
        enum Button
        {
            BUTTON_0,
            BUTTON_1,
            BUTTON_2,
            BUTTON_3,
            BUTTON_4,
            BUTTON_5,
            BUTTON_6,
            BUTTON_7,
            BUTTON_8,
            BUTTON_9,
            BUTTON_PLUS,
            BUTTON_MINUS,
            BUTTON_DIVIDE,
            BUTTON_MULTIPLY,
            BUTTON_SQRT,
            BUTTON_PERCENTAGE,
            BUTTON_EQUAL,
            BUTTON_DOT,
            BUTTON_PLUS_MINUS,
            BUTTON_BACKSPACE,
            BUTTON_CLEAR,
            BUTTON_HIST,
            BUTTON_COPY_MENU_ITEM,
            BUTTON_PASTE_MENU_ITEM,
            BUTTON_NONE
        }

        enum ButtonType
        {
            BUTTON_TYPE_NUMBER,
            BUTTON_TYPE_OPERATOR,
            BUTTON_TYPE_MENU_ITEM,
            BUTTON_TYPE_OTHER,
            BUTTON_TYPE_NONE
        }

        private Button      m_lastClickedButton;
        private Button      m_lastClickedOperator;
        private Button      m_lastClickedNumber;
        private ButtonType  m_lastClickedButtonType;
        private string      m_result;
        private double      m_numberOne; //todo: rename first or numL
        private double      m_numberTwo; //todo: rename second or numR
        private HistoryForm m_histForm;
        private const int   ASCII_OFFSET = 48;
        private const int   MAX_LENGTH   = 20;


        public MainForm()
        {
            InitializeComponent();

            m_lastClickedButton     = Button.BUTTON_NONE;
            m_lastClickedOperator   = Button.BUTTON_NONE;
            m_lastClickedNumber     = Button.BUTTON_NONE;
            m_lastClickedButtonType = ButtonType.BUTTON_TYPE_NONE;
            m_numberOne             = 0;
            m_numberTwo             = 0;
            m_result                = "0";
            m_histForm              = new HistoryForm();
            textBox.SelectionStart  = textBox.Text.Length;
            textBox.ScrollToCaret();

            //uncomment to enable unit tests
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    ExecUnitTests();
            //}
        }


        private void button1_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(1);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(2);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(3);
        }


        private void button4_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(4);
        }


        private void button5_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(5);
        }


        private void button6_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(6);
        }


        private void button7_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(7);
        }


        private void button8_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(8);
        }


        private void button9_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(9);
        }


        private void button0_Click(object sender, EventArgs e)
        {
            NumberButtonClicked(0);
        }


        private void buttonDot_Click(object sender, EventArgs e)
        {
            DotClicked();
        }


        private void buttonDiv_Click(object sender, EventArgs e)
        {
            OperatorButtonClicked(Button.BUTTON_DIVIDE);
        }


        private void buttonMultip_Click(object sender, EventArgs e)
        {
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
        }


        private void buttonMinus_Click(object sender, EventArgs e)
        {
            OperatorButtonClicked(Button.BUTTON_MINUS);
        }


        private void buttonPlus_Click(object sender, EventArgs e)
        {
            OperatorButtonClicked(Button.BUTTON_PLUS);
        }


        private void buttonEqual_Click(object sender, EventArgs e)
        {
            EqualClicked();
        }


        private void buttonSqrt_Click(object sender, EventArgs e)
        {
            SqrtClicked();
        }


        private void buttonPerct_Click(object sender, EventArgs e)
        {
            PercentageClicked();
        }


        private void buttonPlusMinus_Click(object sender, EventArgs e)
        {
            PlusMinusClicked();
        }


        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            BackspaceClicked();
        }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearClicked();
        }


        private void buttonHist_Click(object sender, EventArgs e)
        {
            if(m_histForm.IsDisposed)
            {
                m_histForm = new HistoryForm();
            }

            if(!m_histForm.Visible)
            {
                m_histForm.Show(this);
            }

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
            textBox.Focus();
        }
        

        void OperatorButtonClicked(Button button)
        {
            m_numberOne = Convert.ToDouble(textBox.Text);

            switch(button)
            {
            case Button.BUTTON_PLUS:
            case Button.BUTTON_MINUS:
            case Button.BUTTON_MULTIPLY:
            case Button.BUTTON_DIVIDE:
                m_lastClickedOperator = button;
                m_lastClickedButton   = button;
                break;

            default:
                throw new ArgumentException("Bad button.");
            }

            SetLastClickedButtonType(ButtonType.BUTTON_TYPE_OPERATOR);
        }


        void SetLastClickedButtonType(ButtonType buttonType)
        {
            m_lastClickedButtonType = buttonType;
            textBox.SelectionStart  = textBox.Text.Length;
            textBox.ScrollToCaret();
            textBox.Focus();
        }


        void NumberButtonClicked(int number)
        {
            if (number < 0 || number > 9)
            {
                return;
            }

            if(    TestTextLength()        == false
                && m_lastClickedButtonType != ButtonType.BUTTON_TYPE_OPERATOR)
            {
                return;
            }

            string numberStr = number.ToString();
            Button button    = (Button) number;

            if(     m_lastClickedButtonType == ButtonType.BUTTON_TYPE_OPERATOR
                ||  m_lastClickedButton     == Button.BUTTON_NONE
                ||  (   m_lastClickedButton == Button.BUTTON_SQRT 
                     || m_lastClickedButton == Button.BUTTON_PERCENTAGE 
                     || m_lastClickedButton == Button.BUTTON_PLUS_MINUS 
                     || m_lastClickedButton == Button.BUTTON_EQUAL) )
            {
                m_result     = numberStr;
                SetText(m_result);
            }
            else
            {
                if(     m_lastClickedButtonType == ButtonType.BUTTON_TYPE_NUMBER
                    ||  m_lastClickedButtonType == ButtonType.BUTTON_TYPE_OTHER 
                    ||  m_lastClickedButtonType == ButtonType.BUTTON_TYPE_MENU_ITEM)
                {
                    if(number == 0 && textBox.Text == "0")
                    {
                        m_result = "0";
                        SetText(m_result);
                    }
                    else
                    {
                        if(textBox.Text == "0")
                        {
                            m_result = numberStr;
                            SetText(m_result);
                        }
                        else
                        {
                            if(textBox.Text != "0")
                            {
                                m_result = textBox.Text + numberStr;
                                SetText(m_result);
                            }
                        }
                    }
                }
            }

            m_lastClickedButton = button;
            m_lastClickedNumber = button;
            SetLastClickedButtonType(ButtonType.BUTTON_TYPE_NUMBER);            
        }


        void PercentageClicked()
        {
            if (textBox.Text == "Infinity" || textBox.Text.Contains("E"))
            {
                return;
            }

            if (m_lastClickedButton != Button.BUTTON_PERCENTAGE)
            {
                m_numberTwo = Convert.ToDouble(textBox.Text);
                SetText(Convert.ToString((m_numberOne * m_numberTwo) / 100));
                m_result = textBox.Text;

                m_lastClickedButton = Button.BUTTON_PERCENTAGE;
                SetLastClickedButtonType(ButtonType.BUTTON_TYPE_OTHER);

                Buffer.GetInstance().Log(m_numberOne);
                Buffer.GetInstance().Log(" % ");
                Buffer.GetInstance().Log(m_numberTwo);
                Buffer.GetInstance().Log(" = ");
                Buffer.GetInstance().Log(textBox.Text);
                Buffer.GetInstance().LogLine("");
                m_histForm.UpdateText();
            }

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
            textBox.Focus();
        }


        void PlusMinusClicked()
        {
            if (textBox.Text == "Infinity" || textBox.Text.Contains("E"))
            {
                return;
            }

            SetText(Convert.ToString(Convert.ToDouble(textBox.Text) * -1));
            m_result = textBox.Text;

            m_lastClickedButton = Button.BUTTON_PLUS_MINUS;
            SetLastClickedButtonType(ButtonType.BUTTON_TYPE_OTHER);

            Buffer.GetInstance().Log(Convert.ToDouble(textBox.Text) * -1);
            Buffer.GetInstance().Log(" * -1 = ");
            Buffer.GetInstance().Log(textBox.Text);
            Buffer.GetInstance().LogLine("");
            m_histForm.UpdateText();
        }


        void SqrtClicked()
        {
            if (textBox.Text == "Infinity" || textBox.Text.Contains("E"))
            {
                return;
            }

            double val = Convert.ToDouble(textBox.Text);

            if (val >= 0)
            {
                val = Math.Sqrt(val);
                SetText(Convert.ToString(val));
                m_result = textBox.Text;

                m_lastClickedButton = Button.BUTTON_SQRT;
                SetLastClickedButtonType(ButtonType.BUTTON_TYPE_OTHER);

                Buffer.GetInstance().Log("sqrt(");
                Buffer.GetInstance().Log(val * val);
                Buffer.GetInstance().Log(") = ");
                Buffer.GetInstance().Log(textBox.Text);
                Buffer.GetInstance().LogLine("");
                m_histForm.UpdateText();
            }
            else if (val < 0)
            {
                MessageBox.Show("Cannot take the square root of a negative number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
            textBox.Focus();
        }


        void EqualClicked()
        {
            if (textBox.Text == "Infinity")
            {
                return;
            }

            if (   m_lastClickedButtonType == ButtonType.BUTTON_TYPE_NUMBER
                || m_lastClickedButton     == Button.BUTTON_EQUAL
                || m_lastClickedButton     == Button.BUTTON_PERCENTAGE
                || m_lastClickedButton     == Button.BUTTON_DOT
                || m_lastClickedButton     == Button.BUTTON_PLUS_MINUS
                || m_lastClickedButton     == Button.BUTTON_COPY_MENU_ITEM
                || m_lastClickedButton     == Button.BUTTON_PASTE_MENU_ITEM)
            {
                Button operatorButton = m_lastClickedOperator;
                bool   error          = false;

                //normal calculation: 4+2=6
                if (m_lastClickedButtonType == ButtonType.BUTTON_TYPE_NUMBER)
                {
                    m_numberTwo = Convert.ToDouble(textBox.Text);
                }


                switch(m_lastClickedButton)
                {
                    //fast calculation: 4+2=8=10=12=14...
                    case Button.BUTTON_EQUAL:
                        m_numberOne = Convert.ToDouble(textBox.Text);
                        break;

                    case Button.BUTTON_PERCENTAGE:
                        m_numberTwo = Convert.ToDouble(textBox.Text);
                        break;

                    case Button.BUTTON_COPY_MENU_ITEM:
                    case Button.BUTTON_PASTE_MENU_ITEM:
                        m_numberTwo = Convert.ToDouble(textBox.Text);
                        break;

                    case Button.BUTTON_DOT:
                        //erase the '.' first
                        m_numberTwo = Convert.ToDouble(textBox.Text.Remove(textBox.Text.Length - 1, 1));
                        break;

                    case Button.BUTTON_PLUS_MINUS:
                        m_numberTwo = Convert.ToDouble(textBox.Text);
                        break;
                }

                switch (operatorButton)
                {
                    case Button.BUTTON_PLUS:
                        SetText(Convert.ToString(m_numberOne + m_numberTwo));
                        LogBasicOperation(Button.BUTTON_PLUS);
                        break;

                    case Button.BUTTON_MINUS:
                        SetText(Convert.ToString(m_numberOne - m_numberTwo));
                        LogBasicOperation(Button.BUTTON_MINUS);
                        break;

                    case Button.BUTTON_MULTIPLY:
                        SetText(Convert.ToString(m_numberOne * m_numberTwo));
                        LogBasicOperation(Button.BUTTON_MULTIPLY);
                        break;

                    case Button.BUTTON_DIVIDE:
                        if (m_numberTwo == 0)
                        {
                            MessageBox.Show("Cannot divide by 0.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            m_result     = "0";
                            textBox.Text = "0";
                            m_numberOne  = 0;
                            m_numberTwo  = 0;
                            break;
                        }
                        else
                        {
                            SetText(Convert.ToString(m_numberOne / m_numberTwo));
                            LogBasicOperation(Button.BUTTON_DIVIDE);
                            break;
                        }

                    default:
                        error = true;
                        break;
                }//end of switch

                if(!error)
                {
                    m_result = textBox.Text;
                }

                m_lastClickedButton = Button.BUTTON_EQUAL;
                SetLastClickedButtonType(ButtonType.BUTTON_TYPE_OTHER);
            }
            else
            {
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
                textBox.Focus();
            }
        }


        void DotClicked()
        {
            if (textBox.Text == "Infinity" || textBox.Text.Contains("E"))
            {
                return;
            }

            //if it has no '.'
            if (textBox.Text.IndexOf('.') == -1)
            {
                m_result = m_result + ".";
                SetText(m_result);

                m_lastClickedButton = Button.BUTTON_DOT;
                SetLastClickedButtonType(ButtonType.BUTTON_TYPE_OTHER);
            }
            else
            {
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
                textBox.Focus();
            }
        }


        void BackspaceClicked()
        {
            if (textBox.Text == "Infinity" || textBox.Text.Contains("E"))
            {
                return;
            }

            if (textBox.Text != "0")
            {
                m_result = m_result.Remove(m_result.Length - 1, 1);

                if (m_result == "" || m_result == "-")
                {
                    m_result = "0";
                }

                SetText(m_result);

                m_lastClickedButton = Button.BUTTON_BACKSPACE;
                SetLastClickedButtonType(ButtonType.BUTTON_TYPE_OTHER);
            }
            else
            {
                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
                textBox.Focus();
            }
        }


        void ClearClicked()
        {
            m_result     = "0";
            textBox.Text = "0";
            m_numberOne  = 0;
            m_numberTwo  = 0;

            SetLastClickedButtonType(ButtonType.BUTTON_TYPE_OTHER);
            m_lastClickedButton   = Button.BUTTON_CLEAR;
            m_lastClickedNumber   = Button.BUTTON_NONE;
            m_lastClickedOperator = Button.BUTTON_NONE;
        }


        void LogBasicOperation(Button operatorButton)
        {
            Buffer.GetInstance().Log(m_numberOne);

            switch(operatorButton)
            {
            case Button.BUTTON_PLUS:         
                Buffer.GetInstance().Log(" + ");
                Buffer.GetInstance().Log(m_numberTwo);
                Buffer.GetInstance().Log(" = ");
                Buffer.GetInstance().Log(m_numberOne + m_numberTwo);
                break;

            case Button.BUTTON_MINUS:
                Buffer.GetInstance().Log(" - ");
                Buffer.GetInstance().Log(m_numberTwo);
                Buffer.GetInstance().Log(" = ");
                Buffer.GetInstance().Log(m_numberOne - m_numberTwo);
                break;

            case Button.BUTTON_MULTIPLY:
                Buffer.GetInstance().Log(" * ");
                Buffer.GetInstance().Log(m_numberTwo);
                Buffer.GetInstance().Log(" = ");
                Buffer.GetInstance().Log(m_numberOne * m_numberTwo);
                break;

            case Button.BUTTON_DIVIDE:
                Buffer.GetInstance().Log(" / ");
                Buffer.GetInstance().Log(m_numberTwo);
                Buffer.GetInstance().Log(" = ");
                Buffer.GetInstance().Log(m_numberOne / m_numberTwo);
                break;

            default:
                throw new ArgumentException("Bad operatorButton.");
            }

            Buffer.GetInstance().LogLine("");
            m_histForm.UpdateText();
        }


        bool TestTextLength()
        {
            if(textBox.Text.Length >= MAX_LENGTH)
            {
                return false;
            }

            return true;
        }


        void SetText(string text)
        {
            int size = text.Length;

            if(size <= MAX_LENGTH/2)
            {
                this.textBox.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            else
            {
                this.textBox.Font = new System.Drawing.Font("Comic Sans MS", 16.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            //do not chop text if it contains E
            if(text.Contains("E"))
            {
                textBox.Text = text;
                return;
            }

            if (size >= MAX_LENGTH)
            {
                text = text.Remove(MAX_LENGTH, size - MAX_LENGTH);
            }

            textBox.Text = text;
        }


        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    NumberButtonClicked(Convert.ToInt16(e.KeyChar) - ASCII_OFFSET);
                    break;
 
                case '+':           OperatorButtonClicked(Button.BUTTON_PLUS);        break;
                case '-':           OperatorButtonClicked(Button.BUTTON_MINUS);       break;
                case '/':           OperatorButtonClicked(Button.BUTTON_DIVIDE);      break;
                case '*':           OperatorButtonClicked(Button.BUTTON_MULTIPLY);    break;
                case '.':           DotClicked();                                     break;
                case (char)127:     ClearClicked();                                   break; //todo: del char

                case '=':
                case '\r': //enter
                    EqualClicked();
                    break;

                case '\b': //backspace
                    BackspaceClicked();
                    break;
            }//switch

            e.Handled = true;
        }


        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }


        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }


        void Copy()
        {
            Clipboard.SetText(textBox.Text);

            m_lastClickedButton = Button.BUTTON_COPY_MENU_ITEM;
            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
            textBox.Focus();
        }


        void Paste()
        {
            try
            {
                double val = Convert.ToDouble(Clipboard.GetText());
            }
            catch (FormatException)
            {
                ClearClicked();
                return;
            }

            SetText(Clipboard.GetText());

            m_lastClickedButton = Button.BUTTON_PASTE_MENU_ITEM;
            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
            textBox.Focus();
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Kalculator by Berk C. Celebisoy\nCopyright (c) - All Rights Reserved.", "Kalculator", MessageBoxButtons.OK, MessageBoxIcon.Information);

            textBox.SelectionStart = textBox.Text.Length;
            textBox.ScrollToCaret();
            textBox.Focus();
        }


        void ExecUnitTests()
        {
            //1.1.+2.10
            NumberButtonClicked(1);
            DotClicked();
            NumberButtonClicked(1);
            DotClicked();
            OperatorButtonClicked(Button.BUTTON_PLUS);
            NumberButtonClicked(2);
            DotClicked();
            NumberButtonClicked(1);
            NumberButtonClicked(0);
            EqualClicked();

            if(textBox.Text != "3.2")
                Debugger.Break();

            //40*2
            NumberButtonClicked(4);
            NumberButtonClicked(0);
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(2);
            EqualClicked();

            if (textBox.Text != "80")
                Debugger.Break();

            //100/10.0
            NumberButtonClicked(1);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            OperatorButtonClicked(Button.BUTTON_DIVIDE);
            NumberButtonClicked(1);
            NumberButtonClicked(0);
            DotClicked();
            NumberButtonClicked(0);
            EqualClicked();

            if (textBox.Text != "10")
                Debugger.Break();

            //40-41
            NumberButtonClicked(4);
            NumberButtonClicked(0);
            OperatorButtonClicked(Button.BUTTON_MINUS);
            NumberButtonClicked(4);
            NumberButtonClicked(1);
            EqualClicked();

            if (textBox.Text != "-1")
                Debugger.Break();

            ClearClicked();

            //10+2===BS
            NumberButtonClicked(1);
            NumberButtonClicked(0);
            OperatorButtonClicked(Button.BUTTON_PLUS);
            NumberButtonClicked(2);
            EqualClicked();
            EqualClicked();
            EqualClicked(); //16
            BackspaceClicked();

            if (textBox.Text != "1")
                Debugger.Break();

            ClearClicked();

            //625sqrt,sqrt,sqrt,+/-,*111 , +/- =
            NumberButtonClicked(6);
            NumberButtonClicked(2);
            NumberButtonClicked(5);
            SqrtClicked();
            SqrtClicked();
            PlusMinusClicked(); //-5
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(1);
            NumberButtonClicked(1);
            NumberButtonClicked(1);
            PlusMinusClicked();
            EqualClicked();

            if (textBox.Text != "555")
                Debugger.Break();

            ClearClicked();

            //1000.1 , BS, BS, BS , SQRT , +/- , +/- , +990.0
            NumberButtonClicked(1);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            DotClicked();
            NumberButtonClicked(1);
            BackspaceClicked();
            BackspaceClicked();
            BackspaceClicked(); //100
            SqrtClicked(); //10
            PlusMinusClicked();
            PlusMinusClicked();
            OperatorButtonClicked(Button.BUTTON_PLUS); //10
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(0);
            DotClicked();
            NumberButtonClicked(0);
            EqualClicked(); //1000

            if (textBox.Text != "1000")
                Debugger.Break();

            ClearClicked();

            //400-1 (100 times) , /2=== , -/+ , BS, ...
            NumberButtonClicked(4);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            OperatorButtonClicked(Button.BUTTON_MINUS);
            NumberButtonClicked(1);
            for (int i = 0; i < 100; i++)
                EqualClicked(); //300
            OperatorButtonClicked(Button.BUTTON_DIVIDE);
            NumberButtonClicked(2); //150
            EqualClicked();
            EqualClicked();
            EqualClicked(); //37.5
            PlusMinusClicked();
            BackspaceClicked();
            DotClicked();
            DotClicked();
            DotClicked();

            if (textBox.Text != "-37.")
                Debugger.Break();

            ClearClicked();

            //2*2 (= x 10) , BS , BS , /2 , /2 , -1 , sqrt
            NumberButtonClicked(2);
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(2);
            for (int i = 0; i < 10; i++)
                EqualClicked(); //2048
            BackspaceClicked();
            BackspaceClicked(); //20
            OperatorButtonClicked(Button.BUTTON_DIVIDE);
            NumberButtonClicked(2);
            EqualClicked();
            OperatorButtonClicked(Button.BUTTON_DIVIDE);
            NumberButtonClicked(2);
            EqualClicked(); //5
            OperatorButtonClicked(Button.BUTTON_MINUS);
            NumberButtonClicked(1);
            EqualClicked(); //4
            SqrtClicked();

            if (textBox.Text != "2")
                Debugger.Break();

            //100+50%==-+*4=+50% , +/- , BS , BS , +/- , sqrt , - , 1.5 , =
            NumberButtonClicked(1);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            OperatorButtonClicked(Button.BUTTON_PLUS);
            NumberButtonClicked(5);
            NumberButtonClicked(0);
            PercentageClicked(); //50
            EqualClicked(); //150
            EqualClicked(); //200
            OperatorButtonClicked(Button.BUTTON_MINUS);
            OperatorButtonClicked(Button.BUTTON_PLUS);
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(4);
            EqualClicked(); //800
            OperatorButtonClicked(Button.BUTTON_PLUS);
            NumberButtonClicked(5);
            NumberButtonClicked(0);
            PercentageClicked(); //400
            PlusMinusClicked();
            BackspaceClicked(); //-40
            BackspaceClicked(); //-4
            PlusMinusClicked(); //4
            SqrtClicked(); //2
            OperatorButtonClicked(Button.BUTTON_MINUS);
            NumberButtonClicked(1);
            DotClicked();
            NumberButtonClicked(5);
            EqualClicked();

            if (textBox.Text != "0.5")
                Debugger.Break();

            Copy();
            ClearClicked();

            //paste (0.5) , ***0.05=== , *100000000 , copy , + , paste , =.
            Paste();
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(0);
            DotClicked();
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(5);
            EqualClicked();
            EqualClicked();
            EqualClicked(); //0.0000000625
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(1);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            EqualClicked(); //6.25
            Copy();
            OperatorButtonClicked(Button.BUTTON_PLUS);
            Paste();
            EqualClicked(); //12.5
            DotClicked();

            if (textBox.Text != "12.5")
                Debugger.Break();

            //00012345,BS,BS,4567890 - 123456789,BS,90 , =*123= , +/-
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(1);
            NumberButtonClicked(2);
            NumberButtonClicked(3);
            NumberButtonClicked(4);
            NumberButtonClicked(5);
            BackspaceClicked();
            BackspaceClicked();
            NumberButtonClicked(4);
            NumberButtonClicked(5);
            NumberButtonClicked(6);
            NumberButtonClicked(7);
            NumberButtonClicked(8);
            NumberButtonClicked(9);
            NumberButtonClicked(0);
            OperatorButtonClicked(Button.BUTTON_MINUS);
            NumberButtonClicked(1);
            NumberButtonClicked(2);
            NumberButtonClicked(3);
            NumberButtonClicked(4);
            NumberButtonClicked(5);
            NumberButtonClicked(6);
            NumberButtonClicked(7);
            NumberButtonClicked(8);
            NumberButtonClicked(9);
            BackspaceClicked();
            NumberButtonClicked(9);
            NumberButtonClicked(0);
            EqualClicked(); //0
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(1);
            NumberButtonClicked(2);
            NumberButtonClicked(3);
            EqualClicked(); //0
            PlusMinusClicked();

            if (textBox.Text != "0")
                Debugger.Break();

            //999*999===== BS , % , sqrt , . , +/-
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            EqualClicked();
            EqualClicked();
            EqualClicked();
            EqualClicked();
            EqualClicked();
            BackspaceClicked();
            PercentageClicked();
            SqrtClicked();
            DotClicked();
            PlusMinusClicked();

            if (textBox.Text != "9.94014980014994E+17")
                Debugger.Break();

            ClearClicked();

            //99.-98.=+0.0000000001 , copy , x0=, paste, +0+1=
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            DotClicked();
            OperatorButtonClicked(Button.BUTTON_MINUS);
            NumberButtonClicked(9);
            NumberButtonClicked(8);
            DotClicked();
            EqualClicked(); //1
            OperatorButtonClicked(Button.BUTTON_PLUS);
            NumberButtonClicked(0);
            DotClicked();
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(0);
            NumberButtonClicked(1);
            EqualClicked();
            Copy();
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(0);
            EqualClicked();
            Paste();
            OperatorButtonClicked(Button.BUTTON_PLUS);
            NumberButtonClicked(0);
            EqualClicked();
            OperatorButtonClicked(Button.BUTTON_PLUS);
            NumberButtonClicked(1);
            EqualClicked();

            if (textBox.Text != "2.0000000001")
                Debugger.Break();

            ClearClicked();

            //9999999999*9999999999 , 30 times = , BS , % , sqrt , . , +/- , 1*1===
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            NumberButtonClicked(9);
            for (int i = 0; i < 30; i++)
                EqualClicked();
            BackspaceClicked();
            PercentageClicked();
            SqrtClicked();
            DotClicked();
            PlusMinusClicked();

            if (textBox.Text != "Infinity")
                Debugger.Break();

            NumberButtonClicked(1);
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(1);
            EqualClicked();
            EqualClicked();
            EqualClicked();

            if (textBox.Text != "1")
                Debugger.Break();

            ClearClicked();

            //test chopping
            SetText("1.0000000000000000012");

            if (textBox.Text != "1.000000000000000001")
                Debugger.Break();

            SetText("55500000000000009999123");

            if (textBox.Text != "55500000000000009999")
                Debugger.Break();

            ClearClicked();

            //do not chop if it has E
            SetText("9.58940490108885E+4500000");

            if (textBox.Text != "9.58940490108885E+4500000")
                Debugger.Break();

            ClearClicked();

            //-2*-2 , sqrt, -2*0.01= , +/- , =
            NumberButtonClicked(2);
            PlusMinusClicked();
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(2);
            PlusMinusClicked();
            EqualClicked();
            SqrtClicked(); //2
            OperatorButtonClicked(Button.BUTTON_MINUS);
            NumberButtonClicked(2);
            EqualClicked();
            OperatorButtonClicked(Button.BUTTON_MULTIPLY);
            NumberButtonClicked(0);
            DotClicked();
            NumberButtonClicked(0);
            NumberButtonClicked(1);
            EqualClicked();
            PlusMinusClicked();
            EqualClicked();

            if (textBox.Text != "0")
                Debugger.Break();

        }

    }//class
}//namespace
