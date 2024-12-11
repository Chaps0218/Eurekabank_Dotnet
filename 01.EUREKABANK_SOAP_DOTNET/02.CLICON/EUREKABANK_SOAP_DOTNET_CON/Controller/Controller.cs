using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EUREKABANK_SOAP_DOTNET_CON.Model;
using EUREKABANK_SOAP_DOTNET_CON.Service;
using EUREKABANK_SOAP_DOTNET_CON.View;
namespace EUREKABANK_SOAP_DOTNET_CON.Controller
{
    public class Controller
    {
        private Services _service;
        private Views _view;
        private AccountModel _currentAccount;

        public Controller()
        {
            _service = new Services();
            _view = new Views();
        }

        public async Task RunAsync()
        {
            _view.DisplayWelcome();
            // Login
            while (true)
            {
                string[] credentials = _view.GetLoginCredentials();
                bool loginSuccess = await _service.LoginAsync(credentials[0], credentials[1]);
                if (loginSuccess)
                {
                    _currentAccount = new AccountModel { AccountCode = credentials[0] };
                    break;
                }
                _view.DisplayMessage("Login fallido.", true);
            }

            // Main menu loop
            while (true)
            {
                _view.DisplayMenu();
                string choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "1": // Deposit
                            await PerformTransaction("DEP");
                            break;
                        case "2": // Withdraw
                            await PerformTransaction("RET");
                            break;
                        case "3": // Transfer
                            await PerformTransaction("TRA");
                            break;
                        case "4": // Check Movements
                            await CheckMovements();
                            break;
                        case "5": // Exit
                            _view.DisplayMessage("Thank you for using our banking system. Goodbye!");
                            return;
                        default:
                            _view.DisplayMessage("Invalid option. Please try again.", true);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _view.DisplayMessage($"An error occurred: {ex.Message}", true);
                }
            }
        }

        private async Task PerformTransaction(string transactionType)
        {
            // Get the account code for the transaction
            string accountCode = _view.GetAccountCode(transactionType);

            // Get the transaction amount
            double amount = _view.GetAmount(transactionType);

            // Get destination account for transfers
            string destinationAccount = transactionType == "TRA"
                ? _view.GetDestinationAccount()
                : null;

            // Perform the transaction
            bool success = await _service.PerformTransactionAsync(
                accountCode,
                amount,
                transactionType,
                destinationAccount
            );

            if (success)
            {
                _view.DisplayMessage($"{char.ToUpper(transactionType[0]) + transactionType.Substring(1)} successful!");
            }
            else
            {
                _view.DisplayMessage("Transaction failed.", true);
            }
        }

        private async Task CheckMovements()
        {
            // Get the account code for checking movements
            string accountCode = _view.GetAccountCode("checking movements");

            // Retrieve and display movements for the specified account
            var movements = await _service.GetAccountMovementsAsync(accountCode);
            _view.DisplayMovements(movements);
        }
    }
}