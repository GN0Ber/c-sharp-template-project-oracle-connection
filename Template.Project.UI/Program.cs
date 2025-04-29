using System;
using Template.Project.Controller;
using Template.Project.Model;

namespace Template.Project.UI
{
    class Program
    {
        static void Main()
        {
            User currentUser = TryLogin(maxAttempts: 3);
            if (currentUser == null)
            {
                Console.WriteLine("Número máximo de tentativas excedido. Programa encerrado.");
                return;
            }

            Console.WriteLine($"\nBem‐vindo, {currentUser.FullName}!\n");
            RunMenuLoop(currentUser);
        }

        private static User TryLogin(int maxAttempts)
        {
            var userCtrl = new UserController();
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = ReadPassword();

                User user = userCtrl.Login(username, password);
                if (user != null)
                    return user;

                int remaining = maxAttempts - attempt;
                if (remaining > 0)
                    Console.WriteLine($"Login inválido. Você ainda tem {remaining} tentativa(s).\n");
            }
            return null;
        }

        private static void RunMenuLoop(User currentUser)
        {
            var carCtrl = new CarController();

            while (true)
            {
                ShowMenu();
                string op = Console.ReadLine();

                if (op == "0") break;
                switch (op)
                {
                    case "1": CreateCar(carCtrl, currentUser); break;
                    case "2": ListCars(carCtrl); break;
                    case "3": EditCar(carCtrl); break;
                    case "4": DeleteCar(carCtrl); break;
                    default:
                        Console.WriteLine("Opção inválida\n");
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("1) Cadastrar carro");
            Console.WriteLine("2) Listar todos");
            Console.WriteLine("3) Editar carro");
            Console.WriteLine("4) Deletar carro");
            Console.WriteLine("0) Sair");
            Console.Write("Opção: ");
        }

        private static void CreateCar(CarController carCtrl, User currentUser)
        {
            var carro = new Car
            {
                Brand = Prompt("Marca: "),
                Model = Prompt("Modelo: "),
                Year = int.Parse(Prompt("Ano: ")),
                Price = decimal.Parse(Prompt("Preço: ")),
                CreatedByUserId = currentUser.Id    // registra quem criou
            };

            carCtrl.Create(carro);
            Console.WriteLine("Carro cadastrado!\n");
        }

        private static void ListCars(CarController carCtrl)
        {
            var todos = carCtrl.ListAll();
            Console.WriteLine("\n--- Lista de Carros ---");
            foreach (var c in todos)
                Console.WriteLine($"{c.Id}: {c.Brand} {c.Model} ({c.Year}) – R$ {c.Price}");
            Console.WriteLine();
        }

        private static void EditCar(CarController carCtrl)
        {
            if (!int.TryParse(Prompt("ID do carro a editar: "), out int id))
            {
                Console.WriteLine("ID inválido!\n"); return;
            }

            Car carro = carCtrl.Get(id);
            if (carro == null)
            {
                Console.WriteLine("Carro não encontrado!\n"); return;
            }

            string tmp;
            tmp = Prompt($"Marca ({carro.Brand}): ");
            if (!string.IsNullOrWhiteSpace(tmp)) carro.Brand = tmp;
            tmp = Prompt($"Modelo ({carro.Model}): ");
            if (!string.IsNullOrWhiteSpace(tmp)) carro.Model = tmp;
            tmp = Prompt($"Ano ({carro.Year}): ");
            if (int.TryParse(tmp, out int y)) carro.Year = y;
            tmp = Prompt($"Preço ({carro.Price}): ");
            if (decimal.TryParse(tmp, out decimal p)) carro.Price = p;

            carCtrl.Update(carro);
            Console.WriteLine("Carro atualizado!\n");
        }

        private static void DeleteCar(CarController carCtrl)
        {
            if (!int.TryParse(Prompt("ID do carro a deletar: "), out int id))
            {
                Console.WriteLine("ID inválido!\n"); return;
            }

            carCtrl.Delete(id);
            Console.WriteLine("Carro deletado!\n");
        }

        private static string ReadPassword()
        {
            var pwd = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if (key == ConsoleKey.Backspace && pwd.Length > 0)
                {
                    pwd = pwd[0..^1];
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    pwd += keyInfo.KeyChar;
                    Console.Write("*");
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return pwd;
        }

        private static string Prompt(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }
    }
}
