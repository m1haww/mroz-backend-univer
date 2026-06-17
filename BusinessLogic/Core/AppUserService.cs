using AutoMapper;
using DataAccess.Database;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Domain.Entities.App;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Core;

public class AppUserService : IAppUserService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public AppUserService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AppUserDto>> GetAllAsync(string? appId, CancellationToken ct = default)
    {
        var query = _db.AppUsers.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(appId))
            query = query.Where(u => u.AppId == appId);

        var list = await query.ToListAsync(ct);
        return _mapper.Map<List<AppUserDto>>(list);
    }

    public async Task<AppUserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
        return entity == null ? null : _mapper.Map<AppUserDto>(entity);
    }

    public async Task<AppUserDto> CreateAsync(CreateAppUserDto dto, CancellationToken ct = default)
    {
        var entity = _mapper.Map<AppUser>(dto);
        entity.Id = Guid.NewGuid();
        _db.AppUsers.Add(entity);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<AppUserDto>(entity);
    }

    public async Task<AppUserDto?> UpdateAsync(Guid id, UpdateAppUserDto dto, CancellationToken ct = default)
    {
        var entity = await _db.AppUsers.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (entity == null) return null;

        if (dto.TotalRevenue.HasValue) entity.TotalRevenue = dto.TotalRevenue.Value;
        if (dto.CampaignId.HasValue) entity.CampaignId = dto.CampaignId;
        if (dto.KeywordId.HasValue) entity.KeywordId = dto.KeywordId;
        if (dto.AdGroupId.HasValue) entity.AdGroupId = dto.AdGroupId;
        if (dto.CountryCode != null) entity.CountryCode = dto.CountryCode;
        if (dto.HasTrial.HasValue) entity.HasTrial = dto.HasTrial.Value;

        await _db.SaveChangesAsync(ct);
        return _mapper.Map<AppUserDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.AppUsers.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (entity == null) return false;

        _db.AppUsers.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
