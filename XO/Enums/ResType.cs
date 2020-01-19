using System.ComponentModel;

namespace XO.Enums
{
    /// <summary>
    /// Типы результатов
    /// </summary>
    public enum ResType : byte
    {
        /// <summary>
        /// Нельзя установить
        /// </summary>
        NoSet = 0,
        /// <summary>
        /// Выиграл первый игрок
        /// </summary>
        WinnerPlayerOne = 1,

        /// <summary>
        /// Выиграл второй игрок
        /// </summary>
        WinnerPlayerTwo = 2,

        /// <summary>
        /// Ничего не произошло
        /// </summary>
        Nothing = 3,

        /// <summary>
        /// Ничья
        /// </summary>
        NoWinner = 4
    }
}
