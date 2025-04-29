using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Template.Project.Model;
using Template.Project.Repository;

namespace Template.Project.Controller
{
    public class CarController
    {
        private readonly CarRepository _repo;

        public CarController()
        {
            _repo = new CarRepository();
        }

        public void Create(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            if (car.CreatedByUserId <= 0)
                throw new ArgumentException("É preciso informar o usuário que criou o carro.", nameof(car.CreatedByUserId));

            if (car.Price <= 0)
                throw new ArgumentException("O preço do carro deve ser maior que zero.", nameof(car.Price));

            if (car.Year < 1990)
                throw new InvalidOperationException("Não aceitamos carros com ano anterior a 1990.");

            _repo.Add(car);
        }

        public IEnumerable<Car> ListAll()
        {
            return _repo.GetAll();
        }

        public Car Get(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));

            var car = _repo.GetById(id);
            if (car == null)
                throw new KeyNotFoundException($"Carro com ID {id} não encontrado.");

            return car;
        }

        public void Update(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            if (car.Id <= 0)
                throw new ArgumentException("O ID do carro deve ser válido.", nameof(car.Id));

            _repo.Update(car);
        }

        public void Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.", nameof(id));

            _repo.Delete(id);
        }
    }
}

