CREATE OR REPLACE PROCEDURE pivot_da_middleware_digital.usp_check_existed_function(
	INOUT isExisted boolean DEFAULT NULL::boolean)
LANGUAGE 'plpgsql'
AS $BODY$
begin
    select exists(select 1 from pivot_da_middleware.get_de_sem_data('monthly')) into isExisted;
    commit;
end
$BODY$;

