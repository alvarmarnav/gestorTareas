using System;
using System.Numerics;

namespace GestorTareas.Application.DTOs;

public class PaginationResponseDto<T>
{
    public List<T> Data{get;set;}=new();

    public int ActualPage{get;set;}
    public int TotalPages{get;set;}
    public int TotalItems{get;set;}
    public int ItemsPerPage{get;set;}

    public bool HasPageBefore => ActualPage>1;
    public bool HasPageAfter => ActualPage<TotalPages;
}
