select distinct COALESCE(uom_id, 0) UomId, COALESCE(uom_name, '') as Name from pivot_da_middleware.view_common_path_stream_kpi vcpsk
