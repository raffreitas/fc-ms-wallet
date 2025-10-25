create table if not exists balances
(
    account_id uuid primary key,
    amount     numeric not null
);

create table if not exists transactions
(
    id              uuid primary key,
    account_id_from uuid,
    account_id_to   uuid,
    amount          numeric   not null,
    created_at      timestamp not null default now()
);