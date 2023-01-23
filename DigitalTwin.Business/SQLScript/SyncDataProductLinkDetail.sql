
insert into pivot_da_middleware_digital."ProductLinkDetails" ("Id", "ProductLinkId", "Frequency", "DataDate",
															  "IsRealTime", "IsWeekly", "IsQuarterToDate", "IsYearToMonthly",
															  "IsDaily", "IsMonthly", "IsMonthToDate" , "IsQuarterly" ,
															  "IsYearEndProjection" , "IsYearToDaily" , "UomName" , "Name" ,
															  "CreatedTime" ,"UpdatedTime" , "Color" ,"NorCode" ,
															  "Percentage" ,
															  "Value" ,
															  "Variance",
															  "CreatedBy",
															  "UpdatedBy")
select 
	uuid_in(md5(random()::text || random()::text)::cstring) as Id,
	acad.kpi_id ,
	acad.frequency ,
	acad.data_date ,
	false,
	false,
	false,
	false,
	vcpsk.is_daily as IsDaily,
	vcpsk.is_monthly as IsMonthly,
	vcpsk.is_mtd as IsMonthToDate,
	vcpsk.is_quarterly as IsQuarterly,
	vcpsk.is_yep as IsYearEndProjection,
	vcpsk.is_ytd as IsYearToDaily,
	acad.uom_name ,
	'',
	'2022-09-21 16:28:17.913 +0700',
    '2022-09-21 16:28:17.913 +0700',
    acad.traffic_lights::jsonb -> 'norm_color' as Color,
    acad.traffic_lights::jsonb -> 'norm_code' as NorCode,
    (acad.num_values ->> 'variance_percentage')::numeric ,
    (acad.num_values ->> 'actual')::numeric ,
    (acad.num_values ->> 'variance')::numeric ,
    0,
    0,
	acad.num_values as NumValues
from
	pivot_da_middleware.view_common_path_stream_kpi vcpsk 
	join pivot_da_middleware_digital."Entities" e on vcpsk.entity_map_id = e."EntityId"
	join pivot_da_middleware.agg_common_amend_data acad
		on vcpsk.id = acad.kpi_id 
	and acad.traffic_lights::jsonb <> '{}'::jsonb
where
	vcpsk.has_traffic_light = true;


insert into pivot_da_middleware_digital."ProductLinkDetails" ("Id", "ProductLinkId", "Frequency", "DataDate",
															  "IsRealTime", "IsWeekly", "IsQuarterToDate", "IsYearToMonthly",
															  "IsDaily", "IsMonthly", "IsMonthToDate" , "IsQuarterly" ,
															  "IsYearEndProjection" , "IsYearToDaily" , "UomName" , "Name" ,
															  "CreatedTime" ,"UpdatedTime" , "Color" ,"NorCode" ,
															  "Percentage" ,
															  "Value" ,
															  "Variance",
															  "CreatedBy",
															  "UpdatedBy",
															  "NumValues")
select 
	uuid_in(md5(random()::text || random()::text)::cstring) as Id,
	acad.kpi_id ,
	acad.frequency ,
	acad.data_date ,
	false,
	false,
	false,
	false,
	vcpsk.is_daily as IsDaily,
	vcpsk.is_monthly as IsMonthly,
	vcpsk.is_mtd as IsMonthToDate,
	vcpsk.is_quarterly as IsQuarterly,
	vcpsk.is_yep as IsYearEndProjection,
	vcpsk.is_ytd as IsYearToDaily,
	acad.uom_name ,
	'',
	'2022-09-21 16:28:17.913 +0700',
    '2022-09-21 16:28:17.913 +0700',
    acad.traffic_lights::jsonb -> 'norm_color' as Color,
    acad.traffic_lights::jsonb -> 'norm_code' as NorCode,
    (acad.num_values ->> 'variance_percentage')::numeric ,
    (acad.num_values ->> 'actual')::numeric ,
    (acad.num_values ->> 'variance')::numeric ,
    0,
    0,
	acad.num_values as NumValues
from pivot_da_middleware.agg_common_amend_data acad 
join pivot_da_middleware.view_common_path_stream_kpi vcpsk 
	on acad.kpi_id = vcpsk.id 
	and vcpsk.kpi_name = 'Plant Load'
join pivot_da_middleware_digital."Entities" e 
	on vcpsk.entity_map_id = e."EntityId";