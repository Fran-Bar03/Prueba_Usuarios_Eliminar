using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace UsuariosEliminar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ruta al ChromeDriver
            string chromeDriverPath = @"C:\Users\Hachi\Downloads\chromedriver-win64";

            // Inicializar el WebDriver
            IWebDriver driver = new ChromeDriver(chromeDriverPath);

            try
            {
                // 1. Navegar al sitio de login
                driver.Navigate().GoToUrl("http://elitefitnesscenter.somee.com/Login.aspx");
                driver.Manage().Window.Maximize();
                
                // 2. Ingresar credenciales y hacer login
                driver.FindElement(By.Id("email")).SendKeys("alexagastelum05@gmail.com");
                driver.FindElement(By.Id("pass")).SendKeys("alexa");
                driver.FindElement(By.Id("btningresar")).Click();
                Thread.Sleep(2000);

                // 3. Navegar a la página de usuarios
                driver.Navigate().GoToUrl("http://elitefitnesscenter.somee.com/Usuarios.aspx");
                Thread.Sleep(2000);

                // 4. Buscar al usuario por correo en el GridView
                IWebElement table = driver.FindElement(By.Id("GridView_Usuarios"));
                var rows = table.FindElements(By.TagName("tr"));

                bool userFound = false;

                foreach (var row in rows)
                {
                    if (row.Text.Contains("EstefiCazo@gmail.com")) // Cambia por el correo que buscas
                    {
                        // Esperar hasta que el enlace "Select" sea clickeable
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                        IWebElement selectButton = row.FindElement(By.LinkText("Select"));

                        // Usar JavaScript para hacer clic
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", selectButton);

                        userFound = true;
                        break;
                    }
                }
                
                if (!userFound)
                {
                    Console.WriteLine("El usuario especificado no fue encontrado.");
                    return;
                }

                // 5. Esperar a que los datos se carguen completamente en el formulario
                WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait2.Until(d => !string.IsNullOrEmpty(d.FindElement(By.Id("tbNombre")).GetAttribute("value")));

                // 6. Validar que los campos del formulario se llenan correctamente
                string nombre = driver.FindElement(By.Id("tbNombre")).GetAttribute("value");
                string apellidoPaterno = driver.FindElement(By.Id("tbaPaterno")).GetAttribute("value");
                string apellidoMaterno = driver.FindElement(By.Id("tbaMaterno")).GetAttribute("value");
                string fechaNacimiento = driver.FindElement(By.Id("tbfNac")).GetAttribute("value");
                string email = driver.FindElement(By.Id("tbEmail")).GetAttribute("value");
                string contrasena = driver.FindElement(By.Id("tbPassword")).GetAttribute("value");
                string celular = driver.FindElement(By.Id("tbCelular")).GetAttribute("value");
                string peso = driver.FindElement(By.Id("tbPeso")).GetAttribute("value");
                string altura = driver.FindElement(By.Id("tbAltura")).GetAttribute("value");
                string tipoUsuario = driver.FindElement(By.Id("tbTipo")).GetAttribute("value");

                if (string.IsNullOrWhiteSpace(nombre) ||
                    string.IsNullOrWhiteSpace(apellidoPaterno) ||
                    string.IsNullOrWhiteSpace(apellidoMaterno) ||
                    string.IsNullOrWhiteSpace(fechaNacimiento) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(contrasena) ||
                    string.IsNullOrWhiteSpace(celular) ||
                    string.IsNullOrWhiteSpace(peso) ||
                    string.IsNullOrWhiteSpace (altura) ||
                    string.IsNullOrWhiteSpace(tipoUsuario))
                {
                    Console.WriteLine("Error: Los datos del formulario no se cargaron correctamente.");
                    return;
                }

                Console.WriteLine("Datos cargados en el formulario:");
                Console.WriteLine($"Nombre: {nombre}");
                Console.WriteLine($"Apellido Paterno: {apellidoPaterno}");
                Console.WriteLine($"Apellido Materno: {apellidoMaterno}");
                Console.WriteLine($"Fecha de nacimiento: {fechaNacimiento}");
                Console.WriteLine($"Email: {email}");
                Console.WriteLine($"contrasena: {contrasena}");
                Console.WriteLine($"Celular: {celular}");
                Console.WriteLine($"Peso: {peso}");
                Console.WriteLine($"Altura: {altura}");
                Console.WriteLine($"Tipo usuario: {tipoUsuario}");

                // 7. Hacer clic en el botón "Borrar"
                IWebElement btnBorrar = driver.FindElement(By.Id("btnBorrar"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBorrar);  // Usar JavaScript para hacer clic
                Thread.Sleep(1000); // Esperar para ver el resultado

                Console.WriteLine("Usuario borrado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error: " + ex.Message);
            }
            finally
            {
                // Cerrar el navegador
                driver.Close();
            }
        }
    }
}
