-- Изменение типа данных в столбце status таблицы loans
ALTER TABLE dcs_loans.loans
ALTER COLUMN status TYPE smallint
USING CASE WHEN status = 'InProgress' THEN 1
           	WHEN status = 'Approved' THEN 2
			WHEN status = 'Denied' THEN 3
            ELSE 0 END;