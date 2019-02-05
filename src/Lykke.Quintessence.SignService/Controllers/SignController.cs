using JetBrains.Annotations;
using Lykke.Quintessence.Domain.Services;
using Lykke.Service.BlockchainApi.Contract.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("api/sign")]
    public class SignController : Controller
    {
        private readonly ISignService _signService;

        public SignController(
            ISignService signService)
        {
            _signService = signService;
        }
        

        [HttpPost]
        public ActionResult<SignedTransactionResponse> SignAsync(
            [FromBody] SignTransactionRequest signRequest)
        {
            var encodedRawTransaction = _signService.SignTransaction
            (
                encodedTxParams: signRequest.TransactionContext,
                privateKey: signRequest.PrivateKeys[0]
            );

            return new SignedTransactionResponse
            {
                SignedTransaction = encodedRawTransaction
            };
        }
    }
}
