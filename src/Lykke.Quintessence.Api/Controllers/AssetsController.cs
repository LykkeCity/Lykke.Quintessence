using JetBrains.Annotations;
using Lykke.Quintessence.Core.Utils;
using Lykke.Quintessence.Domain;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Models;
using Lykke.Service.BlockchainApi.Contract;
using Lykke.Service.BlockchainApi.Contract.Assets;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Quintessence.Controllers
{
    [PublicAPI, Route("/api/assets")]
    public class AssetsController : Controller
    {
        private readonly IAssetService _assetService;

        public AssetsController(
            IAssetService assetService)
        {
            _assetService = assetService;
        }
        
        
        [HttpGet("{assetId}")]
        public ActionResult<AssetResponse> GetAsset(
            AssetRequest request)
        {
            var asset = _assetService.Get();
            
            if (asset.Id == request.AssetId)
            {
                return ConvertToAssetResponse(asset);
            }
            else
            {
                return NoContent();
            }
        }
        
        [HttpGet]
        public ActionResult<PaginationResponse<AssetContract>> GetAssetList(
            PaginationRequest request)
        {
            var assets = new [] { _assetService.Get() };
            
            return new PaginationResponse<AssetContract>
            {
                Continuation = null,
                Items = assets.SelectItems(ConvertToAssetResponse)
            };
        }

        private static AssetResponse ConvertToAssetResponse(
            Asset asset)
        {
            return new AssetResponse
            {
                Accuracy = (int) asset.Accuracy,
                Address = asset.Address,
                AssetId = asset.Id,
                Name = asset.Name
            };
        }
    }
}
