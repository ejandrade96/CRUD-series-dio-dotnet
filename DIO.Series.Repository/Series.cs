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

    public IResponse Update(Serie serie)
    {
      var response = new Response();
      var errorMessage = CheckForErrorsToUpdate(serie);

      var serieFound = _seriesList.FirstOrDefault(x => x.Id == serie.Id);
      var serieByTitle = GetSerieByTitle(serie.Title);

      if (string.IsNullOrWhiteSpace(errorMessage))
      {
        var indexSerieFound = _seriesList.IndexOf(serieFound);
        _seriesList[indexSerieFound] = serie;
      }

      else
        response.ErrorMessage = errorMessage;

      return response;
    }

    public IResponse Get(int id)
    {
      var response = new Response();

      var serie = _seriesList.FirstOrDefault(x => x.Id == id);

      if (serie == null)
        response.ErrorMessage = "Série não encontrada!";

      else
        response.Result = serie;

      return response;
    }

    public IResponse Delete(int id)
    {
      throw new NotImplementedException();
    }

    public Serie GetSerieByTitle(string title) => _seriesList.FirstOrDefault(x => x.Title.ToLower().Trim() == title.ToLower().Trim());

    private int GetNextId() => _seriesList.Count + 1;

    private string CheckForErrorsToUpdate(Serie serie)
    {
      var errorMessage = "";

      var serieFound = _seriesList.FirstOrDefault(x => x.Id == serie.Id);

      if (serieFound == null)
        errorMessage = "Série não encontrada!";

      else
      {
        var serieByTitle = GetSerieByTitle(serie.Title);

        if (serieByTitle != null && serieByTitle.Id != serie.Id)
          errorMessage = "Já existe uma série cadastrada com este título";
      }

      return errorMessage;
    }
  }
}