namespace lab04_sem2_text.file.editor
{
  public class Memento
  {
    public string Content { get; set; }
    public string Description { get; set; }


    public interface IOriginator
    {
      object GetMemento();
      void SetMemento(object memento);
    }
  }
}
