CREATE SCHEMA IF NOT EXISTS football;

-- Создание таблицы broadcasts

CREATE TABLE IF NOT EXISTS football.broadcasts (
    id SERIAL PRIMARY KEY,
    date_start TIMESTAMP NOT NULL,
    home_team_name VARCHAR(255) NOT NULL,
    guest_team_name VARCHAR(255) NOT NULL,
    status SMALLINT NOT NULL
    );

COMMENT ON COLUMN football.broadcasts.id IS 'Уникальный идентификатор записи';
COMMENT ON COLUMN football.broadcasts.date_start IS 'Дата начала трансляции';
COMMENT ON COLUMN football.broadcasts.home_team_name IS 'Название домашней команды';
COMMENT ON COLUMN football.broadcasts.guest_team_name IS 'Название гостевой команды';
COMMENT ON COLUMN football.broadcasts.status IS 'Статус трансляции';
