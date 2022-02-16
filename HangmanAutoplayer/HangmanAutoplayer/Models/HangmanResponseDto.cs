using System;
using System.Collections.Generic;
using System.Text;

namespace HangmanAutoplayer.Models
{
    public class HangmanResponseDto
    {
        public HangmanResponseDto()
        {
            correct = false;
        }
        public string hangman { get; set; }
        public bool correct { get; set; }
        public string token { get; set; }

    }
}
