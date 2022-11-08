insert into data.project (system_name) values ('test') on conflict do nothing;
insert into data.object_type (system_name) values ('test_object') on conflict do nothing;
insert into data.data_section (system_name) values ('data1') on conflict do nothing;
insert into data.data_section (system_name) values ('data2') on conflict do nothing;

with settings as (
  select
     (select id from data.object_type where system_name = 'test_object') as test_object
    ,(select id from data.data_section where system_name = 'data1') as data1
    ,(select id from data.data_section where system_name = 'data2') as data2
) 
insert into data.object_type_data_section (object_type_id, data_section_id)
values
   ((select test_object from settings), (select data1 from settings))
  ,((select test_object from settings), (select data2 from settings))
on conflict do nothing;
