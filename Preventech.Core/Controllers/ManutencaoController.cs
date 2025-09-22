using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Infrastructure;
namespace Preventech.Core.Controllers;

[Route("api/manutencao")]
[ApiController]
public class ManutencaoController
{
    private static ApplicationDbContext? _context;

    [DllImport("libc")]
    private static extern void signal(int signum, Action handler);
    private const int SIGUSR1 = 10;

    public ManutencaoController(ApplicationDbContext context)
    {
        Console.WriteLine("== log");
        _context = context;
        signal(SIGUSR1, LeituraDiaria);
    }

    /// <summary>
    /// Define se uma manutenção está agendada para hoje
    /// </summary>
    /// <param name="ultima">última vez que a manutenção foi realizada</param>
    /// <param name="freq">frequência da manutenção</param>
    /// <returns></returns>
    private static StatusManutencao status(DateTime ultima, DateTimeOffset freq)
    {
        if (ultima.Add(freq.Subtract(DateTimeOffset.UnixEpoch)).Date < DateTime.Today)
            return StatusManutencao.Atrasada;

        if (ultima.Add(freq.Subtract(DateTimeOffset.UnixEpoch)).Date < DateTime.Today.AddDays(1))
            return StatusManutencao.ParaHoje;

        return StatusManutencao.EmDia;
    }

    // Essas linhas são o coração da preventech inteira
    // teoricamente isso poderia ser uma procedure com pg_cron, mas
    private static void LeituraDiaria()
    {
        var hoje = DateTime.Today;
        var inicio = DateTime.Now;
        Console.WriteLine($"== Iniciando varredura dia {hoje}");
        Console.WriteLine($"context == null ? {_context == null}");
        foreach (var pred in _context!.Preditivas)
            pred.Status = status(pred.UltimaRealizacao, pred.Frequencia);

        foreach (var prev in _context!.Preventivas)
            prev.Status = status(prev.UltimaRealizacao, prev.Frequencia);

        _context.SaveChanges();
        var diff = DateTime.Now - inicio;
        Console.WriteLine($"== Varredura completa em {diff.ToString()}");
    }

    [HttpGet("preditivas")]
    public async Task<ApiResponse<List<Preditiva>?>> Preditivas()
    {
        try
        {
            return new ApiResponse<List<Preditiva>?>
            {
                Success = true,
                Message = "Manutenções preditivas",
                Data = await _context!.Preditivas.ToListAsync(),
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Preditiva>?>
            {
                Success = false,
                Message = $"Erro ao receber status de preditivas : {ex.ToString()}",
                Data = null,
            };
        }
    }

    [HttpGet("preventivas")]
    public async Task<ApiResponse<List<Preventiva>?>> Preventivas()
    {
        try
        {
            return new ApiResponse<List<Preventiva>?>
            {
                Success = true,
                Message = "Manutenções preventivas",
                Data = await _context!.Preventivas.ToListAsync(),
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Preventiva>?>
            {
                Success = false,
                Message = $"Erro ao receber status de preditivas : {ex.ToString()}",
                Data = null,
            };
        }
    }

    [HttpPost("cadastrar/preditiva")]
    public async Task<ApiResponse<Preditiva?>> CadastrarPreditiva([FromBody] Preditiva pred){
        try
        {
            await _context!.Preditivas.AddAsync(pred);
            await _context!.SaveChangesAsync();

            return new ApiResponse<Preditiva?>
            {
                Success = true,
                Message = "Manutenções preventivas",
                Data = pred,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Preditiva?>
            {
                Success = false,
                Message = $"Erro ao receber status de preditivas : {ex.ToString()}",
                Data = null,
            };
        }
    }

    [HttpPost("cadastrar/preventiva")]
    public async Task<ApiResponse<Preditiva?>> CadastrarPreventiva([FromBody] Preditiva pred)
    {
        try
        {
            await _context!.Preditivas.AddAsync(pred);
            await _context!.SaveChangesAsync();

            return new ApiResponse<Preditiva?>
            {
                Success = true,
                Message = "Manutenções preventivas",
                Data = pred,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Preditiva?>
            {
                Success = false,
                Message = $"Erro ao receber status de preditivas : {ex.ToString()}",
                Data = null,
            };
        }
    }

}