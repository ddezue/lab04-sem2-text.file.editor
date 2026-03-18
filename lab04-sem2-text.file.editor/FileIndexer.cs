using System;
using System.Collections.Generic;
using System.IO;

namespace lab04_sem2_text.file.editor
{
  public class FileIndexerApp
  {
    public int MaxFilesToDisplay;
    public int StartingIndex;

    public TextFileSearcher Searcher;
    public List<string> LastResults;

    public FileIndexerApp()
    {
      MaxFilesToDisplay = 10;
      StartingIndex = 0;
      Searcher = new TextFileSearcher();
      LastResults = new List<string>();
    }

    public void Run()
    {
      bool isRunning;
      string choice;
      isRunning = true;

      while (isRunning) {
        ShowMenu();
        choice = Console.ReadLine();

        switch (choice)
        {
          case "1":
            IndexDirectory();
            break;
          case "2":
            SearchFiles();
            break;
          case "3":
            ShowResults();
            break;
          case "0":
            isRunning = false;
            break;
          default:
            Console.WriteLine("Invalid choice");
            break;
        }

        if (isRunning) {
          Console.WriteLine("\nPress any key to continue...");
          Console.ReadKey();
          Console.Clear();
        }
      }
    }

    private void ShowMenu()
    {
      Console.WriteLine("Text File Indexer\n");
      Console.WriteLine();

      Console.WriteLine("1. Index directory");
      Console.WriteLine("2. Search files by keywords");
      Console.WriteLine("3. Show results");
      Console.WriteLine("0. Exit");
      Console.Write("\nChoose: ");
    }

    private void IndexDirectory()
    {
      Console.Write("Enter directory path: ");
      string directory;
      directory = Console.ReadLine();

      if (Directory.Exists(directory) == false) {
        Console.WriteLine("Directory does not exist");
        return;
      }

      Console.Write("Enter keywords separated by space: ");
      string keywordsInput;
      keywordsInput = Console.ReadLine();

      string[] keywords;
      keywords = keywordsInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      Console.Write("Case sensitive? (y/n): ");
      string caseSensitiveInput;
      caseSensitiveInput = Console.ReadLine();

      bool caseSensitive;

      if (caseSensitiveInput != null && caseSensitiveInput == "y") {
        caseSensitive = true;
      } else {
        caseSensitive = false;
      }

      Searcher.SetKeywords(keywords);
      Searcher.SetCaseSensitive(caseSensitive);

      Console.WriteLine("Search started");
      LastResults = Searcher.SearchInDirectory(directory);

      Console.WriteLine($"Search completed. Files found: {LastResults.Count}");

      if (LastResults.Count > 0) {
        Console.WriteLine("\nFiles found:");

        for (int resultIndex = 0; resultIndex < LastResults.Count && resultIndex < MaxFilesToDisplay; ++resultIndex) {
          Console.WriteLine($"- {LastResults[resultIndex]}");
        }

        if (LastResults.Count > MaxFilesToDisplay) {
          Console.WriteLine($"... and {LastResults.Count - MaxFilesToDisplay} more files");
        }
      }
    }

    private void SearchFiles()
    {
      if (LastResults.Count == 0) {
        Console.WriteLine("Please index a directory first");
        return;
      }

      Console.Write("Enter keywords to search separated by space: ");
      string keywordsInput;
      keywordsInput = Console.ReadLine();

      string[] searchKeywords;
      searchKeywords = keywordsInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

      List<string> filteredResults;
      filteredResults = new List<string>();

      int resultIndex;
      string file;
      string content;
      bool found;
      int keywordIndex;
      string keyword;

      for (resultIndex = StartingIndex; resultIndex < LastResults.Count; ++resultIndex ) {
        file = LastResults[resultIndex];
        try
        {
          FileStream fileStream;
          StreamReader streamReader;

          fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
          streamReader = new StreamReader(fileStream);


          content = streamReader.ReadToEnd();
          content = content.ToLower();

          streamReader.Close();
          fileStream.Close();

          found = true;

          for (keywordIndex = StartingIndex; keywordIndex < searchKeywords.Length; ++keywordIndex) {
            keyword = searchKeywords[keywordIndex];
            keyword = keyword.ToLower();

            if (content.Contains(keyword) == false) {
              found = false;
              break;
            }
          }

          if (found) {
            filteredResults.Add(file);
          }
        }
        catch
        {
          Console.WriteLine($"Could not read file {file}");
        }
      }

      Console.WriteLine($"Files found: {filteredResults.Count}");

      int displayIndex;

      if (filteredResults.Count > 0) {
        Console.WriteLine("\nFiles:");

        for (displayIndex = StartingIndex; displayIndex < filteredResults.Count; ++displayIndex) {
          file = filteredResults[displayIndex];
          Console.WriteLine($"- {file}");
        }
      }
    }

    private void ShowResults()
    {
      string file;

      if (LastResults.Count == 0) {
        Console.WriteLine("No results to display");
        return;
      }

      int resultIndex;

      for (resultIndex = StartingIndex; resultIndex < LastResults.Count; ++resultIndex) {
        file = LastResults[resultIndex];
        Console.WriteLine($"{resultIndex + 1}. {file}");
      }
    }
  }
}