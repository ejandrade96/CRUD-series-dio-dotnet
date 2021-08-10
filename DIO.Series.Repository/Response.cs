using DIO.Series.Domain.Models;
using DIO.Series.Domain.Repository;

namespace DIO.Series.Repository
{
  public class Response : IResponse
  {
    public Serie Result { get; set; }
    
    public string ErrorMessage { get; set; }

    public bool HasError() => !string.IsNullOrWhiteSpace(ErrorMessage);
  }
}