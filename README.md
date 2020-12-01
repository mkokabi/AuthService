# AuthService
The PoC of creating a Backward compatibility layer in front of a SOAP Web Service and REST API and switch between those using the Future Flag.

The backend in the SOAP Web Service is SQL Server while the backend of the REST API is AWS DynamoDB.

The new REST API also have an extra future to support the secondary password. 

The console app has an option to migrate existing users data from the SQL Server to DynamoDB.
