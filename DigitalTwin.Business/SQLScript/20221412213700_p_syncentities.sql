create procedure p_syncentities(INOUT can_update boolean DEFAULT NULL::boolean)
    language plpgsql
as
$$
declare
current_entities_appdb           INTEGER := 0;
    current_entity_rows              INTEGER := 0;
    current_product_link_rows        INTEGER := 0;
    current_product_link_detail_rows INTEGER := 0;
    current_agg_amend_data_rows      INTEGER := 0;
    dummy_value                      INTEGER := 1995;
    is_has_data                      BOOLEAN := true;
    is_view_entity_exist             BOOLEAN := true;
    is_view_kpi_exist                BOOLEAN := true;
    gas_type_id                      uuid    := 'ab0d6efa-c806-8492-09b1-2a004afb9b2a';
    oil_type_id                      uuid    := 'faf7a077-4214-acc6-22d0-82597e755126';
    has_update                       BOOLEAN := true;
begin
select count(e.*) into current_entity_rows from pivot_da_middleware_digital."Entities" e;
select count(pl.*) into current_product_link_rows from pivot_da_middleware_digital."ProductLinks" pl;
select count(pld.*) into current_product_link_detail_rows from pivot_da_middleware_digital."ProductLinkDetails" pld;

if (current_entity_rows = 0 and current_product_link_rows = 0 and current_product_link_detail_rows = 0) then
        is_has_data := false;
end if;

    if (is_has_data = true) then
select exists(select 1 from pivot_da_middleware.view_common_path_stream) into is_view_entity_exist;
select exists(select 1 from pivot_da_middleware.view_common_path_stream_kpi) into is_view_kpi_exist;

if (is_view_entity_exist = false and is_view_kpi_exist = false) then
            can_update := false;
else
            can_update := true;
end if;

        if can_update = true then
            create temp table if not exists new_entity_tmp_tbl
            (
                id               uuid,
                entity_name      text,
                entity_type_id   int,
                entity_parent_id uuid,
                path             text,
                depth            int,
                root_name        text,
                root_id          int,
                level_id         int,
                density          text
            ) on commit drop;

select count(vcps)
into current_entities_appdb
from pivot_da_middleware.view_common_path_stream vcps
where not exists(
        select 1
        from pivot_da_middleware_digital."Entities" e
        where e."ValueChainTypeId" = gas_type_id
          and vcps.id = e."EntityId"
    )

  and vcps.level_id > 1;

if current_entities_appdb > 0 then
                has_update := true;

insert into new_entity_tmp_tbl (id,
                                entity_name,
                                entity_type_id,
                                entity_parent_id,
                                path,
                                depth,
                                root_name,
                                root_id,
                                level_id,
                                density)
select vcps.id,
       vcps.entity_name,
       vcps.entity_id,
       coalesce(vcps.entity_parent_id, null),
       vcps."path",
       vcps."depth",
       vcps.root_name,
       vcps.root_id,
       vcps.level_id,
       vcps.density
from pivot_da_middleware.view_common_path_stream vcps
where exists(
        select 1
        from pivot_da_middleware_digital."Entities" e
        where e."ValueChainTypeId" <> oil_type_id
          and vcps.id <> e."EntityId"
    )

  and vcps.level_id > 1;

insert into pivot_da_middleware_digital."Entities" ("Id", "EntityId", "Name", "EntityTypeMasterId",
                                                    "EntityTypeId",
                                                    "EntityParentId", "KpiPath", "Depth", "CreatedTime",
                                                    "UpdatedTime", "ValueChainTypeId", "RootName",
                                                    "RootId", "Density", "LevelId")
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
       gas_type_id,
       vcps.root_name,
       vcps.root_id,
       vcps.density,
       vcps.level_id
from new_entity_tmp_tbl as vcps
         join pivot_da_middleware_digital."EntityTypes" et on vcps.entity_type_id = et."EntityId";

insert into pivot_da_middleware_digital."ProductLinks" ("Id", "UnitOfMeasureId", "EntityMapId",
                                                        "Name", "FullName", "CreatedTime",
                                                        "UpdatedTime", "LevelId", "LevelName")
select vcpsk.id            as Id
     , uom."Id"            as UnitOfMeasureId
     , vcpsk.entity_map_id as EntityMapId
     , vcpsk.kpi_name      as Name
     , vcpsk.kpi_fullname  as FullName
     , current_timestamp
     , current_timestamp
     , vcpsk.level_id
     , vcpsk.level_name
from pivot_da_middleware.view_common_path_stream_kpi vcpsk
         join new_entity_tmp_tbl e on vcpsk.entity_map_id = e.id
         join pivot_da_middleware_digital."UnitOfMeasures" uom on vcpsk.uom_id = uom."UomId"
where has_traffic_light = true;

insert into pivot_da_middleware_digital."ProductLinkDetails" ("Id", "ProductLinkId", "Frequency",
                                                              "DataDate",
                                                              "IsRealTime", "IsWeekly",
                                                              "IsQuarterToDate",
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

select count(vcps.*)
into current_entities_appdb
from pivot_da_middleware.view_common_path_stream vcps
where exists(select 1
             from pivot_da_middleware_digital."Entities" e
             where vcps.id = e."EntityId"
               and e."ValueChainTypeId" = 'ab0d6efa-c806-8492-09b1-2a004afb9b2a'
               and vcps.entity_name <> e."Name"
               and vcps.path <> e."KpiPath"
               and e."CreatedBy" <> dummy_value
               and e."UpdatedBy" <> dummy_value)
  and vcps.is_standard = true
  and vcps.level_id > 1;

-- Update Entity's Name and Path when they were changed from AppDB  begin
if (current_entities_appdb) then
                    can_update := true;
update pivot_da_middleware_digital."Entities"
set "Name"    = tmp_entities.entity_name,
    "KpiPath" = tmp_entities.path
    from (select vcps.id,
                                 entity_parent_id,
                                 entity_id,
                                 entity_name,
                                 path,
                                 level_id,
                                 root_name,
                                 root_id,
                                 density,
                                 depth,
                                 is_standard,
                                 subgroup,
                                 subgroup_mst_id,
                                 serialized_path
                          from pivot_da_middleware.view_common_path_stream vcps
                          where exists(select 1
                                       from pivot_da_middleware_digital."Entities" e
                                       where vcps.id = e."EntityId"
                                         and e."ValueChainTypeId" = 'ab0d6efa-c806-8492-09b1-2a004afb9b2a'
                                         and vcps.entity_name <> e."Name"
                                         and vcps.path <> e."KpiPath"
                                         and e."CreatedBy" <> dummy_value
                                         and e."UpdatedBy" <> dummy_value)
                            and vcps.is_standard = true
                            and vcps.level_id > 1) as tmp_entities
where "EntityId" = tmp_entities.id;
end if;
                -- Update Entity's Name and Path when they were changed from AppDB  end

                -- Insert new ProductLinks with existing entities begin
select count(vcps.*)
into current_product_link_rows
from pivot_da_middleware.view_common_path_stream_kpi vcpsk
         join pivot_da_middleware.view_common_path_stream vcps on vcpsk.entity_map_id = vcps.id
where not exists(select NULL
                 from pivot_da_middleware_digital."ProductLinks" pl
                 where vcpsk.id = pl."Id"
                   and pl."CreatedBy" <> 1995
                   and pl."UpdatedBy" <> 1995
                   and pl."LevelId" is not null
                   and pl."LevelName" is not null
    )
  and vcps.level_id > 1
  and has_traffic_light = true;

if (current_product_link_rows > 0) then
                    can_update := true;
insert into pivot_da_middleware_digital."ProductLinks" ("Id", "UnitOfMeasureId", "EntityMapId",
                                                        "Name", "FullName", "CreatedTime",
                                                        "UpdatedTime", "LevelId", "LevelName")
select distinct vcpsk.id            as Id
              , uom."Id"            as UnitOfMeasureId
              , vcpsk.entity_map_id as EntityMapId
              , vcpsk.kpi_name      as Name
              , vcpsk.kpi_fullname  as FullName
              , current_timestamp
              , current_timestamp
              , vcpsk.level_id
              , vcpsk.level_name
from pivot_da_middleware.view_common_path_stream_kpi vcpsk
         join pivot_da_middleware.view_common_path_stream vcps on vcpsk.entity_map_id = vcps.id
         left join pivot_da_middleware_digital."UnitOfMeasures" uom on vcpsk.uom_id = uom."UomId"
where not exists(select NULL
                 from pivot_da_middleware_digital."ProductLinks" pl
                 where vcpsk.id = pl."Id"
                   and pl."CreatedBy" <> 1995
                   and pl."UpdatedBy" <> 1995
                   and pl."LevelId" is not null
                   and pl."LevelName" is not null
    )
  and vcps.level_id > 1
  and has_traffic_light = true;
end if;
                -- Insert new ProductLinks with existing entities end

                -- Checking ProductLinkDetails when there are new records begin
select count(acad.*)
into current_agg_amend_data_rows
from pivot_da_middleware.agg_common_amend_data acad
where exists(select 1
             from pivot_da_middleware_digital."ProductLinkDetails" pld
                      join pivot_da_middleware_digital."ProductLinks" pl
                           on pld."ProductLinkId" = pl."Id"
                               and pl."CreatedBy" <> dummy_value
                               and pl."UpdatedBy" <> dummy_value
                      join pivot_da_middleware_digital."Entities" e on pl."EntityMapId" = e."EntityId"
                 and e."CreatedBy" <> dummy_value
                 and e."UpdatedBy" <> dummy_value
                 and e."ValueChainTypeId" = 'ab0d6efa-c806-8492-09b1-2a004afb9b2a'
             where acad.kpi_id = pl."Id"
               and acad.frequency = pld."Frequency"
               and acad.data_date <> pld."DataDate"::TIMESTAMP WITHOUT TIME ZONE
             and pl."CreatedBy" <> dummy_value
                 and pl."UpdatedBy" <> dummy_value)
  and acad.traffic_lights::jsonb <> '{}'::jsonb;

if (current_agg_amend_data_rows > 0) then
                    create temp table if not exists new_kpi_tmp_tbl
                    (
                        id                uuid,
                        is_daily          boolean,
                        is_monthly        boolean,
                        is_mtd            boolean,
                        is_quarterly      boolean,
                        is_yep            boolean,
                        is_ytd            boolean,
                        has_traffic_light boolean
                    ) on commit drop;

insert into new_kpi_tmp_tbl (id, is_daily, is_monthly, is_mtd, is_quarterly, is_yep, is_ytd,
                             has_traffic_light)
select vcpsk.id,
       vcpsk.is_daily,
       vcpsk.is_monthly,
       vcpsk.is_mtd,
       vcpsk.is_quarterly,
       vcpsk.is_yep,
       vcpsk.is_ytd,
       vcpsk.has_traffic_light
from pivot_da_middleware.view_common_path_stream_kpi vcpsk
where exists(select 1
             from pivot_da_middleware_digital."ProductLinks" pl
                      join pivot_da_middleware_digital."Entities" e
                           on pl."EntityMapId" = e."EntityId"
                               and e."ValueChainTypeId" = gas_type_id
                               and e."CreatedBy" <> dummy_value
                               and e."UpdatedBy" <> dummy_value
             where vcpsk.id = pl."Id"
               and pl."CreatedBy" <> dummy_value
               and pl."UpdatedBy" <> dummy_value)
  and vcpsk.has_traffic_light = true;

insert into pivot_da_middleware_digital."ProductLinkDetails" ("Id", "ProductLinkId", "Frequency",
                                                              "DataDate",
                                                              "IsRealTime", "IsWeekly",
                                                              "IsQuarterToDate",
                                                              "IsYearToMonthly",
                                                              "IsDaily", "IsMonthly",
                                                              "IsMonthToDate",
                                                              "IsQuarterly",
                                                              "IsYearEndProjection",
                                                              "IsYearToDaily",
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
         join new_kpi_tmp_tbl vcpsk on acad.kpi_id = vcpsk.id
where exists(select 1
             from pivot_da_middleware_digital."ProductLinkDetails" pld
                      join pivot_da_middleware_digital."ProductLinks" pl
                           on pld."ProductLinkId" = pl."Id"
                               and pl."CreatedBy" <> dummy_value
                               and pl."UpdatedBy" <> dummy_value
                      join pivot_da_middleware_digital."Entities" e
                           on pl."EntityMapId" = e."EntityId"
                               and e."CreatedBy" <> dummy_value
                               and e."UpdatedBy" <> dummy_value
                               and e."ValueChainTypeId" = 'ab0d6efa-c806-8492-09b1-2a004afb9b2a'
             where acad.kpi_id = pl."Id"
               and acad.frequency = pld."Frequency"
               and acad.data_date <> pld."DataDate"::TIMESTAMP WITHOUT TIME ZONE
             and pl."CreatedBy" <> dummy_value
                 and pl."UpdatedBy" <> dummy_value)
  and acad.traffic_lights::jsonb <> '{}'::jsonb;
end if;
                -- Checking ProductLinkDetails when there are new records end
end if;
end if;
else
        can_update := false;

        -- Insert data into Entities begin
insert into pivot_da_middleware_digital."Entities" ("Id", "EntityId", "Name", "EntityTypeMasterId",
                                                    "EntityTypeId",
                                                    "EntityParentId", "KpiPath", "Depth", "CreatedTime",
                                                    "UpdatedTime",
                                                    "ValueChainTypeId", "LevelId", "Density", "RootName",
                                                    "RootId")
select uuid_in(md5(random()::text || random()::text)::cstring) as Id,
       vcps.id                                                 as EntityId,
       vcps.entity_name                                        as Name,
       vcps.entity_id                                          as EntityTypeMasterId,
       et."Id"                                                 as EntityTypeId,
       vcps.entity_parent_id                                   as EntityParentId,
       vcps."path"                                             as KpiPath,
       vcps."depth"                                            as depth,
       current_timestamp,
       current_timestamp,
       gas_type_id,
       vcps.level_id,
       vcps.density,
       vcps.root_name,
       vcps.root_id
from pivot_da_middleware.view_common_path_stream vcps
         join pivot_da_middleware_digital."EntityTypes" et on vcps.entity_id = et."EntityId"
where is_standard = true
order by vcps.level_id;
-- Insert data into Entities end

-- Insert data into ProductLinks begin
insert into pivot_da_middleware_digital."ProductLinks" ("Id", "UnitOfMeasureId", "EntityMapId",
                                                        "Name", "FullName", "CreatedTime", "UpdatedTime",
                                                        "LevelId", "LevelName")
select vcpsk.id            as Id
     , uom."Id"            as UnitOfMeasureId
     , vcpsk.entity_map_id as EntityMapId
     , vcpsk.kpi_name      as Name
     , vcpsk.kpi_fullname  as FullName
     , current_timestamp
     , current_timestamp
     , vcpsk.level_id
     , vcpsk.level_name
from pivot_da_middleware.view_common_path_stream_kpi vcpsk
         join pivot_da_middleware_digital."Entities" e on vcpsk.entity_map_id = e."EntityId"
         join pivot_da_middleware_digital."UnitOfMeasures" uom on vcpsk.uom_id = uom."UomId"
where has_traffic_light = true
union
select vcpsk.id            as Id
     , uom."Id"            as UnitOfMeasureId
     , vcpsk.entity_map_id as EntityMapId
     , vcpsk.kpi_name      as Name
     , vcpsk.kpi_fullname  as FullName
     , current_timestamp
     , current_timestamp
     , vcpsk.level_id
     , vcpsk.level_name
from pivot_da_middleware.view_common_path_stream_kpi vcpsk
         join pivot_da_middleware_digital."Entities" e on vcpsk.entity_map_id = e."EntityId"
         join pivot_da_middleware_digital."UnitOfMeasures" uom on vcpsk.uom_id = uom."UomId"
where vcpsk.kpi_name = 'Plant Load';
-- Insert data into ProductLinks end

-- Insert data into ProductLinkDetails begin
insert into pivot_da_middleware_digital."ProductLinkDetails" ("Id", "ProductLinkId", "Frequency", "DataDate",
                                                              "IsRealTime", "IsWeekly", "IsQuarterToDate",
                                                              "IsYearToMonthly",
                                                              "IsDaily", "IsMonthly", "IsMonthToDate",
                                                              "IsQuarterly",
                                                              "IsYearEndProjection", "IsYearToDaily", "UomName",
                                                              "Name",
                                                              "CreatedTime", "UpdatedTime", "Color", "NorCode",
                                                              "Percentage",
                                                              "Value",
                                                              "Variance",
                                                              "CreatedBy",
                                                              "UpdatedBy", "NumValues")
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
from pivot_da_middleware.view_common_path_stream_kpi vcpsk
         join pivot_da_middleware_digital."Entities" e on vcpsk.entity_map_id = e."EntityId"
         join pivot_da_middleware.agg_common_amend_data acad
              on vcpsk.id = acad.kpi_id
                  and acad.traffic_lights::jsonb <> '{}'::jsonb
where vcpsk.has_traffic_light = true
union
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
                  and vcpsk.kpi_name = 'Plant Load'
         join pivot_da_middleware_digital."Entities" e
              on vcpsk.entity_map_id = e."EntityId";
-- Insert data into ProductLinkDetails end
end if;

commit;
end
$$;

alter procedure p_syncentities(inout boolean) owner to pivotdadevdbuser;