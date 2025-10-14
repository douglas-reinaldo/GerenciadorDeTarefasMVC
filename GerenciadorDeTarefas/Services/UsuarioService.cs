using GerenciadorDeTarefas.Data;
using GerenciadorDeTarefas.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace GerenciadorDeTarefas.Services
{
    public class UsuarioService
    {
        private readonly GerenciadorTarefasDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(GerenciadorTarefasDbContext context) 
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public void AdicionarUsuario(Usuario usuario) 
        {
            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, usuario.Senha);

            _context.Add(usuario);
            _context.SaveChanges();
        }

        public Usuario Autenticar(string email, string senha) 
        {
            var usuario = _context.Usuario.FirstOrDefault(n => n.Email == email);
            if (usuario == null) return null;

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senha);
            if (resultado == PasswordVerificationResult.Success) 
            {
                return usuario;
            }
            return null;
        }


        public Usuario obterPorId(int Id) 
        {
            return _context.Usuario.FirstOrDefault(n => n.Id == Id);
        }

        public Usuario obterPorEmail(string email)
        {
            return _context.Usuario.FirstOrDefault(n => n.Email == email);
        }

        public bool SenhaJaExiste(string senha)
        {
            var usuarios = _context.Usuario.ToList();

            foreach (var usuario in usuarios)
            {
                var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senha);
                if (resultado == PasswordVerificationResult.Success)
                {
                    return true;
                }
            }

            return false; 
        }

    }
}
