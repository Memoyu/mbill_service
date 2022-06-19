﻿namespace mbill.Service.PreOrder.Input;

public class UpdatePreOrderStatusInput
{
    public long Id { get; set; }

    /// <summary>
    /// 账单Id
    /// </summary>
    public long BillId { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; }
}