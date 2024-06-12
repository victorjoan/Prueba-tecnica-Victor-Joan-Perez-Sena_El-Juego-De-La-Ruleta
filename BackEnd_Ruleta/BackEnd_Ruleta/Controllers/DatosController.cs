using BackEnd_Ruleta.Data;
using BackEnd_Ruleta.Models;
using BackEnd_Ruleta.Recursos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd_Ruleta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatosController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public DatosController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetRandomNumberAndColor")]
        [ActionName("GetRandomNumberAndColor")]
        public ActionResult GetRandomNumberAndColor()
        {
            try
            {
                Random random = new Random();
                int number = random.Next(0, 36);
                var ele = Elements.elements.FirstOrDefault(a => a.Key == number);

                return Ok(new { Data = new { Number = ele.Key, Color = ele.Value } });

            }
            catch (Exception exec)
            {

                return StatusCode(500, $"Inconveniente. Detalles: {exec.Message}");
            }
        }

        [HttpGet("GetUser")]
        [ActionName("GetUser")]
        public ActionResult GetUser(string nombreU)
        {
            try
            {
                var usuarios = _context.usuarios.ToList();
                if (usuarios.Count < 1)
                {
                    return NotFound();
                }

                var usuario = usuarios.Where(x => x.Nombre.ToUpper().Trim() == nombreU.ToUpper().Trim()).FirstOrDefault();
                return Ok(new { Data = usuario });
            }
            catch (Exception exec)
            {
                return StatusCode(500, $"Inconveniente. Detalles: {exec.Message}");
            }
        }

        [HttpPost("AddUser")]
        [ActionName("AddUser")]
        public ActionResult AddUser([FromBody] usuario usuario)
        {
            try
            {

                if (_context.usuarios.Where(a => a.Nombre.ToUpper().Trim() == usuario.Nombre.ToUpper().Trim()).ToList().Count() > 0)
                {
                    var userSaved = _context.usuarios.ToList().Where(a => a.Nombre.ToUpper().Trim() == usuario.Nombre.ToUpper().Trim()).FirstOrDefault();
                    if (userSaved != null)
                    {

                        userSaved.Monto = usuario.Monto;
                        _context.Entry(userSaved).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
                else
                {
                    _context.usuarios.Add(new usuario()
                    {
                        Nombre = usuario.Nombre,
                        Monto = usuario.Monto,
                    });
                    _context.SaveChanges();
                }
                return Ok();
            }
            catch (Exception exec)
            {
                return StatusCode(500, $"Inconveniente. Detalles: {exec.Message}");
            }
        }

        [HttpGet("GetMontoApuesta")]
        [ActionName("GetMontoApuesta")]
        public ActionResult GetMontoApuesta(int TipoApuesta, int Monto, string Color, string TipoNumber, int? Numero, int RandomNumber)
        {
            try
            {
                var ele = Elements.elements.FirstOrDefault(a => a.Key == RandomNumber);
                double Resultado = 0;
                bool gano = false;
                string message = "";

                if (Monto < 0 || string.IsNullOrWhiteSpace(Color))
                {
                    message = "El monto debe ser mayor que 0 y el color es obligatorio";
                    return BadRequest(new { message });
                }

                switch (TipoApuesta)
                {
                    case 1:
                        // Apuesta al color
                        if (Color == ele.Value)
                        {
                            Resultado = Monto / 2.0;
                            gano = true;
                        }
                        else
                        {
                            Resultado = -Monto; // Pierde el monto apostado
                        }
                        break;

                    case 2:
                        // Apuesta a pares o impares de un color dado
                        if (TipoNumber.ToUpper() != "PAR" && TipoNumber.ToUpper() != "IMPAR")
                        {
                            message = "Tipo de número debe ser 'PAR' o 'IMPAR'";
                            return BadRequest(new { message });
                        }

                        if (Color == ele.Value)
                        {
                            if (TipoNumber.ToUpper() == "PAR" && ele.Key % 2 == 0)
                            {
                                Resultado = Monto;
                                gano = true;
                            }
                            else if (TipoNumber.ToUpper() == "IMPAR" && ele.Key % 2 != 0)
                            {
                                Resultado = Monto;
                                gano = true;
                            }
                            else
                            {
                                Resultado = -Monto; // Pierde el monto apostado
                            }
                        }
                        else
                        {
                            Resultado = -Monto; // Pierde el monto apostado
                        }
                        break;

                    case 3:
                        // Apuesta a un número y color específicos
                        if (Numero < 0 || Numero > 36)
                        {
                            message = "El número de la apuesta está fuera de rango";
                            return BadRequest(new { message });
                        }

                        if (Color == ele.Value && Numero == ele.Key)
                        {
                            Resultado = Monto * 3.0;
                            gano = true;
                        }
                        else
                        {
                            Resultado = -Monto; // Pierde el monto apostado
                        }
                        break;

                    default:
                        message = "Tipo de apuesta no válido";
                        return BadRequest(new { message });
                }

                return Ok(new { Gano = gano, Ganancia = Math.Abs(Resultado) });
            }
            catch (Exception exec)
            {
                return StatusCode(500, $"Inconveniente. Detalles: {exec.Message}");
            }
        }


    }
}
