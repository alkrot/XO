using System.Collections.Generic;
using System.Windows.Forms;
using XO.Enums;

namespace XO.Classes
{
    public class Logic
    {
        /// <summary>
        /// Инстанс
        /// </summary>
        static Logic instance;

        /// <summary>
        /// Поле
        /// </summary>
        private XOType[,] field = new XOType[3, 3];

        /// <summary>
        /// Заполненость
        /// </summary>
        int fielled = 0;

        /// <summary>
        /// Словарь игроков, ключ символ игрока, значение игрок
        /// </summary>
        public readonly Dictionary<XOType, Player> XOPlayer = new Dictionary<XOType, Player>()
        {
            { XOType.X, new Player() },
            { XOType.O, new Player() }
        };

        private Logic()
        {

        }

        /// <summary>
        /// Может быть только одна логика
        /// </summary>
        public static Logic Instance
        {
            get { return instance ?? (instance = new Logic()); }
        }

        /// <summary>
        /// Ходит первый игрок
        /// </summary>
        public bool PlayerOneWalk { get; private set; } = true;

        /// <summary>
        /// Поле
        /// </summary>
        public XOType[,] Field { get => field; }

        /// <summary>
        /// Количество строк
        /// </summary>
        public int Row { get { return 3; } }

        /// <summary>
        /// Количество столбцов
        /// </summary>
        public int Coulumn { get { return 3; } }

        /// <summary>
        /// Кол-во в ряд
        /// </summary>
        private int CountWin { get { return 3; } }

        /// <summary>
        /// Установим значение в поле по кординатам
        /// </summary>
        /// <param name="btn">Кнопка</param>
        /// <param name="cord">Координаты</param>
        /// <returns>Результат</returns>
        public ResType SetPlayer(Button btn, int[] cord)
        {
            var xoType = PlayerOneWalk ? XOType.X : XOType.O;

            var res = field[cord[0], cord[1]];
            if (res > 0) return ResType.NoSet;

            field[cord[0], cord[1]] = xoType;
            btn.Text = xoType.ToString();
            PlayerOneWalk = !PlayerOneWalk;

            return CheckWinPlayer(xoType, cord, this.field);
        }

        /// <summary>
        /// Проверка на победу
        /// </summary>
        /// <param name="xoType">Тип символа</param>
        /// <param name="cord">Кординаты</param>
        /// <returns>Результат</returns>
        public ResType CheckWinPlayer(XOType xoType, int[] cord, XOType[,] field, bool aiSearchSol = false)
        {
            if (!aiSearchSol)
            {
                var player = XOPlayer[xoType];
                player.Counter++;
                if (player.Counter < 2) return ResType.Nothing;

                fielled++;
            }

            var y = cord[0];
            var x = cord[1];

            int mdig = 0;
            int supdig = 0;

            //Проверка по горизонтали и вертикали
            for (int i = 0; i < Row; i++)
            {
                int hor = 0;
                int ver = 0;
                for (int j = 0; j < Coulumn; j++)
                {
                    if (field[i, j] == xoType)
                        hor++;
                    if (field[j, i] == xoType)
                        ver++;

                    if (hor == CountWin || ver == CountWin)
                        return GetWinner(xoType);
                }
            }


            //Проверка по диагонали
            for (int i = 0; i < Row; i++)
            {

                if (field[i, i] == xoType)
                    mdig++;
                if (field[i, 2 - i] == xoType)
                    supdig++;

                if (mdig == CountWin || supdig == CountWin)
                    return GetWinner(xoType);
            }

            //Чтобы не сканировать весь массив на заполненность
            if (fielled >= 7) return ResType.NoWinner;
            return ResType.Nothing;
        }

        /// <summary>
        /// Новая игра
        /// </summary>
        public void NewGame()
        {
            var instance = Logic.Instance;
            instance.PlayerOneWalk = true;
            instance.fielled = 0;

            foreach (var keyPair in XOPlayer)
            {
                keyPair.Value.Counter = 0;
            }

            field = new XOType[Row, Coulumn];
        }

        /// <summary>
        /// Узнаем чей символ победил
        /// </summary>
        /// <param name="xoType">Символ</param>
        /// <returns>Результат</returns>
        private static ResType GetWinner(XOType xoType)
        {
            return xoType == XOType.X ? ResType.WinnerPlayerOne : ResType.WinnerPlayerTwo;
        }
    }
}
