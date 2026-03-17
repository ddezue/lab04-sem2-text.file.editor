using System;
using System.IO;

namespace lab04_sem2_text.file.editor
{
  public class TextEditorConsole
  {
    public int SeparatorLineLength;
    public int EqualLineLength;

    public TextFileWithMemento _currentFile;
    public Caretaker _caretaker;

    public TextEditorConsole()
    {
      SeparatorLineLength = 50;
      EqualLineLength = 60;
      _caretaker = new Caretaker();
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
            CreateNewFile();
            break;
          case "2":
            OpenFile();
            break;
          case "3":
            EditFile();
            break;
          case "4":
            ViewFile();
            break;
          case "5":
            SaveFile();
            break;
          case "6":
            Undo();
            break;
          case "7":
            Redo();
            break;
          case "8":
            BinarySerialize();
            break;
          case "9":
            BinaryDeserialize();
            break;
          case "10":
            XmlSerialize();
            break;
          case "11":
            XmlDeserialize();
            break;
          case "0":
            isRunning = false;
            break;
          default:
            Console.WriteLine("Invalid choice");
            break;
        }

        if (isRunning) {
          Console.WriteLine("\nPress any key to continue");
          Console.ReadKey();
          Console.Clear();
        }
      }
    }

    private void ShowMenu()
    {
      Console.WriteLine("Text File Editor\n");

      if (_currentFile != null) {
        string fileName;
        fileName = Path.GetFileName(_currentFile.FilePath);

        Console.WriteLine($"Current file: {fileName}");
        Console.WriteLine($"Path: {_currentFile.FilePath}");
        Console.WriteLine($"Last modified: {_currentFile.LastModified}");
        Console.WriteLine();
      } else {
        Console.WriteLine("No file opened\n");
      }

      Console.WriteLine("1. Create new file");
      Console.WriteLine("2. Open file");
      Console.WriteLine("3. Edit file");
      Console.WriteLine("4. View content");
      Console.WriteLine("5. Save file");
      Console.WriteLine("6. Undo");
      Console.WriteLine("7. Redo");
      Console.WriteLine("8. Binary serialize");
      Console.WriteLine("9. Binary deserialize");
      Console.WriteLine("10. XML serialize");
      Console.WriteLine("11. XML deserialize");
      Console.WriteLine("0. Exit");
      Console.Write("\nSelect action: ");
    }

    public void CreateNewFile()
    {
      Console.Write("Enter path for new file: ");
      string path;
      path = Console.ReadLine();

      _currentFile = new TextFileWithMemento(path);
      _caretaker.Clear();
      _caretaker.SaveState(_currentFile);

      Console.WriteLine("File created successfully");
    }

    public void OpenFile()
    {
      Console.Write("Enter file path: ");
      string path;
      path = Console.ReadLine();

      if (File.Exists(path)) {
        _currentFile = new TextFileWithMemento(path);
        _caretaker.Clear();
        _caretaker.SaveState(_currentFile);

        Console.WriteLine("File opened successfully");
      } else {
        Console.WriteLine("File not found");
      }
    }

    public void EditFile()
    {
      string separatorLine;
      separatorLine = new string('-', SeparatorLineLength);

      Console.WriteLine("Current content:");
      Console.WriteLine(separatorLine);
      Console.WriteLine(_currentFile.Content);
      Console.WriteLine(separatorLine);

      Console.WriteLine("Enter new text (empty line to finish):");

      string newContent;
      string line;

      newContent = string.Empty;
      line = Console.ReadLine();

      while (string.IsNullOrEmpty(line) == false) {
        newContent = newContent + line + Environment.NewLine;
        line = Console.ReadLine();
      }

      if (string.IsNullOrEmpty(newContent) == false) {
        _caretaker.SaveState(_currentFile);
        _currentFile.Content = newContent;

        Console.WriteLine("Text updated");
      }
    }

    public void ViewFile()
    {
      string equalLine;
      equalLine = new string('=', EqualLineLength);

      Console.WriteLine("File content:");
      Console.WriteLine(equalLine);
      Console.WriteLine(_currentFile.Content);
      Console.WriteLine(equalLine);
    }

    public void SaveFile()
    {
      _currentFile.SaveToFile();
      _caretaker.SaveState(_currentFile);
      Console.WriteLine("File saved successfully");
    }

    public void Undo()
    {
      if (_caretaker.CanUndo()) {
        _caretaker.Undo(_currentFile);
        Console.WriteLine("Undo performed");
      } else {
        Console.WriteLine("Nothing to undo");
      }
    }

    public void Redo()
    {
      if (_caretaker.CanRedo()) {
        _caretaker.Redo(_currentFile);
        Console.WriteLine("Redo performed");
      } else {
        Console.WriteLine("Nothing to redo");
      }
    }

    public void BinarySerialize()
    {
      Console.Write("Enter path for binary file: ");
      string path;
      path = Console.ReadLine();

      TextFile textFile;
      textFile = new TextFile();
      textFile.FilePath = _currentFile.FilePath;
      textFile.Content = _currentFile.Content;
      textFile.LastModified = _currentFile.LastModified;

      textFile.BinarySerialize(path);
      Console.WriteLine("Binary serialization completed");
    }

    public void BinaryDeserialize()
    {
      Console.Write("Enter path to binary file: ");
      string path;
      path = Console.ReadLine();

      if (File.Exists(path)) {
        TextFile textFile;
        textFile = new TextFile();
        textFile.BinaryDeserialize(path);

        _currentFile = new TextFileWithMemento();
        _currentFile.FilePath = textFile.FilePath;
        _currentFile.Content = textFile.Content;
        _currentFile.LastModified = textFile.LastModified;

        _caretaker.Clear();
        _caretaker.SaveState(_currentFile);

        Console.WriteLine("Binary deserialization completed");
      } else {
        Console.WriteLine("File not found");
      }
    }

    public void XmlSerialize()
    {
      Console.Write("Enter path for XML file: ");
      string path;
      path = Console.ReadLine();

      TextFile textFile;
      textFile = new TextFile();
      textFile.FilePath = _currentFile.FilePath;
      textFile.Content = _currentFile.Content;
      textFile.LastModified = _currentFile.LastModified;

      textFile.XmlSerialize(path);
      Console.WriteLine("XML serialization completed");
    }

    public void XmlDeserialize()
    {
      Console.Write("Enter path to XML file: ");
      string path;
      path = Console.ReadLine();

      if (File.Exists(path)) {
        TextFile textFile;
        textFile = new TextFile();
        textFile.XmlDeserialize(path);

        _currentFile = new TextFileWithMemento();
        _currentFile.FilePath = textFile.FilePath;
        _currentFile.Content = textFile.Content;
        _currentFile.LastModified = textFile.LastModified;

        _caretaker.Clear();
        _caretaker.SaveState(_currentFile);

        Console.WriteLine("XML deserialization completed");
      } else {
        Console.WriteLine("File not found");
      }
    }
  }
}
