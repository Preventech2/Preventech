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
    private ApplicationDbContext _context;
    private DateTime UltimaVarredura;

    [DllImport("libc")]
    private static extern void signal(int signum, Action handler);
    private const int SIGUSR1 = 10;
    public ManutencaoController(ApplicationDbContext context)
    {
        _context = context;
        LeituraDiaria();
        UltimaVarredura = DateTime.Today;
    }

    /// <summary>
    /// Define se uma manutenção está agendada para hoje
    /// </summary>
    /// <param name="ultima">última vez que a manutenção foi realizada</param>
    /// <param name="freq">frequência da manutenção</param>
    /// <returns></returns>
    private static StatusManutencao Status(DateTime ultima, DateTimeOffset freq)
    {
        if (ultima.Add(freq.Subtract(DateTimeOffset.UnixEpoch)).Date < DateTime.Today)
            return StatusManutencao.Atrasada;

        if (ultima.Add(freq.Subtract(DateTimeOffset.UnixEpoch)).Date < DateTime.Today.AddDays(1))
            return StatusManutencao.ParaHoje;

        return StatusManutencao.EmDia;
    }

    // Essas linhas são o coração da preventech inteira
    // teoricamente isso poderia ser uma procedure com pg_cron, mas
    [HttpGet("varredura/")]
    private async Task<ApiResponse<TimeSpan>> LeituraDiaria()
    {
        var hoje = DateTime.Today;
        var inicio = DateTime.Now;
        if (UltimaVarredura - inicio < TimeSpan.FromHours(23)) {
            Console.WriteLine("Sistema já foi varrido recentemente");
            return new ApiResponse<TimeSpan>
            {
                Success = false,
                Message = "Sistema varrido recentemente",
                Data = UltimaVarredura - inicio,
            };
        }
        Console.WriteLine($"== Iniciando varredura dia {hoje}");
        foreach (var pred in _context!.Preditivas)
            pred.Status = Status(pred.UltimaRealizacao, pred.Frequencia);

        foreach (var prev in _context!.Preventivas)
            prev.Status = Status(prev.UltimaRealizacao, prev.Frequencia);

        _context.SaveChanges();
        UltimaVarredura = DateTime.Now;
        var diff = UltimaVarredura - inicio;
        Console.WriteLine($"== Varredura completa em {diff}");
        return new ApiResponse<TimeSpan>
        {
            Success = true,
            Message = "Sistema varrido",
            Data = UltimaVarredura - inicio,
        };
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
                Message = $"Erro ao receber status de preditivas : {ex}",
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
                Message = $"Erro ao receber status de preditivas : {ex}",
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
                Message = $"Erro ao receber status de preditivas : {ex}",
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