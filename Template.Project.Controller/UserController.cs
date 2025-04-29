using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Template.Project.Model;
using Template.Project.Repository;

namespace Template.Project.Controller
{
    public class UserController
    {
        private readonly UserRepository _repo;

        public UserController()
        {
            _repo = new UserRepository();
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("O nome de usuário não pode ser vazio.", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser vazia.", nameof(password));

            var user = _repo.ValidateLogin(username, password);
            return user;  // null se inválido
        }
    }
}

