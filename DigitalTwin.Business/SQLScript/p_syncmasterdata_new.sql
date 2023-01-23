create procedure p_syncmasterdata(INOUT can_update boolean DEFAULT NULL::boolean)
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

alter procedure p_syncmasterdata(inout boolean) owner to pivotdadevdbuser;

