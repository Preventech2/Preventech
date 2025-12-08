using System;
using System.Net;
using System.Net.Http.Json;
using Preventech.Core.Models;
using Preventech.Core.DTOs;
using System.Text.Json;

namespace Preventech.Core.Services;

public class EquipamentoService(HttpClient httpClient)
{

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    /// <summary>
    /// Retorna todos os equipamentos existentes, sem nenhum filtro
    /// </summary>
    /// <returns>Lista de equipamentos</returns>
    /// <exception cref="InvalidOperationException">Caso o filtro falhe, retorne como exceção</exception>
    [Obsolete("A chamada `GetEquipamentos(Equipamento.Vazio)` supre a necessidade desse método")]
    public async Task<ApiResponse<List<Equipamento>>> GetEquipamentosAsync()
    {
        return await httpClient.GetFromJsonAsync<ApiResponse<List<Equipamento>>>("api/equipamentos", _options)
            ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Equipamento>>.");
    }

    /// <summary>
    /// Constrói uma pesquisa de acordo com os valores dados
    /// </summary>
    /// <param name="equip">Equipamento para fazer pesquisa</param>
    /// <param name="Prefixo">Prefixo para valores internos</param>
    /// <returns>string com a pesquisa pronta</returns>
    public static string Query(Equipamento equip, DateTime tempo, string Prefixo = "")
    {
        var itens = new Dictionary<string, string>() { };
        if (!string.IsNullOrWhiteSpace(equip.Nome))
            itens.Add($"{Prefixo}Nome", WebUtility.UrlEncode(equip.Nome));

        if (!string.IsNullOrWhiteSpace(equip.Patrimonio))
            itens.Add($"{Prefixo}Patrimonio", WebUtility.UrlEncode(equip.Patrimonio));

        var res = string.Join("&", from item in itens select $"{item.Key}={item.Value}");
        res = LocalizacaoService.Query(equip.Local, tempo, "Local.") + res;
        if (string.IsNullOrWhiteSpace(res)) return string.Empty;

        res = res[0] == '?' ? res : '?' + res;
        return string.IsNullOrWhiteSpace(res) ? string.Empty : res;
    }

    /// <summary>
    /// Retorna todos os equipamentos que se encaixam no filtro colocado
    /// </summary>
    /// <param name="filtro">Filtro para equipamento, valores nulos/inválidos são considerados como wildcards</param>
    /// <param name="tempo">Tempo da última atualização</param>
    /// <returns>Equipamentos que encaixam ao filtro</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<ApiResponse<List<Equipamento>>> GetEquipamentos(Equipamento filtro)
    {
        var query = Query(filtro, DateTime.MinValue);
        var response = await httpClient.GetFromJsonAsync<ApiResponse<List<Equipamento>>>($"api/equipamentos{query}", _options);
        return response ?? throw new InvalidOperationException("Failed to deserialize ApiResponse<List<Equipamento>>.");
    }

    [Obsolete("A chamada `GetEquipamento(new Equipamento{ Patrimonio = id })` supre a necessidade desse método")]
    public async Task<ApiResponse<Equipamento>> GetEquipamentoByIdRealAsync(int id)
    {
        var response = await GetEquipamentos(new Equipamento { Id = id });
        return new ApiResponse<Equipamento> 
        {
            Success = response.Success,
            Message = response.Message,
            Data = response.Data!.ElementAt(0) ?? null
        };
    }


    public async Task<ApiResponse<Equipamento>?> AddEquipamentoAsync(Equipamento Equipamento)
    {
        var response = await httpClient.PostAsJsonAsync("api/equipamentos", Equipamento);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<Equipamento>>();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Erro ao cadastrar equipamento: {response.StatusCode} - {errorContent}");
        }
    }

    public async Task<ApiResponse<Equipamento>?> UpdateEquipamentoAsync(Equipamento Equipamento)
    {
        var response = await httpClient.PatchAsJsonAsync($"api/equipamentos", Equipamento);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ApiResponse<Equipamento>>();
    }

    public async Task DeleteEquipamentoAsync(string patrimonio)
    {
        var response = await httpClient.DeleteAsync($"api/equipamentos/{patrimonio}");
        response.EnsureSuccessStatusCode();
    }
}
