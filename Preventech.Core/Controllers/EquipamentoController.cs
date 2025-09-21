using Microsoft.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;

namespace Preventech.Core.Controllers
{
    [Route("api/equipamentos")]
    [ApiController]
    public class EquipamentoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipamentoController(ApplicationDbContext context)
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
        public async Task<ApiResponse<List<Equipamento>>> GetEquipamentos()
        {
            try
            {
                var equipamentos = await _context.Equipamentos.ToListAsync();
                return new ApiResponse<List<Equipamento>>
                {
                    Success = true,
                    Message = "Equipamentos recuperados com sucesso",
                    Data = equipamentos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Equipamento>>
                {
                    Success = false,
                    Message = $"Erro ao buscar equipamentos: {ex.Message}",
                    Data = null
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<Equipamento>> GetEquipamentoById(int id)
        {
            try
            {
                var equipamentos = await _context.Equipamentos.FindAsync(id);
                return new ApiResponse<Equipamento>
                {
                    Success = true,
                    Message = "Equipamentos recuperados com sucesso",
                    Data = equipamentos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Equipamento>
                {
                    Success = false,
                    Message = $"Erro ao buscar equipamentos: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
