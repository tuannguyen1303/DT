select distinct coalesce (
	case 
		when traffic_lights::jsonb -> 'norm_color' is null then null
		else traffic_lights::jsonb ->> 'norm_color'
	end,
	''
) as Color,
	coalesce (
	case 
		when traffic_lights::jsonb -> 'norm_code' is null then null
		else traffic_lights::jsonb ->> 'norm_code'
	end,
	'0'
) as NorCode
from pivot_da_middleware.agg_common_amend_data acad;