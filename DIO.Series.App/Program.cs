using System;
using System.Collections.Generic;
using System.Linq;
using DIO.Series.Domain.Models;
using DIO.Series.Domain.Repository;
using DIO.Series.Domain.ValueObjects;

namespace DIO.Series.App
{
  class Program
  {
    static List<Serie> serieList = new List<Serie>();

    static ISeries repositorio = new Repository.Series(serieList);

    static void Main(string[] args)
    {
      string opcaoUsuario = ObterOpcaoUsuario();

      while (opcaoUsuario.ToUpper() != "X")
      {
        switch (opcaoUsuario)
        {
          case "1":
            AddSerie();
            break;
          case "2":
            GetSerieById();
            break;
          case "3":
            GetAllSeries();
            break;
          case "4":
            UpdateSerie();
            break;
          case "5":
            DeleteSerie();
            break;
          case "C":
            Console.Clear();
            break;

          default:
            Console.WriteLine("XXXXXX-Número inválido, por favor tente novamente.-XXXXXX");
            break;
        }

        opcaoUsuario = ObterOpcaoUsuario();
      }

      Console.WriteLine("Obrigado por utilizar nossos serviços.");
      Console.ReadLine();
    }

    private static void AddSerie()
    {
      Console.WriteLine("----------Cadastrar uma nova série----------");
      SkipLine();

      Console.Write("Digite o Título da Série: ");
      var title = Console.ReadLine();
      SkipLine();

      foreach (int index in Enum.GetValues(typeof(Genre)))
      {
        Console.WriteLine("{0}-{1}", index, Enum.GetName(typeof(Genre), index));
      }
      SkipLine();

      Console.Write("Digite o gênero entre as opções acima: ");
      int genreIndex = int.Parse(Console.ReadLine());

      Console.Write("Digite a Descrição da Série: ");
      string description = Console.ReadLine();

      Console.Write("Digite o Ano de Início da Série: ");
      int releaseYear = int.Parse(Console.ReadLine());

      Console.Write("Digite a quantidade de Temporadas atuais da Série: ");
      int seasons = int.Parse(Console.ReadLine());

      Serie serie = new Serie(title, (Genre)genreIndex, description, releaseYear, seasons);

      var response = repositorio.Add(serie);

      if (response.HasError())
        Console.WriteLine($"\n XXXXXX-Não foi possível cadastrar a série. ERRO: {response.ErrorMessage}-XXXXXX");

      else
        Console.WriteLine($"\n ----Série cadastrada com sucesso! ID de acesso: {response.Result.Id}----");
    }

    private static void GetSerieById()
    {
      Console.WriteLine("----------Visualizar série----------");
      SkipLine();

      Console.Write("Digite o id da série: ");
      int id = int.Parse(Console.ReadLine());
      SkipLine();

      var response = repositorio.Get(id);

      if (response.HasError())
        Console.WriteLine($"\n XXXXXX-Ocorreu um erro ao buscar a série. ERRO: {response.ErrorMessage}-XXXXXX");

      else
      {
        var serie = response.Result;
        Console.WriteLine(serie);
      }
    }

    private static void GetAllSeries()
    {
      Console.WriteLine("----------Listar séries----------");
      SkipLine();

      Console.WriteLine("Digite o número de acordo com a opção desejada: ");
      SkipLine();

      Console.WriteLine("Listar todas as séries: 0");
      Console.WriteLine("Listar apenas séries disponíveis: 1");
      Console.WriteLine("Listar apenas séries indisponíveis: 2");
      int selectedOption = int.Parse(Console.ReadLine());
      var series = new List<Serie>();

      switch (selectedOption)
      {
        case 0:
          series = repositorio.GetAll().ToList();
          break;

        case 1:
          series = repositorio.GetAll(true).ToList();
          break;

        case 2:
          series = repositorio.GetAll(false).ToList();
          break;

        default:
          Console.WriteLine("\n XXXXXX-Opção inválida, por favor tente novamente.-XXXXXX");
          return;
      }

      if (series.Count > 0)
        series.ForEach(serie => Console.WriteLine(serie));

      else
        Console.WriteLine("\n Nenhuma série encontrada.");
    }

    private static void UpdateSerie()
    {
      Console.WriteLine("----------Atualizar uma nova série----------");
      SkipLine();

      Console.Write("Digite o Id da Série que deseja atualizar: ");
      var id = int.Parse(Console.ReadLine());
      
      Console.Write("Digite o Título da Série: ");
      var title = Console.ReadLine();
      SkipLine();

      foreach (int index in Enum.GetValues(typeof(Genre)))
      {
        Console.WriteLine("{0}-{1}", index, Enum.GetName(typeof(Genre), index));
      }
      SkipLine();

      Console.Write("Digite o gênero entre as opções acima: ");
      int genreIndex = int.Parse(Console.ReadLine());

      Console.Write("Digite a Descrição da Série: ");
      string description = Console.ReadLine();

      Console.Write("Digite o Ano de Início da Série: ");
      int releaseYear = int.Parse(Console.ReadLine());

      Console.Write("Digite a quantidade de Temporadas atuais da Série: ");
      int seasons = int.Parse(Console.ReadLine());

      Serie serie = new Serie(title, (Genre)genreIndex, description, releaseYear, seasons);
      serie.SetId(id);

      var response = repositorio.Update(serie);

      if (response.HasError())
        Console.WriteLine($"\n XXXXXX-Não foi possível atualizar a série. ERRO: {response.ErrorMessage}-XXXXXX");

      else
        Console.WriteLine("\n ----Série atualizada com sucesso!----");
    }

    private static void DeleteSerie()
    {
      Console.WriteLine("----------Excluir uma série----------");
      SkipLine();

      Console.Write("Digite o Id da Série que deseja excluir: ");
      var id = int.Parse(Console.ReadLine());

      var response = repositorio.Delete(id);

      if (response.HasError())
        Console.WriteLine($"\n XXXXXX-Não foi possível excluir a série. ERRO: {response.ErrorMessage}-XXXXXX");

      else
        Console.WriteLine("\n ----Série excluída com sucesso!----");
    }

    private static string ObterOpcaoUsuario()
    {
      SkipLine();
      Console.WriteLine("DIO Séries a seu dispor!!!");
      Console.WriteLine("Informe a opção desejada:");
      SkipLine();
      Console.WriteLine("1- Cadastrar série");
      Console.WriteLine("2- Visualizar série");
      Console.WriteLine("3- Listar séries");
      Console.WriteLine("4- Atualizar série");
      Console.WriteLine("5- Excluir série");
      Console.WriteLine("C- Limpar Tela");
      Console.WriteLine("X- Sair");
      SkipLine();

      var opcaoUsuario = Console.ReadLine().ToUpper();
      SkipLine();
      return opcaoUsuario;
    }

    private static void SkipLine() => Console.WriteLine();
  }
}
