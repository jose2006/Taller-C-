/*
 * Creado por SharpDevelop.
 * Usuario: usuario
 * Fecha: 17/4/2026
 * Hora: 10:53 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;

namespace TallerIUJO01
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("taller 01");
			
			string registroUsuario = " ID_777; juan; evaluacion; 95 ";
			
			
			Console.WriteLine(registroUsuario);
			string registroLimpio = registroUsuario.Trim();
			Console.WriteLine(registroLimpio);
			
			string[] partes = registroLimpio.Split(';');
			string id = partes[0].Trim();
			string nombre = partes[1].Trim();
			string tarea = partes[2].Trim();
			string nota = partes[3].Trim();
			
			
			Console.WriteLine(string.Format(" el id es: {0} del usuario {1} en la tarea {2} con la nota {3}",id,nombre,tarea,nota));
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}