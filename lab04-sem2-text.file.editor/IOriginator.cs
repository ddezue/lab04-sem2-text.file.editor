using System;
using System.IO;

namespace lab04_sem2_text.file.editor
{
  [Serializable]
  public class TextFileWithMemento : IOriginator
  {
    public string FilePath { get; set; }
    public string Content { get; set; }
    public DateTime LastModified { get; set; }

    public TextFileWithMemento()
    {
      FilePath = string.Empty;
      Content = string.Empty;
      LastModified = DateTime.Now;
    }

    public TextFileWithMemento(string filePath)
    {
      FilePath = filePath;
      LoadFromFile();
    }

    public void LoadFromFile()
    {
      if (File.Exists(FilePath)) {
        FileStream fileStream;
        StreamReader streamReader;

        fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
        streamReader = new StreamReader(fileStream);

        Content = streamReader.ReadToEnd();

        streamReader.Close();
        fileStream.Close();

        LastModified = File.GetLastWriteTime(FilePath);
      }
    }

    public void SaveToFile()
    {
      string directoryPath;
      directoryPath = Path.GetDirectoryName(FilePath);

      if (string.IsNullOrEmpty(directoryPath) == false) {
        Directory.CreateDirectory(directoryPath);
      }

      FileStream fileStream;
      StreamWriter streamWriter;

      fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write);
      streamWriter = new StreamWriter(fileStream);

      if (Content != null) {
        streamWriter.Write(Content);
      }

      streamWriter.Close();
      fileStream.Close();

      LastModified = DateTime.Now;
    }

    public void Print()
    {
      int contentLength;

      if (Content == null) {
        contentLength = 0;
      } else {
        contentLength = Content.Length;
      }

      Console.WriteLine("FilePath={0} ContentLength={1} LastModified={2}",
        FilePath, contentLength, LastModified);
    }

    public object GetMemento()
    {
      Memento memento;
      memento = new Memento();

      memento.Content = Content;
      memento.Description = "State saved";
      memento.Timestamp = DateTime.Now;

      return memento;
    }

    public void SetMemento(object memento)
    {
      if (memento is Memento) {
        Memento mem;
        mem = (Memento)memento;
        Content = mem.Content;
      }
    }
  }
}
