using System;
using System.Threading.Tasks;
using EUREKABANK_SOAP_DOTNET_CON.Controller;
class Program
{
    static async Task Main(string[] args)
    {
        var controller = new Controller();
        await controller.RunAsync();
    }
}