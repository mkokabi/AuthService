# Creating table
## Users
```powershell
aws dynamodb create-table --table-name Users `
 --attribute-definitions `
  AttributeName=UserName,AttributeType=S `
  --key-schema `
  AttributeName=UserName,KeyType=HASH `
  --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1
```

```shell
aws dynamodb create-table --table-name Users \
 --attribute-definitions \
  AttributeName=UserName,AttributeType=S \
  --key-schema \
  AttributeName=UserName,KeyType=HASH \
  --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1
```

## UserCatalg
```shell
aws dynamodb create-table --table-name UsersCatalog \
 --attribute-definitions \
  AttributeName=Id,AttributeType=N \
  --key-schema \
  AttributeName=Id,KeyType=HASH \
  --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1
```

# Delete table
 aws  dynamodb delete-table --table-name Users

# Insert
## Users
```shell
aws dynamodb put-item \
 --table-name Users \
--item '{
"UserName": {"S": "mkokabi"},
"UserID": {"N": "1" },
"Password" : {"S": "123" }
}'
```

## UsersCatalog
```shell
aws dynamodb put-item \
 --table-name UsersCatalog \
--item '{
"Id": {"N": "0"},
"NewId": {"N": "0" }
}'
```

# Scan
```shell
aws dynamodb scan --table-name Users
```

# Query
```shell
aws dynamodb query --table-name Users --key-condition-expression "UserName = :un" --expression-attribute-values '{":un":{"S":"mkokabi"}}'
```