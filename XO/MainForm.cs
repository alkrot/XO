using System;
using System.Drawing;
using System.Windows.Forms;
using XO.Classes;
using XO.Enums;

namespace XO
{
    public partial class MainForm : Form
    {
        private Logic logic;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            logic = Logic.Instance;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button button = new Button
                    {
                        Top = i * 91,
                        Left = j * 91,
                        FlatStyle = FlatStyle.Flat,
                        Size = new Size(90, 90),
                        Font = new Font("Aray", 24),
                        Tag = new int[] { i, j }
                    };
                    button.Click += Button_Click;
                    gameBoard.Controls.Add(button);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var cord = (int[])btn.Tag;
            var res = logic.SetPlayer(btn, cord);
            ShowMessage(res);

            if (!logic.PlayerOneWalk && res == ResType.Nothing)
            {
                cord = AILogic.CordAI(logic, XOType.O, logic.Field);
                foreach (var cntrl in gameBoard.Controls)
                {
                    if (cntrl is Button btnAI)
                    {
                        var btnCord = (int[])btnAI.Tag;
                        if (btnCord[0] == cord[0] && btnCord[1] == cord[1])
                           ShowMessage(logic.SetPlayer(btnAI, cord));
                    }
                }
            }
        }

        private static void ShowMessage(ResType res)
        {
            switch (res)
            {
                case ResType.NoSet:
                    MessageBox.Show("Клетка занята!", "Сообщение");
                    break;
                case ResType.NoWinner:
                    MessageBox.Show("Ничья", "Сообщение");
                    break;
                case ResType.WinnerPlayerOne:
                    MessageBox.Show("Выиграл первый игрок", "Сообщение");
                    break;
                case ResType.WinnerPlayerTwo:
                    MessageBox.Show("Выиграл второй игрок", "Сообщение");
                    break;
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            logic.NewGame();

            foreach (var contrl in gameBoard.Controls)
            {
                if (contrl is Button btn) btn.Text = string.Empty;
            }
        }
    }
}
