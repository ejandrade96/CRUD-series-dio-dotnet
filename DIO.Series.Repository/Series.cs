using DIO.Series.Domain.Repository;
using DIO.Series.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DIO.Series.Repository
{
  public class Series : ISeries
  {
    private readonly List<Serie> _seriesList;

    public Series(List<Serie> serieList)
    {
      _seriesList = serieList;
    }

    public IResponse Add(Serie serie)
    {
      var response = new Response();

      var serieToAdd = serie;
      serieToAdd.SetId(GetNextId());
      var serieByTitle = GetSerieByTitle(serieToAdd.Title);

      if (serieByTitle != null)
        response.ErrorMessage = "Já existe uma série cadastrada com este título";

      else
      {
        _seriesList.Add(serieToAdd);
        response.Result = serieToAdd;
      }

      return response;
    }
    
    public IEnumerable<Serie> GetAll(bool? onlyAvailables) => _seriesList.Where(x => onlyAvailables == true ? x.Available : true);

    public IResponse Get(int id)
    {
      throw new NotImplementedException();
    }

    public IResponse Update(Serie serie)
    {
      throw new NotImplementedException();
    }

    public IResponse Delete(int id)
    {
      throw new NotImplementedException();
    }

    public Serie GetSerieByTitle(string title) => _seriesList.FirstOrDefault(x => x.Title.ToLower().Trim() == title.ToLower().Trim());

    private int GetNextId() => _seriesList.Count + 1;
  }
}