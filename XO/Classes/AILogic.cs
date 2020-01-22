using System;
using System.Collections.Generic;
using System.Linq;
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
            var dicCord = new Dictionary<ResType, List<int[]>>();

            var fieldClone = GetClone(field);
            var isStop = false;

            if (field[1, 1] == 0)
            {
                return new int[] { 1, 1 };
            }

            //Найдем свободную клетку
            for (int i = 0; i < logic.Row; i++)
            {
                for (int j = 0; j < logic.Coulumn; j++)
                {
                    if (fieldClone[i, j] == 0)
                    {
                        cord = new int[] { i, j };
                        fieldClone[i, j] = xoType;

                        ResType resType = logic.CheckWinPlayer(xoType, cord, fieldClone, true);
                        if (!dicCord.ContainsKey(resType)) dicCord[resType] = new List<int[]>();
                        dicCord[resType].Add(cord);
                        fieldClone[i, j] = 0;
                    }
                }

                if (isStop) break;
            }

            if (dicCord.TryGetValue(ResType.WinnerPlayerTwo, out var cords)) return cords.First();
            if (dicCord.TryGetValue(ResType.Nothing, out var cordNothing))
            {
                foreach (var cr in cordNothing)
                {
                    var row = cr[0];
                    var col = cr[1];

                    if (row == 0 && (col == 0 || col == logic.Coulumn - 1) || row == logic.Row - 1 && (col == 0 || col == logic.Coulumn - 1)) {
                        cord = cr;
                        break;
                    }
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
