using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace lab04_sem2_text.file.editor
{
  [Serializable]
  public class TextFile
  {
    public string FilePath { get; set; }
    public string Content { get; set; }
    public DateTime LastModified { get; set; }

    public string FileName
    {
      get
      {
        string fileName;
        fileName = Path.GetFileName(FilePath);
        return fileName;
      }
    }

    public TextFile()
    {
      FilePath = string.Empty;
      Content = string.Empty;
      LastModified = DateTime.Now;
    }

    public TextFile(string filePath)
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
      else {
        Content = string.Empty;
        LastModified = DateTime.Now;
      }
    }

    public void SaveToFile()
    {
      string directoryPath;
      directoryPath = Path.GetDirectoryName(FilePath);

      Directory.CreateDirectory(directoryPath);

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

    public void BinarySerialize(string filePath)
    {
      FileStream fileStream;
      BinaryFormatter binaryFormatter;

      fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
      binaryFormatter = new BinaryFormatter();

      binaryFormatter.Serialize(fileStream, this);
    }

    public void BinaryDeserialize(string filePath)
    {
      FileStream fileStream;
      BinaryFormatter binaryFormatter;
      TextFile deserializedFile;

      fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
      binaryFormatter = new BinaryFormatter();
      deserializedFile = (TextFile)binaryFormatter.Deserialize(fileStream);

      FilePath = deserializedFile.FilePath;
      Content = deserializedFile.Content;
      LastModified = deserializedFile.LastModified;

      fileStream.Close();
    }

    public void XmlSerialize(string filePath)
    {
      FileStream fileStream;
      XmlSerializer xmlSerializer;

      fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
      xmlSerializer = new XmlSerializer(GetType());

      xmlSerializer.Serialize(fileStream, this);
      fileStream.Flush();
      fileStream.Close();
    }

    public void XmlDeserialize(string filePath)
    {
      FileStream fileStream;
      XmlSerializer xmlSerializer;
      TextFile deserializedFile;

      fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
      xmlSerializer = new XmlSerializer(GetType());
      deserializedFile = (TextFile)xmlSerializer.Deserialize(fileStream);

      FilePath = deserializedFile.FilePath;
      Content = deserializedFile.Content;
      LastModified = deserializedFile.LastModified;

      fileStream.Close();
    }

    public void Print()
    {
      int contentLength;

      if (Content == null)  {
        contentLength = 0;
      }
      else {
        contentLength = Content.Length;
      }

      Console.WriteLine("FilePath={0} ContentLength={1} LastModified={2}",
        FilePath, contentLength, LastModified);
    }
  }
}
