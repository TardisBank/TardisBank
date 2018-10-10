Docker support
==============

Included in the repo are Dockerfiles and a `docker-compose.yml` in order to spin up an instance of TardisBank.

You will need to configure the Tardis Bank instance through a .env file if you want non-default values.
Accessible environment variables are:

`TARDISBANK_DB_USER`   the username for the postgres DB
`TARDISBANK_DB_PASSWORD`   the password for the postgres DB
`TARDISBANK_DB_DATABASENAME`   the database name for the postgres DB

`TARDISBANK_KEY` the key used for encryption in the API

You can copy an example `.env`: 

    cp .env.example .env

If you need to spin up a TardisBank instance with different backends (e.g., external database), 
you'll need to customize the `docker-compose.yml`
