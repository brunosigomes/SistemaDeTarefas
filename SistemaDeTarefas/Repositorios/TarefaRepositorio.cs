using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Repositorios
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly SistemaTarefasDBContext _dbContext;
        public TarefaRepositorio(SistemaTarefasDBContext sistemaTarefasDBContext)
        {
            _dbContext = sistemaTarefasDBContext;
        }

        public async Task<TarefaModel> BuscarPorId(int id)
        {
            TarefaModel tarefa = await _dbContext.Tarefas
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.Id == id);
            
            if (tarefa == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não encontrado no banco de dados.");
            }

            return tarefa;
        }

        public async Task<List<TarefaModel>> BuscarTodasTarefas()
        {
            List<TarefaModel> tarefas = await _dbContext.Tarefas
                .Include(t => t.Usuario)
                .ToListAsync();

            if (tarefas == null)
            {
                throw new Exception("Nenhum registro encontrado no banco de dados.");
            }

            return tarefas;
            
        }

        public async Task<TarefaModel> Adicionar(TarefaModel tarefa)
        {
            await _dbContext.Tarefas.AddAsync(tarefa);
            await _dbContext.SaveChangesAsync();

            return tarefa;
        }

        public async Task<TarefaModel> Atualizar(TarefaModel tarefa, int id)
        {
            TarefaModel tarefaBanco = await _dbContext.Tarefas.FirstOrDefaultAsync(t => t.Id == id);

            if (tarefaBanco == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não encontrado no banco de dados.");
            }

            tarefaBanco.Nome = tarefa.Nome;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Status = tarefa.Status;
            tarefaBanco.UsuarioId = tarefa.UsuarioId;

            _dbContext.Tarefas.Update(tarefaBanco);
            _dbContext.SaveChanges();

            return tarefaBanco;

        }

        public async Task<bool> Apagar(int id)
        {
            TarefaModel tarefa = await _dbContext.Tarefas.FirstOrDefaultAsync(t => t.Id == id);

            if (tarefa == null)
            {
                throw new Exception($"Tarefa para o ID: {id} não encontrado no banco de dados.");
            }

            _dbContext.Tarefas.Remove(tarefa);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
