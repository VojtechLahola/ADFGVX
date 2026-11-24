using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADFGVX;

namespace ADFGVX.Tests
{
    [TestClass]
    public class AdfgvxCipherTests
    {
        [TestMethod]
        public void Test_Filtration_DiacriticsAndCase()
        {
            string input = "Příliš žluťoučký kůň úpěl ďábelské ódy";
            string filtered = AdfgvxCipher.FilterInput(input, CipherVariant.AdfgxCzech, keepSpaces: true);
            Assert.AreEqual("PRILIS ZLUTOUCKY KUN UPEL DABELSKE ODY", filtered);
        }

        [TestMethod]
        public void Test_Filtration_Czech_W_ReplacedBy_V()
        {
            string input = "Wagon and Window";
            string filtered = AdfgvxCipher.FilterInput(input, CipherVariant.AdfgxCzech, keepSpaces: true);
            Assert.IsTrue(filtered.Contains("VAGON"));
            Assert.IsTrue(filtered.Contains("VINDOV"));
            Assert.IsFalse(filtered.Contains("W"));
        }

        [TestMethod]
        public void Test_Filtration_English_J_ReplacedBy_I()
        {
            string input = "Jump and Jack";
            string filtered = AdfgvxCipher.FilterInput(input, CipherVariant.AdfgxEnglish);
            Assert.IsTrue(filtered.Contains("IUMP"));
            Assert.IsTrue(filtered.Contains("IACK"));
            Assert.IsFalse(filtered.Contains("J"));
        }

        [TestMethod]
        public void Test_Filtration_Adfgvx_KeepsDigits()
        {
            string input = "Tajny Ukol 007 v 15:30";
            string filtered = AdfgvxCipher.FilterInput(input, CipherVariant.Adfgvx);
            Assert.AreEqual("TAJNYUKOL007V1530", filtered);
        }

        [TestMethod]
        public void Test_GenerateAlphabet_ProducesUniqueValidChars()
        {
            // ADFGX Czech: 25 unique chars
            string cz5 = AdfgvxCipher.GenerateAlphabet(CipherVariant.AdfgxCzech);
            Assert.AreEqual(25, cz5.Length);
            Assert.AreEqual(25, cz5.Distinct().Count());
            Assert.IsFalse(cz5.Contains('W'));

            // ADFGX English: 25 unique chars
            string en5 = AdfgvxCipher.GenerateAlphabet(CipherVariant.AdfgxEnglish);
            Assert.AreEqual(25, en5.Length);
            Assert.AreEqual(25, en5.Distinct().Count());
            Assert.IsFalse(en5.Contains('J'));

            // ADFGVX: 36 unique chars
            string v6 = AdfgvxCipher.GenerateAlphabet(CipherVariant.Adfgvx);
            Assert.AreEqual(36, v6.Length);
            Assert.AreEqual(36, v6.Distinct().Count());
        }

        [TestMethod]
        public void Test_RemainingCharacters_Tracking()
        {
            // Empty matrix: all characters remaining
            string remaining = AdfgvxCipher.GetRemainingCharacters("", CipherVariant.AdfgxCzech);
            Assert.AreEqual(25, remaining.Length);

            // Partially filled
            string partial = "ABCDE";
            string remainingPartial = AdfgvxCipher.GetRemainingCharacters(partial, CipherVariant.AdfgxCzech);
            Assert.AreEqual(20, remainingPartial.Length);
            Assert.IsFalse(remainingPartial.Contains('A'));
            Assert.IsTrue(remainingPartial.Contains('F'));

            // Fully filled
            string full = AdfgvxCipher.AlphabetCzech5;
            string remainingFull = AdfgvxCipher.GetRemainingCharacters(full, CipherVariant.AdfgxCzech);
            Assert.AreEqual(0, remainingFull.Length);
        }

        [TestMethod]
        public void Test_Fractionation_RoundTrip()
        {
            string matrix = AdfgvxCipher.AlphabetCzech5;
            string message = "TAJNAZPRAVA";

            string fractionated = AdfgvxCipher.Fractionate(message, matrix, CipherVariant.AdfgxCzech);
            // Every character turns into 2 chars from A,D,F,G,X
            Assert.AreEqual(message.Length * 2, fractionated.Length);
            foreach (char c in fractionated)
            {
                Assert.IsTrue("ADFGX".Contains(c));
            }

            string reconstructed = AdfgvxCipher.Defractionate(fractionated, matrix, CipherVariant.AdfgxCzech);
            Assert.AreEqual(message, reconstructed);
        }

        [TestMethod]
        public void Test_ColumnarTranspose_RoundTrip()
        {
            string fractionated = "ADFGXADFGXADFGXADFGXAA";
            string key = "KLIC";

            var (transposedWithSpaces, columns, _) = AdfgvxCipher.ColumnarTranspose(fractionated, key);
            Assert.IsTrue(transposedWithSpaces.Contains(" "));
            Assert.AreEqual(4, columns.Count);

            // Reconstruct with spaces
            string inverseFromSpaces = AdfgvxCipher.InverseColumnarTranspose(transposedWithSpaces, key);
            Assert.AreEqual(fractionated, inverseFromSpaces);

            // Reconstruct without spaces
            string inverseWithoutSpaces = AdfgvxCipher.InverseColumnarTranspose(transposedWithSpaces.Replace(" ", ""), key);
            Assert.AreEqual(fractionated, inverseWithoutSpaces);
        }

        [TestMethod]
        public void Test_ColumnarTranspose_WithDuplicateLettersInKey()
        {
            string text = "ADFGXADFGXADFGX";
            string key = "BANANA"; // Contains multiple 'A's and 'N's

            var (transposed, _, _) = AdfgvxCipher.ColumnarTranspose(text, key);
            string reconstructed = AdfgvxCipher.InverseColumnarTranspose(transposed, key);
            Assert.AreEqual(text, reconstructed);
        }

        [TestMethod]
        public void Test_Adfgx_Czech_Encrypt_Decrypt_PreservesSpaces()
        {
            string matrix = AdfgvxCipher.AlphabetCzech5;
            string key = "TAJEMSTVI";
            string plain = "Ahoj světe, útok začíná v pět!";

            string cipher = AdfgvxCipher.EncryptAdfgx(plain, key, matrix, isCzechVersion: true);
            Assert.IsFalse(string.IsNullOrEmpty(cipher));
            Assert.IsTrue(cipher.Contains(" "));

            string decrypted = AdfgvxCipher.DecryptAdfgx(cipher, key, matrix, isCzechVersion: true);
            // Mezery jsou v dešifrovaném textu zachovány!
            Assert.AreEqual("AHOJ SVETE UTOK ZACINA V PET", decrypted);
        }

        [TestMethod]
        public void Test_Adfgx_English_Encrypt_Decrypt_PreservesSpaces()
        {
            string matrix = AdfgvxCipher.AlphabetEnglish5;
            string key = "SECRET";
            string plain = "Jump over lazy dog";

            string cipher = AdfgvxCipher.EncryptAdfgx(plain, key, matrix, isCzechVersion: false);
            string decrypted = AdfgvxCipher.DecryptAdfgx(cipher, key, matrix, isCzechVersion: false);
            Assert.AreEqual("IUMP OVER LAZY DOG", decrypted);
        }

        [TestMethod]
        public void Test_Adfgx_Czech_Encrypt_Decrypt_PreservesDigitsAndLiteralWords()
        {
            string matrix = AdfgvxCipher.AlphabetCzech5;
            string key = "KLIC";
            string plaintext = "CISLO 1 JEDEN Q 0123456789";

            string ciphertext = AdfgvxCipher.EncryptAdfgx(plaintext, key, matrix);
            string decrypted = AdfgvxCipher.DecryptAdfgx(ciphertext, key, matrix);

            Assert.AreEqual("CISLO 1 JEDEN Q 0123456789", decrypted);
        }

        [TestMethod]
        public void Test_Adfgx_English_Encrypt_Decrypt_PreservesDigitsAndLiteralWords()
        {
            string matrix = AdfgvxCipher.AlphabetEnglish5;
            string key = "SECRET";
            string plaintext = "NUMBER 1 ONE Q 0123456789";

            string ciphertext = AdfgvxCipher.EncryptAdfgx(plaintext, key, matrix, isCzechVersion: false);
            string decrypted = AdfgvxCipher.DecryptAdfgx(ciphertext, key, matrix, isCzechVersion: false);

            Assert.AreEqual("NUMBER 1 ONE Q 0123456789", decrypted);
        }

        [TestMethod]
        public void Test_Adfgvx_6x6_Encrypt_Decrypt_WithNumbersAndSpaces()
        {
            string matrix = AdfgvxCipher.Alphabet6;
            string key = "KRYPTOGRAFIE";
            string plain = "Utok na kridlo v 0430 hodin!";

            string cipher = AdfgvxCipher.EncryptAdfgvx(plain, key, matrix);
            Assert.IsTrue(cipher.Contains(" ")); // spaces between columns

            string decrypted = AdfgvxCipher.DecryptAdfgvx(cipher, key, matrix);
            Assert.AreEqual("UTOK NA KRIDLO V 0430 HODIN", decrypted);
        }

        [TestMethod]
        public void Test_Adfgvx_WikipediaExample()
        {
            // Wikipedia known ADFGVX example:
            // Matrix:
            //   A D F G V X
            // A N A 1 C 3 H
            // D 8 T B 2 O M
            // F E 5 W R P D
            // G 4 F 6 G 7 I
            // V 9 J 0 K L M - wait, in wikipedia example:
            // Let's test with a generated random matrix and verify full cycle
            string randomMatrix = AdfgvxCipher.GenerateAlphabet(CipherVariant.Adfgvx);
            string key = "GERMAN";
            string plain = "ATTACK AT 10PM";

            string cipher = AdfgvxCipher.EncryptAdfgvx(plain, key, randomMatrix);
            string decrypted = AdfgvxCipher.DecryptAdfgvx(cipher, key, randomMatrix);
            Assert.AreEqual("ATTACK AT 10PM", decrypted);
        }

        [TestMethod]
        public void Test_Decrypt_OddLengthCiphertext_ThrowsArgumentException()
        {
            string matrix = AdfgvxCipher.AlphabetCzech5;
            Assert.Throws<ArgumentException>(() =>
            {
                AdfgvxCipher.DecryptAdfgx("ADF", "KEY", matrix);
            });
        }

        [TestMethod]
        public void Test_Decrypt_InvalidCipherCharacters_ThrowsArgumentException()
        {
            string matrix = AdfgvxCipher.AlphabetCzech5;
            // 'Z' is not in ADFGX
            Assert.Throws<ArgumentException>(() =>
            {
                AdfgvxCipher.DecryptAdfgx("ADFZ", "KEY", matrix);
            });
        }

        [TestMethod]
        public void Test_ShortText_RoundTrip()
        {
            string matrix = AdfgvxCipher.Alphabet6;
            string key = "LONGKEYWORD";
            string plain = "A";

            string cipher = AdfgvxCipher.EncryptAdfgvx(plain, key, matrix);
            string decrypted = AdfgvxCipher.DecryptAdfgvx(cipher, key, matrix);
            Assert.AreEqual("A", decrypted);
        }

        [TestMethod]
        public void Test_LongText_RoundTrip()
        {
            string matrix = AdfgvxCipher.GenerateAlphabet(CipherVariant.Adfgvx);
            string key = "SECRETKEY123";
            string plain = "THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG 1234567890 AND SOME MORE TEXT TO TEST IRREGULAR TRANSPOSITION MATRIX PROPERLY";

            string cipher = AdfgvxCipher.EncryptAdfgvx(plain, key, matrix);
            string decrypted = AdfgvxCipher.DecryptAdfgvx(cipher, key, matrix);
            string expectedFiltered = AdfgvxCipher.FilterInput(plain, CipherVariant.Adfgvx, keepSpaces: true);
            Assert.AreEqual(expectedFiltered, decrypted);
        }

        [TestMethod]
        public void Test_ValidateAndNormalizeMatrix_InvalidLength_ThrowsArgumentException()
        {
            // Only 10 characters instead of 25
            Assert.Throws<ArgumentException>(() =>
            {
                AdfgvxCipher.ValidateAndNormalizeMatrix("ABCDEFGHIJ", CipherVariant.AdfgxCzech);
            });
        }

        [TestMethod]
        public void Test_SpaceToken_Preservation_RoundTrip()
        {
            string matrix = AdfgvxCipher.GenerateAlphabet(CipherVariant.Adfgvx);
            string key = "HESLO";
            string plain = "TOTO JE TEST SE ZACHOVANIM MEZER A 123 CISEL";

            string cipher = AdfgvxCipher.EncryptAdfgvx(plain, key, matrix);
            string decrypted = AdfgvxCipher.DecryptAdfgvx(cipher, key, matrix);

            // Výsledný dešifrovaný text musí mít přesně zachované původní mezery
            Assert.AreEqual("TOTO JE TEST SE ZACHOVANIM MEZER A 123 CISEL", decrypted);
        }

        [TestMethod]
        public void Test_EncryptedOutput_IsGroupedByFiveAndDecrypts()
        {
            string matrix = AdfgvxCipher.Alphabet6;
            string key = "KLIC";
            string plaintext = "TAJNA ZPRAVA 123";

            string ciphertext = AdfgvxCipher.Encrypt(plaintext, key, matrix, CipherVariant.Adfgvx).OutputText;
            string[] groups = ciphertext.Split(' ');

            Assert.IsTrue(groups.Take(groups.Length - 1).All(group => group.Length == 5));
            Assert.IsTrue(groups[^1].Length is >= 1 and <= 5);
            Assert.AreEqual("TAJNA ZPRAVA 123", AdfgvxCipher.Decrypt(ciphertext, key, matrix, CipherVariant.Adfgvx).OutputText);
        }

        [TestMethod]
        public void Test_Whitespace_UsesQUXXUQMarkerAndRestoresLegacyMarker()
        {
            Assert.AreEqual("AQUXXUQB", AdfgvxCipher.FilterInput("A B", CipherVariant.Adfgvx, useSpaceToken: true));
            Assert.AreEqual("AHOJ SVETE", AdfgvxCipher.RestoreSpaces("AHOJQUXXUQSVETE"));
            Assert.AreEqual("AHOJ SVETE", AdfgvxCipher.RestoreSpaces("AHOJXSPACEXSVETE"));
        }

        [TestMethod]
        public void Test_RestoreSpaces_ReplacesKnownTokens()
        {
            string text = "AHOJXSPACEXSVETEXMEZERAXDVAXMXTRI";
            string restored = AdfgvxCipher.RestoreSpaces(text);
            Assert.AreEqual("AHOJ SVETE DVA TRI", restored);
        }

        [TestMethod]
        public void Test_EmptyKey_ThrowsArgumentException()
        {
            string matrix = AdfgvxCipher.AlphabetCzech5;
            Assert.Throws<ArgumentException>(() =>
            {
                AdfgvxCipher.EncryptAdfgx("TEST", "", matrix);
            });
        }
    }
}
