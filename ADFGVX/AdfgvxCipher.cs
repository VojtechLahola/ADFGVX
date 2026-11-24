using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ADFGVX
{
    public enum CipherVariant
    {
        AdfgxCzech,
        AdfgxEnglish,
        Adfgvx
    }

    public class CipherResult
    {
        public string FilteredInput { get; set; } = string.Empty;
        public string FractionatedText { get; set; } = string.Empty;
        public string OutputText { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public List<int> SortedKeyIndices { get; set; } = new();
        public List<string> TranspositionColumns { get; set; } = new();
        public string MatrixString { get; set; } = string.Empty;
        public CipherVariant Variant { get; set; }
    }

    public static class AdfgvxCipher
    {
        public static readonly char[] Headers5 = { 'A', 'D', 'F', 'G', 'X' };
        public static readonly char[] Headers6 = { 'A', 'D', 'F', 'G', 'V', 'X' };

        public const string SpaceToken = "QUXXUQ";

        public const string AlphabetCzech5 = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
        public const string AlphabetEnglish5 = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        public const string Alphabet6 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const string DigitEncodingAlphabet = "ABCDEFGHIK";

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);

            foreach (char c in normalized)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string FilterInput(string text, CipherVariant variant, bool keepSpaces = false, bool useSpaceToken = false, bool encodeAdfgxDigits = false)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string cleaned = RemoveDiacritics(text).ToUpperInvariant().Trim();
            var sb = new StringBuilder(cleaned.Length);
            bool lastWasSpace = false;

            foreach (char c in cleaned)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (keepSpaces)
                    {
                        if (!lastWasSpace && sb.Length > 0)
                        {
                            sb.Append(' ');
                            lastWasSpace = true;
                        }
                    }
                    else if (useSpaceToken)
                    {
                        if (!lastWasSpace && sb.Length > 0)
                        {
                            if (encodeAdfgxDigits)
                            {
                                AppendEscapedText(sb, SpaceToken);
                            }
                            else
                            {
                                sb.Append(SpaceToken);
                            }

                            lastWasSpace = true;
                        }
                    }
                    continue;
                }

                lastWasSpace = false;

                if (variant == CipherVariant.AdfgxCzech)
                {
                    if (c == 'W')
                    {
                        sb.Append('V');
                    }
                    else if (encodeAdfgxDigits && c == 'Q')
                    {
                        sb.Append("QQ");
                    }
                    else if (AlphabetCzech5.Contains(c))
                    {
                        sb.Append(c);
                    }
                    else if (char.IsDigit(c))
                    {
                        if (encodeAdfgxDigits)
                        {
                            AppendEncodedDigit(sb, c);
                        }
                        else
                        {
                            string digitWord = DigitToWordCzech(c);
                            sb.Append(digitWord);
                        }
                    }
                }
                else if (variant == CipherVariant.AdfgxEnglish)
                {
                    if (c == 'J')
                    {
                        sb.Append('I');
                    }
                    else if (encodeAdfgxDigits && c == 'Q')
                    {
                        sb.Append("QQ");
                    }
                    else if (AlphabetEnglish5.Contains(c))
                    {
                        sb.Append(c);
                    }
                    else if (char.IsDigit(c))
                    {
                        if (encodeAdfgxDigits)
                        {
                            AppendEncodedDigit(sb, c);
                        }
                        else
                        {
                            string digitWord = DigitToWordEnglish(c);
                            sb.Append(digitWord);
                        }
                    }
                }
                else
                {
                    if (Alphabet6.Contains(c))
                    {
                        sb.Append(c);
                    }
                }
            }

            string result = sb.ToString();
            string encodedSpaceToken = encodeAdfgxDigits ? EscapeText(SpaceToken) : SpaceToken;
            while (result.EndsWith(encodedSpaceToken))
            {
                result = result.Substring(0, result.Length - encodedSpaceToken.Length);
            }

            return result;
        }

        private static void AppendEncodedDigit(StringBuilder text, char digit)
        {
            text.Append('Q');
            text.Append(DigitEncodingAlphabet[digit - '0']);
        }

        private static void AppendEscapedText(StringBuilder text, string value)
        {
            foreach (char character in value)
            {
                if (character == 'Q')
                {
                    text.Append("QQ");
                }
                else
                {
                    text.Append(character);
                }
            }
        }

        private static string EscapeText(string value)
        {
            var result = new StringBuilder(value.Length);
            AppendEscapedText(result, value);
            return result.ToString();
        }

        private static string DecodeAdfgxDigits(string text)
        {
            var result = new StringBuilder(text.Length);
            int index = 0;

            while (index < text.Length)
            {
                if (text[index] != 'Q')
                {
                    result.Append(text[index]);
                    index++;
                    continue;
                }

                if (index + 1 < text.Length)
                {
                    char nextCharacter = text[index + 1];

                    if (nextCharacter == 'Q')
                    {
                        result.Append('Q');
                        index += 2;
                        continue;
                    }

                    int digitIndex = DigitEncodingAlphabet.IndexOf(nextCharacter);
                    if (digitIndex >= 0)
                    {
                        result.Append((char)('0' + digitIndex));
                        index += 2;
                        continue;
                    }
                }

                result.Append('Q');
                index++;
            }

            return result.ToString();
        }

        public static string RestoreSpaces(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace(SpaceToken, " ")
                       .Replace("XSPACEX", " ")
                       .Replace("XMEZERAX", " ")
                       .Replace("XMX", " ");
        }

        private static string DigitToWordCzech(char digit)
        {
            switch (digit)
            {
                case '0':
                    return "NULA";
                case '1':
                    return "JEDEN";
                case '2':
                    return "DVA";
                case '3':
                    return "TRI";
                case '4':
                    return "CTYRI";
                case '5':
                    return "PET";
                case '6':
                    return "SEST";
                case '7':
                    return "SEDUM";
                case '8':
                    return "OSM";
                case '9':
                    return "DEVET";
                default:
                    return string.Empty;
            }
        }

        private static string DigitToWordEnglish(char digit)
        {
            switch (digit)
            {
                case '0':
                    return "ZERO";
                case '1':
                    return "ONE";
                case '2':
                    return "TWO";
                case '3':
                    return "THREE";
                case '4':
                    return "FOUR";
                case '5':
                    return "FIVE";
                case '6':
                    return "SIX";
                case '7':
                    return "SEVEN";
                case '8':
                    return "EIGHT";
                case '9':
                    return "NINE";
                default:
                    return string.Empty;
            }
        }

        public static string FilterKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            string cleaned = RemoveDiacritics(key).ToUpperInvariant();
            var sb = new StringBuilder(cleaned.Length);

            foreach (char c in cleaned)
            {
                if (char.IsLetter(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static string GetAllowedCharacters(CipherVariant variant)
        {
            switch (variant)
            {
                case CipherVariant.AdfgxCzech:
                    return AlphabetCzech5;
                case CipherVariant.AdfgxEnglish:
                    return AlphabetEnglish5;
                case CipherVariant.Adfgvx:
                    return Alphabet6;
                default:
                    return Alphabet6;
            }
        }

        public static int GetMatrixSize(CipherVariant variant)
        {
            if (variant == CipherVariant.Adfgvx)
            {
                return 6;
            }

            return 5;
        }

        public static char[] GetHeaders(CipherVariant variant)
        {
            if (variant == CipherVariant.Adfgvx)
            {
                return Headers6;
            }

            return Headers5;
        }

        public static string GenerateAlphabet(CipherVariant variant)
        {
            string baseAlphabet = GetAllowedCharacters(variant);
            char[] chars = baseAlphabet.ToCharArray();

            for (int i = chars.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }

            return new string(chars);
        }

        public static string ValidateAndNormalizeMatrix(string matrixInput, CipherVariant variant)
        {
            int requiredLength = GetMatrixSize(variant) * GetMatrixSize(variant);
            string allowed = GetAllowedCharacters(variant);

            string cleaned = RemoveDiacritics(matrixInput).ToUpperInvariant();
            var sb = new StringBuilder(requiredLength);
            var seen = new HashSet<char>();

            foreach (char c in cleaned)
            {
                char normalizedChar = c;
                if (variant == CipherVariant.AdfgxCzech && c == 'W')
                {
                    normalizedChar = 'V';
                }

                if (variant == CipherVariant.AdfgxEnglish && c == 'J')
                {
                    normalizedChar = 'I';
                }

                if (allowed.Contains(normalizedChar) && seen.Add(normalizedChar))
                {
                    sb.Append(normalizedChar);
                }
            }

            if (sb.Length != requiredLength)
            {
                throw new ArgumentException(
                    $"Matice musí obsahovat přesně {requiredLength} unikátních znaků z povolené abecedy. Nyní obsahuje {sb.Length} znaků.");
            }

            return sb.ToString();
        }

        public static string GetRemainingCharacters(string currentMatrix, CipherVariant variant)
        {
            string allowed = GetAllowedCharacters(variant);
            string cleaned = RemoveDiacritics(currentMatrix ?? string.Empty).ToUpperInvariant();
            var used = new HashSet<char>();

            foreach (char c in cleaned)
            {
                char normalizedChar = c;
                if (variant == CipherVariant.AdfgxCzech && c == 'W')
                {
                    normalizedChar = 'V';
                }

                if (variant == CipherVariant.AdfgxEnglish && c == 'J')
                {
                    normalizedChar = 'I';
                }

                if (allowed.Contains(normalizedChar))
                {
                    used.Add(normalizedChar);
                }
            }

            var remaining = new StringBuilder();
            foreach (char c in allowed)
            {
                if (!used.Contains(c))
                {
                    remaining.Append(c);
                }
            }

            return remaining.ToString();
        }

        public static string Fractionate(string text, string matrix, CipherVariant variant)
        {
            int size = GetMatrixSize(variant);
            char[] headers = GetHeaders(variant);

            var charToCoords = new Dictionary<char, string>();
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    char ch = matrix[r * size + c];
                    charToCoords[ch] = $"{headers[r]}{headers[c]}";
                }
            }

            var sb = new StringBuilder(text.Length * 2);
            foreach (char ch in text)
            {
                if (charToCoords.TryGetValue(ch, out string? coords))
                {
                    sb.Append(coords);
                }
                else
                {
                    throw new ArgumentException($"Znak '{ch}' nebyl nalezen v šifrovací matici.");
                }
            }

            return sb.ToString();
        }

        public static string Defractionate(string fractionatedText, string matrix, CipherVariant variant)
        {
            if (fractionatedText.Length % 2 != 0)
            {
                throw new ArgumentException("Délka substituovaného textu musí být sudá.");
            }

            int size = GetMatrixSize(variant);
            char[] headers = GetHeaders(variant);

            var coordsToChar = new Dictionary<string, char>();
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    char ch = matrix[r * size + c];
                    string key = $"{headers[r]}{headers[c]}";
                    coordsToChar[key] = ch;
                }
            }

            var sb = new StringBuilder(fractionatedText.Length / 2);
            for (int i = 0; i < fractionatedText.Length; i += 2)
            {
                string pair = fractionatedText.Substring(i, 2);
                if (coordsToChar.TryGetValue(pair, out char ch))
                {
                    sb.Append(ch);
                }
                else
                {
                    throw new ArgumentException($"Neplatná dvojice souřadnic '{pair}'. Povolené jsou pouze kombinace znaků {new string(headers)}.");
                }
            }

            return sb.ToString();
        }

        public static (string cipherTextWithSpaces, List<string> columns, List<int> sortedIndices)
            ColumnarTranspose(string fractionatedText, string key)
        {
            string cleanKey = FilterKey(key);
            if (string.IsNullOrEmpty(cleanKey))
            {
                throw new ArgumentException("Klíčové slovo nesmí být prázdné a musí obsahovat alespoň jedno písmeno.");
            }

            int keyLength = cleanKey.Length;
            int textLength = fractionatedText.Length;

            var columnsData = new StringBuilder[keyLength];
            for (int c = 0; c < keyLength; c++)
            {
                columnsData[c] = new StringBuilder();
            }

            for (int i = 0; i < textLength; i++)
            {
                int col = i % keyLength;
                columnsData[col].Append(fractionatedText[i]);
            }

            List<(char Character, int OriginalIndex)> keyPairs = GetSortedKeyCharacters(cleanKey);
            var sortedIndices = new List<int>(keyLength);
            var transposedColumns = new List<string>(keyLength);

            foreach (var keyCharacter in keyPairs)
            {
                sortedIndices.Add(keyCharacter.OriginalIndex);
                transposedColumns.Add(columnsData[keyCharacter.OriginalIndex].ToString());
            }

            string resultWithSpaces = string.Join(" ", transposedColumns);
            return (resultWithSpaces, transposedColumns, sortedIndices);
        }

        private static string GroupCiphertext(string ciphertext)
        {
            var cleanCiphertextBuilder = new StringBuilder(ciphertext.Length);
            foreach (char character in ciphertext)
            {
                if (!char.IsWhiteSpace(character))
                {
                    cleanCiphertextBuilder.Append(character);
                }
            }

            string cleanCiphertext = cleanCiphertextBuilder.ToString();
            var groups = new List<string>();

            for (int index = 0; index < cleanCiphertext.Length; index += 5)
            {
                int groupLength = Math.Min(5, cleanCiphertext.Length - index);
                groups.Add(cleanCiphertext.Substring(index, groupLength));
            }

            return string.Join(" ", groups);
        }

        private static List<(char Character, int OriginalIndex)> GetSortedKeyCharacters(string key)
        {
            var keyCharacters = new List<(char Character, int OriginalIndex)>();

            for (int index = 0; index < key.Length; index++)
            {
                keyCharacters.Add((key[index], index));
            }

            for (int index = 1; index < keyCharacters.Count; index++)
            {
                var current = keyCharacters[index];
                int previousIndex = index - 1;

                while (previousIndex >= 0 && keyCharacters[previousIndex].Character > current.Character)
                {
                    keyCharacters[previousIndex + 1] = keyCharacters[previousIndex];
                    previousIndex--;
                }

                keyCharacters[previousIndex + 1] = current;
            }

            return keyCharacters;
        }

        private static string RemoveWhitespace(string text)
        {
            var result = new StringBuilder(text.Length);

            foreach (char character in text)
            {
                if (!char.IsWhiteSpace(character))
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

        public static string InverseColumnarTranspose(string cipherText, string key)
        {
            string cleanKey = FilterKey(key);
            if (string.IsNullOrEmpty(cleanKey))
            {
                throw new ArgumentException("Klíčové slovo nesmí být prázdné a musí obsahovat alespoň jedno písmeno.");
            }

            string cleanText = RemoveWhitespace(cipherText).ToUpperInvariant();
            if (string.IsNullOrEmpty(cleanText))
            {
                return string.Empty;
            }

            int keyLength = cleanKey.Length;
            int totalLength = cleanText.Length;

            int baseRowCount = totalLength / keyLength;
            int remainder = totalLength % keyLength;

            int[] originalColLengths = new int[keyLength];
            for (int c = 0; c < keyLength; c++)
            {
                if (c < remainder)
                {
                    originalColLengths[c] = baseRowCount + 1;
                }
                else
                {
                    originalColLengths[c] = baseRowCount;
                }
            }

            List<(char Character, int OriginalIndex)> keyPairs = GetSortedKeyCharacters(cleanKey);

            string[] originalColumns = new string[keyLength];
            int textPtr = 0;

            foreach (var keyCharacter in keyPairs)
            {
                int origIndex = keyCharacter.OriginalIndex;
                int colLength = originalColLengths[origIndex];

                if (textPtr + colLength > totalLength)
                {
                    throw new ArgumentException("Zašifrovaný text má neplatnou délku pro zadaný klíč.");
                }

                originalColumns[origIndex] = cleanText.Substring(textPtr, colLength);
                textPtr += colLength;
            }

            int maxRows = (int)Math.Ceiling((double)totalLength / keyLength);
            var sb = new StringBuilder(totalLength);

            for (int r = 0; r < maxRows; r++)
            {
                for (int c = 0; c < keyLength; c++)
                {
                    if (r < originalColumns[c].Length)
                    {
                        sb.Append(originalColumns[c][r]);
                    }
                }
            }

            return sb.ToString();
        }

        public static CipherResult Encrypt(string plaintext, string key, string matrix, CipherVariant variant, bool preserveSpaces = true)
        {
            string validatedMatrix = ValidateAndNormalizeMatrix(matrix, variant);
            bool encodeAdfgxDigits = variant != CipherVariant.Adfgvx;
            string filtered = FilterInput(
                plaintext,
                variant,
                keepSpaces: false,
                useSpaceToken: preserveSpaces,
                encodeAdfgxDigits: encodeAdfgxDigits);
            string fractionated = Fractionate(filtered, validatedMatrix, variant);
            var (cipherWithSpaces, columns, sortedIndices) = ColumnarTranspose(fractionated, key);

            return new CipherResult
            {
                FilteredInput = filtered,
                FractionatedText = fractionated,
                OutputText = GroupCiphertext(cipherWithSpaces),
                Key = FilterKey(key),
                SortedKeyIndices = sortedIndices,
                TranspositionColumns = columns,
                MatrixString = validatedMatrix,
                Variant = variant
            };
        }

        public static CipherResult Decrypt(string ciphertext, string key, string matrix, CipherVariant variant, bool restoreSpaces = true)
        {
            string validatedMatrix = ValidateAndNormalizeMatrix(matrix, variant);
            string cleanCipher = RemoveWhitespace(ciphertext).ToUpperInvariant();

            char[] headers = GetHeaders(variant);
            foreach (char c in cleanCipher)
            {
                if (Array.IndexOf(headers, c) < 0)
                {
                    throw new ArgumentException($"Zašifrovaný text obsahuje neplatný znak '{c}'. Povoleny jsou pouze znaky: {new string(headers)}.");
                }
            }

            if (cleanCipher.Length % 2 != 0)
            {
                throw new ArgumentException("Délka zašifrovaného textu bez mezer musí být sudá (každý znak otevřeného textu tvoří dvojici souřadnic).");
            }

            string reconstructedFractionated = InverseColumnarTranspose(cleanCipher, key);
            string plaintext = Defractionate(reconstructedFractionated, validatedMatrix, variant);

            if (variant != CipherVariant.Adfgvx)
            {
                plaintext = DecodeAdfgxDigits(plaintext);
            }

            if (restoreSpaces)
            {
                plaintext = RestoreSpaces(plaintext);
            }

            return new CipherResult
            {
                FilteredInput = cleanCipher,
                FractionatedText = reconstructedFractionated,
                OutputText = plaintext,
                Key = FilterKey(key),
                MatrixString = validatedMatrix,
                Variant = variant
            };
        }

        public static string EncryptAdfgx(string plaintext, string key, string matrix, bool isCzechVersion = true, bool preserveSpaces = true)
        {
            CipherVariant variant;

            if (isCzechVersion)
            {
                variant = CipherVariant.AdfgxCzech;
            }
            else
            {
                variant = CipherVariant.AdfgxEnglish;
            }

            return Encrypt(plaintext, key, matrix, variant, preserveSpaces).OutputText;
        }

        public static string DecryptAdfgx(string ciphertext, string key, string matrix, bool isCzechVersion = true, bool restoreSpaces = true)
        {
            CipherVariant variant;

            if (isCzechVersion)
            {
                variant = CipherVariant.AdfgxCzech;
            }
            else
            {
                variant = CipherVariant.AdfgxEnglish;
            }

            return Decrypt(ciphertext, key, matrix, variant, restoreSpaces).OutputText;
        }

        public static string EncryptAdfgvx(string plaintext, string key, string matrix, bool preserveSpaces = true)
        {
            return Encrypt(plaintext, key, matrix, CipherVariant.Adfgvx, preserveSpaces).OutputText;
        }

        public static string DecryptAdfgvx(string ciphertext, string key, string matrix, bool restoreSpaces = true)
        {
            return Decrypt(ciphertext, key, matrix, CipherVariant.Adfgvx, restoreSpaces).OutputText;
        }

    }
}
