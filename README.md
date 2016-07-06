# MySql2MSSQL
Tool to extract data from MySql into a csv or MSSQL file and then a way to import the data into MSSQL

Here are the commands available

`--help: prints this help guide`

`--host: the name or ip of the database host to connect to `

`--db: the name of the database`

`--user: database user name`

`--password: database password `

`--table: the name of the table to work on`

`--file: the name of the file to work on`

`--format: the format to work on. csv or sql`

`--target: the target engine either mysql or mssql`

`--timeout: the connection time out, defaults to 0`

`--offset: the record start count`

`--batchSize: the size of the batchs to get from the server`


To export mysql data into a csv file you run

`--host {my-server} --user {my-user} --password {my-password} --db {db-name} --table {table-name} --format csv --target mysql --file {my-file-location}`
