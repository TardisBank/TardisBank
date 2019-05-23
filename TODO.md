# API

- ~~Create application. GET / should work.~~

- ~~Sign up with email/pw. Should create account. Create DB.~~

- ~~Auth login returns token~~.

- ~~Auth header validation for all other requests.~~

- ~~Show account info on GET /~~

- ~~Delete account. DELETE /~~

- Verify Email on signup.

- Forgotten password.

- ~~Add Account.~~

- ~~List Accouts. GET children/~~

- ~~Del Account.~~

- ~~Credit Transaction.~~

- ~~Debit Transaction.~~

- ~~List Transactions. GET children/child-id/transactions~~

- ~~Add Schedule POST children/child-id/schedule~~

- ~~Delete Schedule DELETE children/child-id/schedule/schedule-id~~

- ~~List Schedules GET children/child-id/schedule~~

- Run scheduler.

- ~~Deploy to Azure.~~

# FE

- Homepage.

- ~~Signup.~~

- ~~Login.~~

- ~~Create child.~~

- ~~List children.~~

- ~~Delete child.~~

- ~~Credit Transaction.~~

- ~~Debit Transaction.~~

- ~~List Transactions.~~

- Add schedule.

- List Schedules.

- Delete Schdule.

# Ops

- HTTPS for all public endpoints

- Deploy Dev and Prod to same server

- Database migrations

- Persitent database storage for Prod instance

- Backup of prod database

- Deploy server from AppVeyor

## Docker refactor

The app will be run locally by a developer, during the build by AppVeyor and on a server. The current Docker configurtion
does not support all these scenarios so requires some refactoring:

- Create a base docker-compose file that defines the main services: db, app, client
- Create a development docker-compose that adds a development mail services: rnwood/smpt4dev (this is mail server with a web front end. All messages sent to it can be viewed in a browser)
- Create a prod docker-compose that adds a normal smtp server but also defines the following networks:
  -- internal-${token}
    -- proxy
  The internal-${token} network is used only be the containers in this docker-compose. All by the nginx container use this network
  The proxy network will already be defined on the host. The nginx container uses this network and the internal-\${token} network
- Add \${token} to all container names. This way multiple instances of the same container can be run e.g., tardis-bank-api-pr22 or tardis-bank-api-prod
- Add Traefik labels to the nginx container which will dynamically assign it to the hosts reverse proxy

## Production host confiugration

Configure the single Tardis Bank VPS to host many instances of the site. A key part of this is running a reverse proxy that can route
traffic to the correct container. Traefik is the perfect tool for the job. It is also key that the configuration of this server is recorded and replciated. The tasks are:

- Document the configuration already done on the sever (user accounts, firewall settings, software installed)
- Create a git repo and add a script that will perform the configuration
- Install Traefik as docker container
- Follow the instructions to create a docker network called proxy
- Follow the instructions to configure HTTPS
