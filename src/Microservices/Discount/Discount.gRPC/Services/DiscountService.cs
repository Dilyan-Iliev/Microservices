using Discount.gRPC.Data;
using Discount.gRPC.Models;
using Discount.Grpc;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services
{
    public class DiscountService
        : DiscountProtoService.DiscountProtoServiceBase
    //this DisountProtoService.DiscountProtoServiceBase is generated in obj folder upon build the project
    {
        private readonly DiscountDbContext _db;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(DiscountDbContext db,
            ILogger<DiscountService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request,
            ServerCallContext context)
        {
            var coupon = await _db.Coupons
                .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
            {
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Descr" };
            }

            _logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount: {amount}",
                coupon.ProductName, coupon.Amount);

            var result = coupon.Adapt<CouponModel>();
            return result;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request,
            ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    "Invalid request"));
            }

            await _db.Coupons.AddAsync(coupon);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully created. ProductName: {ProductName}",
                coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request,
            ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument,
                    "Invalid request"));
            }

            _db.Coupons.Update(coupon);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}",
                coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
            ServerCallContext context)
        {
            var coupon = await _db.Coupons
                .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Discount with ProductName={request.ProductName} is not found"));
            }

            _db.Coupons.Remove(coupon);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully deleted. ProductName: {ProduductName}",
                request.ProductName);

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
