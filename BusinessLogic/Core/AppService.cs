using AutoMapper;
using DataAccess.Database;
using BusinessLogic.Interfaces;
using Domain.DTOs;
using Domain.Entities.App;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Core;

public class AppService : IAppService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public AppService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AppDto>> GetAllAsync(Guid userId, CancellationToken ct = default)
    {
        var apps = await _db.Apps
            .AsNoTracking()
            .Where(a => a.UserId == userId)
            .ToListAsync(ct);
        return _mapper.Map<List<AppDto>>(apps);
    }

    public async Task<AppDto?> GetByIdAsync(long id, Guid userId, CancellationToken ct = default)
    {
        var entity = await _db.Apps
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);
        return entity == null ? null : _mapper.Map<AppDto>(entity);
    }

    public async Task<AppDto> CreateAsync(CreateAppDto dto, Guid userId, CancellationToken ct = default)
    {
        var entity = new App
        {
            Name = dto.Name.Trim(),
            UserId = userId
        };
        _db.Apps.Add(entity);
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<AppDto>(entity);
    }

    public async Task<AppDto?> UpdateAsync(long id, UpdateAppDto dto, Guid userId, CancellationToken ct = default)
    {
        var entity = await _db.Apps.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);
        if (entity == null) return null;

        entity.Name = dto.Name.Trim();
        await _db.SaveChangesAsync(ct);
        return _mapper.Map<AppDto>(entity);
    }

    public async Task<bool> DeleteAsync(long id, Guid userId, CancellationToken ct = default)
    {
        var entity = await _db.Apps.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);
        if (entity == null) return false;

        _db.Apps.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
