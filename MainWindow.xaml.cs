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

namespace WindowsCalcWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void ChangeInputData() //Расчёт цены стеклопакета
        {
            if (CheckInputFields())
            {
                string result = Calc();
                result = toMoneyType(result);
                ChangeOutput(result + "₽");
                if (labelEnd != null)
                    labelEnd.Content = "Итого:";
            }
        }

        private string toMoneyType(string text) // Преобразование в денежный формат (1234,56 -> 1 234,56)
        {
            string[] textMassive = text.Split(',');
            string textBeforeSemicolon = textMassive[0];
            string textAfterSemicolon = "";

            if (textMassive.Length > 1)
            {
                textAfterSemicolon = textMassive[1];
            }

            string tempReverseText = "";
            string resultBeforeConcat = "";

            for (int i = textBeforeSemicolon.Length; i > 0; i--)
            {
                tempReverseText += textBeforeSemicolon[i - 1].ToString();
            }

            for (int i = tempReverseText.Length; i > 0; i--)
            {
                if (i % 3 == 0)
                {
                    resultBeforeConcat += " ";
                }
                resultBeforeConcat += tempReverseText[i - 1].ToString();
            }

            string result = resultBeforeConcat;

            if (textMassive.Length > 1)
            {
                result += "," + textAfterSemicolon;
            }

            return result;
        }

        private string Calc() //Расчёт общей суммы
        {
            double costMCubic = 4629; // от 4 629 ₽/м2

            double w = double.Parse(textBoxW.Text);
            double h = double.Parse(textBoxH.Text);

            double result = (w / 1000 * h / 1000) * costMCubic; // Цена стеклопакета

            if (ComboBoxTypes.SelectedIndex != -1) // Тип механизма
            {
                additionalFuncs(true);

                if (ComboBoxTypes.SelectedIndex == 1)
                    result += 2499;  //Поворотный механизм наценка 2499 рублей
                else if (ComboBoxTypes.SelectedIndex == 2)
                    result += 3499; //Поворотно-откидной механизм наценка 3499 рублей
                else
                {
                    additionalFuncs(false);
                }
            }
            else
            {
                additionalFuncs(false);
            }

            if ((bool) checkBox1.IsChecked) // Москитка
            {
                double costMoscito = (w / 1000 * h / 1000) * 833;
                result += costMoscito;
            }

            if ((bool)checkBox2.IsChecked) // Фиксатор
            {
                double costFixer = 200;
                result += costFixer;
            }

            if ((bool)checkBox3.IsChecked) // Микропроветривание
            {
                double costMicro = 500;
                result += costMicro;
            }

            string sResult = "" + Math.Round(result, 2);
            return sResult;
        }

        void additionalFuncs(bool onOff)
        {
            if (!onOff)
            {
                checkBox1.IsChecked = onOff;
                checkBox2.IsChecked = onOff;
                checkBox3.IsChecked = onOff;
            }
            labelFunc.IsEnabled = onOff;
            checkBox1.IsEnabled = onOff;
            checkBox2.IsEnabled = onOff;
            checkBox3.IsEnabled = onOff;
        }

        bool CheckInputFields()
        {
            if (textBoxW.Text == "0" || string.IsNullOrEmpty(textBoxW.Text)) {
                ChangeOutput("Заполните параметр \"Ширина\"");
                return false;
            }
            else if (int.Parse(textBoxW.Text) < 500)
            {
                ChangeOutput("\"Ширина\", должна быть, больше 500мм");
                return false;
            }
            else if (textBoxH.Text == "0" || string.IsNullOrEmpty(textBoxH.Text))
            {
                ChangeOutput("Заполните параметр \"Высота\"");
                return false;
            }
            else if (int.Parse(textBoxH.Text) < 500)
            {
                ChangeOutput("\"Высота\" должна быть, больше 500мм");
                return false;
            }
            else if (ComboBoxTypes.SelectedIndex == -1)
            {
                ChangeOutput("Выберите параметр \"Тип механизма\"");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ChangeOutput(string text)
        {
            if (labelEnd != null)
            labelEnd.Content = "";
            if (labelOutput != null)
            labelOutput.Content = text;
        }
		
        private void SomeChanged(object sender, SelectionChangedEventArgs e) //Выпадающее меню
        {
            ChangeInputData();
        }

        private void SomeChanged(object sender, TextChangedEventArgs e) //Текстовое поле
        {
            ChangeInputData();
        }

        private void SomeChanged(object sender, RoutedEventArgs e) // Чек-бокс
        {
            ChangeInputData();
        }

        private void PreviewTextInputNum(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0; //Разрешение на ввод только цифр
        }
    }
}
