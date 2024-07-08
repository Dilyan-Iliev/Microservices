﻿using Discount.Grpc;
using Grpc.Core;

namespace Discount.gRPC.Services
{
    public class DiscountService
        : DiscountProtoService.DiscountProtoServiceBase
        //this DisountProtoService.DiscountProtoServiceBase is generated in obj folder upon build the project
    {
        public override Task<CouponModel> GetDiscount(GetDiscountRequest request,
            ServerCallContext context)
        {
            //TODO: GetDiscount from DB
            return base.GetDiscount(request, context);
        }

        public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request,
            ServerCallContext context)
        {
            return base.CreateDiscount(request, context);
        }

        public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request,
            ServerCallContext context)
        {
            return base.UpdateDiscount(request, context);
        }

        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, 
            ServerCallContext context)
        {
            return base.DeleteDiscount(request, context);
        }
    }
}
