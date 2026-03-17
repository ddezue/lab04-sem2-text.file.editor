using System.Collections.Generic;
using System.IO;

namespace lab04_sem2_text.file.editor
{
  public class TextFileSearcher
  {
    public string TextFileSearchPattern;
    public int StartingIndex;

    public string[] _keywords;
    public bool _caseSensitive;

    public TextFileSearcher()
    {
      TextFileSearchPattern = "*.txt";
      StartingIndex = 0;
      _keywords = new string[0];
      _caseSensitive = false;
    }

    public void SetKeywords(string[] keywords)
    {
      _keywords = keywords;
    }

    public void SetCaseSensitive(bool caseSensitive)
    {
      _caseSensitive = caseSensitive;
    }

    public List<string> SearchInDirectory(string directoryPath)
    {
      List<string> results;
      string[] files;
      int fileIndex;

      results = new List<string>();

      if (Directory.Exists(directoryPath) == false) {
        return results;
      }

      files = Directory.GetFiles(directoryPath, TextFileSearchPattern, SearchOption.AllDirectories);

      for (fileIndex = StartingIndex; fileIndex < files.Length; fileIndex = fileIndex + 1) {
        string file;
        bool fileContainsKeywords;

        file = files[fileIndex];
        fileContainsKeywords = FileContainsKeywords(file);

        if (fileContainsKeywords) {
          results.Add(file);
        }
      }

      return results;
    }

    public bool FileContainsKeywords(string filePath)
    {
      bool containsKeywords;
      containsKeywords = false;

      try
      {
        string content;
        int keywordIndex;

        FileStream fileStream;
        StreamReader streamReader;

        fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        streamReader = new StreamReader(fileStream);

        content = streamReader.ReadToEnd();

        streamReader.Close();
        fileStream.Close();

        if (_caseSensitive == false) {
          content = content.ToLower();
        }

        for (keywordIndex = StartingIndex; keywordIndex < _keywords.Length; keywordIndex = keywordIndex + 1) {
          string keyword;
          string searchKeyword;

          keyword = _keywords[keywordIndex];

          if (_caseSensitive) {
            searchKeyword = keyword;
          }
          else {
            searchKeyword = keyword.ToLower();
          }

          if (content.Contains(searchKeyword)) {
            containsKeywords = true;
            break;
          }
        }
      }
      catch
      {
        containsKeywords = false;
      }

      return containsKeywords;
    }
  }
}
