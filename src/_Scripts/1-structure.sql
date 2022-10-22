create schema if not exists data;

-- ---

select utils.execute($$
    create table data.project (
        id uuid primary key default public.gen_random_uuid(),

        system_name text not null,

        unique (system_name) -- also "unique index"
    );

$$) where not utils.table_exists('data', 'project');

select utils.execute($$
    create table data.object_type (
        id uuid primary key default public.gen_random_uuid(),

        system_name text not null,

        unique (system_name) -- also "unique index"
    );

$$) where not utils.table_exists('data', 'object_type');

select utils.execute($$
    create table data.data_section (
        id uuid primary key default public.gen_random_uuid(),

        system_name text not null,

        unique (system_name) -- also "unique index"
    );

$$) where not utils.table_exists('data', 'data_section');


select utils.execute($$
    create table data.object_type_data_section (
        object_type_id uuid not null,
        data_section_id uuid not null,
        
        unique (object_type_id, data_section_id), -- also "unique index"
        
        foreign key (object_type_id) references data.object_type (id),     
        foreign key (data_section_id) references data.data_section (id)
    );
$$) where not utils.table_exists('data', 'object_type_data_section');

select utils.execute($$
    create table data.data_header (
        id uuid primary key default public.gen_random_uuid(),
        
        project_id uuid not null,
        object_type_id uuid not null,
        object_identity text not null,
        data_name text not null,
        
        unique (project_id, object_type_id, object_identity, data_name), -- also "unique index"
        
        foreign key (project_id) references data.project (id),     
        foreign key (object_type_id) references data.object_type (id)
    );
$$) where not utils.table_exists('data', 'data_header');

select utils.execute($$
    create table data.data_content (
        id uuid primary key default public.gen_random_uuid(),
        
        data_header_id uuid not null,
        created_at timestamptz not null,
        data_sections json not null,
        hash text not null, -- SHA512
        
        unique (data_header_id, hash), -- also "unique index"
        
        foreign key (data_header_id) references data.data_header (id)
    );
$$) where not utils.table_exists('data', 'data_content');




