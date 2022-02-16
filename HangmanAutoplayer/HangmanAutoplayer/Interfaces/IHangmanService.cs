using HangmanAutoplayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HangmanAutoplayer.Interfaces
{
    public interface IHangmanService
    {
        List<string> GetAllWords();
        void StartGame(ref HangmanResponseDto dto);
        void PutGuess(string letter, ref HangmanResponseDto dto);
        void EndGame(HangmanResponseDto dto);
        bool IsGameOver(ref HangmanResponseDto dto);
        Dictionary<string, int> InitOccurences(Dictionary<string, int> occurences);
        List<string> TuneGuessList(HangmanResponseDto dto, List<string> guessList);
        Dictionary<string, int> UpdateOccurences(List<string> guessList, Dictionary<string, int> occurence);

    }
}
