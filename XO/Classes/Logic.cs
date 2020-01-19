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
        readonly Dictionary<XOType, Player> xoPlayer = new Dictionary<XOType, Player>()
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

            return CheckWinPlayer(xoType, cord,this.field);
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
                var player = xoPlayer[xoType];
                player.Counter++;
                if (player.Counter < 2) return ResType.Nothing;

                fielled++;
            }

            var y = cord[0];
            var x = cord[1];

            int mdig = 0;
            int supdig = 0;

            //Проверка по горизонтали и вертикали
            for (int i = 0; i < 3; i++)
            {
                int hor = 0;
                int ver = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (field[i, j] == xoType)
                        hor++;
                    if (field[j, i] == xoType)
                        ver++;

                    if (hor == 3 || ver == 3)
                        return GetWinner(xoType);
                }
            }


            //Проверка по диагонали
            for (int i = 0; i < 3; i++)
            {

                if (field[i, i] == xoType)
                    mdig++;
                if (field[i, 2 - i] == xoType)
                    supdig++;

                if (mdig == 3 || supdig == 3)
                    return GetWinner(xoType);
            }

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

            foreach (var keyPair in xoPlayer)
            {
                keyPair.Value.Counter = 0;
            }

            field = new XOType[3, 3];
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
