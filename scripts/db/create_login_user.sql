\encoding UTF8

-- ユーザー(ロール)が存在しない場合のみ作成する
SELECT 'CREATE ROLE ' || quote_ident(:'user') || ' WITH LOGIN PASSWORD ' || quote_literal(:'password') || ' CREATEDB'
WHERE NOT EXISTS (
    SELECT FROM pg_catalog.pg_roles WHERE rolname = :'user'
)\gexec
