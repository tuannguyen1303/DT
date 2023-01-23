using DigitalTwin.Business.Extensions;
using DigitalTwin.Business.Helpers;
using DigitalTwin.Business.Services.Chart;
using DigitalTwin.Common.Constants;
using DigitalTwin.Common.Enums;
using DigitalTwin.Data.Database;
using DigitalTwin.Models.Requests.Chart;
using DigitalTwin.Models.Requests.ValueChain;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Category;
using DigitalTwin.Models.Responses.ValueChain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DigitalTwin.Models.DTOs;
using DigitalTwin.Common.Extensions;
using System.Threading;
using DocumentFormat.OpenXml.Drawing.Charts;
using DigitalTwin.Data.Entities;
using System.Linq;

namespace DigitalTwin.Business.Services.ValueChain;

public class ValueChainService : IValueChainService
{
    private readonly DigitalTwinContext _context;
    private readonly IChartService _chartService;
    private readonly IReadResultHelper _readResultHelper;

    public ValueChainService(DigitalTwinContext context, IChartService chartService,
        IReadResultHelper readResultHelper)
    {
        _context = context;
        _chartService = chartService;
        _readResultHelper = readResultHelper;
    }

    public async Task<Response<ValueChainResponse>> GetValueChainByType(ValueChainRequest request,
        CancellationToken token)
    {
        var valueChainResponse = await GetValueChains(request, token);
        return await Task.FromResult(Response.CreateResponse(valueChainResponse));
    }

    public async Task<ValueChainResponse> GetValueChains(ValueChainRequest request, CancellationToken token)
    {
        var frequency = GetRequestFrequencyHelper.GetFrequency(request);
        List<DataProductDto?> dataProductionVolume;
        List<DataProductDto?> dataFeedstock;

        var queryProductionVolume =
            $"select pl.\"Id\", pl.\"EntityMapId\", coalesce(null, null) as EntityId, pl.\"Name\", coalesce(null, '') as KpiPath, coalesce(null, '') as EntityGroupName, uom.\"Name\" as Unit, pld.\"NumValues\", pld.\"Color\" " +
            $"from pivot_da_middleware_digital.\"ProductLinkDetails\" as pld " +
            $"join pivot_da_middleware_digital.\"ProductLinks\" as pl on pld.\"ProductLinkId\" = pl.\"Id\" " +
            $"join pivot_da_middleware_digital.\"UnitOfMeasures\" as uom on pl.\"UnitOfMeasureId\" = uom.\"Id\" " +
            $"where pld.\"DataDate\" >= '{request.FromDate.ToHyphenFormat()}' and pld.\"DataDate\" <= '{request.ToDate.ToHyphenFormat()}' " +
            $"and pl.\"EntityMapId\" is not null and pld.\"Frequency\" = '{frequency}' and pl.\"LevelId\" = 8 " +
            $"and pl.\"LevelName\" = '{DigitalTwinConstants.ProductCateogry}' and pl.\"Name\" IN ('{DigitalTwinConstants.KPI_ProductionVolume}', '{DigitalTwinConstants.CustomerName}'); ";

        var queryFeedstock =
            $"select pl.\"Id\", pl.\"EntityMapId\", coalesce(null, null) as EntityId, pl.\"Name\", coalesce(null, '') as KpiPath, coalesce(null, '') as EntityGroupName, uom.\"Name\" as Unit, pld.\"NumValues\", pld.\"Color\" " +
            $"from pivot_da_middleware_digital.\"ProductLinkDetails\" as pld " +
            $"join pivot_da_middleware_digital.\"ProductLinks\" as pl on pld.\"ProductLinkId\" = pl.\"Id\" " +
            $"join pivot_da_middleware_digital.\"UnitOfMeasures\" as uom on pl.\"UnitOfMeasureId\" = uom.\"Id\" " +
            $"where pld.\"DataDate\" >= '{request.FromDate.ToHyphenFormat()}' and pld.\"DataDate\" <= '{request.ToDate.ToHyphenFormat()}' " +
            $"and pl.\"EntityMapId\" is not null and pld.\"Frequency\" = '{frequency}' " +
            $"and pl.\"FullName\" = '{DigitalTwinConstants.KPI_Feedstock}'; ";

        dataProductionVolume = await _readResultHelper.ExecuteResultFromQueryAsync<DataProductDto?>(_context, queryProductionVolume, token);
        dataFeedstock = await _readResultHelper.ExecuteResultFromQueryAsync<DataProductDto?>(_context, queryFeedstock, token);

        var productLinkOfProductionVolumeDetailDtos = new List<ProductDetailDto>();

        dataProductionVolume.ForEach(c =>
        {
            dynamic data = JsonConvert.DeserializeObject(c!.NumValues);
            productLinkOfProductionVolumeDetailDtos.Add(new ProductDetailDto
            {
                Id = (Guid)c.Id!,
                EntityMapId = (Guid?)c.EntityMapId,
                Name = c.Name != null ? c.Name.ToString() : string.Empty,
                Unit = c.Unit != null ? c.Unit.ToString() : string.Empty,
                Color = c.Color != null ? c.Color.ToString().Replace("\"", string.Empty) : string.Empty,
                Value = data.actual != null && data.actual != 0 ? data.actual.ToString("N2") : string.Empty,
                Target = data._target != null && data._target != 0 ? data._target.ToString("N2") : string.Empty,
                Planned = data.planned != null && data.planned != 0 ? data.planned.ToString("N2") : string.Empty,
                Percentage = data.variance_percentage != null && data.variance_percentage != 0
                    ? data.variance_percentage.ToString("N2")
                    : string.Empty,
                Variance =
                    data.variance != null && data.variance != 0 ? data.variance.ToString("N2") : string.Empty,
                Kbpi = data.kpbi != null && data.kpbi != 0
                    ? data.kpbi.ToString("N2")
                    : string.Empty,
            });
        });


        var entityMapProductionVolumeIds = productLinkOfProductionVolumeDetailDtos.Select(p => p.EntityMapId).Distinct().ToList();
        var entityMapFeedstockIds = dataFeedstock.Select(p => p!.EntityMapId).Distinct().ToList();

        var entityProductionVolumes = (from e in _context.Entities!.Where(c => entityMapProductionVolumeIds.Contains(c.EntityId))
                                       join et in _context.EntityTypes! on e.EntityTypeId equals et.Id
                                       join vct in _context.ValueChainTypes! on e.ValueChainTypeId equals vct.Id
                                       where vct.Id == request.ValueChainTypeId 
                                       select new ProductDto
                                       {
                                           Id = e.Id,
                                           ProductionVolumeId = e.Id,
                                           EntityId = e.EntityId,
                                           Name = e.Name,
                                           Alias = e.Name!.ToLower(),
                                           Path = e.KpiPath,
                                           Category = et.EntityGroupName,
                                           Type = et.Name,
                                           EntityParentId = e.EntityParentId,
                                           EntityTypeId = et.Id,
                                           EntityTypeMasterId = e.EntityTypeMasterId,
                                           Depth = e.Depth,
                                       }).AsNoTracking().OrderBy(c => c.Depth).ToList();

        var entityFeedstocks = (from e in _context.Entities!.Where(c => entityMapFeedstockIds.Contains(c.EntityId))
                                join et in _context.EntityTypes! on e.EntityTypeId equals et.Id
                                join vct in _context.ValueChainTypes! on e.ValueChainTypeId equals vct.Id
                                where vct.Id == request.ValueChainTypeId 
                                select new ProductDto
                                {
                                    Id = e.Id,
                                    ProductionVolumeId = e.Id,
                                    EntityId = e.EntityId,
                                    Name = e.Name,
                                    Alias = e.Name!.ToLower(),
                                    Path = e.KpiPath,
                                    Category = et.EntityGroupName,
                                    Type = et.Name,
                                    EntityParentId = e.EntityParentId,
                                    EntityTypeId = et.Id,
                                    EntityTypeMasterId = e.EntityTypeMasterId,
                                    Depth = e.Depth,
                                }).AsNoTracking().OrderBy(c => c.Depth).ToList();

        entityProductionVolumes.ForEach(c =>
        {
            var productLink = productLinkOfProductionVolumeDetailDtos.Where(d => d.EntityMapId == c.EntityId).ToList();
            c.Value = productLink.Where(d => !string.IsNullOrEmpty(d.Value)).Sum(d => decimal.Parse(d.Value!));
            c.Target = productLink.Where(d => !string.IsNullOrEmpty(d.Target)).Sum(d => decimal.Parse(d.Target!));
            c.Planned = productLink.Where(d => !string.IsNullOrEmpty(d.Planned))
                .Sum(d => decimal.Parse(d.Planned!));
            c.Percentage = productLink.Where(d => !string.IsNullOrEmpty(d.Percentage))
                .Sum(d => decimal.Parse(d.Percentage!));
            c.Variance = productLink.Where(d => !string.IsNullOrEmpty(d.Variance))
                .Sum(d => decimal.Parse(d.Variance!));
            c.Kbpi = productLink.Where(d => !string.IsNullOrEmpty(d.Kbpi)).Sum(d => decimal.Parse(d.Kbpi!));
            c.Color = productLink.FirstOrDefault(d => !string.IsNullOrEmpty(d.Color))?.Color;
            c.Unit = productLink.FirstOrDefault(d => !string.IsNullOrEmpty(d.Unit))?.Unit;
        });

        var entityParentId = entityProductionVolumes.Select(d => d.EntityParentId).Distinct().ToList();
        var entitiesParentProductionVolumes = (from e in _context.Entities!.Where(c => entityParentId.Contains(c.EntityId))
                                               join et in _context.EntityTypes! on e.EntityTypeId equals et.Id
                                               join vct in _context.ValueChainTypes! on e.ValueChainTypeId equals vct.Id
                                               where vct.Id == request.ValueChainTypeId && DigitalTwinConstants.Categories.Contains(et.EntityGroupName!)
                                               select new EntityDto
                                               {
                                                   Id = e!.Id,
                                                   EntityId = e.EntityId,
                                                   Name = e.Name,
                                                   Alias = e.Name!.ToLower(),
                                                   Path = e.KpiPath,
                                                   Category = et.EntityGroupName,
                                                   Type = et.Name,
                                                   EntityParentId = e.EntityParentId,
                                                   EntityTypeId = et.Id,
                                                   EntityTypeMasterId = e.EntityTypeMasterId,
                                                   Depth = e.Depth
                                               }).AsNoTracking().ToList();

        var entityDtos = new List<EntityDto>();
        var productLinkList = new List<ProductDto>();

        foreach (var product in entityProductionVolumes)
        {
            //find feedStocks 
            var feedStocks = entityFeedstocks.Where(c => c.Path!.Contains(product.Path!) && c.Depth > 3).ToList();

            var entityParent = entitiesParentProductionVolumes.FirstOrDefault(c => product.EntityParentId == c.EntityId);
            if (entityParent == null)
            {
                continue;
            }
            var entityParentDto = new EntityDto
            {
                Id = entityParent!.Id,
                EntityId = entityParent.EntityId,
                Name = entityParent.Name,
                Alias = entityParent.Name!.ToLower(),
                Path = entityParent.Path,
                Category = entityParent.Category,
                Type = entityParent.Type,
                EntityParentId = entityParent.EntityParentId,
                EntityTypeId = entityParent.EntityTypeId,
                EntityTypeMasterId = entityParent.EntityTypeMasterId,
                Section = SectionEntity(entityParent.Category!),
                ParentList = new List<Models.Responses.ValueChain.Entity>(),
                ChildrenList = new List<Models.Responses.ValueChain.Entity>()
            };


            if (!entityDtos.Exists(c => c.Name == entityParentDto.Name && c.Category == entityParentDto.Category))
            {
                entityDtos.Add(entityParentDto);
                product.Parent = entityParentDto.EntityId;
            }
            else
            {
                entityDtos.ForEach(c =>
                {
                    if (c.Name == entityParentDto.Name && c.Category == entityParentDto.Category)
                    {
                       product.Parent = c.EntityId;
                       entityParentDto.EntityId = c.EntityId;
                    }
                });
            }

            if (!feedStocks.Any())
            {
                product.Alias = $"{entityParentDto!.Name} -> Empty";
                productLinkList.Add(product);
            }

            foreach (var feedStock in feedStocks)
            {
                entityParentDto.ChildrenList.Add(new Models.Responses.ValueChain.Entity
                {
                    Id = feedStock.Id,
                    Name = feedStock.Name,
                    Alias = feedStock.Name!.ToLower(),
                    Category = feedStock.Category,
                    Type = feedStock.Type,
                    EntityId = feedStock.EntityId,
                    Section = SectionEntity(feedStock.Category!)
                });
                var entityChildrenDto = new EntityDto
                {
                    Id = feedStock!.Id,
                    EntityId = feedStock.EntityId,
                    Name = feedStock.Name,
                    Alias = feedStock.Name!.ToLower(),
                    Path = feedStock.Path,
                    Category = feedStock.Category,
                    Type = feedStock.Type,
                    EntityParentId = feedStock.EntityParentId,
                    EntityTypeId = feedStock.EntityTypeId,
                    EntityTypeMasterId = feedStock.EntityTypeMasterId,
                    Section = SectionEntity(feedStock.Category!),
                    ParentList = new List<Models.Responses.ValueChain.Entity>
                {
                    new()
                    {
                        Id = entityParentDto.Id,
                        Name = entityParentDto.Name,
                        Alias = entityParentDto.Name!.ToLower(),
                        Category = entityParentDto.Category,
                        Type = entityParentDto.Type,
                        EntityId = entityParentDto.EntityId,
                        Section = SectionEntity(entityParentDto.Category!)
                    }
                },
                    ChildrenList = null,
                };

                var productLink = new ProductDto
                {
                    Id = feedStock.Id,
                    ProductionVolumeId = product.Id,
                    EntityId = feedStock.EntityId,
                    Name = product.Name,
                    Path = feedStock.Path,
                    Category = feedStock.Category,
                    Type = feedStock.Name,
                    EntityParentId = feedStock.EntityParentId,
                    EntityTypeId = feedStock.EntityTypeId,
                    EntityTypeMasterId = feedStock.EntityTypeMasterId,
                    Depth = feedStock.Depth,
                    Value = product.Value,
                    Target = product.Target,
                    Planned = product.Planned,
                    Percentage = product.Percentage,
                    Variance = product.Variance,
                    Kbpi = product.Kbpi,
                    Color = product.Color,
                    Unit = product.Unit,
                };

                productLink.Alias = $"{entityParentDto!.Name} -> {feedStock.Name}";
                if (!entityDtos.Exists(c => c.Name == entityChildrenDto.Name && c.Category == entityChildrenDto.Category))
                {
                    entityDtos.Add(entityChildrenDto);
                    productLink.Children = feedStock.EntityId;
                }
                else
                {
                    entityDtos.ForEach(c =>
                    {
                        if (c.Name == entityChildrenDto.Name && c.Category == entityChildrenDto.Category)
                        {
                            if (c.ParentList == null)
                            {
                                c.ParentList = new List<Models.Responses.ValueChain.Entity>();
                            }
                            c.ParentList!.AddRange(entityChildrenDto.ParentList!);
                            productLink.Children = c.EntityId;
                        }
                    });
                }

                if (!entityDtos.Exists(c => c.Name == entityParentDto.Name && c.Category == entityParentDto.Category ))
                {
                    productLink.Parent = entityParentDto.EntityId;
                }
                else
                {
                    entityDtos.ForEach(c =>
                    {
                        if (c.Name == entityParentDto.Name && c.Category == entityParentDto.Category)
                        {
                            if (c.ChildrenList == null)
                            {
                                c.ChildrenList = new List<Models.Responses.ValueChain.Entity>();
                            }
                            c.ChildrenList!.AddRange(entityParentDto.ChildrenList!);
                            productLink.Parent = c.EntityId;
                        }
                    });
                }

                productLinkList.Add(productLink);
            }
        }
        var entityIdFilters = new List<Guid>();
        if (request.IsFilter)
        {
            entityIdFilters = request.Entities!.Any() ? entityDtos.Where(c => !request.Entities!.Contains(c.EntityId)).Select(c => c.EntityId).ToList() : entityDtos.Select(c => c.EntityId).ToList();
        }

        entityDtos = entityDtos.GroupBy(c => new
        {
            c.Id,
            c.Category,
            c.Name,
            c.Path,
            c.Alias,
            c.EntityId,
            c.EntityParentId,
            c.EntityTypeMasterId,
            c.Section,
            c.Type,
            c.EntityTypeId
        }).Select(c => new EntityDto
        {
            Id = c.Key!.Id,
            EntityId = c.Key!.EntityId,
            Name = c.Key!.Name,
            Alias = c.Key!.Name!.ToLower(),
            Path = c.Key!.Path,
            Category = c.Key!.Category,
            Type = c.Key!.Type,
            EntityParentId = c.Key!.EntityParentId,
            EntityTypeId = c.Key!.EntityTypeId,
            EntityTypeMasterId = c.Key!.EntityTypeMasterId,
            Section = c.Key!.Section,
            ParentList = c.Where(d => d.ParentList != null).SelectMany(d => d.ParentList!).GroupBy(d => d.Name)
                .Select(d => d.First()).ToList(),
            ChildrenList = c.Where(d => d.ChildrenList != null).SelectMany(d => d.ChildrenList!)
                .GroupBy(d => d.Name).Select(d => d.First()).ToList()
        }).ToList();

        if (entityIdFilters.Any())
        {
            entityDtos = entityDtos.Where(c => !entityIdFilters.Contains(c.EntityId)).ToList();
            entityDtos.ForEach(c =>
            {
                c.ChildrenList = c.ChildrenList!.Where(d => !entityIdFilters.Contains(d.EntityId)).ToList();
                c.ParentList = c.ParentList!.Where(d => !entityIdFilters.Contains(d.EntityId)).ToList();
            });

            productLinkList = productLinkList.Where(c => !entityIdFilters.Contains(c.Parent!.Value)).ToList();
            productLinkList = productLinkList.Where(c => !entityIdFilters.Contains(c.Children!.Value)).ToList();
        }

        return new ValueChainResponse
        {
            Title = _context.ValueChainTypes?.FirstOrDefault(vct => vct.Id == request.ValueChainTypeId)?.Name
                ?.ToString(),
            Entities = entityDtos.Where(c => c.ParentList!.Any() || c.ChildrenList!.Any() || productLinkList.Any(d => d.Parent == c.EntityId)).ToList(),
            Products = productLinkList.Where(c => c.Parent != Guid.Empty || c.Children != Guid.Empty).ToList()
        };
    }

    private string? StatusEntity(string color)
    {
        switch (color)
        {
            case "green":
                return EntityStatus.Running.ToString();
            case "yellow":
                return EntityStatus.Running.ToString();
            case "red":
                return EntityStatus.ShutDown.ToString();
        }

        return EntityStatus.ShutDown.ToString();
    }

    private string SectionEntity(string category)
    {
        switch (category)
        {
            case string compare when (DigitalTwinConstants.Exploration_Extractions.Contains(compare)):
                return Session.Exploration_Extraction.ToString();
            case string compare when (DigitalTwinConstants.Processings.Contains(compare)):
                return Session.Processing.ToString();
            case string compare when (DigitalTwinConstants.Deliveries.Contains(compare)):
                return Session.Delivery.ToString();
            default:
                return string.Empty;
        }
    }

    public async Task<Response<GetAllFrequencyResponse>> GetFrequencies(CancellationToken token)
    {
        var frequencies = _context.ProductLinkDetails!
                        .Join(_context.ProductLinks!,
                                pld => pld.ProductLinkId,
                                pl => pl.Id,
                                (pld, pl) => new { ProductLinks = pl, ProductLinkDetail = pld })
                        .Where(p => p.ProductLinks.LevelId == 8 && p.ProductLinks.Name == DigitalTwinConstants.KPI_ProductionVolume
                        && p.ProductLinks.LevelName == DigitalTwinConstants.ProductCateogry).Select(c => c.ProductLinkDetail.Frequency).Distinct().ToList();
        return await Task.FromResult(Response.CreateResponse(new GetAllFrequencyResponse
        {
            Frequencies = frequencies
        }));
    }

    public async Task<Response<ValueChainDetailResponse>> GetValueChainDetail(ViewBusinessDashboardRequest request,
        CancellationToken token)
    {
        var valueChainRequest = new ValueChainRequest
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            ValueChainTypeId = request.ValueChainTypeId,
            IsDaily = request.IsDaily,
            IsMonthly = request.IsMonthly,
            IsMonthToDate = request.IsMonthToDate,
            IsQuarterly = request.IsQuarterly,
            IsYearEndProjection = request.IsYearEndProjection,
            IsYearToDaily = request.IsYearToDaily,
            IsYearToMonthly = request.IsYearToMonthly,
            IsRealTime = request.IsRealTime,
            IsHour = request.IsHour,
            IsQuarterToDate = request.IsQuarterToDate,
            IsWeekly = request.IsWeekly,
            IsFilter = false,
            IsBusiness = request.IsBusiness,
        };

        var frequency = GetRequestFrequencyHelper.GetFrequency(valueChainRequest);
        var nameFrequency = GetRequestFrequencyHelper.GetNameFrequency(valueChainRequest);
        var entityResponse = await GetValueChains(valueChainRequest, token);
        var entityDto = request.EntityId != Guid.Empty ? entityResponse.Entities?.FirstOrDefault(c => c.Id == request.EntityId) 
                        : (from e in _context.Entities
                           join et in _context.EntityTypes! on e.EntityTypeId equals et.Id
                           where e.Name == DigitalTwinConstants.Downstream
                           select new EntityDto
                           {
                               Id = e!.Id,
                               EntityId = e.EntityId,
                               Name = e.Name,
                               Alias = e.Name!.ToLower(),
                               Path = e.KpiPath,
                               Category = et.EntityGroupName,
                               Type = et.Name,
                               EntityParentId = e.EntityParentId,
                               EntityTypeId = et.Id,
                               EntityTypeMasterId = e.EntityTypeMasterId,
                               Depth = e.Depth
                           }).FirstOrDefault();
        if (entityDto == null)
        {
            return await Task.FromResult(Response.CreateResponse(new ValueChainDetailResponse()));
        }

        var entityDetailRespones = new ValueChainDetailResponse();
        var throughput = new List<ProductionVolume>();
        var productionVolumes = new List<ProductionVolume>();
        var feedstockSupplies = new List<ProductionVolume>();
        var fromDate = request.ToDate.AddDays(-1);
        var toDate = request.ToDate;
        if (frequency == "hourly" || frequency == "15m")
        {
            fromDate = new DateTime(request.ToDate.Year, request.ToDate.Month, request.ToDate.Day, request.ToDate.Hour, 0, 0).ToUTC(TimeSpan.Zero);
            toDate = new DateTime(request.ToDate.Year, request.ToDate.Month, request.ToDate.Day, request.ToDate.Hour + 1, 0, 0).AddSeconds(-1).ToUTC(TimeSpan.Zero);
        }
        if (request.IsBusiness)
        {
            var dataProduct = new List<DataProductDto?>();

            var queryDataProduct =
                $"select pl.\"Id\", pl.\"EntityMapId\", e.\"Id\" as EntityId, e.\"KpiPath\", e.\"Name\", et.\"EntityGroupName\", uom.\"Name\" as Unit, pld.\"NumValues\", pld.\"Color\"  " +
                $"from pivot_da_middleware_digital.\"ProductLinkDetails\" as pld " +
                $"join pivot_da_middleware_digital.\"ProductLinks\" as pl on pld.\"ProductLinkId\" = pl.\"Id\" " +
                $"join pivot_da_middleware_digital.\"UnitOfMeasures\" as uom on pl.\"UnitOfMeasureId\" = uom.\"Id\" " +
                $"join pivot_da_middleware_digital.\"Entities\" as e on pl.\"EntityMapId\" = e.\"EntityId\" " +
                $"join pivot_da_middleware_digital.\"EntityTypes\" as et ON et.\"Id\" = e.\"EntityTypeId\" " +
                $"where pld.\"DataDate\" >= '{request.FromDate.ToHyphenFormat()}' and pld.\"DataDate\" <= '{request.ToDate.ToHyphenFormat()}' " +
                $"and pl.\"EntityMapId\" is not null and pld.\"Frequency\" = '{frequency}' and pl.\"LevelId\" IN (2,4,5) " +
                $"and pl.\"LevelName\" IN ('{DigitalTwinConstants.OPUName}', '{DigitalTwinConstants.PlantName}', '{DigitalTwinConstants.BusinessCateogry}') and pl.\"Name\" IN ('{DigitalTwinConstants.BusinessName}') " +
                $"and e.\"Name\" = '{entityDto.Name}' and e.\"ValueChainTypeId\" = '{request.ValueChainTypeId}' and uom.\"Name\" = 'kbpd'; ";

            dataProduct =
                await _readResultHelper.ExecuteResultFromQueryAsync<DataProductDto?>(_context, queryDataProduct, token);

            dataProduct.ForEach(c =>
            {
                var kpiPath = (string)c?.KpiPath!;
                var path = kpiPath.Split(" > ").ToList();
                if (path[path.Count - 2] == entityDto.Name)
                {
                    dynamic data = JsonConvert.DeserializeObject(c!.NumValues);
                    throughput.Add(new ProductionVolume
                    {
                        Id = (Guid)c.Id!,
                        EntityId = (Guid)c.EntityId!,
                        Name = c.Name != null ? c.Name.ToString() : string.Empty,
                        Unit = c.Unit != null ? c.Unit.ToString() : string.Empty,
                        Color = c.Color != null ? c.Color.ToString().Replace("\"", string.Empty) : string.Empty,
                        Actual =
                            data.actual != null && data.actual != 0 ? data.actual : null,
                        Planned = data.planned != null && data.planned != 0
                            ? data.planned
                            : null,
                        Variance = data.variance != null && data.variance != 0
                            ? data.variance
                            : null,
                        Kpbi = data.kpbi != null && data.kpbi != 0
                            ? data.kpbi
                            : null,
                    });
                }
            });
        }
        else
        {
            productionVolumes.AddRange(entityResponse.Products!.Where(d => d.Parent == entityDto.EntityId)
                .Select(
                    d => new ProductionVolume
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Alias = string.Join(" - ", d.Alias!.Split(" -> ")),
                        Actual = d.Value,
                        Planned = d.Planned,
                        Kpbi = d.Kbpi,
                        Variance = d.Variance,
                        Unit = d.Unit,
                        Color = d.Color,
                    }));

            if ((DigitalTwinConstants.BusinessTab.Contains(entityDto?.Category!) ||
                 DigitalTwinConstants.Deliveries.Contains(entityDto?.Category!)))
            {
                feedstockSupplies.AddRange(entityResponse.Products!
                    .Where(d => d.Children == entityDto!.EntityId)
                    .Select(
                        d => new ProductionVolume
                        {
                            Id = d.Id,
                            Name = d.Name,
                            Alias = string.Join(" - ", d.Alias!.Split(" -> ")),
                            Actual = d.Value,
                            Planned = d.Planned,
                            Kpbi = d.Kbpi,
                            Variance = d.Variance,
                            Unit = d.Unit,
                            Color = d.Color,
                        }));
            }
        }

        var feedstockSupply = feedstockSupplies.Where(c =>
            DigitalTwinConstants.BusinessTab.Contains(entityDto?.Category!)
            || DigitalTwinConstants.Deliveries.Contains(entityDto?.Category!)).ToList();
        var productIds = productionVolumes.Select(c => c.Id).ToList();
        productIds.AddRange(feedstockSupply.Select(c => c.Id).ToList());
        var productBusinessIds = throughput.Select(c => c.Id).ToList();
        var listProductBusinessId = new List<string>();
        if (request.IsBusiness)
        {
            productIds = productBusinessIds;
            throughput.Select(c => c.EntityId).ToList().ForEach(c => listProductBusinessId.Add($"'{c}'"));
        }

        var listData = new List<SemDataDto?>();

        var query = !request.IsBusiness ?
            "SELECT dd.id, pl.\"Name\", e.\"Name\" as EntityName, e.\"EntityParentId\", data.sem_data_id as SemDataId, data.amend_data_id as AmendDataId, data.path_stream_id as PathStreamId, data.function_id as FunctionId, " +
            "data.kpi_id as KpiId, data.frequency as Frequency, data.data_date as DataDate, data.uom_name as UomName, data.actual_num as ActualNum, data.actual_str, data.target_num as TargetNum, data.target_str, data.variance, data.variance_percentage as VariancePercentage," +
            "data.traffic_light, data.traffic_light_color, data.kpbi_traffic_light, data.kpbi_traffic_light_color, data.justification, data.remark, data.performance_analysis," +
            "data.analysis_published_at, data.analysis_published_by, data.analysis_updated_at, data.analysis_updated_by, data.num_values, data.str_values, data.orig_num_values, data.orig_str_values, " +
            "data.amend_num_values, data.amend_str_values, data.traffic_lights, data.created_at as CreatedDate, data.created_by, data.updated_at, data.updated_by  " +
            $"FROM pivot_da_middleware.get_de_sem_data('{frequency}') as data " +
            "join pivot_da_middleware_digital.\"ProductLinks\" as pl on data.kpi_id = pl.\"Id\" " +
            "join pivot_da_middleware_digital.\"Entities\" as e on pl.\"EntityMapId\" = e.\"EntityId\" " +
            "left join pivot_da_middleware.\"DbdDashboards\" as dd on e.\"EntityTypeMasterId\" = dd.\"mstEntityId\" " +
            $"where data.data_date >= '{fromDate.ToHyphenFormat()}' and data.data_date <= '{toDate.ToHyphenFormat()}' and e.\"Id\" = '{entityDto?.Id}';"
            :
            $"SELECT dd.id, pl.\"Name\", e.\"Name\" as EntityName, e.\"EntityParentId\", data.sem_data_id as SemDataId, data.amend_data_id as AmendDataId, data.path_stream_id as PathStreamId, data.function_id as FunctionId, " +
            $"data.kpi_id as KpiId, \r\ndata.frequency as Frequency, data.data_date as DataDate, data.uom_name as UomName, data.actual_num as ActualNum, data.actual_str, " +
            $"data.target_num as TargetNum, data.target_str, \r\ndata.variance, data.variance_percentage as VariancePercentage,data.traffic_light, " +
            $"data.traffic_light_color, \r\ndata.kpbi_traffic_light, data.kpbi_traffic_light_color, data.justification, data.remark, " +
            $"data.performance_analysis,data.analysis_published_at, data.analysis_published_by, data.analysis_updated_at, " +
            $"data.analysis_updated_by, data.num_values, data.str_values, data.orig_num_values, data.orig_str_values, " +
            $"data.amend_num_values, data.amend_str_values, data.traffic_lights, data.created_at as CreatedDate, " +
            $"data.created_by, data.updated_at, data.updated_by  FROM pivot_da_middleware.get_de_sem_data('{frequency}') as data " +
            $"join pivot_da_middleware_digital.\"ProductLinks\" as pl on data.kpi_id = pl.\"Id\" " +
            $"join pivot_da_middleware_digital.\"Entities\" as e on pl.\"EntityMapId\" = e.\"EntityId\" " +
            $"left join pivot_da_middleware.\"DbdDashboards\" as dd on e.\"EntityTypeMasterId\" = dd.\"mstEntityId\" " +
            $"where data.data_date >= '{fromDate.ToHyphenFormat()}' and data.data_date <= '{toDate.ToHyphenFormat()}' " +
            $"and data.justification is not null" + (listProductBusinessId.Any() ? $" and e.\"Id\" IN ({(string.Join(",", listProductBusinessId))});" : $" and e.\"Id\" = '{entityDto?.Id}'");

        listData = await _readResultHelper.ExecuteResultFromQueryAsync<SemDataDto?>(_context, query, token);

        if (request.IsBusiness)
        {
            var queryName = $"select \"displayName\" as DisplayName from pivot_da_middleware.\"DbdDashboards\"\r\nwhere \"shortName\" = '{entityDto!.Name}'";
            var displayName = await _readResultHelper.ExecuteResultFromQueryAsync<NameDataDto?>(_context, queryName, token);
            var production = listData.Where(c =>
                (DigitalTwinConstants.BusinessTab.Contains(entityDto?.Category!) &&
                 c?.Name == DigitalTwinConstants.BusinessName));
            var actual = throughput.Where(c => c.Actual != null).Sum(c => c.Actual!);
            var target = throughput.Where(c => c.Planned != null).Sum(c => c.Planned!);
            var kbpi = throughput.Where(c => c.Kpbi != null).Sum(c => c.Kpbi!);
            var variance = throughput.Where(c => c.Variance != null).Sum(c => c.Variance!);

            entityDetailRespones.Data = new DataValueChainDetail
            {
                Actual = actual == 0 ? null : actual,
                Target = target == 0 ? null : target,
                Kpbi = kbpi == 0 ? null : kbpi,
                Variance = variance == 0 ? null : kbpi,
                VariancePercentage = kbpi == 0 ? null : kbpi,
                Unit = throughput.Any()
                                ? throughput.FirstOrDefault(c => !string.IsNullOrEmpty(c.Unit))?.Unit
                                : feedstockSupply.FirstOrDefault(c => !string.IsNullOrEmpty(c.Unit))?.Unit,
                Name = $"{entityDto?.Name} > {DigitalTwinConstants.BusinessName}",
                Id = production.Any() ? production.FirstOrDefault()?.Id : Guid.Empty,
                Justifications = production != null ? production.Select(c => new Justification
                {
                    Name = c!.EntityName,
                    UpdatedDate = c.Updated_At.HasValue ? c.Updated_At : c.CreatedDate,
                    Content = c.Justification,
                    Kpi_Id = c.KpiId
                }).ToList() : null,
                Status = StatusEntity(throughput.FirstOrDefault()?.Color!),
                TitleChart = $"{DigitalTwinConstants.BusinessName} ({nameFrequency})",
                FullName = displayName.Any() ? displayName.FirstOrDefault()?.DisplayName : entityDto!.Name
            };
        }
        else
        {
            var production = listData.FirstOrDefault(c =>
                (DigitalTwinConstants.BusinessTab.Contains(entityDto?.Category!) &&
                 c?.Name == DigitalTwinConstants.KPI_ProductionVolume)
                || (DigitalTwinConstants.Deliveries.Contains(entityDto?.Category!) &&
                    c?.Name == DigitalTwinConstants.CustomerName));

            var planned = productionVolumes.Sum(c => c.Planned!);
            var kpbi = productionVolumes.Sum(c => c.Kpbi!);
            entityDetailRespones.Data = new DataValueChainDetail
            {
                Actual = productionVolumes.Sum(c => c.Actual!),
                Target = planned != 0 ? planned : null,
                Kpbi = kpbi != 0 ? kpbi : null,
                Variance = productionVolumes.Sum(c => c.Variance!),
                VariancePercentage = productionVolumes.Sum(c => c.Variance!),
                Unit = productionVolumes.FirstOrDefault(c => !string.IsNullOrEmpty(c.Unit))?.Unit,
                Name = $"{entityDto?.Path} > {(DigitalTwinConstants.Deliveries.Contains(entityDto?.Category!) ? DigitalTwinConstants.CustomerName : DigitalTwinConstants.KPI_ProductionVolume)}",
                Id = production != null && production?.Id != null ? production.Id : Guid.Empty,
                Justifications = production != null ? new List<Justification>
                {
                    new Justification
                    {
                        Name = production.EntityName,
                        UpdatedDate = production.Updated_At.HasValue ? production.Updated_At : production.CreatedDate,
                        Content = production.Justification,
                        Kpi_Id = production.KpiId
                    }
                } : null,
                Status = StatusEntity(productionVolumes.FirstOrDefault()?.Color!),
                TitleChart =
                    $"{(DigitalTwinConstants.Deliveries.Contains(entityDto?.Category!) ? DigitalTwinConstants.CustomerName : DigitalTwinConstants.KPI_ProductionVolume)} ({nameFrequency})",
            };
        }

        if (request.IsBusiness)
        {
            entityDetailRespones.Throughput = throughput.Any()
                ? throughput.Select(
                    c => new ProductionVolume
                    {
                        Name = c.Name,
                        Id = c.Id,
                        Actual = c.Actual,
                        Planned = c.Planned,
                        Variance = c.Variance,
                        Unit = c.Unit,
                    }).ToList()
                : null;
        }
        else
        {
            entityDetailRespones.ProductionVolume = productionVolumes.Any()
                ? productionVolumes.Select(
                    c => new ProductionVolume
                    {
                        Name = c.Name,
                        Id = c.Id,
                        Actual = c.Actual,
                        Alias = c.Alias,
                        Planned = c.Planned,
                        Variance = c.Variance,
                        Unit = c.Unit,
                    }).ToList()
                : null;

            entityDetailRespones.FeedstockSupply =
                feedstockSupply.Any() && DigitalTwinConstants.BusinessTab.Contains(entityDto?.Category!)
                    ? feedstockSupply.Select(
                        c => new ProductionVolume
                        {
                            Name = c.Name,
                            Id = c.Id,
                            Actual = c.Actual,
                            Planned = c.Planned,
                            Variance = c.Variance,
                            Unit = c.Unit,
                            Alias = c.Alias,
                        }).ToList()
                    : null;

            entityDetailRespones.SalesVolume =
                feedstockSupply.Any() && DigitalTwinConstants.Deliveries.Contains(entityDto?.Category!)
                    ? feedstockSupply.Select(
                        c => new ProductionVolume
                        {
                            Name = c.Name,
                            Id = c.Id,
                            Actual = c.Actual,
                            Planned = c.Planned,
                            Variance = c.Variance,
                            Unit = c.Unit,
                            Alias = c.Alias,
                        }).ToList()
                    : null;
        }

        // Chart Data
        var chartRequest = new ChartRequest
        {
            ProductLinkId = productIds,
            IsRealTime = request.IsRealTime,
            IsHour = request.IsHour,
            IsDaily = request.IsDaily,
            IsMonthly = request.IsMonthly,
            IsMonthToDate = request.IsMonthToDate,
            IsQuarterly = request.IsQuarterly,
            IsYearToDaily = request.IsYearToDaily,
            IsYearToMonthly = request.IsYearToMonthly,
            IsYearEndProjection = request.IsYearEndProjection,
            IsWeekly = request.IsWeekly,
            IsQuarterToDate = request.IsQuarterToDate,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            TimeZoneOffset = request.TimeZoneOffset,
            IsBusiness = request.IsBusiness
        };

        entityDetailRespones.ChartData = _chartService.GetChart(chartRequest, token);
        entityDetailRespones.IsShowChart =
            entityDetailRespones.ChartData.Actual!.Any(c => !string.IsNullOrEmpty(c));
        if (request.IsBusiness)
        {
            // Chain Overview
            var chainProductionVolumes = entityResponse.Products!.Where(c => c.Parent == entityDto!.EntityId).GroupBy(c => c.Parent).Select(c => new ProductDto
            {
                Id = Guid.NewGuid(),
                Alias = c.First().Alias,
                Name = $"{entityDto!.Name} > Production Volume",
                Unit = c.First().Unit,
                EntityId = c.Key!.Value,
                Color = c.First().Color,
                Value = c.Sum(d => d.Value!),
                Target = c.Sum(d => d.Target!),
                Planned = c.Sum(d => d.Planned!) == 0 ? null : c.Sum(d => d.Planned!),
                Percentage = c.Sum(d => d.Percentage!),
                Variance = c.Sum(d => d.Variance!),
                Kbpi = c.Sum(d => d.Kbpi!) == 0 ? null : c.Sum(d => d.Kbpi!),
            }).Where(c => c.Value != 0 || c.Variance != 0 || c.Percentage != 0).ToList();

            var chainFeedStocks = entityResponse.Products!.Where(c => c.Children == entityDto!.EntityId).GroupBy(c => c.Children).Select(c => new ProductDto
            {
                Id = Guid.NewGuid(),
                Alias = c.First().Alias,
                Name = $"{entityDto!.Name} > Feedstock",
                Unit = c.First().Unit,
                EntityId = c.Key!.Value,
                Color = c.First().Color,
                Value = c.Sum(d => d.Value!),
                Target = c.Sum(d => d.Target!),
                Planned = c.Sum(d => d.Planned!) == 0 ? null : c.Sum(d => d.Planned!),
                Percentage = c.Sum(d => d.Percentage!),
                Variance = c.Sum(d => d.Variance!),
                Kbpi = c.Sum(d => d.Kbpi!) == 0 ? null : c.Sum(d => d.Kbpi!),
            }).Where(c => c.Value != 0 || c.Variance != 0 || c.Percentage != 0).ToList();
            chainProductionVolumes.AddRange(chainFeedStocks);
            entityDetailRespones.ChainOverview = chainProductionVolumes;
        }

        return await Task.FromResult(Response.CreateResponse(entityDetailRespones));
    }

    public async Task<Response<GetAllKPIResponse>> GetAllKPI(GetAllKPIRequest request, CancellationToken token)
    {
        var frequency = GetRequestFrequencyHelper.GetFrequency(request);
        var valueChainRequest = new ValueChainRequest
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            ValueChainTypeId = request.ValueChainTypeId,
            IsDaily = request.IsDaily,
            IsMonthly = request.IsMonthly,
            IsMonthToDate = request.IsMonthToDate,
            IsQuarterly = request.IsQuarterly,
            IsYearEndProjection = request.IsYearEndProjection,
            IsYearToDaily = request.IsYearToDaily,
            IsYearToMonthly = request.IsYearToMonthly,
            IsRealTime = request.IsRealTime,
            IsHour = request.IsHour,
            IsQuarterToDate = request.IsQuarterToDate,
            IsWeekly = request.IsWeekly,
            IsFilter = false,
            IsBusiness = false
        };
        var entityResponse = await GetValueChains(valueChainRequest, token);
        var nameFrequency = GetRequestFrequencyHelper.GetNameFrequency(valueChainRequest);

        var productionVolumes = entityResponse.Products!.GroupBy(c => new { c.Parent, c.Name }).Select(c => new ProductDto
        {
            Id = Guid.NewGuid(),
            Alias = c.Key.Name,
            Name = $"{(entityResponse.Entities!.FirstOrDefault(d => d.EntityId == c.Key.Parent)?.Name)} > {c.Key.Name}",
            Unit = c.First().Unit,
            EntityId = entityResponse.Entities!.FirstOrDefault(d => d.EntityId == c.Key.Parent)!.EntityId,
            Color = c.First().Color,
            Value = c.Sum(d => d.Value!),
            Target = c.Sum(d => d.Target!),
            Planned = c.Sum(d => d.Planned!) == 0 ? null : c.Sum(d => d.Planned!),
            Percentage = c.Sum(d => d.Percentage!),
            Variance = c.Sum(d => d.Variance!),
            Kbpi = c.Sum(d => d.Kbpi!) == 0 ? null : c.Sum(d => d.Kbpi!),
        }).Where(c => c.Value != 0 || c.Variance != 0 || c.Percentage != 0).ToList();

        var feedStocks = entityResponse.Products!.Where(c => c.Children != null)!.GroupBy(c => new { c.Children, c.Name }).Select(c => new ProductDto
        {
            Id = Guid.NewGuid(),
            Alias = c.Key.Name,
            Name = $"{c.Key.Name} > {entityResponse.Entities!.FirstOrDefault(d => d.EntityId == c.Key.Children)?.Name}",
            Unit = c.First().Unit,
            EntityId = entityResponse.Entities!.FirstOrDefault(d => d.EntityId == c.Key.Children)!.EntityId,
            Color = c.First().Color,
            Value = c.Sum(d => d.Value!),
            Target = c.Sum(d => d.Target!),
            Planned = c.Sum(d => d.Planned!) == 0 ? null : c.Sum(d => d.Planned!),
            Percentage = c.Sum(d => d.Percentage!),
            Variance = c.Sum(d => d.Variance!),
            Kbpi = c.Sum(d => d.Kbpi!) == 0 ? null : c.Sum(d => d.Kbpi!),
        }).Where(c => c.Value != 0 || c.Variance != 0 || c.Percentage != 0).ToList();

        productionVolumes.AddRange(feedStocks);
        var kpiResponse = new GetAllKPIResponse
        {
            ProductList = productionVolumes
        };

        return await Task.FromResult(Response.CreateResponse(kpiResponse));
    }

    public async Task<Response<GetProductDetailResponse>> GetProductDetail(GetProductDetailRequest request,
        CancellationToken token)
    {
        var valueChainRequest = new ValueChainRequest
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            ValueChainTypeId = request.ValueChainTypeId,
            IsDaily = request.IsDaily,
            IsMonthly = request.IsMonthly,
            IsMonthToDate = request.IsMonthToDate,
            IsQuarterly = request.IsQuarterly,
            IsYearEndProjection = request.IsYearEndProjection,
            IsYearToDaily = request.IsYearToDaily,
            IsYearToMonthly = request.IsYearToMonthly,
            IsRealTime = request.IsRealTime,
            IsHour = request.IsHour,
            IsQuarterToDate = request.IsQuarterToDate,
            IsWeekly = request.IsWeekly,
            IsFilter = false,
        };
        var frequency = GetRequestFrequencyHelper.GetFrequency(valueChainRequest);
        var nameFrequency = GetRequestFrequencyHelper.GetNameFrequency(valueChainRequest);
        var entityResponse = await GetValueChains(valueChainRequest, token);
        var productDtos = new List<ProductDto>();

        productDtos = request.ProductId.HasValue
            ? entityResponse.Products?.Where(c => c.Id == request.ProductId).ToList()
            : entityResponse.Products?.Where(c => request.ProductName! == c.Name!).ToList();

        if (!productDtos!.Any())
        {
            return null!;
        }

        var productionVolume = new List<ProductionVolume>();
        if (!string.IsNullOrEmpty(request.ProductName!))
        {
            productionVolume = productDtos!.Select(c => new ProductionVolume
            {
                Id = c.Id,
                Planned = c.Planned,
                Variance = c.Variance,
                Actual = c.Value,
                Name = c.Alias,
                Kpbi = c.Kbpi,
                Unit = c.Unit,
            }).ToList();
        }

        var entityDetailRespones = new GetProductDetailResponse();
        var listData = new List<SemDataDto?>();
        var listProductIds = new List<string>();
        productDtos!.ForEach(c => listProductIds.Add($"'{c.ProductionVolumeId}'"));
        var fromDate = request.ToDate.AddDays(-1);
        var toDate = request.ToDate;
        if (frequency == "hourly" || frequency == "15m")
        {
            fromDate = new DateTime(request.ToDate.Year, request.ToDate.Month, request.ToDate.Day, request.ToDate.Hour, 0, 0).ToUTC(TimeSpan.Zero);
            toDate = new DateTime(request.ToDate.Year, request.ToDate.Month, request.ToDate.Day, request.ToDate.Hour + 1, 0, 0).AddSeconds(-1).ToUTC(TimeSpan.Zero);
        }
        var query =
            $"SELECT pl.\"Name\", e.\"Name\" as EntityName, e.\"EntityParentId\", e.\"Id\" as Id, data.sem_data_id as SemDataId, data.amend_data_id, data.path_stream_id as PathStreamId, data.function_id, " +
            $"data.kpi_id as KpiId, data.frequency, data.data_date as DataDate, data.uom_name as UomName, data.actual_num as ActualNum, data.actual_str, data.target_num as TargetNum, data.target_str, data.variance, data.variance_percentage as VariancePercentage," +
            $"data.traffic_light, data.traffic_light_color, data.kpbi_traffic_light, data.kpbi_traffic_light_color, data.justification, data.remark, data.performance_analysis," +
            $"data.analysis_published_at, data.analysis_published_by, data.analysis_updated_at, data.analysis_updated_by, data.num_values, data.str_values, data.orig_num_values, data.orig_str_values, " +
            $"data.amend_num_values, data.amend_str_values, data.traffic_lights, data.created_at as CreatedDate, data.created_by, data.updated_at, data.updated_by  " +
            $"FROM pivot_da_middleware.get_de_sem_data('{frequency}') as data " +
            $"join pivot_da_middleware_digital.\"ProductLinks\" as pl on data.kpi_id = pl.\"Id\" " +
            $"join pivot_da_middleware_digital.\"Entities\" as e on pl.\"EntityMapId\" = e.\"EntityId\" " +
            $"where data.data_date >= '{fromDate.ToHyphenFormat()}' and data.data_date <= '{toDate.ToHyphenFormat()}' and e.\"Id\" IN ({string.Join(',', listProductIds)});";

        listData = await _readResultHelper.ExecuteResultFromQueryAsync<SemDataDto?>(_context, query, token);

        var justifiactions = listData.Where(c => c?.Name == DigitalTwinConstants.KPI_ProductionVolume).Select(c => new Justification
                {
                    Name = entityResponse.Products!.FirstOrDefault(e => e.ProductionVolumeId == c!.Id)?.Alias,
                    Kpi_Id = c!.KpiId,
                    Content = c.Justification,
                    UpdatedDate = c.Updated_At.HasValue ? c.Updated_At : c.CreatedDate,
                }).ToList();
        var paths = productDtos.First().Path!.Split(" > ").ToList();
        paths.RemoveAt(paths.Count - 2);

        var planned = productDtos!.Sum(c => c.Planned!);
        var kpbi = productDtos!.Sum(c => c.Kbpi!);
        entityDetailRespones.Data = new DataValueChainDetail
        {
            Actual = productDtos!.Sum(c => c.Value),
            Target = planned != 0 ? planned : null,
            Kpbi = kpbi != 0 ? kpbi : null,
            Variance = productDtos!.Sum(c => c.Variance!),
            VariancePercentage = productDtos!.Sum(c => c.Percentage!),
            Unit = productDtos!.First().Unit,
            Name =
                $"{(request.ProductId.HasValue ? productDtos.First().Path : productDtos.First().Name)} > {(request.ProductId.HasValue ? string.Empty : "Total ")}{(DigitalTwinConstants.Deliveries.Contains(productDtos.First().Category!) ? DigitalTwinConstants.CustomerName : DigitalTwinConstants.KPI_ProductionVolume)}",
            Id = Guid.NewGuid(),
            Justifications = justifiactions,
            Status = StatusEntity(productDtos.FirstOrDefault()?.Color!),
            TitleChart =
                $"{(request.ProductId.HasValue ? string.Empty : "Total ")}{(request.ProductId.HasValue ? string.Empty : $"{productDtos.First()?.Name} ")}{(DigitalTwinConstants.Deliveries.Contains(productDtos.First().Category!) ? DigitalTwinConstants.CustomerName : DigitalTwinConstants.KPI_ProductionVolume)} ({nameFrequency})",
            Alias = request.ProductId.HasValue ? productDtos.First().Alias : "Overall",
        };

        entityDetailRespones.ProductionVolume = productionVolume.Any() ? productionVolume : null;

        var chartRequest = new ChartRequest
        {
            ProductLinkId = productDtos.Select(c => c.ProductionVolumeId).ToList(),
            IsRealTime = request.IsRealTime,
            IsHour = request.IsHour,
            IsDaily = request.IsDaily,
            IsMonthly = request.IsMonthly,
            IsMonthToDate = request.IsMonthToDate,
            IsQuarterly = request.IsQuarterly,
            IsYearToDaily = request.IsYearToDaily,
            IsYearToMonthly = request.IsYearToMonthly,
            IsYearEndProjection = request.IsYearEndProjection,
            IsWeekly = request.IsWeekly,
            IsQuarterToDate = request.IsQuarterToDate,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            TimeZoneOffset = request.TimeZoneOffset,
        };

        entityDetailRespones.ChartData = _chartService.GetChart(chartRequest, token);
        entityDetailRespones.IsShowChart =
            entityDetailRespones.ChartData.Actual!.Any(c => !string.IsNullOrEmpty(c));
        return await Task.FromResult(Response.CreateResponse(entityDetailRespones));
    }
}