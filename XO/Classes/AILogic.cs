using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XO.Enums;

namespace XO.Classes
{
    public class AILogic
    {
        public static int[] CordAI(Logic logic, XOType xoType, XOType[,] field)
        {
            return CheckPlayer(field, logic, xoType);
        }

        private static int[] CheckPlayer(XOType[,] field, Logic logic, XOType xoType, XOType[,] fieldClone = null, bool first = true)
        {
            var cord = new int[2];

            if (fieldClone == null) fieldClone = GetClone(field);
            var isStop = false;

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

            xoType = xoType == XOType.X ? XOType.O : XOType.X;
            var cord2 = new int[2];

            if (IsOtherPlayerTwoOrMore(xoType, field))
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
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

        private static XOType[,] GetClone(XOType[,] field)
        {
            return (XOType[,])field.Clone();
        }

        private static bool IsOtherPlayerTwoOrMore(XOType xoType, XOType[,] fieldClone)
        {
            int i = 0;
            foreach (var symbol in fieldClone)
            {
                if (symbol == xoType) i++;
                if (i >= 2) return true;
            }
            return false;
        }
    }
}
