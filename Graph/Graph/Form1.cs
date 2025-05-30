using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public partial class Form1 : Form
    {
        static double a = 0.01; // шаг с которым двигаемся по графику
        static double lowerX; //  нижняя граница икс
        static double lowerY = -5;
        static double upperY = 5;
        static double upperX; // верхняя граница икс
        static bool upperXInput = false;
        static bool lowerXInput = false;
        static bool GraphInput = false;
        public Form1() // открытие окна
        {
            InitializeComponent();
        }
        // построение графика по кнопке "построить график"
        private void showGraph_Click(object sender, EventArgs e)
        {
            if (double.TryParse(this.getUpperX.Text, out double upperx))
            {
                upperX = upperx;  // запоминаем значение
                upperXInput = true; // помечаем, что получили его
            }
            else // иначе выводим сообщение об ошибке
            {       
                MessageBox.Show("Вы ввели некорректные данные. Пожалуйста, введите границы в ячейки");
                return;
            }
            // Повторяем то же самое для нижней границы
            if (double.TryParse(this.getLowerX.Text, out double lowerx))
            {
                lowerX = lowerx;
                lowerXInput = true;
            }
            else
            {
                MessageBox.Show("Вы ввели некорректные данные. Пожалуйста, введите границы в ячейки");
                return;
            }
            if (upperXInput && lowerXInput) // если обе границы введены, можем начинать строить график
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                // функция и производная:
                /*
                 *             2x               2
                 * y = arcsin _____    y'= - ______
                             1+x^2           1+x^2
                 */
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                chart1.ChartAreas[0].AxisY.Maximum = upperY;
                chart1.ChartAreas[0].AxisY.Minimum = lowerY;
                // на всякий случай проверяем на корректность введённые границы, если что, меняем местами
                if (lowerX > upperX)
                {
                    double temp = upperX;
                    upperX = lowerX;
                    lowerX = temp;
                }
                double x = lowerX; // будем двигаться начиная с нижней границы
                while (x < upperX) // до тех пор пока икс меньше верхней границы
                {
                    double function = (2 * x) / (1 + Math.Pow(x, 2)); // считаем функцию от которой будем брать арксинус
                    double dy = -2 / (1 + Math.Pow(x, 2)); // считаем произодную
                    // т. к. arcsin(t) определён при -1<=t<=1
                    // если функция больше единицы либо меньше минус единицы, могут появиться точки разрыва
                    if (function > 1 || function < -1)
                    {
                        // инициализируем игрик как дабл.нан
                        chart1.Series[0].Points.AddXY(x, double.NaN);
                        chart1.Series[1].Points.AddXY(x, dy);
                        x += a;
                        continue;
                    }
                    // сюда переходим если арксинус можно высчитать 
                    double y = Math.Asin(function); // считаем арксинус
                    // добавляем точки
                    chart1.Series[0].Points.AddXY(x, y);
                    chart1.Series[1].Points.AddXY(x, dy);
                    x += a;
                }
                GraphInput = true; // помечаем, что график был введён 
            }
            else
                MessageBox.Show("Пожалуйста, введите границы в ячейки");
        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (GraphInput) // если график введён, мы можем показывать на нём координаты
            {
                // преобразуем позицию на экране в координаты:
                Point chartLocationOnForm = chart1.FindForm().PointToClient(chart1.Parent.PointToScreen(chart1.Location));
                double x = chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X - chartLocationOnForm.X);
                double y = chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y - chartLocationOnForm.Y);
                // проверяем, находится ли она в рамках нашего окошка
                if (x < upperX && x > lowerX && y < upperY && y > lowerY)
                {
                    label3.Location = new Point(e.X+10, e.Y+10); // если да, то меняем локацию лейбла на локацию курсора мыши
                    label3.Text = ("X = " + x.ToString() + ", Y = " + y.ToString()); // и меняем текст на координаты
                }
                else // если нет, то лейбл пустой
                    label3.Text = " ";
            }
        }
    }
}
