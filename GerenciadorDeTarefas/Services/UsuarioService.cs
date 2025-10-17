using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly PasswordHasher<Usuario> _passwordHasher;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository usuarioRepository, ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = new PasswordHasher<Usuario>();
            _logger = logger;
        }


        public async Task AdicionarUsuarioAsync(Usuario usuario)
        {
            try
            {
                _logger.LogInformation("Criando novo usuário com email {Email}", usuario.Email);

                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, usuario.Senha);
                await _usuarioRepository.AdicionarUsuarioAsync(usuario);
                await _usuarioRepository.SalvarMudancasAsync();

                _logger.LogInformation("Usuário ID {UserId} criado com sucesso", usuario.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro ao salvar usuário com email {Email}", usuario.Email);
                throw new InvalidOperationException("Erro ao salvar usuário. Verifique se o email já está cadastrado.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao adicionar usuário {Email}", usuario.Email);
                throw new InvalidOperationException("Erro inesperado ao adicionar usuário.", ex);
            }
        }



        public async Task<Usuario?> Autenticar(string email, string senha)
        {
            try
            {
                _logger.LogInformation("Tentativa de login para {Email}", email);

                var usuario = await ObterPorEmailAsync(email);
                if (usuario == null)
                {
                    _logger.LogWarning("Tentativa de login com email não cadastrado: {Email}", email);
                    return null;
                }

                var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senha);
                if (resultado == PasswordVerificationResult.Success)
                {
                    _logger.LogInformation("Login bem-sucedido para usuário ID {UserId}", usuario.Id);
                    return usuario;
                }

                _logger.LogWarning("Tentativa de login com senha incorreta para {Email}", email);
                return null;
            }
            catch (InvalidOperationException) 
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao autenticar usuário {Email}", email);
                return null;
            }
        }



        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Tentativa de buscar usuário com ID inválido: {UserId}", id);
                throw new ArgumentException("ID deve ser maior que zero", nameof(id));
            }

            try
            {
                _logger.LogInformation("Buscando usuário por ID {UserId}", id);
                Usuario usuario =  await _usuarioRepository.ObterUsuarioPorIdAsync(id);

                if (usuario == null) 
                {
                    _logger.LogWarning("Usuário não encontrado para ID {UserId}", id);
                    return null;
                }
               
                _logger.LogInformation("Usuário ID {UserId} encontrado: {Email}", id, usuario.Email);
                return usuario;
            }
            catch (DbUpdateException e) 
            {
                _logger.LogError(e, "Não foi possivel obter o usuário pelo {Id}", id);
                throw new InvalidOperationException("Erro ao buscar usuário pelo ID.", e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar usuário ID {UserId}", id);
                throw new InvalidOperationException("Erro inesperado ao buscar usuário.", ex);
            }
        }



        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            if (String.IsNullOrWhiteSpace(email) || !email.Contains('@')) 
            {
                _logger.LogWarning("Tentativa de buscar usuário com email inválido");
                throw new ArgumentException("Email inválido", nameof(email));
            }

            try 
            {
                _logger.LogInformation("Buscando usuário por email {Email}", email);
                Usuario usuario = await _usuarioRepository.ObterUsuarioPorEmailAsync(email);

                if (usuario == null) 
                {
                    _logger.LogInformation("Usuário não encontrado para email {Email}", email);
                    return null;
                }

                _logger.LogInformation("Usuário encontrado: ID {UserId} - {Email}", usuario.Id, email);
                return usuario;
            }
            catch (DbUpdateException e) 
            {
                _logger.LogError(e, "Não foi possivel obter o usuário pelo {Email}", email);
                throw new InvalidOperationException("Erro ao buscar usuário pelo email.", e);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao buscar usuário com email {Email}", email);
                throw;
            }

        }

       

    }
}
