using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace Zadanie2Krypto
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string seed = SeedTextBox.Text;
            int[] taps = TapsTextBox.Text
                .Split(',')
                .Select(int.Parse)
                .ToArray();
            string plaintext = StringTextBox.Text;
            Lfsr lfsr = new Lfsr(seed, taps);

            var sb1 = new StringBuilder();
            sb1.AppendLine("\nEncryption:");
            var ciphertext = new StringBuilder();

            foreach (char c in plaintext)
            {
                int bit = lfsr.Next();
                char encryptedChar = (char)(c ^ bit);
                ciphertext.Append(encryptedChar);
                sb1.AppendLine($"'{c}' XOR {bit} = '{encryptedChar}'");
            }

            TextBoxOut1.Text = sb1.ToString();
            TextBoxOut1.Text += $"\nCiphertext: {ciphertext}";

            var sb2 = new StringBuilder();
            sb2.AppendLine("\nDecryption:");
            lfsr.Reset();
            var decryptedText = new StringBuilder();

            foreach (char c in ciphertext.ToString())
            {
                int bit = lfsr.Next();
                char decryptedChar = (char)(c ^ bit);
                decryptedText.Append(decryptedChar);
                sb2.AppendLine($"'{c}' XOR {bit} = '{decryptedChar}'");
            }

            TextBoxOut2.Text = sb2.ToString();
            TextBoxOut2.Text += $"\nPlaintext: {decryptedText}";
        }

        public class Lfsr
        {
            private string seed;
            private int[] taps;
            private int currentIndex;

            public Lfsr(string seed, int[] taps)
            {
                this.seed = seed;
                this.taps = taps;
                this.currentIndex = 0;
            }

            public int Next()
            {
                int result = seed[currentIndex] - '0';
                int tapResult = 0;
                foreach (int tap in taps)
                {
                    tapResult ^= (seed[(currentIndex + seed.Length - tap) % seed.Length] - '0');
                }
                currentIndex = (currentIndex + 1) % seed.Length;
                seed = seed.Remove(currentIndex, 1).Insert(currentIndex, tapResult.ToString());
                return result;
            }

            public void Reset()
            {
                currentIndex = 0;
                seed = seed.Remove(currentIndex, 1).Insert(currentIndex, "0");
            }
        }
    }
}