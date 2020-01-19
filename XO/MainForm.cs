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
            DoubleBuffered = true;
            logic = Logic.Instance;

            for (int i = 0; i < logic.Row; i++)
            {
                for (int j = 0; j < logic.Coulumn; j++)
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

            progressGame.Maximum = logic.Row * logic.Coulumn;
            progressGame.Step = 1;
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

        private void ShowMessage(ResType res)
        {
            switch (res)
            {
                case ResType.NoSet:
                    toolResult.Text = "Клетка занята!";
                    toolResult.ForeColor = Color.DarkRed;
                    break;
                case ResType.NoWinner:
                    toolResult.Text = "Ничья";
                    toolResult.ForeColor = Color.DarkBlue;
                    gameBoard.Enabled = false;
                    break;
                case ResType.Nothing:
                    SetInformation();
                    progressGame.PerformStep();
                    break;
                default:
                    var player = res == ResType.WinnerPlayerOne ? "первый" : "второй";
                    toolResult.ForeColor = Color.Green;
                    toolResult.Text = $"Выиграл {player} игрок";
                    progressGame.Value = progressGame.Maximum;
                    gameBoard.Enabled = false;
                    break;
            }
        }

        private void SetInformation()
        {
            toolResult.Text = "Информация";
            toolResult.ForeColor = Color.Black;
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            logic.NewGame();
            SetInformation();
            gameBoard.Enabled = true;
            progressGame.Value = 0;

            foreach (var contrl in gameBoard.Controls)
            {
                if (contrl is Button btn) btn.Text = string.Empty;
            }
        }
    }
}
