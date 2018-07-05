CREATE TABLE login (
	login_id integer generated always AS IDENTITY primary key,
	email text UNIQUE NOT NULL,
	password_hash text NOT NULL
);

CREATE INDEX login_email_index ON login (email);

CREATE TABLE account (
	account_id integer generated always AS IDENTITY primary key,
	login_id integer REFERENCES login (login_id),
	account_name text NOT NULL
);

CREATE TABLE trans (
	trans_id integer generated always AS IDENTITY primary key,
	account_id integer REFERENCES account (account_id),
	trans_date timestamp NOT NULL,
	amount money NOT NULL,
	balance money NOT NULL
);

CREATE TABLE schedule (
	schedule_id integer generated always AS IDENTITY primary key,
	account_id integer REFERENCES account (account_id),
	time_period text NOT NULL CHECK (time_period IN ('day', 'week', 'month')),
	next_run date NOT NULL
);