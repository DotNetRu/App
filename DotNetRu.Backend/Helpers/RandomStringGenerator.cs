using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace XamarinEvolve.Backend.Helpers
{
    /// <summary>
    /// This class can generate random strings and supports following settings:
    /// 1) 4 character sets (UpperCase, LowerCase, Numeric and Special characters)
    /// 2) Variable number of the character sets in use
    /// 3) Minimal number of each type of the characters
    /// 4) Pattern driven string generation
    /// 5) Unique string generation
    /// 6) Using each character only once
    /// It can be easily used for generation of a password or an identificator.
    /// </summary>
    public class RandomStringGenerator
    {
        public RandomStringGenerator(bool UseUpperCaseCharacters = true,
                                     bool UseLowerCaseCharacters = true,
                                     bool UseNumericCharacters = true,
                                     bool UseSpecialCharacters = true)
        {
            m_UseUpperCaseCharacters = UseUpperCaseCharacters;
            m_UseLowerCaseCharacters = UseLowerCaseCharacters;
            m_UseNumericCharacters = UseNumericCharacters;
            m_UseSpecialCharacters = UseSpecialCharacters;
            CurrentGeneralCharacters = new char[0]; // avoiding null exceptions
            UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            LowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            NumericCharacters = "0123456789".ToCharArray();
            SpecialCharacters = ",.;:?!/@#$%^&()=+*-_{}[]<>|~".ToCharArray();
            MinUpperCaseCharacters = MinLowerCaseCharacters = MinNumericCharacters = MinSpecialCharacters = 0;
            RepeatCharacters = true;
            PatternDriven = false;
            Pattern = "";
            Random = new RNGCryptoServiceProvider();
            ExistingStrings = new List<string>();
        }

        #region character sets managers
        /// <summary>
        /// True if we need to use upper case characters
        /// </summary>
        public bool UseUpperCaseCharacters
        {
            get
            {
                return m_UseUpperCaseCharacters;
            }
            set
            {
                if (CurrentUpperCaseCharacters != null)
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Except(CurrentUpperCaseCharacters).ToArray();
                if (value)
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Concat(CurrentUpperCaseCharacters).ToArray();
                m_UseUpperCaseCharacters = value;
            }
        }

        /// <summary>
        /// Sets or gets upper case character set.
        /// </summary>
        public char[] UpperCaseCharacters
        {
            get
            {
                return CurrentUpperCaseCharacters;
            }
            set
            {
                if (UseUpperCaseCharacters)
                {
                    if (CurrentUpperCaseCharacters != null)
                        CurrentGeneralCharacters = CurrentGeneralCharacters.Except(CurrentUpperCaseCharacters).ToArray();
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Concat(value).ToArray();
                }
                CurrentUpperCaseCharacters = value;
            }
        }

        /// <summary>
        /// True if we need to use lower case characters
        /// </summary>
        public bool UseLowerCaseCharacters
        {
            get
            {
                return m_UseLowerCaseCharacters;
            }
            set
            {
                if (CurrentLowerCaseCharacters != null)
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Except(CurrentLowerCaseCharacters).ToArray();
                if (value)
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Concat(CurrentLowerCaseCharacters).ToArray();
                m_UseLowerCaseCharacters = value;
            }
        }

        /// <summary>
        /// Sets or gets lower case character set.
        /// </summary>
        public char[] LowerCaseCharacters
        {
            get
            {
                return CurrentLowerCaseCharacters;
            }
            set
            {
                if (UseLowerCaseCharacters)
                {
                    if (CurrentLowerCaseCharacters != null)
                        CurrentGeneralCharacters = CurrentGeneralCharacters.Except(CurrentLowerCaseCharacters).ToArray();
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Concat(value).ToArray();
                }
                CurrentLowerCaseCharacters = value;
            }
        }

        /// <summary>
        /// True if we need to use numeric characters
        /// </summary>
        public bool UseNumericCharacters
        {
            get
            {
                return m_UseNumericCharacters;
            }
            set
            {
                if (CurrentNumericCharacters != null)
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Except(CurrentNumericCharacters).ToArray();
                if (value)
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Concat(CurrentNumericCharacters).ToArray();
                m_UseNumericCharacters = value;
            }
        }

        /// <summary>
        /// Sets or gets numeric character set.
        /// </summary>
        public char[] NumericCharacters
        {
            get
            {
                return CurrentNumericCharacters;
            }
            set
            {
                if (UseNumericCharacters)
                {
                    if (CurrentNumericCharacters != null)
                        CurrentGeneralCharacters = CurrentGeneralCharacters.Except(CurrentNumericCharacters).ToArray();
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Concat(value).ToArray();
                }
                CurrentNumericCharacters = value;
            }
        }

        /// <summary>
        /// True if we need to use special characters
        /// </summary>
        public bool UseSpecialCharacters
        {
            get
            {
                return m_UseSpecialCharacters;
            }
            set
            {
                if (CurrentSpecialCharacters != null)
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Except(CurrentSpecialCharacters).ToArray();
                if (value)
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Concat(CurrentSpecialCharacters).ToArray();
                m_UseSpecialCharacters = value;
            }
        }

        /// <summary>
        /// Sets or gets special character set.
        /// </summary>
        public char[] SpecialCharacters
        {
            get
            {
                return CurrentSpecialCharacters;
            }
            set
            {
                if (UseSpecialCharacters)
                {
                    if (CurrentSpecialCharacters != null)
                        CurrentGeneralCharacters = CurrentGeneralCharacters.Except(CurrentSpecialCharacters).ToArray();
                    CurrentGeneralCharacters = CurrentGeneralCharacters.Concat(value).ToArray();
                }
                CurrentSpecialCharacters = value;
            }
        }
        #endregion

        #region character limits
        /// <summary>
        /// Sets or gets minimal number of the upper case characters in generated strings.
        /// </summary>
        public int MinUpperCaseCharacters
        {
            get { return m_MinUpperCaseCharacters; }
            set { m_MinUpperCaseCharacters = value; }
        }

        /// <summary>
        /// Sets or gets minimal number of the lower case characters in generated strings.
        /// </summary>
        public int MinLowerCaseCharacters
        {
            get { return m_MinLowerCaseCharacters; }
            set { m_MinLowerCaseCharacters = value; }
        }

        /// <summary>
        /// Sets or gets minimal number of the numeric characters in generated strings.
        /// </summary>
        public int MinNumericCharacters
        {
            get { return m_MinNumericCharacters; }
            set { m_MinNumericCharacters = value; }
        }

        /// <summary>
        /// Sets or gets minimal number of the special characters in generated strings.
        /// </summary>
        public int MinSpecialCharacters
        {
            get { return m_MinSpecialCharacters; }
            set { m_MinSpecialCharacters = value; }
        }
        #endregion

        #region pattern
        private string m_pattern;

        /// <summary>
        /// Defines the pattern to be followed to generate a string.
        /// This value is ignored if it equals empty string.
        /// Patterns are:
        /// L - for upper case letter
        /// l - for lower case letter
        /// n - for number
        /// s - for special character
        /// * - for any character
        /// </summary>
        private string Pattern
        {
            get
            {
                return m_pattern;
            }
            set
            {
                if (!value.Equals(String.Empty))
                    PatternDriven = true;
                else
                    PatternDriven = false;
                m_pattern = value;
            }
        }
        #endregion

        #region generators
        /// <summary>
        /// Generate a string which follows the pattern.
        /// Possible characters are:
        /// L - for upper case letter
        /// l - for lower case letter
        /// n - for number
        /// s - for special character
        /// * - for any character
        /// </summary>
        /// <param name="Pattern">The pattern to follow while generation</param>
        /// <returns>A random string which follows the pattern</returns>
        public string Generate(string Pattern)
        {
            this.Pattern = Pattern;
            string res = GenerateString(Pattern.Length);
            this.Pattern = "";
            return res;
        }

        /// <summary>
        /// Generate a string of a variable length from MinLength to MaxLength. The possible 
        /// character sets should be defined before calling this function.
        /// </summary>
        /// <param name="MinLength">Minimal length of a string</param>
        /// <param name="MaxLength">Maximal length of a string</param>
        /// <returns>A random string from the selected range of length</returns>
        public string Generate(int MinLength, int MaxLength)
        {
            if (MaxLength < MinLength)
                throw new ArgumentException("Maximal length should be grater than minumal");
            int length = MinLength + (GetRandomInt() % (MaxLength - MinLength));
            return GenerateString(length);
        }

        /// <summary>
        /// Generate a string of a fixed length. The possible 
        /// character sets should be defined before calling this function.
        /// </summary>
        /// <param name="FixedLength">The length of a string</param>
        /// <returns>A random string of the desirable length</returns>
        public string Generate(int FixedLength)
        {
            return GenerateString(FixedLength);
        }

        /// <summary>
        /// Main generation method which chooses the algorithm to use for the generation.
        /// It checks some exceptional situations as well.
        /// </summary>
        private string GenerateString(int length)
        {
            if (length == 0)
                throw new ArgumentException("You can't generate a string of a zero length");
            if (!UseUpperCaseCharacters && !UseLowerCaseCharacters && !UseNumericCharacters && !UseSpecialCharacters)
                throw new ArgumentException("There should be at least one character set in use");
            if (!RepeatCharacters && (CurrentGeneralCharacters.Length < length))
                throw new ArgumentException("There is not enough characters to create a string without repeats");
            string result = ""; // This string will contain the result
            if (PatternDriven)
            {
                // Using the pattern to generate a string
                result = PatternDrivenAlgo(Pattern);
            }
            else if (MinUpperCaseCharacters == 0 && MinLowerCaseCharacters == 0 &&
                     MinNumericCharacters == 0 && MinSpecialCharacters == 0)
            {
                // Using the simpliest algorithm in this case
                result = SimpleGenerateAlgo(length);
            }
            else
            {
                // Paying attention to limits
                result = GenerateAlgoWithLimits(length);
            }
            // Support for unique strings
            // Recursion, but possibility of the stack overflow is low for big strings (> 3 chars).
            if (UniqueStrings && ExistingStrings.Contains(result))
                return GenerateString(length);
            AddExistingString(result); // Saving history
            return result;
        }

        /// <summary>
        /// Generate a random string following the pattern
        /// </summary>
        private string PatternDrivenAlgo(string Pattern)
        {
            string result = "";
            List<char> Characters = new List<char>();
            foreach (char character in Pattern.ToCharArray())
            {
                char newChar = ' ';
                switch (character)
                {
                    case 'L':
                        {
                            newChar = GetRandomCharFromArray(CurrentUpperCaseCharacters, Characters);
                            break; 
                        }
                    case 'l':
                        {
                            newChar = GetRandomCharFromArray(CurrentLowerCaseCharacters, Characters);
                            break; 
                        }
                    case 'n':
                        {
                            newChar = GetRandomCharFromArray(CurrentNumericCharacters, Characters);
                            break; 
                        }
                    case 's':
                        {
                            newChar = GetRandomCharFromArray(CurrentSpecialCharacters, Characters);
                            break;   
                        }
                    case '*':
                        {
                            newChar = GetRandomCharFromArray(CurrentGeneralCharacters, Characters);
                            break;  
                        }
                    default:
                        {
                            throw new Exception("The character '" + character + "' is not supported");
                        }
                }
                Characters.Add(newChar);
                result += newChar;
            }
            return result;
        }

        /// <summary>
        /// The simpliest algorithm of the random string generation. It doesn't pay attention to
        /// limits and patterns.
        /// </summary>
        private string SimpleGenerateAlgo(int length)
        {
            string result = "";
            // No special limits
            for (int i = 0; i < length; i++)
            {
                char newChar = CurrentGeneralCharacters[GetRandomInt() % CurrentGeneralCharacters.Length];
                if (!RepeatCharacters && result.Contains(newChar))
                {
                    do
                    {
                        newChar = CurrentGeneralCharacters[GetRandomInt() % CurrentGeneralCharacters.Length];
                    } while (result.Contains(newChar));
                }
                result += newChar;
            }
            return result;
        }

        /// <summary>
        /// Generate a random string with specified number of minimal characters of each character set.
        /// </summary>
        private string GenerateAlgoWithLimits(int length)
        {
            // exceptional situations
            if (MinUpperCaseCharacters + MinLowerCaseCharacters +
                MinNumericCharacters + MinSpecialCharacters > length)
            {
                throw new ArgumentException("Sum of MinUpperCaseCharacters, MinLowerCaseCharacters," +
                    " MinNumericCharacters and MinSpecialCharacters is greater than length");
            }
            if (!RepeatCharacters && (MinUpperCaseCharacters > CurrentUpperCaseCharacters.Length))
                throw new ArgumentException("Can't generate a string with this number of MinUpperCaseCharacters");
            if (!RepeatCharacters && (MinLowerCaseCharacters > CurrentLowerCaseCharacters.Length))
                throw new ArgumentException("Can't generate a string with this number of MinLowerCaseCharacters");
            if (!RepeatCharacters && (MinNumericCharacters > CurrentNumericCharacters.Length))
                throw new ArgumentException("Can't generate a string with this number of MinNumericCharacters");
            if (!RepeatCharacters && (MinSpecialCharacters > CurrentSpecialCharacters.Length))
                throw new ArgumentException("Can't generate a string with this number of MinSpecialCharacters");
            int AllowedNumberOfGeneralChatacters = length - MinUpperCaseCharacters - MinLowerCaseCharacters
                - MinNumericCharacters - MinSpecialCharacters;

            string result = "";
            // generation character set in order to support unique characters
            List<char> Characters = new List<char>();

            // adding chars to an array
            for (int i = 0; i < MinUpperCaseCharacters; i++)
                Characters.Add(GetRandomCharFromArray(UpperCaseCharacters,Characters));
            for (int i = 0; i < MinLowerCaseCharacters; i++)
                Characters.Add(GetRandomCharFromArray(LowerCaseCharacters, Characters));
            for (int i = 0; i < MinNumericCharacters; i++)
                Characters.Add(GetRandomCharFromArray(NumericCharacters, Characters));
            for (int i = 0; i < MinSpecialCharacters; i++)
                Characters.Add(GetRandomCharFromArray(SpecialCharacters, Characters));
            for (int i = 0; i < AllowedNumberOfGeneralChatacters; i++)
                Characters.Add(GetRandomCharFromArray(CurrentGeneralCharacters, Characters));

            // generating result
            for (int i = 0; i < length; i++)
            {
                int position = GetRandomInt() % Characters.Count;
                char CurrentChar = Characters[position];
                Characters.RemoveAt(position);
                result += CurrentChar;
            }
            return result;
        }

        #endregion

        /// <summary>
        /// True if characters can be repeated.
        /// </summary>
        public bool RepeatCharacters;

        /// <summary>
        /// True if it's not possible to create similar strings.
        /// </summary>
        public bool UniqueStrings;

        /// <summary>
        /// Adding the string to the history array to support unique string generation.
        /// </summary>
        public void AddExistingString(string s)
        {
            ExistingStrings.Add(s);
        }

        #region misc tools
        /// <summary>
        /// A 16bit integer number generator.
        /// </summary>
        /// <returns>A random integer value from 0 to 65576</returns>
        private int GetRandomInt()
        {
            byte[] buffer = new byte[2]; // 16 bit = 2^16 = 65576 (more than necessary)
            Random.GetNonZeroBytes(buffer);
            int index = BitConverter.ToInt16(buffer, 0);
            if (index < 0)
                index = -index; // manage negative random values
            return index;
        }

        /// <summary>
        /// Get a random char from the selected array of chars. It pays attention to
        /// the RepeatCharacters flag.
        /// </summary>
        /// <param name="array">Source of symbols</param>
        /// <param name="existentItems">Existing symbols. Can be null if RepeatCharacters flag is false</param>
        /// <returns>A random character from the array</returns>
        private char GetRandomCharFromArray(char[] array, List<char> existentItems)
        {
            char Character = ' ';
            do
            {
                Character = array[GetRandomInt() % array.Length];
            } while (!RepeatCharacters && existentItems.Contains(Character));
            return Character;
        }
        #endregion

        #region internal state
        private bool m_UseUpperCaseCharacters, m_UseLowerCaseCharacters, m_UseNumericCharacters, m_UseSpecialCharacters;
        private int m_MinUpperCaseCharacters, m_MinLowerCaseCharacters, m_MinNumericCharacters, m_MinSpecialCharacters;
        private bool PatternDriven;
        private char[] CurrentUpperCaseCharacters;
        private char[] CurrentLowerCaseCharacters;
        private char[] CurrentNumericCharacters;
        private char[] CurrentSpecialCharacters;
        private char[] CurrentGeneralCharacters; // All used characters
        private RNGCryptoServiceProvider Random;
        private List<string> ExistingStrings; // History
        #endregion
    }
}
