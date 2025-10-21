using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;

namespace Preventech.Core.Controllers
{
    [Route("api/localizacao")]
    [ApiController]
    public class LocalizacaoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocalizacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ApiResponse<Localizacao>> CadastrarLocalizacao([FromBody] Localizacao localizacao)
        {

            try
            {
                // Adiciona o equipamento ao contexto
                _context.Localizacoes.Add(localizacao);
                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                return new ApiResponse<Localizacao>
                {
                    Success = true,
                    Message = "Localização cadastrado com sucesso",
                    Data = localizacao
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Localizacao>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar localização: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPatch]
        public async Task<ApiResponse<Localizacao>> EditarLocalizacao([FromBody] Localizacao localizacao)
        {
            try
            {
                if (await _context.Localizacoes.FindAsync(localizacao.Id) is Localizacao loc) {
                    _context.Entry(loc).CurrentValues.SetValues(localizacao);
                    await _context.SaveChangesAsync();
                }

                // Salva as mudanças no banco de dados

                return new ApiResponse<Localizacao>
                {
                    Success = true,
                    Message = "Localização editada com sucesso",
                    Data = localizacao
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Localizacao>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar localização: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet]
        public async Task<ApiResponse<List<Localizacao>>> GetLocalizacoes(
            string? Apelido,
            int? Campus,
            int? Predio,
            int? Andar,
            int? Sala)
        {
            try
            {
                var all = _context.Localizacoes.ToList();

                var locs = from loc in _context.Localizacoes
                           where Apelido == null || loc.Apelido == Apelido
                           where Campus == null || loc.Campus == Campus
                           where Predio == null || loc.Predio == Predio
                           where Andar == null || loc.Andar == Andar
                           where Sala == null || loc.Numero == Sala
                           select loc;

                if (!locs.Any())
                {
                    return new ApiResponse<List<Localizacao>>
                    {
                        Success = false,
                        Message = $"Localizacao não encontrada",
                        Data = null
                    };
                }

                return new ApiResponse<List<Localizacao>>
                {
                    Success = true,
                    Message = $"Salas recuperadas",
                    Data = await locs.ToListAsync()
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Localizacao>>
                {
                    Success = false,
                    Message = $"Erro ao buscar localização: {ex.Message}",
                    Data = null
                };
            }
        }

    }
}
