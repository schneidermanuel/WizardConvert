using WizardConvert.Convert;

namespace WizardConvert
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Grüezi! Willkommen bei WizardConvert!");
            Console.WriteLine("=====================================\n");
            var path = ReadDirectory();
            Console.WriteLine("1) Bildconvertierung");
            Console.WriteLine("2) Videoconvertierung");
            Console.WriteLine("3) Custom Converter-Command");
            var selection = ReadInput(3);
            Console.WriteLine("1) Ursprüngliche Dateien behalten");
            Console.WriteLine("2) Ursprüngliche Dateien löschen");
            var keepFilesInput = ReadInput(2);
            var keepFiles = keepFilesInput == 1;
            switch (selection)
            {
                case 1:
                    Converter.PerformConvertion("magick convert INFILE OUTFILE", path, keepFiles);
                    break;
                case 2:
                    Converter.PerformConvertion("ffmpeg -i INFILE OUTFILE", path, keepFiles);
                    break;
                case 3:
                    Console.WriteLine("Umwanlungsvefehl eingeben. Platzhalter INFILE und OUTFILE verwenden ('ffmpeg -i INFILE -c:a aac -c:v libx264 OUTFILE')");
                    break;
                default:
                    throw new ArgumentException("Invalid Argument");
            }
        }

        private static string ReadDirectory()
        {
            var path = string.Empty;
            while (!Directory.Exists(path))
            {
                Console.WriteLine("Übergeordneter Pfad eingeben");
                path = Console.ReadLine();
            }

            return path;
        }

        private static int ReadInput(int max)
        {
            var input = 0;
            while (input < 1 || input > max)
            {
                var read = Console.ReadLine();
                if (!int.TryParse(read, out input))
                {
                    Console.WriteLine("Ungültige Eingabe");
                }
            }

            return input;
        }
    }
}