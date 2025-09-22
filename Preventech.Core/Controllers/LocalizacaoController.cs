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

        [HttpPost("cadastro")]
        public async Task<ApiResponse<Equipamento>> CadastrarEquipamento([FromBody] Equipamento equipamento)
        {

            try
            {
                // Adiciona o equipamento ao contexto
                _context.Equipamentos.Add(equipamento);

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                return new ApiResponse<Equipamento>
                {
                    Success = true,
                    Message = "Equipamento cadastrado com sucesso",
                    Data = equipamento
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Equipamento>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar equipamento: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet]
        public async Task<ApiResponse<List<Localizacao>>> GetLocalizacao()
        {
            try
            {
                return new ApiResponse<List<Localizacao>>
                {
                    Success = true,
                    Message = "Localizações recuperadas com sucesso",
                    Data = await _context.Localizacoes.ToListAsync()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Localizacao>>
                {
                    Success = false,
                    Message = $"Erro ao buscar equipamentos: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet("{campus}")]
        public async Task<ApiResponse<List<Localizacao>>> GetDentroCampus(int campus)
        {
            try
            {
                var localizacoes = from loc in _context.Localizacoes
                                   where loc.Campus == campus
                                   select loc;

                if (localizacoes.Any())
                {
                    return new ApiResponse<List<Localizacao>>
                    {
                        Success = false,
                        Message = $"Campus {campus} não encontrado",
                        Data = null
                    };
                }

                return new ApiResponse<List<Localizacao>>
                {
                    Success = true,
                    Message = $"Salas do campus recuperadas",
                    Data = await localizacoes.ToListAsync()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Localizacao>>
                {
                    Success = false,
                    Message = $"Erro ao buscar localização por campus: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpPost("cadastro/range")]
        public async Task<ApiResponse<int>> AddPredio([FromBody] Localizacao locais)
        {
            try
            {
                var salas = from i in Enumerable.Range(1, locais.Andar)
                            from j in Enumerable.Range(1, locais.Numero)
                            select new Localizacao(locais.Campus, locais.Predio, i, j);

                _context.Localizacoes.AddRange(salas);
                await _context.SaveChangesAsync();
                var qtd = salas.Count();
                return new ApiResponse<int>
                {
                    Success = true,
                    Message = $"{qtd} salas cadastradas no prédio {locais.Predio}",
                    Data = qtd
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = $"Erro ao cadastrar maquina: {ex.Message}",
                    Data = 0
                };
            }

        }
    }
}
