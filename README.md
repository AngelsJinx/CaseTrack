# Running everything with docker.
The entire tech test can be stood up using docker compose with a single command: `docker-compose up --build`

> It's possible to set the postgres password in the `.env` file.

This will build & run both the back-end and front-end projects, and spin up a postgres database. Depending on whether you have the relevant docker images locally, this may take a while.

For ease of inspection, both front-end and API are exposed as follows: \
Backend port 9876 - view API documentation at [Api documentation](http://localhost:9876/swagger/index.html) \
Frontend port 8080 - [Frontend](http://localhost:8080) \
If you would like to expose Postgres for direct querying, just uncomment the port section of the postgres service in `./compose.yaml`