using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;
using RpgApi.Models.Enuns;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagensExercicioController : ControllerBase
    {
        private static List<Personagem> personagens = new List<Personagem>()
        {
            new Personagem() { Id = 1, Nome = "Frodo", PontosVida=100, Forca=17, Defesa=23, Inteligencia=33, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 2, Nome = "Sam", PontosVida=100, Forca=15, Defesa=25, Inteligencia=30, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 3, Nome = "Galadriel", PontosVida=100, Forca=18, Defesa=21, Inteligencia=35, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 4, Nome = "Gandalf", PontosVida=100, Forca=18, Defesa=18, Inteligencia=37, Classe=ClasseEnum.Mago },
            new Personagem() { Id = 5, Nome = "Hobbit", PontosVida=100, Forca=20, Defesa=17, Inteligencia=31, Classe=ClasseEnum.Cavaleiro },
            new Personagem() { Id = 6, Nome = "Celeborn", PontosVida=100, Forca=21, Defesa=13, Inteligencia=34, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 7, Nome = "Radagast", PontosVida=100, Forca=25, Defesa=11, Inteligencia=35, Classe=ClasseEnum.Mago }
        };
    
        [HttpGet("GetByNome")]
        public IActionResult GetByNome(string nome)
        {
            List<Personagem> listaBusca = personagens.FindAll(p => p.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (listaBusca.Count > 0)
            {
                return Ok(listaBusca);
            }

            return NotFound("Personagem não encontrado");  
        }

        [HttpGet("GetClerigoMago")]
        public IActionResult GetClerigoMago()
        {
            List<Personagem> lista = personagens.FindAll(p => p.Classe == ClasseEnum.Clerigo || p.Classe == ClasseEnum.Mago);
            return Ok(lista.OrderByDescending(p => p.Inteligencia));
        }

        [HttpGet("GetEstatisticas")]
        public IActionResult GetEstatisticas()
        {
            int quantidade = personagens.Count;
            int somatorioForca = personagens.Sum(p => p.Forca);
            double mediaInteligencia = personagens.Average(p => p.Inteligencia);

            var estatisticas = new
            {
                TotalPersonagens = quantidade,
                SomatorioForca = somatorioForca,
                MediaInteligencia = mediaInteligencia
            };

            return Ok(estatisticas);
        }

        [HttpPost]
        public IActionResult PostValidacao(Personagem novoPersonagem)
        {
            if (novoPersonagem.Forca > 100)
            {
                return BadRequest("A força do personagem não pode ser maior do que 100.");
            }

            personagens.Add(novoPersonagem);
            return Ok(personagens);
        }

        [HttpPost("PostValidacaoMago")]
        public IActionResult PostValidacaoMago(Personagem novoPersonagem)
        {
            if (novoPersonagem.Classe == ClasseEnum.Mago && novoPersonagem.Inteligencia < 35)
            {
                return BadRequest("Personagens da classe Mago precisam ter Inteligência igual ou superior a 35.");
            }

            personagens.Add(novoPersonagem);
            return Ok(personagens);
        }

        [HttpGet("GetByClasse/{classe}")]
        public IActionResult GetByClasse(int classe)
        {
            ClasseEnum classeEnum = (ClasseEnum)classe;
            List<Personagem> lista = personagens.FindAll(p => p.Classe == classeEnum);
            return Ok(lista);
        }
    }
}