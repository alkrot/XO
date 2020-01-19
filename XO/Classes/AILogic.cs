using XO.Enums;

namespace XO.Classes
{
    /// <summary>
    /// Логика бота
    /// </summary>
    public class AILogic
    {
        /// <summary>
        /// Заруск поиска кординат
        /// </summary>
        /// <param name="logic">Логика игры</param>
        /// <param name="xoType">Тип символа</param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static int[] CordAI(Logic logic, XOType xoType, XOType[,] field)
        {
            return CheckPlayer(field, logic, xoType);
        }

        /// <summary>
        /// Проверка на лучшие кординаты условно говоря
        /// </summary>
        /// <param name="field">Поле</param>
        /// <param name="logic">Логика игры</param>
        /// <param name="xoType">Тип символа</param>
        /// <returns>Вернем строку и столбец</returns>
        private static int[] CheckPlayer(XOType[,] field, Logic logic, XOType xoType)
        {
            var cord = new int[2];

            var fieldClone = GetClone(field);
            var isStop = false;

            if (field[1, 1] == 0)
            {
                cord[0] = 1;
                cord[1] = 1;
            }
            else
            {

                //Найдем свободную клетку
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (fieldClone[i, j] == 0)
                        {
                            cord[0] = i;
                            cord[1] = j;

                            fieldClone[i, j] = xoType;
                            isStop = true;
                            break;
                        }
                    }

                    if (isStop) break;
                }
            }

            //Получим символ следующего игрока
            xoType = xoType == XOType.X ? XOType.O : XOType.X;
            var cord2 = new int[2];

            //Если у него ходов было больше двух, то проверим, есть ли выигрышный следующий ход
            //и вернем эти кординаты вместо прошлых
            if (logic.XOPlayer[xoType].Counter >= 2)
            {
                for (int i = 0; i < logic.Row; i++)
                {
                    for (int j = 0; j < logic.Coulumn; j++)
                    {
                        if (fieldClone[i, j] == 0)
                        {
                            fieldClone[i, j] = xoType;
                            cord2[0] = i;
                            cord2[1] = j;

                            if (logic.CheckWinPlayer(xoType, cord2, fieldClone, true) == ResType.WinnerPlayerOne)
                            {
                                return cord2;
                            }
                            fieldClone[i, j] = 0;
                        }
                    }
                }
            }

            return cord;
        }

        /// <summary>
        /// Вернуть клон поле, чтоб не засорять основное
        /// </summary>
        /// <param name="field">Поле</param>
        /// <returns>Клон</returns>
        private static XOType[,] GetClone(XOType[,] field)
        {
            return (XOType[,])field.Clone();
        }
    }
}
