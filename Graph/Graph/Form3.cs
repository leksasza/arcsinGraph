using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Graph
{
    public partial class Form3 : Form
    {
        static double a = 0.01;
        static double lowerX;
        static double lowerY = -5;
        static double upperY = 5;
        static double upperX;
        static bool upperXInput = false;
        static bool lowerXInput = false;
        static bool GraphInput = false;
        static GraphPane pane;
        public Form3()
        {
            InitializeComponent();
            pane = zedGraphControl1.GraphPane;
            pane.Title.Text = "Нули функции: (0,0); Kритические точки отсутствуют";
            pane.XAxis.Title.Text = "X";
            pane.YAxis.Title.Text = "Y";
        }
        private void showGraph_Click(object sender, EventArgs e)
        {
            if (double.TryParse(this.getUpperX.Text, out double upperx))
            {
                upperX = upperx;  // запоминаем значение
                upperXInput = true; // помечаем, что получили его
            }
            else
            {
                MessageBox.Show("Вы ввели некорректные данные. Пожалуйста, введите границы в ячейки");
                return;
            }
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
            if (upperXInput && lowerXInput)
            {
                // функция и производная:
                /*
                 *             2x               2
                 * y = arcsin _____    y'= - ______
                             1+x^2           1+x^2
                 */
                pane.YAxis.Scale.Min = lowerY;
                pane.YAxis.Scale.Max = upperY;
                // на всякий случай проверяем на корректность введённые границы, если что, меняем их местами
                if (lowerX > upperX)
                {
                    double temp = upperX;
                    upperX = lowerX;
                    lowerX = temp;
                }
                pane.XAxis.Scale.Min = lowerX;
                pane.XAxis.Scale.Max = upperX;
                pane.CurveList.Clear(); // очищаем холст если до этого что-то рисовали
                double x = lowerX; // начинаем с нижней границы
                PointPairList list_xy = new PointPairList(); // создаём список точек ху для функции
                PointPairList list_xdy = new PointPairList(); // и хdу для производной
                while (x < upperX) // до тех пор пока не дошли до верхней границы
                {
                    double function = (2 * x) / (1 + Math.Pow(x, 2)); // считаем функцию от которой будем брать арксинус
                    double dy = -2 / (1 + Math.Pow(x, 2)); // считаем произодную
                    // т. к. arcsin(t) определён при -1<=t<=1
                    // если функция больше единицы либо меньше минус единицы, могут появиться точки разрыва
                    if (function > 1 || function < -1)
                    {
                        // инициализируем игрик как дабл.нан
                        list_xy.Add(x, double.NaN);
                        list_xdy.Add(x, dy);
                        x += a;
                        continue;
                    }
                    // сюда переходим если арксинус можно высчитать 
                    double y = Math.Asin(function); // считаем арксинус
                    // добавляем точки
                    list_xy.Add(x, y);
                    list_xdy.Add(x, dy);
                    x += a;
                }
                // создаём линии по спискам, называем, ставим цвет
                LineItem myCurve = pane.AddCurve("f(x)", list_xy, Color.Blue, SymbolType.None); // синияя функция
                LineItem myCurve2 = pane.AddCurve("f'(x)", list_xdy, Color.Red, SymbolType.None); // красная производная
                // Меняем толщину линии
                myCurve.Line.Width = 3.0f;
                myCurve2.Line.Width = 3.0f;
                GraphInput = true; // помечаем что график введён
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }
            else
                MessageBox.Show("Пожалуйста, заполните точность");
        }

        private void zedGraphControl1_MouseMove(object sender, MouseEventArgs e)
        {
            // если график был введён, можем пытаться показывать координаты на графике рядом с курсором мыши
            if (GraphInput)
            {
                double x, y;
                // переводим точку на графике
                zedGraphControl1.GraphPane.ReverseTransform(e.Location, out x, out y);
                if (x < upperX && x > lowerX && y < upperY && y > lowerY) // если она принадлежит графику
                {
                    label4.Location = new Point(e.X + 10, e.Y + 10); // меняем локацию лейбла на экране по курсору мыши + отступ
                    label4.Text = ("X = " + x.ToString() + ", Y = " + y.ToString()); // переводим в строки координаты
                }
                else // иначе, если точка не принадлежит графику, делаем текст пустым
                    label4.Text = " ";
            }
        }
    }
}
