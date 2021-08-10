using DIO.Series.Domain.Models;

namespace DIO.Series.Domain.Repository
{
  public interface IResponse
  {
    Serie Result { get; set; }

    string ErrorMessage { get; set; }

    bool HasError();
  }
}