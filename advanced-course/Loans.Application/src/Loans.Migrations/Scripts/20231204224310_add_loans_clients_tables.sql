CREATE SCHEMA IF NOT EXISTS dcs_loans;

-- Создание таблицы clients
CREATE TABLE IF NOT EXISTS dcs_loans.clients (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR,
    last_name VARCHAR,
    middle_name VARCHAR,
    birth_date TIMESTAMP,
    salary DECIMAL(10, 2) CHECK (salary >= 0)
    );

COMMENT ON COLUMN dcs_loans.clients.id IS 'Уникальный идентификатор клиента';
COMMENT ON COLUMN dcs_loans.clients.first_name IS 'Имя клиента';
COMMENT ON COLUMN dcs_loans.clients.last_name IS 'Фамилия клиента';
COMMENT ON COLUMN dcs_loans.clients.middle_name IS 'Отчество клиента';
COMMENT ON COLUMN dcs_loans.clients.birth_date IS 'Дата рождения клиента';
COMMENT ON COLUMN dcs_loans.clients.salary IS 'Зарплата клиента с точностью до двух знаков после запятой';

-- Создание таблицы loans
CREATE TABLE IF NOT EXISTS dcs_loans.loans (
    id SERIAL PRIMARY KEY,
    client_id INTEGER REFERENCES dcs_loans.clients(id),
    term_in_years INTEGER CHECK (term_in_years > 0),
    amount DECIMAL(15, 2) CHECK (amount >= 0),
    status VARCHAR,
    expected_interest_rate DECIMAL(5, 2) CHECK (expected_interest_rate >= 0),
    creation_date TIMESTAMP,
    reject_reason VARCHAR
    );

COMMENT ON COLUMN dcs_loans.loans.id IS 'Уникальный идентификатор займа';
COMMENT ON COLUMN dcs_loans.loans.client_id IS 'Ссылка на клиента через внешний ключ';
COMMENT ON COLUMN dcs_loans.loans.term_in_years IS 'Срок займа в годах должен быть положительным';
COMMENT ON COLUMN dcs_loans.loans.amount IS 'Сумма займа с точностью до двух знаков после запятой должна быть неотрицательной';
COMMENT ON COLUMN dcs_loans.loans.status IS 'Статус займа';
COMMENT ON COLUMN dcs_loans.loans.expected_interest_rate IS 'Ожидаемая процентная ставка с точностью до двух знаков после запятой должна быть неотрицательной';
COMMENT ON COLUMN dcs_loans.loans.creation_date IS 'Дата создания займа';
COMMENT ON COLUMN dcs_loans.loans.reject_reason IS 'Причина отклонения займа';
