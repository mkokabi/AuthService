# Creating table
```powershell
aws dynamodb create-table --table-name Users `
 --attribute-definitions `
  AttributeName=UserName,AttributeType=S `
  AttributeName=CreatedDate,AttributeType=S `
  --key-schema `
  AttributeName=UserName,KeyType=HASH `
  AttributeName=CreatedDate,KeyType=RANGE `
  --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1
```

```shell
aws dynamodb create-table --table-name Users \
 --attribute-definitions \
  AttributeName=UserName,AttributeType=S \
  AttributeName=CreatedDate,AttributeType=S \
  --key-schema \
  AttributeName=UserName,KeyType=HASH \
  AttributeName=CreatedDate,KeyType=RANGE \
  --provisioned-throughput ReadCapacityUnits=1,WriteCapacityUnits=1
```

# Delete table
 aws  dynamodb delete-table --table-name Users

# Insert
```shell
aws dynamodb put-item \
 --table-name Users \
--item '{
"UserName": {"S": "mkokabi"},
"UserID": {"N": "1" },
"Password" : {"S": "123" }
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