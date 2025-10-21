using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;

namespace Preventech.Core.Controllers
{
    [Route("api/equipamentos")]
    [ApiController]
    public class EquipamentoController(ApplicationDbContext context) : ControllerBase
    {
        /// <summary>
        /// Handler POST para equipamentos, recebe o equipamento a ser
        /// adicionado via corpo da requisição
        /// </summary>
        /// <param name="equipamento">Equipamento a ser adicionado</param>
        /// <returns>Equipamento adicionado com ID válido</returns>
        [HttpPost]
        public async Task<ApiResponse<Equipamento>> CadastrarEquipamento([FromBody] Equipamento equipamento)
        {
            try
            {
                equipamento.Local = (await context.Localizacoes.FindAsync(equipamento.Local.Id))!;
                context.Equipamentos.Add(equipamento);

                await context.SaveChangesAsync();

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

        /// <summary>
        /// Handler GET para receber equipamentos de acordo com um filtro
        /// </summary>
        /// <param name="filtro">Filtro recebido via query</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse<List<Equipamento>>> GetEquipamentos([FromQuery] Equipamento filtro)
        {
            try
            {
                var equipamentos = await context.Equipamentos
                    .Include(eqp => eqp.Local)
                    .Include(eqp => eqp.Local.Responsavel)
                    .Where(query =>
                         (filtro.Id <= 0 || filtro.Id == query.Id)
                      && (string.IsNullOrWhiteSpace(filtro.Patrimonio) || filtro.Patrimonio.Equals(query.Patrimonio))
                      && (string.IsNullOrWhiteSpace(filtro.Nome) || filtro.Nome.Equals(query.Nome))
                      && (filtro.Status == StatusEquipamento.Invalido || filtro.Status == query.Status )
                      && (filtro.Local == null || filtro.Local.Campus <= 0 || query.Local.Campus == filtro.Local.Campus)
                      && (filtro.Local == null || filtro.Local.Predio <= 0 || query.Local.Predio == filtro.Local.Predio)
                      && (filtro.Local == null || filtro.Local.Andar <= 0 || query.Local.Andar == filtro.Local.Andar)
                      && (filtro.Local == null || filtro.Local.Numero <= 0 || query.Local.Numero == filtro.Local.Numero)
                      && (filtro.Local == null || string.IsNullOrWhiteSpace(filtro.Local.Apelido) || filtro.Local.Apelido.Equals(query.Local.Apelido)))
                    .ToListAsync();

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

        /// <summary>
        /// Handler PATCH para edição de equipamentos
        /// </summary>
        /// <param name="equipamento">Equipamento a ser editado</param>
        /// <returns>Equipamento editado</returns>
        [HttpPatch]
        public async Task<ApiResponse<Equipamento>> EditarEquipamento([FromBody] Equipamento equipamento)
        {
            try
            {
                var updatedEquipamento = await context.Equipamentos
                    .Include(eqp => eqp.Local)
                    .Include(eqp => eqp.Local.Responsavel)
                    .FirstOrDefaultAsync(eqp => eqp.Id == equipamento.Id);

                if (updatedEquipamento == null)
                {
                    return new ApiResponse<Equipamento>
                    {
                        Success = false,
                        Message = "Equipamento não encontrado",
                        Data = null
                    };
                }

                var novoLocal = await context.Localizacoes.FindAsync(equipamento.Local.Id);

                if (novoLocal == null)
                {
                    return new ApiResponse<Equipamento>
                    {
                        Success = false,
                        Message = "Localização não encontrada",
                        Data = null
                    };
                }

                updatedEquipamento.Nome = equipamento.Nome;
                updatedEquipamento.Patrimonio = equipamento.Patrimonio;
                updatedEquipamento.Local = novoLocal;

                await context.SaveChangesAsync();

                return new ApiResponse<Equipamento>
                {
                    Success = true,
                    Message = "Localização editada com sucesso",
                    Data = equipamento
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Equipamento>
                {
                    Success = false,
                    Message = $"Erro ao editar localização: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
