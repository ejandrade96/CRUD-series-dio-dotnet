using DIO.Series.Domain.Models;
using System.Collections.Generic;

namespace DIO.Series.Domain.Repository
{
  public interface ISeries
  {
    IResponse Add(Serie serie);

    IResponse Get(int id);
    
    IEnumerable<Serie> GetAll(bool? onlyAvailables = null);

    IResponse Update(Serie serie);

    IResponse Delete(int id);

    Serie GetSerieByTitle(string title);
  }
}