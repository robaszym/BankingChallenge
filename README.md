# BankingChallenge

## Running application
1. Download code (clone repository and pull changes)
2. Open solution in VisualStudio and Run project
3. OR: Open cmd and go to `Danske.BankingChallenge\Danske.BankingChallenge.Services` directory. Run: `dotnet run`

## Testing application
You can test API from Swagger UI, by opening in web browser url: https://localhost:5001/swagger/index.html when application is running.
Alternatively you can use POSTman or other tool for sending requests, including web any browser. Simply open url: https://localhost:5001/api/Loan/Calculate?Amount=500000&Duration=120

_500000_ and _120_ values represent loan parameters. Change them to test API.
