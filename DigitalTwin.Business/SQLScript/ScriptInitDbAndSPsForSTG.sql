create table pivot_da_middleware_digital."EntityTypes"
(
    "Id"              uuid                     not null
        constraint "PK_EntityTypes"
            primary key,
    "Name"            text,
    "CreatedTime"     timestamp with time zone not null,
    "UpdatedTime"     timestamp with time zone not null,
    "EntityGroupName" text        default ''::text,
    "EntityId"        integer     default 0    not null,
    "FullName"        text        default ''::text,
    "CreatedBy"       numeric(20) default 0.0  not null,
    "UpdatedBy"       numeric(20) default 0.0  not null
);

alter table pivot_da_middleware_digital."EntityTypes"
    owner to pdadbstguser;

create table pivot_da_middleware_digital."ProductLinkStatuses"
(
    "Id"          uuid                     not null
        constraint "PK_ProductLinkStatuses"
            primary key,
    "NorCode"     text,
    "Color"       text,
    "Name"        text,
    "CreatedTime" timestamp with time zone not null,
    "UpdatedTime" timestamp with time zone not null,
    "CreatedBy"   numeric(20) default 0.0  not null,
    "UpdatedBy"   numeric(20) default 0.0  not null
);

alter table pivot_da_middleware_digital."ProductLinkStatuses"
    owner to pdadbstguser;

create table pivot_da_middleware_digital."UnitOfMeasures"
(
    "Id"          uuid                     not null
        constraint "PK_UnitOfMeasures"
            primary key,
    "Name"        text,
    "CreatedTime" timestamp with time zone not null,
    "UpdatedTime" timestamp with time zone not null,
    "UomId"       integer     default 0    not null,
    "CreatedBy"   numeric(20) default 0.0  not null,
    "UpdatedBy"   numeric(20) default 0.0  not null
);

alter table pivot_da_middleware_digital."UnitOfMeasures"
    owner to pdadbstguser;

create table pivot_da_middleware_digital."ValueChainTypes"
(
    "Id"          uuid                     not null
        constraint "PK_ValueChainTypes"
            primary key,
    "Type"        integer                  not null,
    "Name"        text,
    "CreatedTime" timestamp with time zone not null,
    "UpdatedTime" timestamp with time zone not null,
    "CreatedBy"   numeric(20) default 0.0  not null,
    "UpdatedBy"   numeric(20) default 0.0  not null
);

alter table pivot_da_middleware_digital."ValueChainTypes"
    owner to pdadbstguser;

create table pivot_da_middleware_digital."Entities"
(
    "Id"                 uuid                                                             not null
        constraint "PK_Entities"
            primary key,
    "EntityTypeId"       uuid
        constraint "FK_Entities_EntityTypes_EntityTypeId"
            references pivot_da_middleware_digital."EntityTypes",
    "ValueChainTypeId"   uuid
        constraint "FK_Entities_ValueChainTypes_ValueChainTypeId"
            references pivot_da_middleware_digital."ValueChainTypes",
    "KpiPath"            text,
    "Name"               text,
    "CreatedTime"        timestamp with time zone                                         not null,
    "UpdatedTime"        timestamp with time zone                                         not null,
    "CreatedBy"          numeric(20) default 0.0                                          not null,
    "Depth"              integer     default 0                                            not null,
    "EntityParentId"     uuid,
    "UpdatedBy"          numeric(20) default 0.0                                          not null,
    "EntityTypeMasterId" integer     default 0                                            not null,
    "EntityId"           uuid        default '00000000-0000-0000-0000-000000000000'::uuid not null,
    "LevelId"            integer,
    "Density"            varchar,
    "RootName"           varchar,
    "RootId"             integer
);

alter table pivot_da_middleware_digital."Entities"
    owner to pdadbstguser;

create index "IX_Entities_EntityTypeId"
    on pivot_da_middleware_digital."Entities" ("EntityTypeId");

create index "IX_Entities_ValueChainTypeId"
    on pivot_da_middleware_digital."Entities" ("ValueChainTypeId");

create table pivot_da_middleware_digital."ProductLinks"
(
    "Id"              uuid                     not null
        constraint "PK_ProductLinks"
            primary key,
    "UnitOfMeasureId" uuid
        constraint "FK_ProductLinks_UnitOfMeasures_UnitOfMeasureId"
            references pivot_da_middleware_digital."UnitOfMeasures",
    "EntityMapId"     uuid,
    "Value"           numeric,
    "Percentage"      numeric,
    "Variance"        numeric,
    "Name"            text,
    "CreatedTime"     timestamp with time zone not null,
    "UpdatedTime"     timestamp with time zone not null,
    "CreatedBy"       numeric(20) default 0.0  not null,
    "UpdatedBy"       numeric(20) default 0.0  not null,
    "FullName"        text,
    "LevelId"         integer,
    "LevelName"       varchar2
);

alter table pivot_da_middleware_digital."ProductLinks"
    owner to pdadbstguser;

create index "IX_ProductLinks_UnitOfMeasureId"
    on pivot_da_middleware_digital."ProductLinks" ("UnitOfMeasureId");

create table pivot_da_middleware_digital."ProductLinkDetails"
(
    "Id"                  uuid                     not null
        constraint "PK_ProductLinkDetails"
            primary key,
    "ProductLinkId"       uuid,
    "Frequency"           text,
    "DataDate"            timestamp with time zone,
    "IsRealTime"          boolean                  not null,
    "IsDaily"             boolean                  not null,
    "IsWeekly"            boolean                  not null,
    "IsMonthly"           boolean                  not null,
    "IsMonthToDate"       boolean                  not null,
    "IsQuarterly"         boolean                  not null,
    "IsQuarterToDate"     boolean                  not null,
    "IsYearToDaily"       boolean                  not null,
    "IsYearToMonthly"     boolean                  not null,
    "IsYearEndProjection" boolean                  not null,
    "UomName"             text,
    "Name"                text,
    "CreatedTime"         timestamp with time zone not null,
    "UpdatedTime"         timestamp with time zone not null,
    "CreatedBy"           numeric(20)              not null,
    "UpdatedBy"           numeric(20)              not null,
    "Color"               text,
    "NorCode"             text,
    "Percentage"          numeric,
    "Value"               numeric,
    "Variance"            numeric,
    "NumValues"           jsonb
);

alter table pivot_da_middleware_digital."ProductLinkDetails"
    owner to pdadbstguser;

create procedure pivot_da_middleware_digital.p_syncmasterdata(INOUT can_update boolean DEFAULT NULL::boolean)
    language plpgsql
as
$$
declare
    is_first_init         BOOLEAN := false;
    value_chain_type_rows int     := 0;
    uom_rows              int     := 0;
    entity_types_rows     int     := 0;
    new_entity_types      int     := 0;
    new_uom               int     := 0;
    is_view_kpi_exist     BOOLEAN := true;
begin
    select count(vct.*) into value_chain_type_rows from pivot_da_middleware_digital."ValueChainTypes" vct;
    select count(umo.*) into uom_rows from pivot_da_middleware_digital."UnitOfMeasures" umo;
    select count(et.*) into entity_types_rows from pivot_da_middleware_digital."EntityTypes" et;

    if (value_chain_type_rows = 0 and uom_rows = 0 and entity_types_rows = 0) then
        is_first_init := true;
    end if;

    if (is_first_init = true) then
        can_update := false;
        if value_chain_type_rows = 0 then
            -- Insert data into ValueChainTypes table begin
            INSERT INTO pivot_da_middleware_digital."ValueChainTypes" ("Id", "Type", "Name", "CreatedTime",
                                                                       "UpdatedTime",
                                                                       "CreatedBy", "UpdatedBy")
            VALUES ('ab0d6efa-c806-8492-09b1-2a004afb9b2a', 1, 'Gas chain', current_timestamp,
                    current_timestamp, 0, 0);
            INSERT INTO pivot_da_middleware_digital."ValueChainTypes" ("Id", "Type", "Name", "CreatedTime",
                                                                       "UpdatedTime",
                                                                       "CreatedBy", "UpdatedBy")
            VALUES ('faf7a077-4214-acc6-22d0-82597e755126', 2, 'Oil chain', current_timestamp,
                    current_timestamp, 0, 0);
            -- Insert data into ValueChainTypes table end

            -- Insert data into Unit Of Measure table
            create temp table if not exists new_uom_tmp
            (
                id   int,
                name text
            ) on commit drop;

            insert into new_uom_tmp (id, name)
            select distinct COALESCE(uom_id, 0) UomId, COALESCE(uom_name, '') as Name
            from pivot_da_middleware.view_common_path_stream_kpi vcpsk;

            insert into pivot_da_middleware_digital."UnitOfMeasures" ("Id",
                                                                      "Name",
                                                                      "CreatedTime",
                                                                      "UpdatedTime",
                                                                      "UomId",
                                                                      "CreatedBy",
                                                                      "UpdatedBy")
            select uuid_in(md5(random()::text || random()::text)::cstring),
                   new_uom_tmp.name,
                   current_timestamp,
                   current_timestamp,
                   new_uom_tmp.id,
                   0,
                   0
            from new_uom_tmp;
            -- Insert data into Unit Of Measure table end

            -- Insert data into EntityTypes table begin
            insert into pivot_da_middleware_digital."EntityTypes"("Id", "Name", "CreatedTime", "UpdatedTime",
                                                                  "EntityGroupName", "EntityId", "FullName",
                                                                  "CreatedBy", "UpdatedBy")
            select uuid_in(md5(random()::text || random()::text)::cstring),
                   mme.entity_name,
                   current_timestamp,
                   current_timestamp,
                   mme.entity_group,
                   mme.id,
                   mme.entity_fullname,
                   0,
                   0
            from pivot_da_middleware.meta_mst_entity mme;
            -- Insert data into EntityTypes table end
        end if;
    else
        select exists(select 1 from pivot_da_middleware.view_common_path_stream_kpi) into is_view_kpi_exist;

        if is_view_kpi_exist = false then
            can_update := false;
        else
            can_update := true;
        end if;

        if can_update = true then
            select count(mme)
            into new_entity_types
            from pivot_da_middleware.meta_mst_entity mme
            where not exists(
                    select 1
                    from pivot_da_middleware_digital."EntityTypes" et
                    where mme.id = et."EntityId");

            select count(distinct COALESCE(uom_id, 0))
            into new_uom
            from pivot_da_middleware.view_common_path_stream_kpi vcpsk
            where not exists(select 1
                             from pivot_da_middleware_digital."UnitOfMeasures" uom
                             where uom."UomId" = vcpsk.uom_id)
              and vcpsk.uom_id <> 0;

            if new_entity_types > 0 then
                insert into pivot_da_middleware_digital."EntityTypes" ("EntityId",
                                                                       "Name",
                                                                       "FullName",
                                                                       "EntityGroupName",
                                                                       "Id",
                                                                       "CreatedTime",
                                                                       "UpdatedTime",
                                                                       "CreatedBy",
                                                                       "UpdatedBy")
                select mme.id,
                       mme.entity_name,
                       mme.entity_fullname,
                       mme.entity_group,
                       uuid_in(md5(random()::text || random()::text)::cstring),
                       current_timestamp,
                       current_timestamp,
                       0,
                       0
                from pivot_da_middleware.meta_mst_entity mme
                where not exists(
                        select 1
                        from pivot_da_middleware_digital."EntityTypes" et
                        where mme.id = et."EntityId"
                    );
            end if;

            if new_uom > 0 then
                create temp table if not exists new_uom_tmp
                (
                    id   int,
                    name text
                ) on commit drop;

                insert into new_uom_tmp (id, name)
                select distinct COALESCE(uom_id, 0) UomId, COALESCE(uom_name, '') as Name
                from pivot_da_middleware.view_common_path_stream_kpi vcpsk
                where not exists(select 1
                                 from pivot_da_middleware_digital."UnitOfMeasures" uom
                                 where uom."UomId" = vcpsk.uom_id)
                  and vcpsk.uom_id <> 0;

                insert into pivot_da_middleware_digital."UnitOfMeasures" ("Id",
                                                                          "Name",
                                                                          "CreatedTime",
                                                                          "UpdatedTime",
                                                                          "UomId",
                                                                          "CreatedBy",
                                                                          "UpdatedBy")
                select uuid_in(md5(random()::text || random()::text)::cstring),
                       new_uom_tmp.name,
                       current_timestamp,
                       current_timestamp,
                       new_uom_tmp.id,
                       0,
                       0
                from new_uom_tmp;
            end if;
        end if;
    end if;

    commit;
end
$$;

alter procedure pivot_da_middleware_digital.p_syncmasterdata(inout boolean) owner to pdadbstguser;

create procedure pivot_da_middleware_digital.p_syncentities(INOUT can_update boolean DEFAULT NULL::boolean)
    language plpgsql
as
$$
declare
    current_entities_appdb           INTEGER := 0;
    current_entity_rows              INTEGER := 0;
    current_product_link_rows        INTEGER := 0;
    current_product_link_detail_rows INTEGER := 0;
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
                                                                        "Name", "FullName", "CreatedTime",
                                                                        "UpdatedTime")
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

alter procedure pivot_da_middleware_digital.p_syncentities(inout boolean) owner to pdadbstguser;

