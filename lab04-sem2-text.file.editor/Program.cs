using System;

namespace lab04_sem2_text.file.editor
{
  class Program
  {
    static void Main(string[] args)
    {
      bool isRunning;
      string choice;
      isRunning = true;

      while (isRunning) {
        Console.Clear();
        Console.WriteLine("Main Menu\n");
        Console.WriteLine("1. Text Editor");
        Console.WriteLine("2. File Indexer");
        Console.WriteLine("0. Exit");
        Console.Write("\nChoose: ");

        choice = Console.ReadLine();

        while (Console.KeyAvailable) {
          Console.ReadKey(true);
        }

        switch (choice?.Trim())
        {
          case "1":
            Console.Clear();
            TextEditorConsole editor = new TextEditorConsole();
            editor.Run();
            break;
          case "2":
            Console.Clear();
            FileIndexerApp indexer = new FileIndexerApp();
            indexer.Run();
            break;
          case "0":
            isRunning = false;
            break;
          default:
            Console.WriteLine("Invalid choice");
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
            break;
        }
      }
    }
  }
}