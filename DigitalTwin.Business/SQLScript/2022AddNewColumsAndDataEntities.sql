alter table pivot_da_middleware_digital."Entities"
    add LevelId int null,
    add Density  varchar null,
    add RootName varchar null,
    add RootId   int     null;
--
update pivot_da_middleware_digital."Entities" e
set "LevelId"  = subquery.level_id,
    "Density"  = subquery.density,
    "RootName" = subquery.root_name,
    "RootId"   = subquery.root_id from (select vcps.level_id, vcps.density, vcps.root_name, vcps.root_id, vcps.id
          from pivot_da_middleware.view_common_path_stream vcps) as subquery
where e."LevelId" is null
  and e."Density" is null
  and e."RootId" is null
  and e."RootName" is null
  and e."EntityId" = subquery.id;