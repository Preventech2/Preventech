using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Preventech.Core.Models;
using Preventech.Core.DatabaseContexts;
using Preventech.Core.DTOs;
using Preventech.Core.Extensions;

namespace Preventech.Core.Controllers
{
    [Route("api/localizacao")]
    [ApiController]
    public class LocalizacaoController(ApplicationDbContext context) : ControllerBase
    {


        [HttpGet("timestamp")]
        public async Task<DateTime> GetUltimaAtualizacao()
        {
            try 
            {
                var query = await context.Atualizacoes
                    .Where(x => x.Id == IndiceAtualizacao.Localizacao)
                    .Select(x => x.Ultima)
                    .FirstOrDefaultAsync();
                return DateTime.SpecifyKind(query, DateTimeKind.Utc);
            } catch (Exception) {
                return DateTime.UnixEpoch;
            }
        }

        /// <summary>
        /// Handler GET para localizações com filtro, valores inválidos 
        /// são wildcards
        /// </summary>
        /// <param name="filtro">Filtro para localizações</param>
        /// <returns>Lista de localizações que condizem ao filtro</returns>
        [HttpGet]
        public async Task<string> GetLocalizacoes([FromQuery] Localizacao filtro, [FromQuery] DateTime atualizacao)
        {
            atualizacao = DateTime.SpecifyKind(atualizacao, DateTimeKind.Utc);
            try
            {
                var query = context.Localizacoes
                    .Include(loc => loc.Responsavel)
                    .AsQueryable();

                query = query.Where(q => q.AtualizadoEm >= atualizacao);

                if (filtro.Campus > 0) 
                    query = query.Where(q => q.Campus == filtro.Campus);
                
                if (filtro.Predio > 0)
                    query = query.Where(q => q.Predio == filtro.Predio);

                if (filtro.Andar > 0)
                    query = query.Where(q => q.Andar == filtro.Andar);

                if (filtro.Numero > 0)
                    query = query.Where(q => q.Numero == filtro.Numero);

                if (!string.IsNullOrWhiteSpace(filtro.Apelido))
                    query = query.Where(q => q.Apelido == filtro.Apelido);

                if (!query.Any()) return string.Empty;

                var res = await query.ToListAsync();

                return LocalizacaoExtension.Compactar(res, filtro, atualizacao);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Handler POST para criação de novas localizações
        /// </summary>
        /// <param name="localizacao">Localização a ser adicionada</param>
        /// <returns>Localização que foi adicionada</returns>
        [HttpPost]
        public async Task<ApiResponse<Localizacao>> CadastrarLocalizacao([FromBody] Localizacao localizacao)
        {
            localizacao.AtualizadoEm = DateTime.Now;
            try
            {
                localizacao.Responsavel = (await context.Usuarios.FindAsync(localizacao.Responsavel.Id))!;
                await context.Localizacoes.AddAsync(localizacao);

                var atualizacao = await context.Atualizacoes
                    .FindAsync(IndiceAtualizacao.Localizacao).AsTask()
                    .ContinueWith(x => x.Result!.Ultima = DateTime.UtcNow);

                await context.SaveChangesAsync();
                return new ApiResponse<Localizacao>
                {
                    Success = true,
                    Message = "Localização cadastrada com sucesso",
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

        /// <summary>
        /// Handler PATCH para edição de uma localização
        /// </summary>
        /// <param name="localizacao">localização para ser editada</param>
        /// <returns>Localização editada</returns>
        [HttpPatch]
        public async Task<ApiResponse> EditarLocalizacao([FromBody] Localizacao localizacao)
        {
            localizacao.AtualizadoEm = DateTime.Now;
            try
            {
                var locExistente = await context.Localizacoes
                    .Include(loc => loc.Responsavel)
                    .FirstOrDefaultAsync(loc => loc.Id == localizacao.Id);

                if (locExistente == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Localização não encontrada"
                    };
                }

                var locResponsavel = await context.Usuarios
                    .FindAsync(localizacao.Responsavel.Id);

                if (locResponsavel == null)
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Responsável não encontrado"
                    };
                

                locExistente.Apelido = localizacao.Apelido;
                locExistente.Campus = localizacao.Campus;
                locExistente.Predio = localizacao.Predio;
                locExistente.Andar = localizacao.Andar;
                locExistente.Numero = localizacao.Numero;
                locExistente.Responsavel = locResponsavel;

                var atualizacao = await context.Atualizacoes
                    .FindAsync(IndiceAtualizacao.Localizacao).AsTask()
                    .ContinueWith(x => x.Result!.Ultima = DateTime.UtcNow);
            

                await context.SaveChangesAsync();

                return new ApiResponse
                {
                    Success = true,
                    Message = "Localização editada com sucesso",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Erro ao editar localização: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Handler DELETE para localizações
        /// </summary>
        /// <param name="localizacao">Localização para deletar</param>
        /// <returns>Localização deletada</returns>
        [HttpDelete]
        public async Task<ApiResponse> RemoverLocalização([FromBody] Localizacao localizacao)
        {
            localizacao.AtualizadoEm = DateTime.Now;
            try
            {
                await context.Localizacoes
                    .Where(loc => loc == localizacao)
                    .ExecuteDeleteAsync();

                return new ApiResponse
                {
                    Success = true,
                    Message = "Localização deletada com sucesso"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Erro ao deletar localização: {ex.Message}"
                };
            }
        }
    }
}
