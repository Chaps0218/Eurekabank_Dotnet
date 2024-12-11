using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EUREKABANK_SOAP_DOTNET_CON.View
{
    public class Views
    {
        public void DisplayWelcome()
        {
            Console.WriteLine("===== Bienvenidos al banco Monster =====");
        }

        public void DisplayMenu()
        {
            Console.WriteLine("\nMAIN MENU:");
            Console.WriteLine("1. Deposito");
            Console.WriteLine("2. Retiro");
            Console.WriteLine("3. Transferencia");
            Console.WriteLine("4. Revisar Movimientos");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");
        }

        public string[] GetLoginCredentials()
        {
            Console.Write("Ingrese Usuario: ");
            string username = Console.ReadLine();
            Console.Write("Ingrese contraseña: ");
            string password = Console.ReadLine();
            return new[] { username, password };
        }

        public string GetAccountCode(string transactionType)
        {
            Console.Write($"Enter the account code for {transactionType}: ");
            return Console.ReadLine();
        }

        public double GetAmount(string transactionType)
        {
            while (true)
            {
                Console.Write($"Enter {transactionType} amount: ");
                if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
                {
                    return amount;
                }
                Console.WriteLine("Invalid amount. Please try again.");
            }
        }

        public string GetDestinationAccount()
        {
            Console.Write("Enter destination account code: ");
            return Console.ReadLine();
        }

        public void DisplayMovements(ServiceReference1.CustomMovimientoModel[] movements)
        {
            Console.WriteLine("\n===== Account Movements =====");
            Console.WriteLine(" {0,-15} {1,-15} {2,-15} {3,-15} {4,-15}",
                "Movement #", "Date", "Employee", "Type", "Amount");
            Console.WriteLine(new string('-', 90));
            foreach (var movement in movements)
            {
                string typeText = "";
                switch (movement.CodigoTipoMovimiento)
                {
                    case "003":
                        typeText = "DEPOSIT";
                        break;
                    case "001":
                        typeText = "ACCOUNT CREATION";
                        break;
                    case "004":
                        typeText = "WITHDRAWAL";
                        break;
                    case "008":
                        typeText = "OUTGOING TRANSFER";
                        break;
                    case "009":
                        typeText = "INCOMING TRANSFER";
                        break;
                    default:
                        typeText = movement.CodigoTipoMovimiento;
                        break;
                }

                Console.WriteLine("{0,-15} {1,-20:yyyy-MM-dd} {2,-15} {3,-15} {4,-15:$0.00}",
                    movement.NumeroMovimiento,
                    movement.FechaMovimiento,
                    movement.CodigoEmpleado,
                    typeText,
                    movement.ImporteMovimiento);
            }
        }

        public void DisplayMessage(string message, bool isError = false)
        {
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}