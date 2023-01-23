--insert new columns into ProductLinks
alter table pivot_da_middleware_digital."ProductLinks"
    add LevelId  int     null,
    add LeveName varchar null;

--update data for new columns in ProductLinks
update pivot_da_middleware_digital."ProductLinks" pl
set levelid  = subquery.level_id,
    levename = subquery.level_name
    from (select vcpsk.level_id, vcpsk.level_name, vcpsk.id
      from pivot_da_middleware.view_common_path_stream_kpi vcpsk) as subquery
where pl.LevelId is null
  and pl.LeveName is null
  and pl."Id" = subquery.id;