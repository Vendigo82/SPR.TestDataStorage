-- 

CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- --- System procedures

create schema if not exists utils;

create or replace function utils.execute(text)
    returns void
    language PlPgSQL strict as $$
begin
    execute $1;
end
$$;

create or replace function utils.eval(_expr text)
    returns text
    language PlPgSQL strict as $$
declare
    _result text;
begin
    execute concat('select ', _expr) into _result;
    return _result;
end
$$;

create or replace function utils.table_exists(_schema_name text, _table_name text)
    returns bool
    language SQL strict
as $$
    select exists(
        select 1
        from pg_tables
        where schemaname = _schema_name
            and tablename = _table_name
    );
$$;

create or replace function utils.index_exists(_schema_name text, _table_name text, _index_name text)
    returns bool
    language SQL strict
as $$
    select exists(
        select 1
        from pg_indexes
        where schemaname = _schema_name
            and tablename = _table_name
            and indexname = _index_name
    );
$$;

create or replace function utils.column_exists(_schema_name text, _table_name text, _column_name text)
    returns bool
    language SQL strict
as $$
    select exists(
        select 1
        from pg_attribute a, pg_type t, pg_class c, pg_namespace ns
        where a.attnum > 0 -- The number of the column. Ordinary columns are numbered from 1 up. System columns, such as oid, have (arbitrary) negative numbers.
            and a.atttypid = t.oid -- The data type of this column
            and a.attrelid = c.oid -- The table this column belongs to
            and c.relnamespace = ns.oid -- The OID of the namespace that contains this relation
            and ns.nspname = _schema_name
            and c.relname = _table_name
            and a.attname = _column_name -- The column name
    );
$$;

create or replace function utils.constraint_exists(_schema_name text, _table_name text, _constraint_name text)
    returns bool
    language SQL strict
as $$
    select exists(
        select 1
        from pg_constraint a, pg_namespace ns, pg_class c
        where a.conrelid = c.oid -- The table this column belongs to
            and a.connamespace = ns.oid -- The OID of the namespace that contains this relation
            and ns.nspname = _schema_name
            and c.relname = _table_name
            and a.conname = _constraint_name
    );
$$;

create or replace function utils.fk_exists(_schema_name text, _table_name text, _fk_name text)
    returns bool
    language SQL strict
as $$
    select exists(
        select 1
        from pg_constraint a, pg_namespace ns, pg_class c
        where a.contype = 'f'
            and a.conrelid = c.oid -- The table this column belongs to
            and a.connamespace = ns.oid -- The OID of the namespace that contains this relation
            and ns.nspname = _schema_name
            and c.relname = _table_name
            and a.conname = _fk_name
    );
$$;

create or replace function utils.pk_def_exists(
    _schema_name text,
    _table_name text,
    _idx_def_array text[] -- like ARRAY['1:field1', '2:field2', '3:field3']
)
    returns bool
    -- language PlPgSQL VOLATILE
    language PlPgSQL strict as $$
declare
    _cnt integer;
begin
    select count(*)
        into _cnt
    from (
        select a.attname,
            ( -- Position of the field in _index_. No field DDT index.
                select tmpx2.i + 1
                from (
                    SELECT generate_series(array_lower(i.indkey, 1), array_upper(i.indkey, 1)) as i
                ) tmpx2
                where i.indkey[i] = a.attnum
            ) as posicao
        from pg_class c, pg_namespace ns, pg_index i, pg_attribute a
        where
            c.oid = _table_name::regclass
            and ns.oid = c.relnamespace
            and ns.nspname = _schema_name
            and i.indrelid = c.oid
            and i.indisprimary
            and a.attrelid = c.oid
            and a.attnum = any(i.indkey)
    ) tmp
    where (tmp.posicao || ':' || attname::text) in (select * from unnest(_idx_def_array))
    ;
    return array_length(_idx_def_array, 1) = _cnt;
end
$$;

create or replace function utils.column_type(_schema_name text, _table_name text, _column_name text)
    returns text
    language SQL strict
as $$
    select isc.data_type
    from information_schema.columns isc
    where isc.table_schema = _schema_name
        and isc.table_name = _table_name
        and isc.column_name = _column_name
    ;
$$;

create or replace function utils.column_is_nullable(_schema_name text, _table_name text, _column_name text)
    returns boolean
    language SQL strict
as $$
    select (isc.is_nullable = 'YES')
    from information_schema.columns isc
    where isc.table_schema = _schema_name
        and isc.table_name = _table_name
        and isc.column_name = _column_name
    ;
$$;

create or replace function utils.drop_function(_schema_name text, _func_name text)
    returns text
    language PlPgSQL volatile as $$
declare
    _row record;
    _dropped_count smallint := 0;
    _params_count int;
    _func_proto text;
    _i int;
begin
    for _row in (
        select p.proargtypes
        from pg_proc p, pg_namespace ns
        where p.pronamespace = ns.oid
            and ns.nspname = _schema_name
            and p.proname = _func_name
    ) loop
        _params_count = array_upper(_row.proargtypes, 1) + 1;

        _func_proto = _schema_name || '.' ||_func_name || '(';
        _i = 0;
        while _i < _params_count loop
            if _i > 0 then
                _func_proto = _func_proto || ', ';
            end if;
            _func_proto = _func_proto || (select typname from pg_type where oid = _row.proargtypes[_i]);
            _i = _i + 1;
        end loop;
        _func_proto = _func_proto || ');';

        execute 'drop function ' || _func_proto;
        _dropped_count = _dropped_count + 1;
    end loop;
    return 'Dropped ' || _dropped_count || ' functions with name ' || _schema_name || '.' || _func_name;
END;
$$;

-- ---

create or replace function utils.create_partitions_range_dates(
    _schema_name text,
    _table_name text,
    _start_date date,
    _date_part text, -- week, month
    _partitions_count int,
    _period_length int default 1
)
    returns void
    language PlPgSQL volatile as $$
declare
    _interval interval;
    _end_date date;
    _d date;
    _next_date date;
    _sql text;
begin
    _interval = (_period_length || ' ' || _date_part)::interval;
    _start_date = date_trunc(_date_part, _start_date);
    _end_date = _start_date + (_partitions_count - 1) * _interval;

    for _d in (
        select generate_series(_start_date, _end_date, _interval)
    ) loop
        _next_date = _d + _interval;

        _sql = format(
            'create table if not exists %s_%s_%s partition of %s for values from (%L) to (%L)',
            _schema_name || '.' || _table_name,
            to_char(_d, 'YYYY'),
            case when _date_part = 'week' then 'w' || to_char(_d, 'WW') when _date_part = 'month' then 'm' || to_char(_d, 'MM') else '???' end || (case when _period_length > 1 then 'plus' else '' end),
            _schema_name || '.' || _table_name,
            _d, -- lower bound is an inclusive bound
            _next_date -- upper bound is an exclusive bound
        );

        raise notice '%', _sql;
        execute _sql;
    end loop;
end;
$$;

create or replace function utils.create_partitions_modulus(
    _schema_name text,
    _table_name text,
    _modulus int
)
    returns void
    language PlPgSQL volatile as $$
declare
    _max_reminder int;
    _maxlen_reminder int;
    _d int;
    _sql text;
begin
    _max_reminder = _modulus - 1;
    _maxlen_reminder = length(_max_reminder::text);

    for _d in (
        select generate_series(0, _max_reminder, 1)
    ) loop
        _sql = format(
            'create table if not exists %s_%s partition of %s for values with (modulus %s, remainder %s)',
            _schema_name || '.' || _table_name,
            'r' || lpad(_d::text, _maxlen_reminder, '0'),
            _schema_name || '.' || _table_name,
            _modulus,
            _d
        );
        raise notice '%', _sql;
        execute _sql;
    end loop;
end;
$$;

create or replace function utils._get_partitions_info(
    _schema_name text,
    _table_name text
)
    returns table (
        schema_name text,
        table_name text,
        expr text,
        constraintdef text
    )
    language PlPgSQL volatile as $$
begin
    return query
        select
            ns.nspname::text,
            c.relname::text,
            pg_catalog.pg_get_expr(c.relpartbound, inhrelid),
            pg_catalog.pg_get_partition_constraintdef(inhrelid)
        from pg_catalog.pg_class c
            join pg_namespace ns on ns.oid = c.relnamespace
            join pg_type t on t.oid = c.reltype -- for tables only. 'indexes, sequences, and toast tables' have no trltype
            join pg_catalog.pg_inherits i on c.oid = inhrelid
        where c.relispartition
            and i.inhparent::regclass = concat(_schema_name, '.', _table_name)::regclass
    ;
end;
$$;

/*
Examples:

select utils.create_partitions_range_dates(
    'aaa', -- _schema_name
    'bbb', -- _table_name
    current_date, -- _start_date
    'week', -- _date_part
    5, -- _partitions_count
    2 -- _period_length
);

select utils.create_partitions_range_dates(
    'aaa', -- _schema_name
    'bbb', -- _table_name
    current_date, -- _start_date
    'month', -- _date_part
    5, -- _partitions_count
    2 -- _period_length
);

select utils.create_partitions_modulus(
    'aaa', -- _schema_name
    'bbb', -- _table_name
    16 -- _modulus
);

-- Check that partition exists for the next week. cnt will be 0, if no such partition exists.
select count(pi.*) cnt
from utils._get_partitions_info('commacts', 'system_events') pi
where utils.eval(replace(pi.constraintdef, 'event_time', concat('''', (current_date + 7), '''')))::boolean
;

*/

-- ---
