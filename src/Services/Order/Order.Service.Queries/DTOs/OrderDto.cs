﻿using static Order.Common.Enums;

namespace Order.Domain
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public OrderPayment PaymentType { get; set; }
        public int ClientId { get; set; }
        public ICollection<OrderDetailDto> Items { get; set; } = new List<OrderDetailDto>();
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set;}
    }
}