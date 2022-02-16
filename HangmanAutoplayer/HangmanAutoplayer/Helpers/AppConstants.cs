using System;
using System.Collections.Generic;
using System.Text;

namespace HangmanAutoplayer.Helpers
{
    public static class AppConstants
    {
        public static readonly string GetAllWordsUrl = @"https://raw.githubusercontent.com/despo/hangman/master/words";
        public static readonly string HangmanApiUrl = @"https://hangman-api.herokuapp.com/hangman";
        public static readonly int MaxWrongGuesses = 7;

    }
}
