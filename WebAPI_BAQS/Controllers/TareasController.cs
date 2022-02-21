using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.DTOs;
using WebAPI_BAQS.DTOs.Tarea;
using WebAPI_BAQS.Entities;
using WebAPI_BAQS.Helpers;

namespace WebAPI_BAQS.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public TareasController(ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = mapper;
        }


        //POST: api/tareas/crear
        [HttpPost("crear")]
        public async Task<ActionResult> Crear(TareaCreacionDTO tareaCreacionDTO)
        {

            Tarea tarea = new()
            {
                IdCabecera = tareaCreacionDTO.IdCabecera,
                Filtro = tareaCreacionDTO.Filtro,
                Nombre = tareaCreacionDTO.Nombre,
                Descripcion = tareaCreacionDTO.Descripcion,
                Fecha = tareaCreacionDTO.Fecha,
                Hora = tareaCreacionDTO.Hora,
                Intervalo = tareaCreacionDTO.Intervalo,
                Ultima_ejecucion = null,
                Ejecucion = $"A las {tareaCreacionDTO.Hora} cada {tareaCreacionDTO.Intervalo} días",
                Estado = true
            };

            context.Add(tarea);

            await context.SaveChangesAsync();



            bool result = CreateTaskScheduler(tareaCreacionDTO.Nombre, tareaCreacionDTO.Descripcion, tareaCreacionDTO.Fecha, tareaCreacionDTO.Hora, tareaCreacionDTO.Intervalo, tarea.IdTarea, tareaCreacionDTO.Filtro);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Error al crear tarea.");
        }


        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Editar([FromRoute] int id, [FromBody] TareaCreacionDTO tareaCreacionDTO)
        {
            var tarea = await context.Tareas.FirstOrDefaultAsync(x => x.IdTarea == id);

            if (tarea == null)
            {
                return NotFound();
            }

            tarea.IdCabecera = tareaCreacionDTO.IdCabecera;
            tarea.Filtro = tareaCreacionDTO.Filtro;
            tarea.Nombre = tareaCreacionDTO.Nombre;
            tarea.Descripcion = tareaCreacionDTO.Descripcion;
            tarea.Fecha = tareaCreacionDTO.Fecha;
            tarea.Hora = tareaCreacionDTO.Hora;
            tarea.Intervalo = tareaCreacionDTO.Intervalo;
            tarea.Ejecucion = $"A las {tareaCreacionDTO.Hora} cada {tareaCreacionDTO.Intervalo} días";


            await context.SaveChangesAsync();

            bool result = CreateTaskScheduler(tareaCreacionDTO.Nombre, tareaCreacionDTO.Descripcion, tareaCreacionDTO.Fecha, tareaCreacionDTO.Hora, tareaCreacionDTO.Intervalo, tarea.IdTarea, tareaCreacionDTO.Filtro);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Error al actualizar tarea.");


        }


        //GET: api/tareas/tareasPaginacion
        [HttpGet("tareasPaginacion")]
        public async Task<ActionResult<List<TareaDTO>>> TareasPaginacion([FromQuery] PaginacionDTO paginacionDTO)
        {
            var tareas = await context.Tareas.Include(x => x.BaqCabecera).ToListAsync();

            int cantidad = tareas.Count;

            HttpContext.InsertarParametrosPaginacionEnCabeceraPersonalizado(cantidad);

            var queryable = tareas.AsQueryable();

            var tareasPaginacion = queryable.Paginar(paginacionDTO).ToList();

            return tareasPaginacion.Select(x => new TareaDTO
            {
                IdTarea = x.IdTarea,
                IdCabecera = x.IdCabecera,
                NombreBAQ = x.BaqCabecera.Nombre,
                Filtro = x.Filtro,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Fecha = x.Fecha,
                Hora = x.Hora,
                Intervalo = x.Intervalo,
                Ultima_ejecucion = x.Ultima_ejecucion,
                Ejecucion = x.Ejecucion,
                Estado = x.Estado

            }).ToList();

        }


        //GET: api/tareas/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TareaEditarDTO>> Get([FromRoute] int id)
        {
            var tarea = await context.Tareas.FirstOrDefaultAsync(x => x.IdTarea == id);

            if (tarea == null)
            {
                return NotFound();
            }

            return mapper.Map<TareaEditarDTO>(tarea);

        }

        //GET: api/tareas/ejecutar/{id}
        [HttpGet("ejecutar/{filtro}/{idTarea:int}")]
        public async Task<ActionResult> Ejecutar([FromRoute] string filtro, int idTarea)
        {
            ////

            ///
            //tarea
            var tarea = await context.Tareas.FirstOrDefaultAsync(x => x.IdTarea == idTarea);
            if (tarea == null)
            {
                return NotFound();
            }

            //de acuerdo al filtro filtrar api y consumir
            var baq = await context.BaqCabeceras.FirstOrDefaultAsync(x => x.IdCabecera == tarea.IdCabecera);

            //lista de columnas
            var baqDetalles = await context.BaqDetalles.Where(x => x.IdCabecera == tarea.IdCabecera).ToListAsync();

            if (baq == null)
            {
                return NotFound();
            }

            //filtro de URL
            if (filtro == "General")
            {

            }

            else if (filtro == "Mensual")
            {
                //Calular filtro mes
            }
            else if (filtro == "Semanal")
            {
                //Calcular filtro semanal

            }
            else if (filtro == "Diario")
            {
                //Calcular filtro diario

            }

            //Eliminar registros

            //Guardar Datos consumidos
            await SaveGenericBAQ(baq.Nombre, baq._url, baqDetalles);

            //Actualizo la útlima ejecución
            tarea.Ultima_ejecucion = DateTime.Now;

            await context.SaveChangesAsync();

            //SaveBAQ(baq.Nombre, baq._url, baqDetalles);

            return NoContent();
        }

        //PUT: api/tareas/desactivar/{id}
        [HttpPut("desactivar/{id:int}")]
        public async Task<ActionResult> Desactivar(int id)
        {
            var tarea = await context.Tareas.FirstOrDefaultAsync(x => x.IdTarea == id);

            if (tarea == null)
            {
                return NotFound();
            }

            tarea.Estado = false;

            await context.SaveChangesAsync();

            return NoContent();
        }

        //PUT: api/tareas/activar/{id}
        [HttpPut("activar/{id:int}")]
        public async Task<ActionResult> Activar(int id)
        {
            var tarea = await context.Tareas.FirstOrDefaultAsync(x => x.IdTarea == id);

            if (tarea == null)
            {
                return NotFound();
            }

            tarea.Estado = true;

            await context.SaveChangesAsync();

            return NoContent();
        }


        //DELETE: api/tareas/eliminar/{nombre}
        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> Eliminar([FromRoute] int id)
        {

            var tarea = await context.Tareas.FirstOrDefaultAsync(x => x.IdTarea == id);

            if (tarea == null)
            {
                return NotFound();
            }


            using TaskService ts = new();
            if (ts.GetTask(tarea.Nombre) != null)
            {

                ts.RootFolder.DeleteTask(tarea.Nombre);

                context.Remove(tarea);
                await context.SaveChangesAsync();

                return NoContent();

            }

            //Eliminar archivo --pendiente eliminar

            return NotFound();

        }

        //PUT: api/tareas/activar/{nombre}
        [HttpDelete("activar/{nombre}")]
        public ActionResult Activar(string nombre)
        {
            using TaskService ts = new();

            if (ts.GetTask(nombre) != null)
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(nombre);

                TaskDefinition td = task.Definition;

                td.Settings.Enabled = true;

                return NoContent();
            }

            return NotFound();
        }


        //PUT: api/tareas/desactivar/{nombre}
        [HttpDelete("desactivar/{nombre}")]
        public ActionResult Desactivar(string nombre)
        {
            using TaskService ts = new();

            if (ts.GetTask(nombre) != null)
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(nombre);

                TaskDefinition td = task.Definition;

                td.Settings.Enabled = false;


                return NoContent();
            }

            return NotFound();
        }


        //GET api/tareas/correo
        [HttpGet("correo")]
        public ActionResult Correo()
        {
            CorreoNotificacion.SendMail("Prueba", "Este es un correo de prueba.");

            return NoContent();
        }





        private bool CreateTaskScheduler(string nombre, string descripcion, DateTime fecha, string hora, int intervalo, int id, string filtro)
        {
            try
            {
                // Get the service on the local machine
                using (TaskService ts = new TaskService())
                {
                    // Create a new task definition and assign properties
                    TaskDefinition td = ts.NewTask();

                    td.RegistrationInfo.Description = descripcion;

                    // Create a trigger that will fire the task at this time every other day

                    DateTime fechaInicio = Convert.ToDateTime(fecha.ToShortDateString() + " " + hora);

                    //td.Triggers.Add(new DailyTrigger { StartBoundary = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " 10:10:00"), DaysInterval = 2 });
                    td.Triggers.Add(new DailyTrigger { StartBoundary = fechaInicio, DaysInterval = Convert.ToInt16(intervalo) });

                    //td.Triggers.Add(new MonthlyTrigger { DaysOfMonth = 1,mon MonthsOfTheYear.AllMonths });

                    //--Generar script para powershell
                    //--mandar argumentos el script
                    string path = CreateScript(id, filtro);
                    // Create an action that will launch Notepad whenever the trigger fires
                    //td.Actions.Add(new ExecAction("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", "-command \"C:\\Scripts\\JobMaterials\\JobMaterials_Daily.ps1\"", null));
                    td.Actions.Add(new ExecAction("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", $"-command \"{path}\"", null));

                    // Register the task in the root folder
                    ts.RootFolder.RegisterTaskDefinition(@$"{nombre}", td);

                    // Remove the task we just created
                    //ts.RootFolder.DeleteTask("Test");

                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private string CreateScript(int idTarea, string filtro)
        {

            string path = $"{configuration.GetValue<string>("path1")}\\{ Guid.NewGuid()}.ps1";
            string endpoint = configuration.GetValue<string>("endpoint");

            if (!System.IO.File.Exists(path))
            {
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine($"$response = Invoke-RestMethod \'{endpoint}/api/tareas/ejecutar/{filtro}/{idTarea}\'");
                };

                return path;
            }

            return null;

        }

        private async System.Threading.Tasks.Task SaveBAQ(string nombreBAQ, string url, List<BaqDetalle> baqDetalles)
        {
            try
            {
                var client = new RestClient(url);

                client.Authenticator = new NtlmAuthenticator(configuration.GetValue<string>("email"), configuration.GetValue<string>("password"));
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Accept", "application/json");

                IRestResponse response = await client.ExecuteAsync(request);
                var content = response.Content;
                int index = content.IndexOf(Environment.NewLine);
                string newText = content.Substring(index + Environment.NewLine.Length);
                int index2 = newText.IndexOf(Environment.NewLine);
                string newText2 = newText.Substring(index2 + Environment.NewLine.Length);
                newText2 = newText2.Remove(newText2.LastIndexOf(Environment.NewLine));
                newText2 = "[" + newText2.Replace(Environment.NewLine, "");

                DataTable dsTopics = JsonConvert.DeserializeObject<DataTable>(newText2);

                //si contiene registros
                if (dsTopics.Rows.Count > 0)
                {
                    SqlConnection cn = new SqlConnection(configuration.GetValue<string>("connectionDB"));
                    SqlBulkCopy objBulk = new SqlBulkCopy(cn);
                    objBulk.DestinationTableName = nombreBAQ;


                    foreach (var columna in baqDetalles)
                    {
                        objBulk.ColumnMappings.Add(columna.NombreColumna, columna.NombreColumna);
                    }

                    await cn.OpenAsync();
                    await objBulk.WriteToServerAsync(dsTopics);
                    await cn.CloseAsync();

                    //UpdateDate.updateJobs(3);
                    //SuccessfulLog.SaveFileJobMaterials("Job materials");
                    //InsertLog.Insert("Job materials", option, startDate, finalDate, "Successful at " + DateTime.Now, "Successful registration.");
                }
                else
                {
                    //UpdateDate.updateJobs(3);
                    //SuccessfulLog.SaveFileJobMaterials("Job materials");
                    //InsertLog.Insert("Job materials", option, startDate, finalDate, "Successful at " + DateTime.Now, "Empty data table.");
                }

            }
            catch (Exception ex)
            {
                //ErrorLog.SaveFileJobMaterials("Job Materials", ex);
                //ErrorLog.SendMail("Job Materials", ex);
                //InsertLog.Insert("Job materials", option, startDate, finalDate, "Registration failed at " + DateTime.Now, "Error: " + ex.Message);
            }
        }

        private async System.Threading.Tasks.Task SaveGenericBAQ(string nombreBAQ, string url, List<BaqDetalle> baqDetalles)
        {

            //UserDialogs.Instance.ShowLoading("Conectando...");
            var client = new RestClient(url);
            //client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", UserData.token));
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = await client.ExecuteAsync(request);

            var statusCode = response.StatusDescription;

            DataTable dsTopics = JsonConvert.DeserializeObject<DataTable>(response.Content);

            //si contiene registros
            if (dsTopics.Rows.Count > 0)
            {
                try
                {
                    SqlConnection cn = new SqlConnection(configuration.GetValue<string>("connectionDB"));
                    SqlBulkCopy objBulk = new SqlBulkCopy(cn);
                    objBulk.DestinationTableName = nombreBAQ;

                    foreach (var columna in baqDetalles)
                    {
                        objBulk.ColumnMappings.Add(columna.NombreColumna, columna.NombreColumna);
                    }

                    cn.Open();
                    objBulk.WriteToServer(dsTopics);
                    cn.Close();
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

            //var objAnticiposList = new List<AnticipoPorComprobar>();

            //if (statusCode == "OK")
            //{
            //    //var objAnticiposList = new List<AnticipoPorComprobar>();
            //    objAnticiposList = JsonConvert.DeserializeObject<List<AnticipoPorComprobar>>(response.Content);


            //    return Tuple.Create<bool, IList<AnticipoPorComprobar>, string>(true, objAnticiposList, null);
            //}

        }
    }
}
