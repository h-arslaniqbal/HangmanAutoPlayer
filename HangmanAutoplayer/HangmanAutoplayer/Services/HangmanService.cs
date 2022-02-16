using HangmanAutoplayer.Helpers;
using HangmanAutoplayer.Interfaces;
using HangmanAutoplayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace HangmanAutoplayer.Services
{
    class HangmanService : IHangmanService
    {
        public static HttpClientHandler _wordsHandler = new HttpClientHandler();
        public static HttpClientHandler _hangmanHandler = new HttpClientHandler();
        private int remainingTries = AppConstants.MaxWrongGuesses;

        public List<string> GetAllWords()
        {
            List<string> allWords = new List<string>();
            using (var httpClient = new HttpClient(_wordsHandler, false))
            {
                var  response = httpClient.GetAsync(AppConstants.GetAllWordsUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    string resposneStr = response.Content.ReadAsStringAsync().Result;
                    var wordsArray = resposneStr.Split('\n');
                    allWords = new List<string>(wordsArray);
                }
            }
            return allWords;
        }

        
        public void StartGame(ref HangmanResponseDto dto)
        {
            remainingTries = AppConstants.MaxWrongGuesses;
            using (var httpClient = new HttpClient(_hangmanHandler, false))
            {
                var response = httpClient.PostAsync(AppConstants.HangmanApiUrl, null).Result;
                if (response.IsSuccessStatusCode)
                {
                    string resposneStr = response.Content.ReadAsStringAsync().Result;
                    dto = JsonConvert.DeserializeObject<HangmanResponseDto>(resposneStr);
                    Console.WriteLine("Word To Guess: " + dto.hangman);
                }
            }
            return;
        }

        public void EndGame(HangmanResponseDto dto)
        {
            if (dto.hangman.Contains("_"))
            {
                Console.WriteLine("Game is Lost");
            }
            else
            {
                Console.WriteLine("Game is Won. Word = " + dto.hangman);
            }
        }

        public void PutGuess(string letter, ref HangmanResponseDto dto)
        {
            using (var httpClient = new HttpClient(_hangmanHandler, false))
            {
                var requestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("letter", letter),
                    new KeyValuePair<string, string>("token", dto.token)
                });

                var response = httpClient.PutAsync(AppConstants.HangmanApiUrl, requestContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string resposneStr = response.Content.ReadAsStringAsync().Result;
                    dto = JsonConvert.DeserializeObject<HangmanResponseDto>(resposneStr);
                    if (!dto.correct)
                    {
                        remainingTries += -1;
                    }
                }
                Console.WriteLine($"Word To Guess: {dto.hangman}  - Letter Guessed:{letter}  -  Remaining Tries: {remainingTries}");
            }
            return;
        }

        public bool IsGameOver(ref HangmanResponseDto dto)
        {
            if (remainingTries < 1 || !dto.hangman.Contains("_"))
            {
                return true;
            }
            else
                return false;
        }

        public Dictionary<string, int> InitOccurences(Dictionary<string, int> occurences)
        {
            //var occurencesNew = new Dictionary<string, int>();
            string alphabets = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            List<string> alphabetsList = new List<string>(alphabets.Split(','));
            foreach(var alpha in alphabetsList)
            {
                if (!occurences.ContainsKey(alpha))
                {
                    occurences.Add(alpha, 0);
                }
                else if (occurences.ContainsKey(alpha) && occurences[alpha] > -1)
                {
                    occurences[alpha] = 0;
                }
            }
            return occurences;
        }
 
        public List<string> TuneGuessList(HangmanResponseDto dto, List<string> guessList)
        {
            string regex = dto.hangman;

            // regex 
            // every underscore needs to be replaces with [a-z] with restriction that the character 
            // is to be only of one character
            // the character that is gussed 

            regex = regex.Replace("_", "[a-zA-Z]{1}") + "\\b";
            var dynamicRegex = new Regex(regex);
            guessList = guessList.Where(x=> dynamicRegex.IsMatch(x)).ToList();

            return guessList;
        }

        public Dictionary<string, int>  UpdateOccurences(List<string> guessList, Dictionary<string, int> occurence)
        {
            occurence = InitOccurences(occurence);
            foreach (string ele in guessList)
            {
                string word = ele.ToLower();
                char[] wordArray = word.ToCharArray();
                foreach (var character in wordArray)
                {
                    if (occurence.ContainsKey(character.ToString()))
                    {
                        var currentVal = occurence[character.ToString()];
                        if (currentVal > -1)
                        {
                            occurence[character.ToString()] = currentVal + 1;
                        }
                    }
                }
            }
            return occurence;
        }
    }
}
