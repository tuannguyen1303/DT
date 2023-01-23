create or replace procedure pivot_da_middleware_digital.p_syncentities(INOUT can_update boolean DEFAULT NULL::boolean)
    language plpgsql
as
$$
declare
current_entities_appdb INTEGER := 0;
    is_view_entity_exist   BOOLEAN := true;
    is_view_kpi_exist      BOOLEAN := true;
    gas_type_id            uuid    := 'ab0d6efa-c806-8492-09b1-2a004afb9b2a';
    oil_type_id            uuid    := 'faf7a077-4214-acc6-22d0-82597e755126';
    has_update             BOOLEAN := true;
begin
select exists(select 1 from pivot_da_middleware.view_common_path_stream) into is_view_entity_exist;
select exists(select 1 from pivot_da_middleware.view_common_path_stream_kpi) into is_view_kpi_exist;

if is_view_entity_exist = false then
        can_update := false;
    elseif is_view_kpi_exist = false then
        can_update := false;
    else
            can_update := true;
end if;

    if can_update = true then
select count(vcps)
into current_entities_appdb
from pivot_da_middleware.view_common_path_stream vcps
where not exists(
        select 1
        from pivot_da_middleware_digital."Entities" e
        where e."ValueChainTypeId" <> oil_type_id
          and vcps.id = e."EntityId"
    )
  and is_standard = true
  and vcps.level_id > 1;

if current_entities_appdb > 0 then
            has_update := true;

            create temp table if not exists new_entity_tmp_tbl
            (
                id               uuid,
                entity_name      text,
                entity_type_id   int,
                entity_parent_id uuid,
                path             text,
                depth            int
            ) on commit drop;

insert into new_entity_tmp_tbl (id,
                                entity_name,
                                entity_type_id,
                                entity_parent_id,
                                path,
                                depth)
select vcps.id,
       vcps.entity_name,
       vcps.entity_id,
       coalesce(vcps.entity_parent_id, null),
       vcps."path",
       vcps."depth"
from pivot_da_middleware.view_common_path_stream vcps
where not exists(
        select 1
        from pivot_da_middleware_digital."Entities" e
        where e."ValueChainTypeId" <> oil_type_id
          and vcps.id = e."EntityId"
    )
  and is_standard = true
  and vcps.level_id > 1;

insert into pivot_da_middleware_digital."Entities" ("Id", "EntityId", "Name", "EntityTypeMasterId",
                                                    "EntityTypeId",
                                                    "EntityParentId", "KpiPath", "Depth", "CreatedTime",
                                                    "UpdatedTime", "ValueChainTypeId")
select uuid_in(md5(random()::text || random()::text)::cstring) as Id,
       vcps.id                                                 as EntityId,
       vcps.entity_name                                        as Name,
       vcps.entity_type_id                                     as EntityTypeMasterId,
       et."Id"                                                 as EntityTypeId,
       vcps.entity_parent_id                                   as EntityParentId,
       vcps."path"                                             as KpiPath,
       vcps."depth"                                            as Depth,
       current_timestamp,
       current_timestamp,
       gas_type_id
from new_entity_tmp_tbl as vcps
         join pivot_da_middleware_digital."EntityTypes" et on vcps.entity_type_id = et."EntityId";

insert into pivot_da_middleware_digital."ProductLinks" ("Id", "UnitOfMeasureId", "EntityMapId",
                                                        "Name", "FullName", "CreatedTime", "UpdatedTime")
select vcpsk.id            as Id
     , uom."Id"            as UnitOfMeasureId
     , vcpsk.entity_map_id as EntityMapId
     , vcpsk.kpi_name      as Name
     , vcpsk.kpi_fullname  as FullName
     , current_timestamp
     , current_timestamp
from pivot_da_middleware.view_common_path_stream_kpi vcpsk
         join new_entity_tmp_tbl e on vcpsk.entity_map_id = e.id
         join pivot_da_middleware_digital."UnitOfMeasures" uom on vcpsk.uom_id = uom."UomId"
where has_traffic_light = true;

insert into pivot_da_middleware_digital."ProductLinkDetails" ("Id", "ProductLinkId", "Frequency",
                                                              "DataDate",
                                                              "IsRealTime", "IsWeekly", "IsQuarterToDate",
                                                              "IsYearToMonthly",
                                                              "IsDaily", "IsMonthly", "IsMonthToDate",
                                                              "IsQuarterly",
                                                              "IsYearEndProjection", "IsYearToDaily",
                                                              "UomName",
                                                              "Name",
                                                              "CreatedTime", "UpdatedTime", "Color",
                                                              "NorCode",
                                                              "Percentage",
                                                              "Value",
                                                              "Variance",
                                                              "CreatedBy",
                                                              "UpdatedBy",
                                                              "NumValues")
select uuid_in(md5(random()::text || random()::text)::cstring) as Id,
       acad.kpi_id,
       acad.frequency,
       acad.data_date,
       false,
       false,
       false,
       false,
       vcpsk.is_daily                                          as IsDaily,
       vcpsk.is_monthly                                        as IsMonthly,
       vcpsk.is_mtd                                            as IsMonthToDate,
       vcpsk.is_quarterly                                      as IsQuarterly,
       vcpsk.is_yep                                            as IsYearEndProjection,
       vcpsk.is_ytd                                            as IsYearToDaily,
       acad.uom_name,
       '',
       current_timestamp,
       current_timestamp,
       acad.traffic_lights::jsonb -> 'norm_color'              as Color,
        acad.traffic_lights::jsonb -> 'norm_code'               as NorCode,
        (acad.num_values ->> 'variance_percentage')::numeric,
        (acad.num_values ->> 'actual')::numeric,
        (acad.num_values ->> 'variance')::numeric,
        0,
       0,
       acad.num_values                                         as NumValues
from pivot_da_middleware.agg_common_amend_data acad
         join pivot_da_middleware.view_common_path_stream_kpi vcpsk
              on acad.kpi_id = vcpsk.id
         join new_entity_tmp_tbl e
              on vcpsk.entity_map_id = e.id;

else
            has_update := false;
end if;
end if;

commit;
end
$$;

