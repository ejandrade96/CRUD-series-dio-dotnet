using DIO.Series.Domain.ValueObjects;

namespace DIO.Series.Domain.Models
{
  public class Serie : EntityBase
  {
    public string Title { get; private set; }
    
    public Genre Genre { get; private set; }
    
    public string Description { get; private set; }
    
    public int ReleaseYear { get; private set; }
    
    public int Seasons { get; private set; }
    
    public bool Available { get; private set; }

    public Serie(string title, Genre genre, string description, int releaseYear, int seasons)
    {
      Title = title;
      Genre = genre;
      Description = description;
      ReleaseYear = releaseYear;
      Seasons = seasons;
      Available = true;
    }

    public void MakeUnavailable() => Available = false;

    public void SetId(int id) => Id = id;
  }
}