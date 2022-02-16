using HangmanAutoplayer.Interfaces;
using HangmanAutoplayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangmanAutoplayer.Services
{
    public class Application : IApplication
    {
        IHangmanService _hangmanService;
        public Application(IHangmanService hangmanService)
        {
            _hangmanService = hangmanService;
        }
        public void Run()
        {
            Console.WriteLine("It's a Hangman Autoplayer program. It tries to guess the word. Maximum wrong tries = 7. ");
            Console.WriteLine();
            var playGame = "y";
            // Get all English words
            var allwords = _hangmanService.GetAllWords();
            while ("y".Equals(playGame))
            {
                var guessList = allwords;
                Dictionary<string, int> occurences = new Dictionary<string, int>();
                HangmanResponseDto gameDto = new HangmanResponseDto();
                
                // Start hangman service
                _hangmanService.StartGame(ref gameDto);
                while (!_hangmanService.IsGameOver(ref gameDto))
                {
                    // reduce guess list based on the word 
                    guessList = _hangmanService.TuneGuessList(gameDto, guessList);
                    
                    // store occurence of every alphabet found in each word
                    occurences = _hangmanService.UpdateOccurences(guessList, occurences);

                    // character with maximum occurences is taken as the guess letter
                    string possibleCharacter = occurences.OrderByDescending(x => x.Value).FirstOrDefault().Key;

                    _hangmanService.PutGuess(possibleCharacter, ref gameDto);
                    
                    // set occurence value of the passed character as -1, so that next time this alphabet is not considered 
                    // for guess alphabet.
                    occurences[possibleCharacter] = -1;
                }
                _hangmanService.EndGame(gameDto);
                Console.WriteLine("Do you want to play again? y/n?");
                playGame = Console.ReadLine();
            }
            Console.WriteLine("Thank you!");
           
        }
    }
}
