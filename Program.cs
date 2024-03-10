using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Configuration;

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
      string directoryName;
      string oldPhoneNumberPattern = @"\((\d{3})\) (\d{3})-(\d{2})-(\d{2})";
      string newPhoneNumberPattern = @"+380 $1 $2 $3 $4";

      do {
        Console.Write("Введите абсолютный путь директории, файлы которой будут обработаны (пропустите, чтобы оставить по умолчанию): ");
        directoryName = Console.ReadLine();
        if (directoryName == "") {
          directoryName = Directory.GetCurrentDirectory();
        }
      } while ( !Directory.Exists(directoryName) );

      string[] files = Directory.GetFiles(directoryName, "*.txt");

      foreach (string file in files) {
        string fileContent = File.ReadAllText(file);
        foreach (KeyValuePair<string, string> incorrectWord in incorrectWords) {
          fileContent = fileContent.Replace(incorrectWord.Key, incorrectWord.Value);
        }

        Regex regex = new Regex(oldPhoneNumberPattern);
        fileContent = regex.Replace(fileContent, newPhoneNumberPattern);

        File.WriteAllText(file, fileContent);
        Console.WriteLine($"Файл {file} обработан");
      }

      Console.WriteLine("Работа программы завершена. Нажмите любую клавишу, чтобы закрыть");
      // Ожидание нажатия клавиши (чтобы окно не закрывалось сразу после выполнения программы)
      Console.ReadKey();
    }
  }
}
