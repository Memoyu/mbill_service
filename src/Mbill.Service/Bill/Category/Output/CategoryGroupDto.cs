﻿namespace Mbill.Service.Bill.Category.Output;

public class CategoryGroupDto
{
    public long BId { get; set; }

    public string Name { get; set; }

    public List<CategoryDto> Childs { get; set; }
}
