using AccountingSystem.Application.DTOs;
using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System.Text.Json.Serialization;

namespace AccountingSystem.Application.Commands.Companies;

// ========================================
// CREATE
// ========================================
public class CreateCompanyCommand : IRequest<CompanyDto>
{
    [JsonIgnore]
    public int Firmid { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Taxid { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
    public bool RestoreExistingData { get; set; } = false;
}

// ========================================
// UPDATE
// ========================================
public class UpdateCompanyCommand : IRequest<CompanyDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
}

// ========================================
// SOFT DELETE
// ========================================
public class DeleteCompanyCommand : IRequest<Unit>
{
    public int Id { get; set; }

    public DeleteCompanyCommand(int id)
    {
        Id = id;
    }
}

// ========================================
// HARD DELETE
// ========================================
public class DeleteCompanyPermanentlyCommand : IRequest<Unit>
{
    public int Id { get; set; }

    public DeleteCompanyPermanentlyCommand(int id)
    {
        Id = id;
    }
}
