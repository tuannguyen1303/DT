insert into pivot_da_middleware_digital."Entities" ("Id",
                                                    "EntityId", "Name", "EntityTypeMasterId", "EntityTypeId",
                                                    "EntityParentId", "KpiPath", "Depth", "IsDaily", "IsMonthly",
                                                    "IsMonthToDate", "IsQuarterly", "IsYearEndProjection",
                                                    "IsYearToDaily", "DataDate", "ValueChainTypeId", "IsRealTime",
                                                    "IsWeekly", "IsQuarterToDate", "IsYearToMonthly", "CreatedTime",
                                                    "UpdatedTime")
select uuid_in(md5(random()::text || random()::text)::cstring) as Id,
       vcps.id                                                 as EntityId,
       vcps.entity_name                                        as Name,
       vcps.entity_id                                          as EntityTypeMasterId,
       et."Id"                                                 as EntityTypeId,
       vcps.entity_parent_id                                   as EntityParentId,
       vcps."path"                                             as KpiPath,
       vcps."depth"                                            as Depth,
       vcpsk.is_daily                                          as IsDaily,
       vcpsk.is_monthly                                        as IsMonthly,
       vcpsk.is_mtd                                            as IsMonthToDate,
       vcpsk.is_quarterly                                      as IsQuarterly,
       vcpsk.is_yep                                            as IsYearEndProjection,
       vcpsk.is_ytd                                            as IsYearToDaily,
       acad.data_date                                          as DataDate,
       'ab0d6efa-c806-8492-09b1-2a004afb9b2a'                  as ValueChainTypeId,
       false,
       false,
       false,
       false,
       '2022-09-21 16:28:17.913 +0700',
       '2022-09-21 16:28:17.913 +0700'
from pivot_da_middleware.view_common_path_stream vcps
         join pivot_da_middleware_digital."EntityTypes" et on vcps.entity_id = et."EntityId"
         join pivot_da_middleware.view_common_path_stream_kpi vcpsk on vcps.id = vcpsk.entity_map_id
         join pivot_da_middleware.agg_common_amend_data acad on vcpsk.id = acad.kpi_id
where is_standard = true
  and vcps.level_id > 1
order by vcps.level_id;