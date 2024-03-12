using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace StringsCollections {
  internal class Program {
    static void Main(string[] args) {
      Dictionary<string, string> incorrectWords = new Dictionary<string, string> {
        {"слоово", "слово"},
        {"солво", "слово"},
        {"солово", "слово"},
        {"сво", "слово"},
        {"слво", "слово"},
        {"слвоо", "слово"},
      };
      string userDirectoryPath;
      string oldPhoneNumberPattern = @"\((\d{3})\) (\d{3})-(\d{2})-(\d{2})"; // Например: (012) 345-67-89
      // string newPhoneNumberPattern = @"+380 $1 $2 $3 $4"; // не позволяет убрать 0

      do {
        Console.Write("Введите абсолютный путь директории, файлы которой будут обработаны (пропустите, чтобы оставить по умолчанию): ");
        userDirectoryPath = Console.ReadLine();
        if (userDirectoryPath == "") {
          userDirectoryPath = Directory.GetCurrentDirectory();
        }
      } while (!Directory.Exists(userDirectoryPath));

      string[] files = Directory.GetFiles(userDirectoryPath, "*.txt");

      foreach (string file in files) {
        string fileContent = File.ReadAllText(file);
        foreach (KeyValuePair<string, string> incorrectWord in incorrectWords) {
          fileContent = fileContent.Replace(incorrectWord.Key, incorrectWord.Value);
        }

        MatchCollection numberMatches = Regex.Matches(fileContent, oldPhoneNumberPattern);
        foreach (Match numberMatch in numberMatches) {
          fileContent = changePhoneNumber(fileContent, numberMatch);
        }

        File.WriteAllText(file, fileContent);
        Console.WriteLine($"Файл {file} обработан");
      }

      Console.WriteLine("Работа программы завершена. Нажмите любую клавишу, чтобы закрыть");
      // Ожидание нажатия клавиши (чтобы окно не закрывалось сразу после выполнения программы)
      Console.ReadKey();
    }

    static string changePhoneNumber(string input, Match match) {
      string phoneNumber = match.Value;
      string phoneNumberInNewFormat = "+38" + phoneNumber.Substring(1, 1) + " " + phoneNumber.Substring(2, 2) + " " + phoneNumber.Substring(6, 3) + " " + phoneNumber.Substring(10, 2) + " " + phoneNumber.Substring(13, 2);

      return input.Replace(match.Value, phoneNumberInNewFormat);
    }
  }
}
