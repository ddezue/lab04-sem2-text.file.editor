using System;

namespace lab04_sem2_text.file.editor
{
  public class Memento
  {
    public string Content { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }

    public Memento()
    {
      Timestamp = DateTime.Now;
    }
  }

  public interface IOriginator
  {
    object GetMemento();
    void SetMemento(object memento);
  }
}
