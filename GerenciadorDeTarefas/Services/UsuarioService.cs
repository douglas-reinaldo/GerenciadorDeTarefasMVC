using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace GerenciadorDeTarefas.Services
{
    public class UsuarioService
    {
        private readonly GerenciadorTarefasDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(GerenciadorTarefasDbContext context, ILogger<UsuarioService> logger)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
            _logger = logger;
        }

        public void AdicionarUsuario(Usuario usuario)
        {
            try
            {
                _logger.LogInformation("Criando novo usuário com email {Email}", usuario.Email);

                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, usuario.Senha);
                _context.Add(usuario);
                _context.SaveChanges();

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



        public Usuario Autenticar(string email, string senha)
        {
            try
            {
                _logger.LogInformation("Tentativa de login para {Email}", email);

                var usuario = obterPorEmail(email);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao autenticar usuário {Email}", email);
                return null;
            }
        }



        public Usuario obterPorId(int Id)
        {
            return _context.Usuario.FirstOrDefault(n => n.Id == Id);
        }

        public Usuario obterPorEmail(string email)
        {
            return _context.Usuario.FirstOrDefault(n => n.Email == email);
        }

       

    }
}
