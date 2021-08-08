using Xunit;

namespace DIO.Series.Unit.Repository
{
  public class Series
  {
    private readonly IRepository _series;

    [Fact]
    public void Deve_Cadastrar_Uma_Serie_Quando_Enviar_Dados_Certos()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      var response = _series.Add(serie);
      var serieAdded = response.Result;

      serieAdded.Id.Should().NotBeNull();
      serieAdded.Id.Should().BeGreaterThan(0);
      serieAdded.Available.Should().BeTrue();
    }

    [Fact]
    public void Deve_Notificar_O_Usuario_Quando_Tentar_Cadastrar_Uma_Serie_Com_Titulo_Repetido()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);
      var serieRepeated = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      _ = _series.Add(serie);

      var response = _series.Add(serieRepeated);

      response.ErrorMessage.Should().Be("Já existe uma série cadastrada com este título");
    }

    [Fact]
    public void Deve_Listar_Todas_As_Series()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);
      var serie2 = new Serie("Treadstone", Genre.Acao, "Treadstone é uma série de televisão de drama de ação americana, conectada e baseada na série de filmes Bourne.", 2019, 1);

      _ = _series.Add(serie);
      _ = _series.Add(serie2);

      var series = _series.GetAll();

      series.Should().HaveCount(2);
    }

    [Fact]
    public void Deve_Listar_Todas_As_Series_Disponiveis()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);
      serie.MakeUnavailable();
      var serieUnavailable = new Serie("Treadstone", Genre.Acao, "Treadstone é uma série de televisão de drama de ação americana, conectada e baseada na série de filmes Bourne.", 2019, 1);

      _ = _series.Add(serie);
      _ = _series.Add(serieUnavailable);

      var availableSeries = _series.GetAll(Status.Available);

      availableSeries.Should().HaveCount(1);
    }

    [Fact]
    public void Deve_Atualizar_Uma_Serie_Quando_Enviar_Dados_Certos()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      var response = _series.Add(serie);
      var serieAdded = response.Result;

      var serieToUpdate = new Serie("Mr. Robot 2", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4)
      { Id = serieAdded.Id };

      var responseUpdate = _series.Update(serieToUpdate);

      responseUpdate.ErrorMessage.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    public void Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Uma_Serie_Inexistente()
    {
      var serieToUpdate = new Serie("Mr. Robot 2", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4)
      { Id = 1 };

      var responseUpdate = _series.Update(serieToUpdate);

      responseUpdate.ErrorMessage.Should().Be("Série não encontrada!");
    }

    [Fact]
    public void Deve_Retornar_Uma_Serie_Por_Id()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      var id = _series.Add(serie);

      var response = _serie.Get(id);
      var serieFound = respose.Result;

      response.ErrorMessage.Should().BeNullOrWhiteSpace();
      serieFound.Id.Should().Be(id);
    }

    [Fact]
    public void Deve_Notificar_O_Usuario_Quando_Tentar_Buscar_Uma_Serie_Inexistente_Por_Id()
    {
      var response = _serie.Get(1);

      response.ErrorMessage.Should().Be("Série não encontrada!");
    }

    [Fact]
    public void Deve_Deletar_Uma_Serie()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      var id = _series.Add(serie);

      var response = _serie.Delete(id);

      response.ErrorMessage.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    public void Deve_Notificar_O_Usuario_Quando_Tentar_Deletar_Uma_Serie_Inexistente()
    {
      var response = _serie.Delete(1);

      response.ErrorMessage.Should().Be("Série não encontrada!");
    }
  }
}