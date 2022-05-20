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


namespace MaratProject
{
    public partial class Form1 : Form
    {
        public static int cnt = 0; // Счетчик "Далее"
        public Point panelLockation = new Point(28, 150); // точка нахождения панели
        public int a; // Множимое
        public int b; // Множитель
        public int[] dA; // Множимое в двоичном виде
        public int[] dB; // Множитель в двоичном виде
        public List<TextBox> NowMultiplicandTB = new List<TextBox>(); // Новое значение множимого
        public List<TextBox> PastMultiplicandTB = new List<TextBox>(); // Прошлое значение множимого
        public List<TextBox> NowMultiplierTB = new List<TextBox>(); // Новое значение множителя
        public List<TextBox> PastMultiplierTB = new List<TextBox>(); // Прошлое значение множителя
        public List<TextBox> NowResultTB = new List<TextBox>(); // Результат
        public List<TextBox> PastResultTB = new List<TextBox>(); // Прошлый результат
        public List<int> result = new List<int>(); // Список с конечным результатом
        public List<int>[] state = new List<int>[6]; // Кол-во строк для ячеек
        public string action = ""; // Описание события



        public Form1()
        {
            InitializeComponent();
            FormSets();
        }

        private void SetChanges()
        {
            if(cnt == 1)
            {
                howWorkPanel.Visible = false;
                getDataPanel.Visible = true;
                getDataPanel.Location = panelLockation;
            }
            else if(cnt == 2)
            {
                try
                {
                    a = int.Parse(multiplicandTextBox.Text);
                    b = int.Parse(multiplierTextBox.Text);

                    dA = BinCalc.FromTenToDouble(a).ToArray();
                    dB = BinCalc.FromTenToDouble(b).ToArray();

                    result = BinCalc.FromTenToDouble(a * b).ToList();
                    state[0] = dA.ToList();
                    state[1] = dB.ToList();
                    state[2] = CreateZeroList(result.Count);
                    state[3] = CreateZeroList(dA.Length);
                    state[4] = CreateZeroList(dB.Length);
                    state[5] = CreateZeroList(result.Count);
                    action = "Инициализация";

                    getDataPanel.Visible = false;
                    algoritmPanel.Location = new Point(28, 90);
                    Algoritm();
                }
                catch 
                {
                    MessageBox.Show("Что-то пошло не так. Попробуйте снова.");
                    cnt--;
                }
            }
        }


        private void OpenLastForm()
        {
            next.Visible = false;
            algoritmPanel.Visible = false;
            answerPanel.Visible = true;
            answerPanel.Location = panelLockation;
            answerLabel.Text = BinCalc.ToStringArray(state[2]) + " или " + a*b;
        }

        async private void Algoritm() 
        {
            next.Enabled = false;
            SetPanels();
            actionLabel.Text = action;
            FullPanels(NowResultTB, state[2]);
            FullPanels(NowMultiplicandTB, state[0]);
            FullPanels(NowMultiplierTB, state[1]);

            for (int i = 0; i < dB.Length; i++)
            {
                await Task.Delay(2000);
                ShiftSCHP();
                await Task.Delay(2000);
                AddToSCHP();
                await Task.Delay(5000);
                ShiftMultiplicand();
            }
            MessageBox.Show("That`s All");


            await Task.Delay(2000);
            OpenLastForm();
        }

        public List<int> CreateZeroList(int n)
        {
            List<int> res = new List<int>();

            for (int i = 0; i < n; i++)
                res.Add(0);

            return res;
        }

        private void ShiftSCHP() 
        {
            state[5] = state[2];
            state[2] = BinCalc.ShiftLeft(state[2]);
            action = "Сдвиг СЧП на один бит влево";
            SetState();
        }

        private void AddToSCHP()
        {
            if(state[1][0] == 1)
            {
                state[5] = state[2];
                state[2] = BinCalc.DoubleSumm(state[2], state[0]);
                action = "Если старший бит множителя 1, то с СЧП прибавляется множимое, иначе сложение не производится";
                SetState();
            }
        }

        private void ShiftMultiplicand()
        {
            state[4] = state[1];
            state[1] = BinCalc.ShiftLeft(state[1]);
            action = "Сдвиг множителя на один бит влево";
            SetState();
        }

        private void SetState()
        {
            FullPanels(NowMultiplicandTB, state[0]);
            FullPanels(NowMultiplierTB, state[1]);
            FullPanels(NowResultTB, state[2]);
            FullPanels(PastMultiplicandTB, state[3]);
            FullPanels(PastMultiplierTB, state[4]);
            FullPanels(PastResultTB, state[5]);
            actionLabel.Text = action;
        }

        private void FullPanels(List<TextBox> tb, List<int> tbInf) 
        {
            for (int i = 0; i < tbInf.Count; i++) 
            {
                tb[i].Text = Convert.ToString(tbInf[i]);
            }
        }

        private void SetPanels()
        {
            Point firstPoint = new Point(firstMulLabel.Location.X + 160, firstMulLabel.Location.Y - 3);
            Point secondPoint = new Point(secondMulLabel.Location.X + 160, secondMulLabel.Location.Y - 3);
            Point thirdPoint = new Point(resultLabel.Location.X + 160, resultLabel.Location.Y - 3);
            Size tbSize = new Size(17, 25);

            for (int i = 0; i < dA.Length; i++)
            {
                TextBox tb = new TextBox
                {
                    Location = new Point(firstPoint.X+ 20 * i, firstPoint.Y),
                    Size = tbSize,
                };

                tb.ReadOnly = true;

                NowMultiplicandTB.Add(tb);
                algoritmPanel.Controls.Add(tb);

                TextBox ts = new TextBox
                {
                    Location = new Point(firstPoint.X+ 20 * i + 286, firstPoint.Y),
                    Size = tbSize,
                };

                ts.ReadOnly = true;

                PastMultiplicandTB.Add(ts);
                algoritmPanel.Controls.Add(ts);
                
            }

            for (int i = 0; i < dB.Length; i++)
            {
                TextBox tb = new TextBox
                {
                    Location = new Point(secondPoint.X + 20 * i, secondPoint.Y),
                    Size = tbSize,
                };

                tb.ReadOnly = true;

                NowMultiplierTB.Add(tb);
                algoritmPanel.Controls.Add(tb);

                TextBox ts = new TextBox
                {
                    Location = new Point(secondPoint.X + 20 * i + 286, secondPoint.Y),
                    Size = tbSize,
                };

                ts.ReadOnly = true;

                PastMultiplierTB.Add(ts);
                algoritmPanel.Controls.Add(ts);
            }

            for (int i = 0; i < result.Count; i++)
            {
                TextBox tb = new TextBox
                {
                    Location = new Point(thirdPoint.X + 20 * i, thirdPoint.Y),
                    Size = tbSize,
                };

                tb.ReadOnly = true;

                NowResultTB.Add(tb);
                algoritmPanel.Controls.Add(tb);

                TextBox ts = new TextBox
                {
                    Location = new Point(thirdPoint.X + 20 * i + 286, thirdPoint.Y),
                    Size = tbSize,
                };

                ts.ReadOnly = true;

                PastResultTB.Add(ts);
                algoritmPanel.Controls.Add(ts);
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            cnt++;
            SetChanges();
        }

        public void FormSets()
        {
            this.BackColor = Color.FromArgb(212, 217, 202);
            mainPanel.BackColor = Color.White;
            getDataPanel.BackColor = Color.White;
            answerPanel.BackColor = Color.White;
            algoritmPanel.BackColor = Color.White;
            boldLabel.Text = "Виртуализация аппаратной реализации операций умножения и деления чисел";
            this.Size = new Size(756, 474);
        }

        private void getDataPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
