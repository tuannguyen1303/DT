insert into pivot_da_middleware_digital."ProductLinks" ("Id", "UnitOfMeasureId", "EntityMapId", 
														"Name", "FullName", "CreatedTime", "UpdatedTime")
select vcpsk.id as Id
	  ,uom."Id" as UnitOfMeasureId
	  ,vcpsk.entity_map_id as EntityMapId
	  ,vcpsk.kpi_name as Name
	  ,vcpsk.kpi_fullname as FullName
	  ,'2022-09-21 16:28:17.913 +0700'
      ,'2022-09-21 16:28:17.913 +0700'
from pivot_da_middleware.view_common_path_stream_kpi vcpsk 
	join pivot_da_middleware_digital."Entities" e on vcpsk.entity_map_id = e."EntityId" 
	join pivot_da_middleware_digital."UnitOfMeasures" uom on vcpsk.uom_id = uom."UomId"
where has_traffic_light = true;

insert into pivot_da_middleware_digital."ProductLinks" ("Id", "UnitOfMeasureId", "EntityMapId", 
														"Name", "FullName", "CreatedTime", "UpdatedTime")
select vcpsk.id as Id
	  ,uom."Id" as UnitOfMeasureId
	  ,vcpsk.entity_map_id as EntityMapId
	  ,vcpsk.kpi_name as Name
	  ,vcpsk.kpi_fullname as FullName
	  ,current_timestamp
      ,current_timestamp
from pivot_da_middleware.view_common_path_stream_kpi vcpsk 
	join pivot_da_middleware_digital."Entities" e on vcpsk.entity_map_id = e."EntityId" 
	join pivot_da_middleware_digital."UnitOfMeasures" uom on vcpsk.uom_id = uom."UomId"
where vcpsk.kpi_name = 'Plant Load';