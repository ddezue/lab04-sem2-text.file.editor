using System.Collections.Generic;
using System.IO;

namespace lab04_sem2_text.file.editor
{
  public class TextFileSearcher
  {
    public string TextFileSearchPattern;
    public int StartingIndex;

    public string[] Keywords;
    public bool CaseSensitive;

    public TextFileSearcher()
    {
      TextFileSearchPattern = "*.txt";
      StartingIndex = 0;
      Keywords = new string[0];
      CaseSensitive = false;
    }

    public void SetKeywords(string[] keywords)
    {
      Keywords = keywords;
    }

    public void SetCaseSensitive(bool caseSensitive)
    {
      CaseSensitive = caseSensitive;
    }

    public List<string> SearchInDirectory(string directoryPath)
    {
      List<string> results;
      string[] files;
      int fileIndex;
      string file;
      bool fileContainsKeywords;

      results = new List<string>();

      if (Directory.Exists(directoryPath) == false) {
        return results;
      }

      files = Directory.GetFiles(directoryPath, TextFileSearchPattern, SearchOption.AllDirectories);

      for (fileIndex = StartingIndex; fileIndex < files.Length; ++fileIndex) {
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
        string keyword;
        string searchKeyword;

        FileStream fileStream;
        StreamReader streamReader;

        fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        streamReader = new StreamReader(fileStream);

        content = streamReader.ReadToEnd();

        streamReader.Close();
        fileStream.Close();

        if (CaseSensitive == false) {
          content = content.ToLower();
        }

        for (keywordIndex = StartingIndex; keywordIndex < Keywords.Length; ++keywordIndex) {
          keyword = Keywords[keywordIndex];

          if (CaseSensitive) {
            searchKeyword = keyword;
          } else {
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
