/*
 * Creado por SharpDevelop.
 * Usuario: usuario
 * Fecha: 17/4/2026
 * Hora: 10:53 a. m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.IO;
using System.Text;

namespace TallerIUJO
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE GESTIÓN EDUCATIVA IUJO - INICIADO ===\n");

            string registroBruto = " ID_777 ; Jose Yepez ; EXAMEN_FINAL.PDF ; 95 ";
            string dataLimpia = registroBruto.Trim();
            string[] partes = dataLimpia.Split(';');
            
            string id = partes[0].Trim();
            string nombre = partes[1].Trim().ToUpper();
            string archivo = partes[2].Trim().ToLower();
            string nota = partes[3].Trim();

            string rutaRaiz = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DatosIUJO");
            string rutaReportes = Path.Combine(rutaRaiz, "Reportes");
            
            if (!Directory.Exists(rutaReportes))
            {
                Directory.CreateDirectory(rutaReportes);
                Console.WriteLine("> Carpeta de reportes creada exitosamente.");
            }

            // Persistencia de texto
            string archivoTexto = Path.Combine(rutaReportes, "notas.txt");
            using (StreamWriter sw = new StreamWriter(archivoTexto, true))
            {
                sw.WriteLine(string.Format("{0} ESTUDIANTE: {1} | NOTA: {2}", 
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm"), nombre, nota));
            }
            Console.WriteLine("> Registro de texto persistido en disco.");

            // Persistencia binaria
            string archivoBin = Path.Combine(rutaRaiz, "auditoria.dat");
            using (FileStream fs = new FileStream(archivoBin, FileMode.Create, FileAccess.Write))
            {
                byte[] bytesID = Encoding.UTF8.GetBytes(id + "|");
                fs.Write(bytesID, 0, bytesID.Length);
            }
            Console.WriteLine("> Auditoría binaria generada.");

            // Lectura secuencial
            Console.WriteLine("\n>> Contenido actual del Reporte:");
            if (File.Exists(archivoTexto))
            {
                using (StreamReader sr = new StreamReader(archivoTexto))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("   LÍNEA: " + linea);
                    }
                }
            }

            // Menu para los desafios
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n===== TALLER PRÁCTICO: DESAFÍOS =====");
                Console.WriteLine("1. Validador de Seguridad (Desafío 1)");
                Console.WriteLine("2. Clonador de Imágenes (Desafío 2)");
                Console.WriteLine("3. Buscador de Archivos Pesados (Desafío 3)");
                Console.WriteLine("4. Salir");
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Desafio1_ValidadorSeguridad();
                        break;
                    case "2":
                        Desafio2_ClonadorImagenes();
                        break;
                    case "3":
                        Desafio3_BuscadorArchivosPesados();
                        break;
                    case "4":
                        salir = true;
                        Console.WriteLine("Saliendo del programa...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
            }

            Console.WriteLine("\n=== PROCESO FINALIZADO. Presione una tecla para salir. ===");
            Console.ReadLine();
        }

        static void Desafio1_ValidadorSeguridad()
        {
            Console.Write("\n[Desafío 1] Ingrese usuario y clave (formato: usuario;clave): ");
            string entrada = Console.ReadLine();
            string[] partes = entrada.Split(';');

            if (partes.Length == 2)
            {
                string usuario = partes[0].Trim();
                string clave = partes[1].Trim();

                if (clave.Contains("123"))
                {
                    string rutaSeguridad = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "seguridad.txt");
                    using (StreamWriter sw = new StreamWriter(rutaSeguridad, true))
                    {
                        sw.WriteLine(string.Format("{0}: Clave débil detectada para el usuario '{1}'", 
                            DateTime.Now, usuario));
                    }
                    Console.WriteLine("¡ADVERTENCIA! Clave débil. Registro guardado en seguridad.txt");
                }
                else
                {
                    Console.WriteLine("Clave aceptable. No se generó alerta.");
                }
            }
            else
            {
                Console.WriteLine("Formato incorrecto. Debe ser: usuario;clave");
            }
        }

        static void Desafio2_ClonadorImagenes()
        {
            Console.Write("\n[Desafío 2] Ingrese el nombre de la imagen a copiar (ej. avatar.jpg): ");
            string origen = Console.ReadLine().Trim();
            
            if (!File.Exists(origen))
            {
                Console.WriteLine(string.Format("Error: El archivo '{0}' no existe.", origen));
                return;
            }

            string destino = "respaldo_" + Path.GetFileName(origen);

            try
            {
                using (FileStream fsOrigen = new FileStream(origen, FileMode.Open, FileAccess.Read))
                using (FileStream fsDestino = new FileStream(destino, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[1024];
                    int bytesLeidos;
                    long totalBytes = 0;

                    while ((bytesLeidos = fsOrigen.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fsDestino.Write(buffer, 0, bytesLeidos);
                        totalBytes += bytesLeidos;
                    }
                    Console.WriteLine(string.Format("Copia completada: {0} ({1} bytes copiados)", destino, totalBytes));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error al copiar: {0}", ex.Message));
            }
        }

        static void Desafio3_BuscadorArchivosPesados()
        {
            Console.Write("\n[Desafío 3] Ingrese la ruta de la carpeta (vacío para actual): ");
            string carpeta = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(carpeta)) carpeta = ".";

            if (!Directory.Exists(carpeta))
            {
                Console.WriteLine("La carpeta especificada no existe.");
                return;
            }

            string[] archivos = Directory.GetFiles(carpeta);
            int eliminados = 0;
            long limite = 5 * 1024; // 5 KB

            foreach (string archivo in archivos)
            {
                FileInfo info = new FileInfo(archivo);
                if (info.Length > limite)
                {
                    Console.WriteLine(string.Format("Eliminando: {0} - Tamaño: {1} bytes", info.Name, info.Length));
                    try
                    {
                        File.Delete(archivo);
                        eliminados++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("No se pudo eliminar {0}: {1}", info.Name, ex.Message));
                    }
                }
            }
            Console.WriteLine(string.Format("Proceso finalizado. {0} archivo(s) eliminado(s).", eliminados));
        }
    }
}
