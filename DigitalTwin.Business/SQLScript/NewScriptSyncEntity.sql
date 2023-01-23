insert into pivot_da_middleware_digital."Entities" ("Id", "EntityId", "Name", "EntityTypeMasterId", "EntityTypeId",
                                                    "EntityParentId", "KpiPath", "Depth", "CreatedTime", "UpdatedTime",
                                                    "ValueChainTypeId")
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
       'ab0d6efa-c806-8492-09b1-2a004afb9b2a'
from pivot_da_middleware.view_common_path_stream vcps
         join pivot_da_middleware_digital."EntityTypes" et on vcps.entity_id = et."EntityId"
where is_standard = true
  and vcps.level_id > 1
order by vcps.level_id;