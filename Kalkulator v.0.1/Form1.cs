using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Kalkulator_v._0._1
{
    public partial class Form1 : Form
    {
        double tall_1 = 0;
        double tall_2 = 0;
        char operator_1;
        bool math_executed = false;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Kalkulator";
        }

        #region GUI
        private void num_click(object sender, EventArgs e) // Når man klikker på talltastene i GUI
        {
            Button num = (Button)sender;
            char num_char = num.Text[0];
            num_input(num_char);
            btn_equals.Focus();
        }
        private void Display_TextChanged(object sender, EventArgs e) // Endrer posisjon på pekeren til bakerste tall
        {
            display.SelectionStart = display.Text.Length;
            display.SelectionLength = 0;
        }
        private void display_click(object sender, EventArgs e)
        {
            display.SelectionStart = display.Text.Length;
            display.SelectionLength = 0;
        }
        private void Comma_btn_Click(object sender, EventArgs e)
        {
            add_comma();
        }
        private void Clear_btn_Click(object sender, EventArgs e)
        {
            clear();
        }
        private void Backspace_btn_Click(object sender, EventArgs e)
        {
            backspace();
        }
        private void operator_click(object sender, EventArgs e) 
        {
            fixEmptyComma(); 
            tall_1 = Double.Parse(display.Text);
            Button op = (Button)sender;
            operator_1 = op.Text[0];
            input_history.Text = display.Text + operator_1;
            display.Text = "0";
        }
        private void Btn_square_Click(object sender, EventArgs e)
        {
            fixEmptyComma();
            tall_1 = Double.Parse(display.Text);
            double squared = Math.Pow(tall_1, 2);
            display.Text = squared.ToString();
            input_history.Text = tall_1.ToString() + '²';
            math_executed = true;
        }
        private void Btn_qube_Click(object sender, EventArgs e)
        {
            fixEmptyComma();
            tall_1 = Double.Parse(display.Text);
            double squared = Math.Pow(tall_1, 3);
            display.Text = squared.ToString();
            input_history.Text = tall_1.ToString() + '³';
            math_executed = true;
        }
        private void Btn_sqrt_Click(object sender, EventArgs e)
        {
            fixEmptyComma();
            tall_1 = Double.Parse(display.Text);
            double sqrt = Math.Sqrt(tall_1);
            display.Text = sqrt.ToString();
            input_history.Text = "√" + tall_1;
            math_executed = true;
        }
        private void Btn_pow_Click(object sender, EventArgs e)
        {
            fixEmptyComma();
            tall_1 = Double.Parse(display.Text);
            operator_1 = '^';
            input_history.Text = tall_1.ToString() + '^';
            display.Text = "0";
        }
        private void Btn_equals_Click(object sender, EventArgs e)
        {
            fixEmptyComma();
            execute_math();
        }
        private void Change_sign_Click(object sender, EventArgs e)
        {
            double i = Convert.ToDouble(display.Text) * -1;
            display.Text = i.ToString();
        }

        #endregion

        #region Tastatur (behandler inndata fra tastaturet)
        private void key_press(object sender, KeyPressEventArgs e) // Behandle trykk fra PC-tastatur
        {
            display.Focus();
            switch (e.KeyChar)
            {
                case 'c':
                case 'C':
                    clear();
                    break;
                case ',':
                case '.':
                    add_comma();
                    break;

                case '+':
                case '-':
                case '*':
                case '/':
                    set_operator(e.KeyChar);
                    break;
                case (char)13: // Enter key
                    execute_math();
                    break;
                case (char)8: //backspace
                    backspace();
                    break;
                default:

                    if (Convert.ToInt32(e.KeyChar) >= 48 && Convert.ToInt32(e.KeyChar) <= 57) // Sjekk om char er et tall [0-9]
                        num_input(e.KeyChar);
                    break;
            }
        }
        #endregion

        #region Delte funksjoner (brukt av både ved GUI og Tastatur grensesnitt)

        private void num_input (char num) // Skriver tall i args til display.
        {
            if (display.Text == "0" || math_executed == true)
            {
                display.Text = "";
                if (input_history.Text.Length == 1 || math_executed == true)
                    input_history.Text = "";
                else if (operator_1 == ' ')
                    input_history.Text = "";
                math_executed = false;
            }
            else if (input_history.Text.IndexOf('=') != -1) // Hvis '=' er i historikken
                input_history.Text = display.Text;

            display.Text += num; // Sett tall inn i display
            input_history.Text += num;
        }
        private void backspace()
        {
            if (input_history.Text.Contains('='))
                input_history.Text = display.Text;
            if(display.Text.Length == 2 && display.Text[0] == '-')
            {
                clear();
            }
            else if (display.Text.Length == 1) {
                if (display.Text != "0" && input_history.Text.Length > 0)
                    input_history.Text = input_history.Text.Remove(input_history.Text.Length - 1);
                display.Text = "0";
            }
            else if (display.Text.Length > 0)
            {
                if (input_history.Text.Length > 0)
                    input_history.Text = input_history.Text.Remove(input_history.Text.Length - 1);
                display.Text = display.Text.Remove(display.Text.Length - 1);

            }
        }
        private void add_comma()
        {
            if (!display.Text.Contains('.'))
            {
                display.Text += '.';
                if (!input_history.Text.Contains('.'))
                    input_history.Text += '.';
            }
        }
        private void fixEmptyComma() // Sletter komma hvis det er siste tegnet i display (kan føre til masse bugs)
        {
            if (display.Text[display.Text.Length - 1] == '.' || display.Text[display.Text.Length - 1] == ',')
                display.Text = display.Text.Remove(display.Text.Length - 1);
        }
        private void clear()
        {
            tall_1 = 0;
            tall_2 = 0;
            display.Text = "0";
            input_history.Text = "";
        }
        private void set_operator(char op) // Setter opperator og display når man trykker inn operator på tastaturet. 
        {
            fixEmptyComma();
            math_executed = false;
            tall_1 = Double.Parse(display.Text);
            input_history.Text = display.Text+ op;
            display.Text = "0";
            operator_1 = op;  
        }
        private void execute_math() 
        {
            fixEmptyComma();
            tall_2 = Double.Parse(display.Text);
            switch (operator_1)
            {
                case '+':
                    double sum = tall_1 + tall_2;
                    display.Text = sum.ToString();
                    break;
                case '-':
                    double difference = tall_1 - tall_2;
                    display.Text = difference.ToString();
                    break;
                case '*':
                    double product = tall_1 * tall_2;
                    display.Text = product.ToString();
                    break;
                case '/':
                    double quotient = tall_1 / tall_2;
                    display.Text = quotient.ToString();
                    break;
                case '^':
                    double result = Math.Pow(tall_1, tall_2);
                    display.Text = result.ToString();
                    break;
            }
            math_executed = true;
            if (operator_1 != ' ') // Forhindrer at input history blir rar når man trykker '=' flere ganger
            {
                input_history.Text = tall_1.ToString() + operator_1 + tall_2.ToString() + '=';
            }
            operator_1 = ' ';
        }
    }
    #endregion
}

