using Xunit;
using FluentAssertions;
using DIO.Series.Domain.ValueObjects;
using System.Collections.Generic;
using DIO.Series.Domain.Models;

namespace DIO.Series.Unit.Repository
{
  public class Series
  {
    private readonly List<Serie> _seriesList;

    private readonly Domain.Repository.ISeries _series;

    public Series()
    {
      _seriesList = new List<Serie>();
      _series = new DIO.Series.Repository.Series(_seriesList);
    }

    [Fact]
    public void Deve_Cadastrar_Uma_Serie_Quando_Enviar_Dados_Certos()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      var response = _series.Add(serie);
      var serieAdded = response.Result;

      response.ErrorMessage.Should().BeNullOrWhiteSpace();
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

      var availableSeries = _series.GetAll(true);

      availableSeries.Should().HaveCount(1);
    }

    [Fact]
    public void Deve_Atualizar_Uma_Serie_Quando_Enviar_Dados_Certos()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      var response = _series.Add(serie);
      var serieAdded = response.Result;

      var serieToUpdate = new Serie("Mr. Robot 2", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);
      serieToUpdate.SetId(serieAdded.Id);

      var responseUpdate = _series.Update(serieToUpdate);

      responseUpdate.ErrorMessage.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    public void Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Uma_Serie_Inexistente()
    {
      var serieToUpdate = new Serie("Mr. Robot 2", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);
      serieToUpdate.SetId(1);

      var responseUpdate = _series.Update(serieToUpdate);

      responseUpdate.ErrorMessage.Should().Be("Série não encontrada!");
    }

    [Fact]
    public void Deve_Notificar_O_Usuario_Quando_Tentar_Atualizar_Uma_Serie_Com_Titulo_Ja_Existente()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);
      var serieToUpdate = new Serie("Treadstone", Genre.Acao, "Treadstone é uma série de televisão de drama de ação americana, conectada e baseada na série de filmes Bourne.", 2019, 1);
      
      var serieRepeated = new Serie("Mr. Robot", Genre.Acao, "Treadstone é uma série de televisão de drama de ação americana, conectada e baseada na série de filmes Bourne.", 2019, 1);
      serieRepeated.SetId(2);

      _ = _series.Add(serie);
      _ = _series.Add(serieToUpdate);

      var response = _series.Update(serieRepeated);

      response.ErrorMessage.Should().Be("Já existe uma série cadastrada com este título");
    }

    [Fact]
    public void Deve_Retornar_Uma_Serie_Por_Id()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      var id = _series.Add(serie).Result.Id;

      var response = _series.Get(id);
      var serieFound = response.Result;

      response.ErrorMessage.Should().BeNullOrWhiteSpace();
      serieFound.Id.Should().Be(id);
    }

    [Fact]
    public void Deve_Notificar_O_Usuario_Quando_Tentar_Buscar_Uma_Serie_Inexistente_Por_Id()
    {
      var response = _series.Get(1);

      response.ErrorMessage.Should().Be("Série não encontrada!");
    }

    [Fact]
    public void Deve_Deletar_Uma_Serie()
    {
      var serie = new Serie("Mr. Robot", Genre.Drama, "Elliot é um jovem programador que trabalha como engenheiro de segurança virtual durante o dia", 2015, 4);

      var id = _series.Add(serie).Result.Id;

      var response = _series.Delete(id);

      response.ErrorMessage.Should().BeNullOrWhiteSpace();
    }

    [Fact]
    public void Deve_Notificar_O_Usuario_Quando_Tentar_Deletar_Uma_Serie_Inexistente()
    {
      var response = _series.Delete(1);

      response.ErrorMessage.Should().Be("Série não encontrada!");
    }
  }
}