CREATE OR REPLACE PROCEDURE pivot_da_middleware_digital.p_syncmasterdata(inout can_update boolean default null::boolean)
    LANGUAGE plpgsql
AS
$procedure$
declare
new_entity_types  int     := 0;
    new_uom           int     := 0;
    is_view_kpi_exist BOOLEAN := true;
begin
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

commit;
end
$procedure$
;
